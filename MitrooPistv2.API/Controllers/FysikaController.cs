using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class FysikaController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<FysikaController> _logger;

        public FysikaController(IConfiguration config, ILogger<FysikaController> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "NLog injected into FysikaController");
        }

        [HttpGet("{id}"), AllowAnonymous]
        public ActionResult<tblFysika> Get(long id)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            tblFysika obj;
            using (tblFysikaDac dac = new tblFysikaDac(connStr))
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
        public ActionResult<List<tblFysika>> Get()
        {
            _logger.LogError(1, "Get All is called");
            string connStr = configuration.GetConnectionString("DefaultConnection");
            List<tblFysika> fysikaList;
            using (tblFysikaDac dac = new tblFysikaDac(connStr))
            {
                try
                {
                    fysikaList = dac.GetAll();
                    fysikaList.Shuffle();
                    if (fysikaList == null)
                    {
                        return NotFound();
                    }
                    return Ok(fysikaList);
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }


        [HttpPost, AllowAnonymous]
        public IActionResult Post([FromBody] tblFysika obj)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            using (tblFysikaDac dac = new tblFysikaDac(connStr))
            {
                try
                {
                    long id = dac.Insert(obj);
                    if (id > 0)
                        return Ok();
                    else
                        return Conflict();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut("{id}"), AllowAnonymous]
        public IActionResult Put(int id, [FromBody] tblFysika obj)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            using (tblFysikaDac dac = new tblFysikaDac(connStr))
            {
                try
                { 
                    bool isSuccess = dac.Update(obj); 
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return Ok();
        }

        // DELETE: api/
        [HttpDelete("{id}"), Authorize]
        public void Delete(int id)
        {
        }

    }
}
