using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using BrainDump.Models;
using BrainDump.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrainDump.Controllers {
    [Produces("application/json")]
    public class AuthController : Controller {
        private readonly AuthorizationManager _authManager;

        public AuthController(AuthorizationManager authManager) {
            _authManager = authManager;
        }

        [HttpPost]
        [Route("auth/login")]
        public IActionResult Login([FromBody]TokenRequest request) {
            if (request == null) {
                return BadRequest();
            }

            JwtSecurityToken token;
            try {
                token = _authManager.Login(request.UserName, request.Password);
            } catch (InvalidCredentialsException) {
                return Unauthorized();
            }

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        [HttpPost]
        [Route("auth/createuser")]
        public IActionResult CreateUser([FromBody]NewUserRequest request) {
            if (request == null) {
                return BadRequest();
            }

            try {
                var token = _authManager.CreateUser(request.UserName, request.Password);
                return Ok(new {token = new JwtSecurityTokenHandler().WriteToken(token)});
            } catch (DuplicateUserException) {
                return BadRequest("User already exists");
            }
        }
    }
}