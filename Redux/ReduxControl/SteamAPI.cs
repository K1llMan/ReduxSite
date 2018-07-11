using System;
using System.IO;
using System.Net;

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

        private string GetResponse(string url, string method = "GET")
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                return new StreamReader(response.GetResponseStream()).ReadToEnd();
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
            if (string.IsNullOrEmpty(key))
                return null;

            string url = $"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={key}&steamids={string.Join(",", ids)}";
            return JObject.Parse(GetResponse(url));
        }

        public SteamAPI(string apiKey)
        {
            key = apiKey;
        }

        #endregion Основные функции
    }
}
