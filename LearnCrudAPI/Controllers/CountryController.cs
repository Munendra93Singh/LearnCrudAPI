using LearnCrudAPI.Repos;
using LearnCrudAPI.Repos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LearnCrudAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly LearndataContext learndata;

        public CountryController(LearndataContext learndata)
        {
            this.learndata = learndata;
        }
        [HttpDelete("Getall")]
        public async Task <ActionResult>GetAll()
        {
             string sqlquery = "exec sp_stateDistTah_List";
            var data = await this.learndata.TblCountrys.FromSqlRaw(sqlquery).ToListAsync();
            if(data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }
    }
}
