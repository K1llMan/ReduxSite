using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Common;

using Newtonsoft.Json.Linq;

namespace Redux
{
    public class SteamAPI
    {
        #region Поля

        private string key;

        #endregion Поля

        #region Вспомогательные функции

        private string GetResponse(string url, string method)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                return new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при запросе к SteamAPI: {ex}", TraceMessageKind.Error);
                return string.Empty;
            }
        }

        #endregion Вспомогательные функции

        #region Основные функции

        public JToken GetPlayersData(params string[] ids)
        {
            string url = $"api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={key}&steamids={string.Join(",", ids)}";
            return JObject.Parse(GetResponse(url, "GET"));
        }

        public SteamAPI(string apiKey)
        {
            key = apiKey;
        }

        #endregion Основные функции
    }
}
