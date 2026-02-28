using Microsoft.EntityFrameworkCore;
using ToDoWebAPIProject.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>  
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();  
    });
});

builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserCon")));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

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