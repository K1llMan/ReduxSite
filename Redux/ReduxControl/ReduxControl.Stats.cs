﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Common;

namespace Redux
{
    public class ReduxStats
    {
        #region Поля

        private Database db;

        #endregion Поля

        #region Основные функции

        public ReduxStats(Database database)
        {
            db = database;
        }

        #endregion Основные функции
    }
}
