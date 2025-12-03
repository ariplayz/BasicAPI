using BasicApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasicAPI.Models;

namespace BasicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YourEntitiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public YourEntitiesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
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
        
    }
}