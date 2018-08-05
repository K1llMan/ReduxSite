using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Redux
{
    public class ReduxBuilds
    {
        #region Поля

        private Database db;

        #endregion Поля

        #region Свойства

        public decimal Count
        {
            get
            {
                var result = db.Query(
                    "select count(*)" +
                    " from redux_builds");

                return result == null ? 0 : (decimal)result.Single().count;
            }
        }

        #endregion Свойства

        #region Вспомогательные функции

        private string GetMD5Hash(string hashStr)
        {
            MD5 md5 = MD5.Create();
            return string.Join("", md5.ComputeHash(Encoding.ASCII.GetBytes(hashStr)).Select(b => b.ToString("X2"))).ToLower();
        }

        #endregion Вспомогательные функции

        #region Основные функции

        /// <summary>
        /// Постраничное формирование списка билдов
        /// </summary>
        public dynamic GetBuilds(int page, int count, string sort)
        {
            dynamic rows = db.Query(
                "select *" +
                " from redux_builds" +
                $" order by {sort} desc" +
                $" limit {count} offset {(page - 1) * count}");

            foreach (dynamic row in rows)
                row.abilities = JArray.Parse(row.abilities);

            return rows;
        }

        public string Submit(JObject data)
        {
            string hashStr = GetMD5Hash(JsonConvert.SerializeObject(data["abilities"].OrderBy(a => a)));

            dynamic row = db.Query(
                "select exists" +
                    " (select *" +
                    " from redux_builds" +
                    $" where hash = '{hashStr}')").Single();

            if (row.exists)
                return "exists";

            Dictionary<string, object> savingData = (Dictionary<string, object>) data.ToNetObject();
            savingData.Add("hash", hashStr);
            savingData["abilities"] = JsonConvert.SerializeObject(savingData["abilities"]);

            db.Execute(
                "insert into redux_builds (Hash, SteamID, Title, Description, Abilities, HeroName, Attribute)" +
                " values (@hash, @steamId, @title, @description, @abilities, @heroname, @attribute)", savingData);

            return string.Empty;
        }

        public void Vote(JObject data)
        {
            dynamic row = db.Query(
                "select builds" +
                " from redux_players" +
                $" where steamid = '{data["steamID"]}'").FirstOrDefault();

            if (row == null)
                return;

            Dictionary<string, object> builds = (Dictionary<string, object>) ((JObject)JsonConvert.DeserializeObject(row.builds)).ToNetObject();
            foreach (string field in new [] { "votes_up", "votes_down" })
                if (!builds.ContainsKey(field))
                    builds.Add(field, new List<object>());

            string type = $"votes_{data["type"]}";
            string unusedType = type == "votes_up" ? "votes_down" : "votes_up";

            int rating = 0;
            string build = data["build"].ToString();

            if (((List<object>) builds[type]).Contains(build))
            {
                ((List<object>) builds[type]).Remove(build);
                rating--;
            }
            else
            {
                ((List<object>)builds[type]).Add(build);
                rating++;
            }

            if (((List<object>)builds[unusedType]).Contains(build))
            {
                ((List<object>)builds[unusedType]).Remove(build);
                rating++;
            }

            // Обновление билдов пользователя
            db.Execute(
                "update redux_players" +
                $" set builds = '{JsonConvert.SerializeObject(builds)}'" +
                $" where steamid = '{data["steamID"]}'");

            // Обновление рейтинга билда
            rating = type == "votes_down" ? -rating : rating;
            db.Execute(
                "update redux_builds" +
                $" set rating = rating + {rating}" +
                $" where hash = '{data["build"]}'");
        }

        public void Favorites(JObject data)
        {
            dynamic row = db.Query(
                "select builds" +
                " from redux_players" +
                $" where steamid = '{data["steamID"]}'").FirstOrDefault();

            if (row == null)
                return;

            Dictionary<string, object> builds = (Dictionary<string, object>)((JObject)JsonConvert.DeserializeObject(row.builds)).ToNetObject();
            foreach (string field in new[] { "favorites" })
                if (!builds.ContainsKey(field))
                    builds.Add(field, new List<object>());

            string build = data["build"].ToString();

            if (((List<object>)builds["favorites"]).Contains(build))
                ((List<object>)builds["favorites"]).Remove(build);
            else
                ((List<object>)builds["favorites"]).Add(build);

            // Обновление билдов пользователя
            db.Execute(
                "update redux_players" +
                $" set builds = '{JsonConvert.SerializeObject(builds)}'" +
                $" where steamid = '{data["steamID"]}'");
        }

        public ReduxBuilds(Database database)
        {
            db = database;
        }

        #endregion Основные функции
    }
}
