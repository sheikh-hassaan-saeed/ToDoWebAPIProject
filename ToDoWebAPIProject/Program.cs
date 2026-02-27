using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ToDoWebAPIProject.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(Options =>
{

    Options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
    });

});

builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserCon")));


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();  // 👈 This should come AFTER all services are registered

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}
app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();