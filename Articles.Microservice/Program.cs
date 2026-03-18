using Microsoft.EntityFrameworkCore;
using Articles.Microservice.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Articles.Microservice.Data.ArticlesContext>(options =>
    options.UseSqlite("Data Source=articles.db"));

builder.Services.AddScoped<Articles.Microservice.Repositories.IArticleRepository, Articles.Microservice.Repositories.ArticleRepository>();

builder.Services.AddHttpClient<AuthorIntegrationService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7022");
});

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