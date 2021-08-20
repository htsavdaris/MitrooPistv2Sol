using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MitrooPistV2.Data;

namespace MitrooPistv2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NomikaController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public NomikaController(IConfiguration config)
        {
            this.configuration = config;
        }

        [HttpGet("{id}"), AllowAnonymous]
        public ActionResult<tblNomika> Get(long id)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            tblNomika obj;
            using (tblNomikaDac dac = new tblNomikaDac(connStr))
            {
                try
                {
                    obj = dac.Get(id);
                    if (obj != null)
                        return Ok(obj);
                    else
                        return NotFound();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    return BadRequest(ex.Message);
                }

            }
        }


        [HttpGet, AllowAnonymous]
        public ActionResult<List<tblNomika>> Get()
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            List<tblNomika> nomikaList;
            using (tblNomikaDac dac = new tblNomikaDac(connStr))
            {
                try
                {
                    nomikaList = dac.GetAll();
                    nomikaList.Shuffle();
                    if (nomikaList == null)
                    {
                        return NotFound();
                    }
                    return Ok(nomikaList);
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }


        [HttpPost, Authorize]
        public IActionResult Post([FromBody] tblNomika obj)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            using (tblNomikaDac dac = new tblNomikaDac(connStr))
            {
                try
                {
                    long id = dac.Insert(obj);
                    if (id > 0)
                        return CreatedAtRoute("fysika", new { id = id }, obj);
                    else
                        return Conflict();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut("{id}"), Authorize]
        public IActionResult Put(int id, [FromBody] tblNomika obj)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            using (tblNomikaDac dac = new tblNomikaDac(connStr))
            {
                bool isSuccess = dac.Update(obj);
            }
            return NoContent();
        }

        // DELETE: api/
        [HttpDelete("{id}"), Authorize]
        public void Delete(int id)
        {
        }
    }
}
