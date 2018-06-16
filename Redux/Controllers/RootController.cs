using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

namespace Redux.Controllers
{
    [Produces("application/json")]
    public class RootController : Controller
    {
        // GET api/modules
        /// <summary>
        /// Возвращает доступные для отображения модули
        /// </summary>
        [HttpGet("api/modules")]
        public object GetModules()
        {
            Dictionary<string, Dictionary<string, string>> modules = new Dictionary<string, Dictionary<string, string>> {
                { "#messages", new Dictionary<string, string> {
                        { "displayName", "Messages" },
                        { "script", "messages.js" },
                        { "template", "messages.tmp" }
                    }
                },
                { "#abilities", new Dictionary<string, string> {
                        { "displayName", "Abilities" },
                        { "script", "abilities.js" },
                        { "template", "abilities.tmp" }
                    }
                },
                { "#heroes", new Dictionary<string, string> {
                        { "displayName", "Heroes" },
                        { "script", "heroes.js" },
                        { "template", "heroes.tmp" }
                    }
                }
            };

            // Администратору видна страничка загрузки билдов
            if (User.IsInRole("Admin"))
                modules.Add("#builds", new Dictionary<string, string> {
                        {"displayName", "Builds"},
                        {"script", "builds.js"},
                        {"template", "builds.tmp"}
                    }
                );

            return modules;
        }
    }
}