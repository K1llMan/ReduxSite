using System;
using System.IO;
using System.Linq;
using System.Security.Claims;

using Common;

namespace Redux
{
    public class ReduxControl: IDisposable
    {
        #region Свойства

        public SteamAPI Steam { get; set; }

        public ReduxSettings Settings { get; set; }

        public ReduxMessages Messages { get; }

        public ReduxStats Stats { get; }

        public ReduxPlayers Players { get; }

        public ReduxMatch Matches { get; }

        public ReduxBuilds Builds { get; }

        /// <summary>
        /// JWT (JSON Web Token) для авторизации пользователей
        /// </summary>
        public JwtControl JWT { get; }

        #endregion Свойства

        #region Основные функции

        public ReduxControl()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            Settings = new ReduxSettings(Path.Combine(baseDir, "ReduxSettings.json"));
            // Соединение с базой
            try
            {
                Settings.DB.Connect();
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при подключении к базе: {ex}", TraceMessageKind.Error);
            }

            Steam = new SteamAPI(Settings.SteamAPIKey);
            Messages = new ReduxMessages(Settings.DB);
            Stats = new ReduxStats(Settings.DB);
            Players = new ReduxPlayers(Settings.DB);
            Matches = new ReduxMatch(Settings.DB);
            Builds = new ReduxBuilds(Settings.DB);

            if (Settings.JWTKey.Length < 16)
                Logger.WriteToTrace("Для корректной работы JWT ключ должен быть не менее 16 символов.", TraceMessageKind.Error);

            // Делегат проверки пользователя и формирования требований к пользователю
            CheckUser check = d => {
                dynamic row = Settings.DB.Query(
                    "select *" +
                    " from redux_users" +
                    $" where Username = '{d["user"]}'").FirstOrDefault();

                if (row == null)
                    return null;

                if (row.password.ToString() != d["password"])
                    return null;

                return new Claim[] {
                    new Claim(ClaimTypes.Name, row.username.ToString()),
                    new Claim(ClaimTypes.Role, row.role.ToString())
                };
            };
            
            JWT = new JwtControl(check, Settings.JWTKey);
        }

        #endregion Основные функции

        public void Dispose()
        {
            Settings?.Dispose();
        }
    }
}
