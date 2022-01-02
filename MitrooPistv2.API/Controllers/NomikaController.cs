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
using Microsoft.Extensions.Logging;
using NLog;

namespace MitrooPistv2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NomikaController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<NomikaController> _logger;

        public NomikaController(IConfiguration config, ILogger<NomikaController> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "NLog injected into Nomika Controller");
        }

        [HttpGet("{id}"), AllowAnonymous]
        public ActionResult<tblNomika> Get(long id)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            tblNomika obj;
            using (tblNomikaDac dac = new tblNomikaDac(connStr, _logger))
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
            using (tblNomikaDac dac = new tblNomikaDac(connStr, _logger))
            {
                try
                {
                    nomikaList = dac.GetAll();
                    nomikaList.Shuffle();
                    if (nomikaList == null)
                    {
                        _logger.LogTrace(1, "Not Found");
                        return NotFound();
                    }
                    _logger.LogTrace(1, "No of Items:" + nomikaList.Count().ToString());
                    nomikaList.Shuffle();
                    return Ok(nomikaList);
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                    return BadRequest(ex.Message);
                }
            }
        }


        [HttpPost, Authorize]
        public IActionResult Post([FromBody] tblNomika obj)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            using (tblNomikaDac dac = new tblNomikaDac(connStr, _logger))
            {
                try
                {
                    long? id = dac.Insert(obj);
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
            using (tblNomikaDac dac = new tblNomikaDac(connStr, _logger))
            {
                try
                {
                    bool isSuccess = dac.Update(obj);
                    if (isSuccess)
                        return Ok();
                    else
                        return BadRequest();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        // DELETE: api/
        [HttpDelete("{id}"), Authorize]
        public IActionResult Delete(int id)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            using (tblNomikaDac dac = new tblNomikaDac(connStr, _logger))
            {
                try
                {
                    bool isSuccess = dac.Delete(id);
                    if (isSuccess)
                        return Ok();
                    else
                        return BadRequest();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
