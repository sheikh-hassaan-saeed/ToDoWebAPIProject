using Microsoft.EntityFrameworkCore;
using ToDoWebAPIProject.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>  // ✅ Changed 'Options' to 'options' (convention, but works either way)
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();  // ✅ Added missing semicolon here!
    });
});

builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserCon")));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseCors("AllowReactApp");  // ✅ This is in the right place!

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();