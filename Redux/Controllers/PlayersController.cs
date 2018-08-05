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
        // GET api/players/{steamId}/fields
        [HttpGet("{steamId}/fields")]
        public object GetField(string steamId, string fields)
        {
            return Program.Control.Players.GetFields(steamId, fields?.Split(','));
        }

        // POST api/players/{steamId}/{field}
        [HttpPost("{steamId}/{field}")]
        [Authorize(Roles = "Game")]
        public void SetField(string steamId, string field)
        {
            try
            {
                StreamReader reader = new StreamReader(Request.Body);
                JObject data = JObject.Parse(reader.ReadToEnd());

                Program.Control.Players.SetField(steamId, field, data);
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при сохранении поля {field} игрока {steamId}: {ex}", TraceMessageKind.Error);
            }
        }

        // GET api/players
        [HttpGet]
        public object GetPlayersInfo(string ids, string fields)
        {
            try
            {
                return Program.Control.Players.GetPlayersInfo(ids?.Split(','), fields?.Split(','));
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при получении данных игроков {ids.Split(',')}: {ex}", TraceMessageKind.Error);
            }

            return null;
        }

        // POST api/players/roles
        [HttpPost("roles")]
        public object SetRoles()
        {
            try
            {
                StreamReader reader = new StreamReader(Request.Body);
                JObject data = JObject.Parse(reader.ReadToEnd());

                Program.Control.Players.SetRoles(data);
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при обновлении ролей: {ex}", TraceMessageKind.Error);
            }

            return null;
        }
    }
}