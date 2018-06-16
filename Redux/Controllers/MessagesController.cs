using System.Collections.Generic;
using System.IO;

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
        public object GetMessages(int page = 0, int count = 10)
        {
            return new List<Dictionary<string, object>> {
                new Dictionary<string, object>{
                    { "ID", 0 },
                    {"Comment", "test"},
                    {"SteamID", 99349097012},
                    {"Nickname", "TestNick"},
                    {"Reply", "test"},
            }};
        }

        // POST api/messages
        [HttpPost]
        [Authorize(Roles = "Game")]
        public void PostMessage()
        {
        }

        // PUT api/messages
        [HttpPut]
        [Authorize(Roles = "Admin, Game")]
        public void ChangeMessage()
        {
            StreamReader reader = new StreamReader(Request.Body);
            JObject obj = JObject.Parse(reader.ReadToEnd());
        }

        // DELETE api/messages
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public void DeleteMessage()
        {
        }
    }
}