using BasicApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasicAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BasicAPI.Controllers
{
    [Route("/")]
    [ApiController]
    public class YourEntitiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public YourEntitiesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("shit")]
        public async Task<ActionResult<IEnumerable<Entity>>> GetEntities()
        {
            return await _context.Entities.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Entity>> GetEntity(int id)
        {
            var entity = await _context.Entities.FindAsync(id);
            if (entity == null) return NotFound();
            return entity;
        }

        [HttpPost]
        public async Task<ActionResult<Entity>> PostEntity(Entity entity)
        {
            _context.Entities.Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEntity), new { id = entity.Id }, entity);
        }
        
        [HttpGet("helloworld")]
        public Microsoft.AspNetCore.Mvc.OkObjectResult GetHelloWorld([FromQuery] string thingy)
        {
            return Ok("Hello World, " + thingy);
        }
        
    }
}