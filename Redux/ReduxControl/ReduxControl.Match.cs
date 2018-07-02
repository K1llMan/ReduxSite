using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Common;

using Newtonsoft.Json.Linq;

namespace Redux
{
    public class ReduxMatch
    {
        #region Поля

        private Database db;

        #endregion Поля

        #region Основные функции

        public void Save(JToken matchInfo)
        {
            
        }

        public ReduxMatch(Database database)
        {
            db = database;
        }

        #endregion Основные функции
    }
}
