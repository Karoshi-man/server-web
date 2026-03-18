using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Підключення бази даних та репозиторію
builder.Services.AddDbContext<Authors.Microservice.Data.AuthorsContext>(options =>
    options.UseSqlite("Data Source=authors.db"));

builder.Services.AddScoped<Authors.Microservice.Repositories.IAuthorRepository, Authors.Microservice.Repositories.AuthorRepository>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();