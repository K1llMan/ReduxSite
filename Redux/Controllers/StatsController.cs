using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

namespace Redux.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StatsController : Controller
    {
        // GET api/stats/heroes
        [HttpGet("heroes")]
        public object GetHeroes()
        {
            return null;
        }

        // GET api/stats/abilities
        [HttpGet("abilities")]
        public object GetAbilities()
        {
            return null;
        }
    }
}