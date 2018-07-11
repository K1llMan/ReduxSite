using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Common;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Redux.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class RootController : Controller
    {
        // GET api/modules
        /// <summary>
        /// Возвращает доступные для отображения модули
        /// </summary>
        [HttpGet("modules")]
        public object GetModules()
        {
            Dictionary<string, Dictionary<string, string>> modules = new Dictionary<string, Dictionary<string, string>> {
                { "#messages", new Dictionary<string, string> {
                        { "displayName", "Messages" },
                        { "script", "messages.js" },
                        { "template", "messages.tmp" }
                    }
                },
                { "#mathes", new Dictionary<string, string> {
                        { "displayName", "Matches" },
                        { "script", "matches.js" },
                        { "template", "matches.tmp" }
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

        // GET api/methods
        [HttpGet("methods")]
        public object GetMethods()
        {
            List<string> apiList = new List<string>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.GetTypes().Where(t => t.Name.IsMatch("Controller")))
            {
                foreach (Attribute attr in type.GetCustomAttributes())
                {
                    if (attr is RouteAttribute)
                        apiList.Add(((RouteAttribute)attr).Template.Replace("[controller]", type.Name.Replace("Controller", string.Empty).ToLower()));
                    if (attr is HttpMethodAttribute)
                        apiList.Add(((HttpMethodAttribute)attr).Template);
                }

                foreach (MethodInfo method in type.GetMethods())
                {
                    foreach (Attribute attr in method.GetCustomAttributes())
                    {
                        if (attr is HttpMethodAttribute)
                            apiList.Add(((HttpMethodAttribute)attr).Template);
                    }
                }
            }

            return null;
        }
    }
}