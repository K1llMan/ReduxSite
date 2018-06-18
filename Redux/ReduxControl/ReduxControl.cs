using System;
using System.IO;
using System.Security.Claims;

using Common;

namespace Redux
{
    public class ReduxControl: IDisposable
    {
        #region Свойства

        public ReduxSettings Settings { get; set; }

        public ReduxMessages Messages { get; }

        public ReduxStats Stats { get; }

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
            Messages = new ReduxMessages(Settings.DB);
            Stats = new ReduxStats(Settings.DB);

            if (Settings.JWTKey.Length < 16)
                Logger.WriteToTrace("Для корректной работы JWT ключ должен быть не менее 16 символов.", TraceMessageKind.Error);

            // Делегат проверки пользователя и формирования требований к пользователю
            CheckUser check = d => {
                dynamic row = Settings.DB.Query(
                    "select *" +
                    " from redux_users" +
                    $" where \"Username\" = '{d["user"]}'");

                if (row == null)
                    return null;

                row = row.Single();
                if (row.Password.ToString() != d["password"])
                    return null;

                return new Claim[] {
                    new Claim(ClaimTypes.Name, row.Username.ToString()),
                    new Claim(ClaimTypes.Role, row.Role.ToString())
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
