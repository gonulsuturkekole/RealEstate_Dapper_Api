using Dapper;
using Microsoft.AspNetCore.Mvc;
using RealEstate_Dapper_Api.Models;
using RealEstate_Dapper_Api.Models.DapperContext;
using RealEstate_Dapper_Api.Repositories.ProductRepository;

namespace RealEstate_Dapper_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly Context _context;

        public CitiesController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ProductList()
        {
            using (var connection = _context.CreateConnection())
            {;
                var result = await connection.QueryAsync<City>("SELECT * FROM Cities");
                return Ok(result);
            }
        }
    }
}
