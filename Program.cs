using System.Configuration;
using BasicApi.Data;
using Microsoft.EntityFrameworkCore;
using BasicAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21))));

builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.Urls.Add("http://localhost:8080");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.MapGet("/GetEntities", async (AppDbContext db) =>
{
    return await db.Entities.ToListAsync();
});

app.MapGet("/GetEntity/{id}", async (int id, AppDbContext db) =>
{
    var entity = await db.Entities.FindAsync(id);
    return entity is not null ? Results.Ok(entity) : Results.NotFound();
});

app.MapPost("/NewEntity", async (Entity entity, AppDbContext db) =>
{
    db.Entities.Add(entity);
    await db.SaveChangesAsync();
    return Results.Created($"/GetEntity/{entity.Id}", entity);
});

app.MapPut("/EditEntity/{id}", async (int id, Entity updatedEntity, AppDbContext db) =>
{
    var entity = await db.Entities.FindAsync(id);
    if (entity is null) return Results.NotFound();

    entity.Name = updatedEntity.Name; // Update properties as needed
    // Update other properties here...

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/DeleteEntity/{id}", async (int id, AppDbContext db) =>
{
    var entity = await db.Entities.FindAsync(id);
    if (entity is null) return Results.NotFound();

    db.Entities.Remove(entity);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapControllers();

app.Run();