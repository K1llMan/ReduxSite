using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json.Linq;

namespace Redux.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MatchController : Controller
    {
        // GET api/modules
        /// <summary>
        /// Возвращает доступные для отображения модули
        /// </summary>
        [HttpPost]
        public void SaveMatchInfo()
        {
            FileStream fs = null;
            StreamReader sr = null;
            try
            {
                fs = new FileStream("matchData.json", FileMode.Open);
                sr = new StreamReader(fs);
                JToken data = JObject.Parse(sr.ReadToEnd());
                Program.Control.Matches.Save(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                fs?.Close();
                sr?.Close();
            }

        }
    }
}