using System;
using System.IO;

using Newtonsoft.Json.Linq;

namespace Common
{
    #region Настройки

    public class ReduxSettings: IDisposable
    {
        #region Свойства

        public Database DB { get; }

        public string JWTKey { get; }

        #endregion Свойства

        #region Основные функции

        public ReduxSettings(string settings)
        {
            JObject data = JsonCommon.Load(settings);

            DB = new Database(data["Database"].ToString());
            JWTKey = data["JWTKey"].ToString();
        }

        #endregion Основные функции

        public void Dispose()
        {
            DB?.Disconnect();
        }
    }

    #endregion Настройки
}
