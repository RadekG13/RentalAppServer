using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentalApp3.Services;
using RentalApp3.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RentalApp3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private IUserService _userService;
       

        public AuthController(IUserService userService)
        {

            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model);
                if (result.IsSuccess)
                {
                   // return JsonConvert.SerializeObject("ok");
                    return Ok(result);
                }
                return BadRequest(result);
            }

            return BadRequest(new { message = "bad request" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(model);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);

            }

            return BadRequest("Properties are not valid");


        }


        [HttpGet("secret")]
        [Authorize]
        public string Get()
        {
            string userid = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string secret= "This is a secret: " + userid;

            return JsonConvert.SerializeObject(secret);
        }

    }
}
