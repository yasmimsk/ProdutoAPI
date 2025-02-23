using Microsoft.EntityFrameworkCore;
using ProdutoAPI.Database;
using ProdutoAPI.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProdutoDbContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ProdutoDbContext>();

    //Popular base
    if (!context.Produtos.Any())
    {
        context.Produtos.AddRange(
            new Produto { Nome = "Mouse", Estoque = 500, Valor = 111.75m },
            new Produto { Nome = "PlayStation 5", Estoque = 30, Valor = 3539.9m },
            new Produto { Nome = "Controle sem fio", Estoque = 122, Valor = 334.89m },
            new Produto { Nome = "Base de carregamento", Estoque = 48, Valor = 179 },
            new Produto { Nome = "Fone de ouvido", Estoque = 200, Valor = 109.9m }
        );

        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
