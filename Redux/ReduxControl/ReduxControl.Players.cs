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
        public JToken GetFields(string steamID, params string[] fields)
        {
            string[] avaliableFields = { "abilities", "gamemodes", "heroes", "settings", "bans", "builds" };
            string[] existFields = avaliableFields.Intersect(fields).ToArray();

            if (existFields.Length == 0)
                return null;

            return null;
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
