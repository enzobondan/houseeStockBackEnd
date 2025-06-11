using api_stock.Data;
using api_stock.Interfaces;
using api_stock.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using api_stock.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<PlaceInterface, PlaceRepository>();
builder.Services.AddScoped<ItemInterface, ItemRepository>();
builder.Services.AddScoped<ContainerInterface, ContainerRepository>();
builder.Services.AddScoped<TagInterface, TagRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();



app.MapControllers();


app.Run();