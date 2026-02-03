
using apidemo.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ClientContext>(options =>
    options.UseInMemoryDatabase("ClientDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ClientContext>();
    await db.Database.EnsureCreatedAsync();
}

app.MapGet("/clients", async (
    int? offset,
    int? limit,
    ClientContext _context) =>
{
    var skip = offset ?? 0;
    var take = limit ?? 10;

    if (skip < 0) skip = 0;
    if (take < 1) take = 10;
    if (take > 100) take = 100; // Max limit

    var totalCount = await _context.Clients.CountAsync();

    var clients = await _context.Clients
        .Skip(skip)
        .Take(take)
        .ToListAsync();

    return Results.Ok(new
    {
        data = clients,
        pagination = new
        {
            offset = skip,
            limit = take,
            totalCount
        }
    });
});

app.MapGet("/clients/{client_id}", async (
    int client_id,
    int? fund_id,
    DateTime? start_date,
    DateTime? end_date,
    ClientContext _context) =>
{
    var query = _context.Clients.Where(c => c.client_id == client_id);

    if (fund_id.HasValue)
    {
        query = query.Where(c => c.fund_id == fund_id.Value);
    }

    if (start_date.HasValue)
    {
        query = query.Where(c => c.as_of_date >= start_date.Value);
    }

    if (end_date.HasValue)
    {
        query = query.Where(c => c.as_of_date <= end_date.Value);
    }

    var results = await query.ToListAsync();

    if (!results.Any())
    {
        return Results.NotFound();
    }

    return Results.Ok(results);
});

app.MapGet("/summary/{client_id}", async (
    int client_id,
    int? fund_id,
    DateTime? start_date,
    DateTime? end_date,
    ClientContext _context) =>
{
    var query = _context.Clients.Where(c => c.client_id == client_id);

    if (fund_id.HasValue)
    {
        query = query.Where(c => c.fund_id == fund_id.Value);
    }

    if (start_date.HasValue)
    {
        query = query.Where(c => c.as_of_date >= start_date.Value);
    }

    if (end_date.HasValue)
    {
        query = query.Where(c => c.as_of_date <= end_date.Value);
    }

    var data = await query.ToListAsync();

    if (!data.Any())
    {
        return Results.NotFound();
    }

    var summary = new
    {
        client_id,
        count = data.Count,
        sum = data.Sum(c => c.metric_value),
        byMetric = data
            .GroupBy(c => c.metric_name)
            .Select(g => new
            {
                metric_name = g.Key,
                count = g.Count(),
                sum = g.Sum(c => c.metric_value)
            })
            .ToList()
    };

    return Results.Ok(summary);
});

app.Run();
