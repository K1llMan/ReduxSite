using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

using Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Redux.Controllers
{
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
                RouteAttribute route = (RouteAttribute)type.GetCustomAttributes(typeof(RouteAttribute)).FirstOrDefault();
                string mainRoute = route.Template.Replace("[controller]", type.Name.Replace("Controller", string.Empty).ToLower());

                foreach (MethodInfo method in type.GetMethods())
                {
                    foreach (HttpMethodAttribute attr in method.GetCustomAttributes(typeof(HttpMethodAttribute)))
                    {
                        AuthorizeAttribute authAttr = (AuthorizeAttribute)method.GetCustomAttributes(typeof(AuthorizeAttribute)).FirstOrDefault();

                        List<string> methodDesc = new List<string>{
                            $"{string.Join(", ", attr.HttpMethods)} {string.Join("/", new string[] { mainRoute, attr.Template }.Where(s => !string.IsNullOrEmpty(s)))}" +
                            $"{ (authAttr != null ? $" Auth: {authAttr.Roles} {authAttr.Policy}" : string.Empty) } \n"
                        };
                        
                        foreach (ParameterInfo parameter in method.GetParameters())
                            methodDesc.Add($"\t{parameter.Name}: {parameter.ParameterType.Name} Default: {parameter.RawDefaultValue}\n");

                        apiList.Add(string.Join("", methodDesc));
                    }
                }
            }

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = string.Join("", apiList)
            };
        }
    }
}