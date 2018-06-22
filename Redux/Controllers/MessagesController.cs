using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json.Linq;

namespace Redux.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        // GET api/messages
        [HttpGet]
        public object GetMessages(int page = 1, int pageSize = 50)
        {
            int total = (int) Program.Control.Messages.Count;
            return new Dictionary<string, object> {
                { "total", total},
                { "page", page },
                { "pageCount", total / pageSize + 1 },
                { "pageSize", pageSize },
                { "rows", Program.Control.Messages.GetMessages(page, pageSize) }
            };
        }

        // GET api/messages/{SteamID list}
        [HttpGet("{ids}")]
        [Authorize(Roles = "Game")]
        public object GetPlayersMessages(string ids)
        {
            try
            {
                List<decimal> idList = ids.Split(',').Select(i => Convert.ToDecimal(i)).ToList();

                return Program.Control.Messages.GetPlayersMessages(idList);
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка фомировании списка SteamID: {ex}", TraceMessageKind.Error);
            }

            return null;
        }

        // POST api/messages
        [HttpPost]
        [Authorize(Roles = "Game")]
        public void PostMessage()
        {
            try
            {
                StreamReader reader = new StreamReader(Request.Body);
                JObject obj = JObject.Parse(reader.ReadToEnd());

                Program.Control.Messages.PostMessage(obj);
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при добавления сообщения: {ex}", TraceMessageKind.Error);
            }
        }

        // PUT api/messages
        [HttpPut]
        [Authorize(Roles = "Admin, Game")]
        public void ChangeMessage()
        {
            try
            {
                StreamReader reader = new StreamReader(Request.Body);
                JObject obj = JObject.Parse(reader.ReadToEnd());

                Program.Control.Messages.ChangeMessage(obj);
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при изменении сообщения: {ex}", TraceMessageKind.Error);
            }
        }

        // DELETE api/messages
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public void DeleteMessage()
        {
            try
            {
                StreamReader reader = new StreamReader(Request.Body);
                JObject obj = JObject.Parse(reader.ReadToEnd());

                Program.Control.Messages.DeleteMessages(obj);
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при удалении сообщения: {ex}", TraceMessageKind.Error);
            }
        }
    }
}