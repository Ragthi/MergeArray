using MergeArraysApi.Data;
using MergeArraysApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// DB (SQLite)
builder.Services.AddDbContext<MergeDbContext>(opt =>
{
    var conn = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=Data/merge.db";
    opt.UseSqlite(conn);
});
builder.Services.AddScoped<IArrayMergeService, ArrayMergeService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ensure DB exists
Directory.CreateDirectory("Data");
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MergeDbContext>();
    db.Database.EnsureCreated();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
