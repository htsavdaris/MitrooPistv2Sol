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
            using (tblFysikaDac dac = new tblFysikaDac(connStr,_logger))
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
            _logger.LogTrace(1, "Get All is called");
            string connStr = configuration.GetConnectionString("DefaultConnection");            
            List<tblFysika> fysikaList;
            using (tblFysikaDac dac = new tblFysikaDac(connStr, _logger))
            {
                try
                {
                    fysikaList = dac.GetAll();
                    _logger.LogTrace(1, "DAC GetAll Called");
                    if (fysikaList == null)
                    {
                        _logger.LogTrace(1, "Not Found");
                        return NotFound();
                    }
                    _logger.LogTrace(1, "No of Items:" + fysikaList.Count().ToString());
                    fysikaList.Shuffle();
                    return Ok(fysikaList);
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                    return BadRequest(ex.Message);
                }
            }
        }


        [HttpPost, Authorize]
        public IActionResult Post([FromBody] tblFysika obj)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            using (tblFysikaDac dac = new tblFysikaDac(connStr,_logger))
            {
                try
                {
                    long? id = dac.Insert(obj);
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

        [HttpPut("{id}"), Authorize]
        public IActionResult Put(int id, [FromBody] tblFysika obj)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            using (tblFysikaDac dac = new tblFysikaDac(connStr,_logger))
            {
                try
                {
                    bool isSuccess = dac.Update(obj);
                    if ( isSuccess)
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

        // DELETE: api/
        [HttpDelete("{id}"), Authorize]
        public IActionResult Delete(int id)
        {
            string connStr = configuration.GetConnectionString("DefaultConnection");
            using (tblFysikaDac dac = new tblFysikaDac(connStr, _logger))
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

