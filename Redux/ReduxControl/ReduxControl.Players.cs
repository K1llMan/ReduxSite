using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

using Common;

using Newtonsoft.Json;
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
            string[] avaliableObjectFields = { "abilities", "gamemodes", "heroes", "settings", "bans", "builds" };
            string[] avaliableArrayFields = { "roles" };

            string[] existFields = fields.Length == 0
                ? new string[] { "*" }
                : avaliableObjectFields.Union(avaliableArrayFields).Intersect(fields).ToArray();

            string query =
                $"select {string.Join(", ", existFields)}" +
                " from redux_players" +
                $" where steamid = '{steamID}'";

            var row = (IDictionary<string, object>)db.Query(query).SingleOrDefault();
            if (row == null)
                return null;

            foreach (string field in row.Keys)
            {
                // Поля, конвертируемые в json
                if (avaliableObjectFields.Contains(field))
                    row[field] = JObject.Parse(row[field] == null ? "{}" : row[field].ToString());
                if (avaliableArrayFields.Contains(field))
                    row[field] = JArray.Parse(row[field] == null ? "[]" : row[field].ToString());
            }
            return row;
        }

        /// <summary>
        /// Обновление данных поля
        /// </summary>
        public void SetField(string steamID, string field, JToken data)
        {
            string[] avaliableFields = { "settings", "bans" };
            if (!avaliableFields.Contains(field))
                return;

            db.Execute(
                "update redux_players" +
                $" set field = '{JsonConvert.SerializeObject(data)}'" +
                $" where steamid = '{steamID}'");
        }

        /// <summary>
        /// Добавляет новых игроков и возвращает информацию
        /// </summary>
        public dynamic GetPlayersInfo(string[] ids, string[] fields)
        {
            if (ids == null || ids.Length == 0)
                return null;

            dynamic idRows = ids.Select(id => {
                dynamic r = new ExpandoObject();
                r.id = id;
                return r;
            });

            // Изначально добавляем игроков
            db.Execute(
                "insert into redux_players (steamid)" +
                " values(@id)" +
                " on conflict (steamid) do nothing", idRows);

            dynamic rows = ids.Select(id => GetFields(id, fields));

            return rows;
        }

        /// <summary>
        /// Обновление ролей пользователей
        /// </summary>
        public void SetRoles(JObject data)
        {
            int count = data["steamID"].Select(s => s.ToString()).Count(s => !string.IsNullOrEmpty(s));
            if (data["steamID"].IsNullOrEmpty() || count == 0)
                return;

            dynamic rows = db.Query(
                "select steamid, roles" +
                " from redux_players" +
                $" where steamid in ({string.Join(", ", ((JArray)data["steamID"]).Select(t => $"'{t.ToString()}'")) })");

            foreach (dynamic row in rows)
            {
                row.roles = ((JArray)JArray.Parse(row.roles == null ? "[]" : row.roles)).ToNetObject();
                foreach (JToken role in data["roles"])
                    if (((List<object>) row.roles).Contains(role.ToString()))
                        ((List<object>) row.roles).Remove(role.ToString());
                    else
                        ((List<object>)row.roles).Add(role.ToString());

                row.roles = JsonConvert.SerializeObject(row.roles);
            }

            db.Execute(
                "update redux_players" +
                " set roles = @roles" +
                " where steamid = @steamid", rows);
        }

        public ReduxPlayers(Database database)
        {
            db = database;
        }

        #endregion Основные функции
    }
}
