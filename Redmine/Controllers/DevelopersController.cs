using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Redmine; 

namespace Redmine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevelopersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DevelopersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Developers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Developer>>> GetDevelopers()
        {
            var developers = await _context.Developers.ToListAsync();
            return Ok(developers);
        }

    }
}