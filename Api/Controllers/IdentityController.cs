using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers {
    [Route ("identity")]
    public class IdentityController : ControllerBase {

        [Authorize (Roles = "superadmin")]
        [HttpGet]
        public IActionResult Get () {
            Console.WriteLine(">> " + User.FindFirstValue("Username"));
            return new JsonResult (from c in HttpContext.User.Claims select new { c.Type, c.Value });
        }

        [Authorize (Roles = "admin")]
        [Route ("{id}")]
        [HttpGet]
        public string Get (int id) {
            return id.ToString ();
        }
    }
}