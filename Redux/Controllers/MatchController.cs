using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

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

        }
    }
}