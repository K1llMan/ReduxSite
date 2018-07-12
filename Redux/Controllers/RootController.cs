using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

using Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

using Newtonsoft.Json.Linq;

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
            JObject apiList = new JObject();
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.GetTypes().Where(t => t.Name.IsMatch("Controller")))
            {
                JArray controllerMethods = new JArray();

                RouteAttribute route = (RouteAttribute)type.GetCustomAttributes(typeof(RouteAttribute)).FirstOrDefault();
                string controllerName = type.Name.Replace("Controller", string.Empty);
                string mainRoute = route.Template.Replace("[controller]", controllerName.ToLower());

                foreach (MethodInfo method in type.GetMethods())
                    foreach (HttpMethodAttribute attr in method.GetCustomAttributes(typeof(HttpMethodAttribute)))
                    {
                        JObject apiMethod = new JObject {
                            { "route", $"{string.Join(", ", attr.HttpMethods)} {string.Join("/", new string[] { mainRoute, attr.Template }.Where(s => !string.IsNullOrEmpty(s)))}" }
                        };

                        AuthorizeAttribute authAttr = (AuthorizeAttribute)method.GetCustomAttributes(typeof(AuthorizeAttribute)).FirstOrDefault();
                        if (authAttr != null)
                            apiMethod.Add("auth", new JObject{
                                { "roles", authAttr.Roles },
                                { "policy", authAttr.Policy }
                            } );

                        ParameterInfo[] parameters = method.GetParameters();
                        if (parameters.Length > 0)
                        {
                            JArray methodParams = new JArray();
                            foreach (ParameterInfo parameter in method.GetParameters())
                                methodParams.Add(new JObject { 
                                    { "name", parameter.Name },
                                    { "type", parameter.ParameterType.Name },
                                    { "default", parameter.RawDefaultValue.ToString() }
                                });
                            apiMethod.Add("params", methodParams);
                        }
                        controllerMethods.Add(apiMethod);
                    }

                apiList.Add(controllerName, controllerMethods);
            }

            return apiList;
        }
    }
}