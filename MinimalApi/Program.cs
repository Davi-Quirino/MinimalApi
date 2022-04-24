using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Clients"));


var app = builder.Build();
app.UseSwagger();

app.MapGet("/Clients", async (AppDbContext dbContext) => await dbContext.Clients.ToListAsync());

app.MapGet("/Clients/{id}", async (int id, AppDbContext dbContext) => await dbContext.Clients.FirstOrDefaultAsync(a => a.Id == id));

app.MapPost("/Clients", async (Client client, AppDbContext dbContext) =>
    {
        dbContext.Clients.Add(client);
        await dbContext.SaveChangesAsync();

        return client;
    });

app.MapPut("/Clients/{id}", async (int id, Client client, AppDbContext dbContext) =>
{
    dbContext.Entry(client).State = EntityState.Modified;
    await dbContext.SaveChangesAsync();

    return client;
});

app.MapDelete("/Clients/{id}", async (int id, AppDbContext dbContext) =>
{
    var client = await dbContext.Clients.FirstOrDefaultAsync(a => a.Id == id);
    
    if(client != null)
    {
        dbContext.Clients.Remove(client);
        await dbContext.SaveChangesAsync();
    }
    return;
});

app.UseSwaggerUI();

app.Run();


public class Client
{
    public int Id { get; set; }

    public string? Nome { get; set; }
}

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
}