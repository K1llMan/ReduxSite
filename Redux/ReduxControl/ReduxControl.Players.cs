using System.Collections.Generic;
using System.Linq;

using Common;

using Newtonsoft.Json.Linq;

namespace Redux
{
    public class ReduxPlayers
    {
        #region Поля

        private Database db;

        #endregion Поля

        #region Основные функции

        /// <summary>
        /// Возвращает одно или несколько полей из списка
        /// </summary>
        public object GetFields(string steamID, params string[] fields)
        {
            string[] avaliableFields = { "abilities", "gamemodes", "heroes", "settings", "bans", "builds" };

            string[] existFields = fields == null 
                ? new string[] { "*" }
                : avaliableFields.Intersect(fields).ToArray();

            if (existFields.Length == 0)
                return null;

            string query =
                $"select {string.Join(", ", existFields)}" +
                " from redux_players" +
                $" where steamid = '{steamID}'";

            var row = (IDictionary<string, object>)db.Query(query).SingleOrDefault();
            if (row == null)
                return null;

            foreach (string field in row.Keys)
                // Поля, конвертируемые в json
                if (avaliableFields.Contains(field))
                    row[field] = JObject.Parse(row[field] == null ? "{}" : row[field].ToString());
            return row;
        }

        public void SetField(string steamID, string field, JToken data)
        {
            string[] avaliableFields = { "settings", "bans" };
            if (!avaliableFields.Contains(field))
                return;
        }

        /// <summary>
        /// Добавляет новых игроков и возвращает информацию
        /// </summary>
        public dynamic GetPlayersInfo(params string[] ids)
        {
            return null;
        }

        public ReduxPlayers(Database database)
        {
            db = database;
        }

        #endregion Основные функции
    }
}
