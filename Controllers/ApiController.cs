using BasicApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasicAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BasicAPI.Controllers
{
    [Route("/")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ApiController(AppDbContext context)
        {
            _context = context;
        }
       
        [HttpGet("GetEntities")]
        public async Task<ActionResult<IEnumerable<Entity>>> GetEntities()
        {
            return await _context.Entities.ToListAsync();
        }
        
        [HttpGet("GetEntity")]
        public async Task<Microsoft.AspNetCore.Http.IResult> GetEntity([FromQuery] int id)
        {
            var entity = await _context.Entities.FindAsync(id);
            return entity is not null ? Results.Ok(entity) : Results.NotFound();
        }

        [HttpPost("NewEntity")]
        public async Task<Microsoft.AspNetCore.Http.IResult> NewEntity([FromQuery] string Name)
        {
            var entity = new Entity();
            entity.Name = Name;
            _context.Entities.Add(entity);
            await _context.SaveChangesAsync();
            return Results.Created($"/GetEntity/{entity.Id}", entity);  
        }
        
        [HttpPut("EditEntity")]
        public async Task<Microsoft.AspNetCore.Http.IResult> EditEntity([FromQuery] int Id, [FromQuery] string NewName)
        {
            var entity = await _context.Entities.FindAsync(Id);
            if (entity is null) return Results.NotFound();

            entity.Name = NewName;

            await _context.SaveChangesAsync();
            return Results.Ok(entity);
        }

        [HttpDelete("DeleteEntity")]
        public async Task<Microsoft.AspNetCore.Http.IResult> DeleteEntity([FromQuery] int id)
        {
            var entity = await _context.Entities.FindAsync(id);
            if (entity is null) return Results.NotFound();

            _context.Entities.Remove(entity);
            await _context.SaveChangesAsync();
            return Results.Ok($"Deleted {entity.Name} Successfully");
        }
    }
}