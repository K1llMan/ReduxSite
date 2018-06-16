using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Mvc;

namespace Redux.Controllers
{
    [Produces("application/json")]
    [Route("api/token")]
    public class TokenController : Controller
    {
        [HttpPost]
        public IActionResult Create(string username, string password)
        {
            JwtSecurityToken token = Program.Control.JWT.GenerateToken(username, password);
            if (token != null)
                return new ObjectResult(new JwtSecurityTokenHandler().WriteToken(token));
            return BadRequest();
        }
    }
}