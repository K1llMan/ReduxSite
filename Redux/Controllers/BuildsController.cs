using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

namespace Redux.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BuildsController : Controller
    {
        // GET api/builds
        [HttpGet]
        public object GetBuilds()
        {
            return null;
        }

        // GET api/builds/{steamID}
        [HttpGet("{steamID}")]
        public object GetBuildsFavBuilds(string steamID)
        {
            return null;
        }

    }
}