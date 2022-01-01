using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MitrooPistV2.Data;
using MitrooPistv2.API.Models;
using Microsoft.Extensions.Logging;
using NLog;

namespace MitrooPistv2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IConfiguration configuration;
        private readonly ILogger<UserController> _logger;
        public UserController(IConfiguration config)
        {
            this.configuration = config;
        }

        [HttpGet("{id}"), AllowAnonymous]
        public ActionResult<tblUser> Get(long id)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            tblUser obj;
            using (tblUserDac dac = new tblUserDac(connStr, _logger))
            {
                obj = dac.Get(id);
                if (obj != null)
                    return Ok(obj);
                else
                    return NotFound();
            }
        }

        [HttpGet, AllowAnonymous]
        public ActionResult<List<tblUser>> Get()
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            List<tblUser> userList;
            using (tblUserDac dac = new tblUserDac(connStr, _logger))
            {
                userList = dac.GetAll();
                userList.Shuffle();
                if (userList == null)
                {
                    return NotFound();
                }
                return Ok(userList);
            }
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        [EnableCors("MyPolicy")]
        public IActionResult Authenticate([FromBody] UserAuth userDto)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            tblUser user;
            using (tblUserDac dac = new tblUserDac(connStr, _logger))
            {
                user = dac.Authenticate(userDto.login, userDto.password);
            }

            if (user == null)
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Username or password is incorrect" });

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345!"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.flduserid.ToString()),
                new Claim(ClaimTypes.Role, "Manager")
            };

            var tokeOptions = new JwtSecurityToken(
                issuer: "http://localhost:5000",
                audience: "http://localhost:5000",
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return Ok(new
            {
                id = user.flduserid,
                login = user.fldlogin,
                token = tokenString
            });
        }

        [Authorize]
        [HttpPost("changepassword")]
        public IActionResult ChangePassword([FromBody] ChangePass changePass)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            bool res;
            using (tblUserDac dac = new tblUserDac(connStr, _logger))
            {
                res = dac.ChangePassword(changePass.login, changePass.oldpassword, changePass.newpassword);
            }
            if (res)
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "Password Change Failed" });
            }

        }

    }

    
}
