using System;
using System.Collections.Generic;
using System.IO;

using Common;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json.Linq;

namespace Redux.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BuildsController : Controller
    {
        // GET api/builds
        [HttpGet]
        //[Authorize(Roles = "User")]
        public object GetBuilds(int page = 1, int pageSize = 100, string sort = "rating")
        {
            int total = (int)Program.Control.Builds.Count;
            dynamic rows = Program.Control.Builds.GetBuilds(page, pageSize, sort);

            return new Dictionary<string, object> {
                { "total", total },
                { "page", page },
                { "pageCount", total / pageSize + 1 },
                { "pageSize", pageSize },
                { "rows", rows }
            };
        }

        // POST api/builds
        [HttpPost]
        //[Authorize(Roles = "User")]
        public object Submit()
        {
            try
            {
                StreamReader reader = new StreamReader(Request.Body);
                JObject obj = JObject.Parse(reader.ReadToEnd());
                Program.Control.Builds.Submit(obj);
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при добавлении билда: {ex}", TraceMessageKind.Error);
            }

            return null;
        }

        // POST api/builds/vote
        [HttpPost("vote")]
        //[Authorize(Roles = "User")]
        public void Vote()
        {
            try
            {
                StreamReader reader = new StreamReader(Request.Body);
                JObject obj = JObject.Parse(reader.ReadToEnd());

                Program.Control.Builds.Vote(obj);
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при голосовании за билд: {ex}", TraceMessageKind.Error);
            }
        }

        // POST api/builds/favorite
        [HttpPost("favorite")]
        //[Authorize(Roles = "User")]
        public void AddToFavorite()
        {
            try
            {
                StreamReader reader = new StreamReader(Request.Body);
                JObject obj = JObject.Parse(reader.ReadToEnd());

                Program.Control.Builds.Favorites(obj);
            }
            catch (Exception ex)
            {
                Logger.WriteToTrace($"Ошибка при добавлении в избранное: {ex}", TraceMessageKind.Error);
            }
        }
    }
}