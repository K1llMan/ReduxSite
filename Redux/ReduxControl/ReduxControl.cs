using System;
using System.IO;

using Common;

namespace Redux
{
    public class ReduxControl: IDisposable
    {
        #region Свойства

        public ReduxSettings Settings { get; set; }

        /// <summary>
        /// JWT (JSON Web Token) для авторизации пользователей
        /// </summary>
        public JWTControl JWT { get; }

        #endregion Свойства

        #region Основные функции

        public ReduxControl()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            Settings = new ReduxSettings(Path.Combine(baseDir, "ReduxSettings.json"));

            if (Settings.JWTKey.Length < 16)
                Logger.WriteToTrace("Для корректной работы JWT ключ должен быть не менее 16 символов.", TraceMessageKind.Error);

            JWT = new JWTControl(Settings.DB, Settings.JWTKey);
        }

        #endregion Основные функции

        public void Dispose()
        {
            Settings?.Dispose();
        }
    }
}
