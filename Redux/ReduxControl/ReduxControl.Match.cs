using System;
using System.Collections.Generic;
using System.Linq;

using Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Redux
{
    public class ReduxMatch
    {
        #region Поля

        private Database db;

        #endregion Поля

        #region Вспомогательные функции

        #region Глобальная статистика

        private void UpdateGlobalAbilities(JToken matchInfo, string gamemode)
        {
            try
            {
                var abilities = matchInfo.SelectTokens("$..abilities.*[*]")
                    .Select(a => new object[] {
                        a.ToString(),
                        ((JProperty)a.Parent.Parent).Name,
                        a.Ancestors().First(p => p is JObject && ((JObject)p).ContainsKey("isWinner"))
                    })
                    .GroupBy(r => r[0])
                    .Select(g => new Dictionary<string, object>{
                        { "gamemode", gamemode },
                        { "name", g.Key },
                        { "picks", g.Count(r => r[1].ToString() == "picks") },
                        { "bans", g.Count(r => r[1].ToString() == "bans") },
                        { "wins", g.Count(r => r[1].ToString() != "bans" && Convert.ToBoolean(((JObject)r[2])["isWinner"])) }
                    });

                string query =
                    "insert into redux_abilities (gamemode, name, picks, bans, wins) " +
                    " VALUES (@gamemode, @name, @picks, @bans, @wins)" +
                    " on conflict (gamemode, name, timestamp) do update" +
                    " set picks = redux_abilities.picks + @picks, bans = redux_abilities.bans + @bans, wins = redux_abilities.wins + @wins";

                db.Execute(query, abilities);
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при обновлении статистики способностей: {ex}", TraceMessageKind.Error);
            }
        }

        private void UpdateGlobalHeroes(JToken matchInfo, string gamemode)
        {
            try
            {
                var heroes = matchInfo.SelectTokens("$..hero")
                    .Select(a => new object[] {
                        a.ToString(),
                        a.Ancestors().First(p => p is JObject && ((JObject)p).ContainsKey("isWinner"))
                    })
                    .GroupBy(r => r[0])
                    .Select(g => new Dictionary<string, object>{
                        { "gamemode", gamemode },
                        { "heroname", g.Key },
                        { "picks", g.Count() },
                        { "wins", g.Count(r => Convert.ToBoolean(((JObject)r[1])["isWinner"])) }
                    });

                string query =
                    "insert into redux_heroes (gamemode, heroname, picks, wins) " +
                    " VALUES (@gamemode, @heroname, @picks, @wins)" +
                    " on conflict (gamemode, heroname, timestamp) do update" +
                    " set picks = redux_heroes.picks + @picks, wins = redux_heroes.wins + @wins";

                db.Execute(query, heroes);
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при обновлении статистики способностей: {ex}", TraceMessageKind.Error);
            }
        }

        #endregion Глобальная статистика

        #region Статистика по игроку

        private string UpdateAbilities(string abilitiesStr, JToken matchAbilities)
        {
            try
            {
                JObject abilities = abilitiesStr == null
                    ? new JObject()
                    : JObject.Parse(abilitiesStr);

                // Пики
                foreach (JToken matchAbility in matchAbilities.SelectTokens("picks[*]"))
                {
                    string abilityName = matchAbility.ToString();
                    if (!abilities.ContainsKey(abilityName))
                        abilities.Add(abilityName, new JObject());

                    if (!((JObject)abilities[abilityName]).ContainsKey("picks"))
                        ((JObject)abilities[abilityName]).Add("picks", 0);

                    abilities[abilityName]["picks"] = Convert.ToInt32(abilities[abilityName]["picks"]) + 1;
                }

                // Баны
                foreach (JToken matchAbility in matchAbilities.SelectTokens("bans[*]"))
                {
                    string abilityName = matchAbility.ToString();
                    if (!abilities.ContainsKey(abilityName))
                        abilities.Add(abilityName, new JObject());

                    if (!((JObject)abilities[abilityName]).ContainsKey("bans"))
                        ((JObject)abilities[abilityName]).Add("bans", 0);

                    abilities[abilityName]["bans"] = Convert.ToInt32(abilities[abilityName]["bans"]) + 1;
                }

                return JsonConvert.SerializeObject(abilities);
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при обновлении данных о способностях пользователя: {ex}", TraceMessageKind.Error);
                return abilitiesStr;
            }
        }

        private string UpdateHeroes(string heroesStr, string gamemode, string hero, bool isWinner)
        {
            try
            {
                JObject heroes = heroesStr == null
                    ? new JObject()
                    : JObject.Parse(heroesStr);

                if (!heroes.ContainsKey(hero))
                    heroes.Add(hero, new JObject());

                if (!((JObject) heroes[hero]).ContainsKey(gamemode))
                    ((JObject) heroes[hero]).Add(gamemode, new JObject { { "matches", 0 }, { "wins", 0 } });

                heroes[hero][gamemode]["matches"] = Convert.ToInt32(heroes[hero][gamemode]["matches"]) + 1;
                if (isWinner)
                    heroes[hero][gamemode]["wins"] = Convert.ToInt32(heroes[hero][gamemode]["wins"]) + 1;

                return JsonConvert.SerializeObject(heroes);
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при обновлении данных о героях пользователя: {ex}", TraceMessageKind.Error);
                return heroesStr;
            }
        }

        private string UpdateGamemodes(string gamemodeStr, string gamemode, bool isWinner)
        {
            try
            {
                JObject gamemodes = gamemodeStr == null
                    ? new JObject()
                    : JObject.Parse(gamemodeStr);

                if (!gamemodes.ContainsKey(gamemode))
                {
                    gamemodes.Add(gamemode, new JObject());
                    ((JObject)gamemodes[gamemode]).Add("matches", 0);
                    ((JObject)gamemodes[gamemode]).Add("wins", 0);
                }

                gamemodes[gamemode]["matches"] = Convert.ToInt32(gamemodes[gamemode]["matches"]) + 1;
                if (isWinner)
                    gamemodes[gamemode]["wins"] = Convert.ToInt32(gamemodes[gamemode]["wins"]) + 1;

                return JsonConvert.SerializeObject(gamemodes);
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при обновлении данных о режимах пользователя: {ex}", TraceMessageKind.Error);
                return gamemodeStr;
            }
        }

        private void SavePlayer(JToken data, string gamemode, bool isWinner)
        {
            try
            {
                string query = 
                    "select * from redux_players" + 
                    $" where steamid = '{data["steamId"]}'";
                dynamic playerData = (IDictionary<string, object>)db.Query(query).Single();

                string abilities = UpdateAbilities(playerData.abilities, data["abilities"]);
                string heroes = UpdateHeroes(playerData.heroes, gamemode, data["hero"].ToString(), isWinner);
                string gamemodes = UpdateGamemodes(playerData.gamemodes, gamemode, isWinner);

                query =
                    "update redux_players " +
                    $" set abilities = '{abilities}', gamemodes = '{gamemodes}', heroes = '{heroes}'" +
                    $" where steamid = '{data["steamId"]}'";

                db.Execute(query);
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при сохранении данных пользователя: {ex}", TraceMessageKind.Error);
            }
        }

        #endregion Статистика по игроку

        #endregion Вспомогательные функции

        #region Основные функции

        public int GetMatchesCount(string gamemode)
        {
            string query =
                "select count(*)" +
                " from redux_matches" +
                (string.IsNullOrEmpty(gamemode)
                    ? string.Empty
                    : $" where gamemode in ({ string.Join(",", gamemode.Split(',').Select(s => "'" + s + "'")) })");

            var result = db.Query(query);

            return result == null ? 0 : (int)result.Single().count;
        }

        /// <summary>
        /// Постраничное формирование списка матчей
        /// </summary>
        public dynamic GetMatches(int page, int count, string gamemode)
        {
            return db.Query(
                "select matchid, duration, gamemode, timestamp" +
                " from redux_matches" +
                (string.IsNullOrEmpty(gamemode)
                    ? string.Empty
                    : $" where gamemode in ({ string.Join(",", gamemode.Split(',').Select(s => "'" + s + "'")) })") + 
                " order by matchid desc" +
                $" limit {count} offset {(page - 1) * count}");
        }

        /// <summary>
        /// Полная информация о матче
        /// </summary>
        public dynamic GetMatch(decimal matchID)
        {
            return db.Query(
                "select *" +
                " from redux_matches" +
                $" where matchid = {matchID}").Single();
        }

        public void Save(JToken matchInfo)
        {
            //var a = db.Query("get_redux_abilities", new { fromdate = 20180706, todate = 20180630 }, CommandType.StoredProcedure);

            try
            {
                db.BeginTransaction();
                
                string gamemode = matchInfo["gamemode"].ToString();

                // Сохранение информации о матче
                string query =
                    "insert into redux_matches" +
                    " (matchid, gamemode, map, duration, teams, gameoptions)" +
                    $" values ({matchInfo["matchId"]}, '{gamemode}', '{matchInfo["map"]}', {matchInfo["duration"]}," +
                    $" '{JsonConvert.SerializeObject(matchInfo["teams"])}', '{JsonConvert.SerializeObject(matchInfo["gameOptions"])}')";

                db.Execute(query);
                
                // Обновление данных по каждому игроку
                foreach (JToken team in matchInfo.SelectTokens("teams[*]"))
                {
                    bool isWinner = Convert.ToBoolean(team["isWinner"]);
                    foreach (JToken player in team.SelectTokens("players[?(@.isAbandoned != true)]"))
                        SavePlayer(player, gamemode, isWinner);
                }

                // Обновление глобальной статистики
                UpdateGlobalAbilities(matchInfo, gamemode);
                UpdateGlobalHeroes(matchInfo, gamemode);
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при сохранении данных матча: {ex}", TraceMessageKind.Error);
            }
            finally
            {
                db.Commit();
            }
        }

        public ReduxMatch(Database database)
        {
            db = database;
        }

        #endregion Основные функции
    }
}
