using System;
using System.Collections.Generic;
using System.IO;

using Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json.Linq;

namespace Redux.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MatchesController : Controller
    {
        // GET api/matches
        [HttpGet]
        public object GetMatches(int page = 1, int pageSize = 50, string gamemode = "")
        {
            int total = Program.Control.Matches.GetMatchesCount(gamemode);
            return new Dictionary<string, object> {
                { "total", total},
                { "page", page },
                { "pageCount", total / pageSize + 1 },
                { "pageSize", pageSize },
                { "rows", Program.Control.Matches.GetMatches(page, pageSize, gamemode) }
            };
        }

        // GET api/matches/{matchID}
        [HttpGet("{matchID}")]
        public object GetMatch(decimal matchID)
        {
            dynamic row = Program.Control.Matches.GetMatch(matchID);
            if (row == null)
                return row;

            row.teams = JArray.Parse(row.teams);
            row.gameoptions = JObject.Parse(row.gameoptions);

            return row;
        }

        // POST api/matches
        [HttpPost]
        [Authorize(Roles = "Game")]
        public void SaveMatchInfo()
        {
            try
            {
                StreamReader reader = new StreamReader(Request.Body);
                JObject obj = JObject.Parse(reader.ReadToEnd());

                Program.Control.Matches.Save(obj);
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при обновлении статистики матча: {ex}", TraceMessageKind.Error);
            }
        }
    }
}