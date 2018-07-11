using System;
using System.IO;

using Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json.Linq;

namespace Redux.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PlayersController : Controller
    {
        // GET api/players/fields
        [HttpGet("fields")]
        public object GetField(string steamID, string fields)
        {
            return Program.Control.Players.GetFields(steamID, fields.Split(','));
        }

        // POST api/players/{field}
        [HttpPost("{field}")]
        [Authorize(Roles = "Game")]
        public void SetField(string steamID, string field)
        {
            try
            {
                StreamReader reader = new StreamReader(Request.Body);
                JObject data = JObject.Parse(reader.ReadToEnd());

                Program.Control.Players.SetField(steamID, field, data);
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при сохранении поля {field} игрока {steamID}: {ex}", TraceMessageKind.Error);
            }
        }

        // GET api/players
        [HttpGet]
        public object GetPlayersInfo(string ids)
        {
            try
            {
                return Program.Control.Players.GetPlayersInfo(ids.Split(','));
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при получении данных игроков {ids.Split(',')}: {ex}", TraceMessageKind.Error);
            }

            return null;
        }
    }
}