﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Common;

using Newtonsoft.Json.Linq;

namespace Redux
{
    public class ReduxMessages
    {
        #region Поля

        private Database db;

        #endregion Поля

        #region Свойства

        public decimal Count
        {
            get
            {
                return (decimal)db.Query(
                    "select count(*)" +
                    " from redux_messages").Single().count;
            }
        }

        #endregion Свойства

        #region Основные функции

        /// <summary>
        /// Постраничное формирование списка сообщений
        /// </summary>
        public dynamic GetMessages(int page, int count)
        {
            return db.Query(
                "select *" +
                " from redux_messages" +
                $" limit {count} offset {(page - 1) * count}");
        }

        /// <summary>
        /// Формирование списка ответов для заданных пользователей
        /// </summary>
        public dynamic GetPlayersMessages(List<decimal> ids)
        {
            return db.Query(
                "select *" +
                " from redux_messages" +
                $" where limit \"SteamID\" in ({string.Join(", ", ids)}) and not Reply is null and not IsPlayerRead");
        }

        /// <summary>
        /// Добавление сообщения
        /// </summary>
        public void PostMessage(JObject message)
        {
            db.Execute(
                "insert into redux_messages (\"SteamID\", \"Nickname\", \"Comment\", \"TimeStamp\")" +
                " values (@SteamID, @Nickname, @Message, @TimeStamp)", message.ToNetObject());
        }

        /// <summary>
        /// Изменение сообщения
        /// </summary>
        public void ChangeMessage(JObject message)
        {
            string query = 
                "update redux_messages" +
                " set {0} = {1}" +
                " where \"ID\" = {2}";

            if (!message["Reply"].IsNullOrEmpty())
                query = string.Format(query, "\"Reply\"", $"'{message["Reply"].ToString()}'", message["ID"]);
            else if (!message["IsPlayerRead"].IsNullOrEmpty())
                query = string.Format(query, "\"IsPlayerRead\"", "true", message["ID"]);
            else 
                return;

            db.Execute(query);
        }

        /// <summary>
        /// Удаление сообщений
        /// </summary>
        public void DeleteMessages(JObject message)
        {
            string ids = string.Join(", ", message["ids"].Values());
            db.Execute($"delete from redux_messages where \"ID\" in ({ids})");
        }

        public ReduxMessages(Database database)
        {
            db = database;
        }

        #endregion Основные функции
    }
}