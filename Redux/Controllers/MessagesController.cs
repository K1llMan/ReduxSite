using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

namespace Redux.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        // GET api/messages
        [HttpGet("")]
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
    }
}