using BasicAPI.Models;
using BasicAPI;
using Microsoft.EntityFrameworkCore;

namespace BasicApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Entity> Entities { get; set; } // Define DbSet for your entities
}