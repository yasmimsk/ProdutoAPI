using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdutoAPI.Controllers;
using ProdutoAPI.Database;
using ProdutoAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdutoAPI.Tests.Tests
{
    public class ProdutoControllerTests
    {
        private ProdutoDbContext CriarDbContextInMemory()
        {
            var options = new DbContextOptionsBuilder<ProdutoDbContext>()
                .UseInMemoryDatabase(databaseName: "ProdutoDbTeste")
                .Options;
            return new ProdutoDbContext(options);
        }

        [Fact]
        public async Task ObterProdutos_DeveRetornarListaDeProdutosOrdenada()
        {
            var dbContext = CriarDbContextInMemory();

            dbContext.Produtos.AddRange(new List<Produto>
            {
                new Produto { Nome = "Produto 1", Estoque = 11, Valor = 123 },
                new Produto { Nome = "Produto 2", Estoque = 22, Valor = 456 }
            });
            await dbContext.SaveChangesAsync();

            var controller = new ProdutoController(dbContext);
            var resultado = await controller.ObterProdutos("Valor", true);

            Assert.NotNull(resultado.Value);
            Assert.Equal(2, resultado.Value.Count());
            Assert.True(resultado.Value.First().Valor >= resultado.Value.Last().Valor);
        }

        [Fact]
        public async Task ObterProdutos_OrdenarPorPropriedadeInexistente_DeveRetornarBadRequest()
        {
            var dbContext = CriarDbContextInMemory();

            dbContext.Produtos.AddRange(new List<Produto>
            {
                new Produto { Nome = "Produto 1", Estoque = 11, Valor = 123 },
                new Produto { Nome = "Produto 2", Estoque = 22, Valor = 456 }
            });
            await dbContext.SaveChangesAsync();

            var controller = new ProdutoController(dbContext);
            var resultado = await controller.ObterProdutos("Descricao");

            var actionResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.Equal("A propriedade Descricao não existe.", actionResult.Value);
        }

        [Fact]
        public async Task ObterProduto_DeveRetornarProdutoPorId()
        {
            var dbContext = CriarDbContextInMemory();

            Produto produto = new Produto { Nome = "Produto 1", Estoque = 11, Valor = 123 };
            dbContext.Produtos.Add(produto);
            await dbContext.SaveChangesAsync();

            var controller = new ProdutoController(dbContext);
            var resultado = await controller.ObterProduto(produto.Id);

            Assert.Equal("Produto 1", resultado.Value.Nome);
            Assert.Equal(11, resultado.Value.Estoque);
            Assert.Equal(123, resultado.Value.Valor);
        }

        [Fact]
        public async Task ObterProdutoPorNome_DeveRetornarProdutoPorNome()
        {
            var dbContext = CriarDbContextInMemory();

            dbContext.Produtos.AddRange(new List<Produto>
            {
                new Produto { Nome = "Produto 1", Estoque = 11, Valor = 123 },
                new Produto { Nome = "Produto 2", Estoque = 22, Valor = 456 }
            });
            await dbContext.SaveChangesAsync();

            var controller = new ProdutoController(dbContext);
            var resultado = await controller.ObterProdutoPorNome("Produto 2");

            Assert.NotNull(resultado.Value);
            Assert.Single(resultado.Value);
            Assert.Equal("Produto 2", resultado.Value.First().Nome);
        }

        [Fact]
        public async Task CadastrarProduto_DeveCadastrarProduto()
        {
            var dbContext = CriarDbContextInMemory();

            var controller = new ProdutoController(dbContext);

            Produto novoProduto = new Produto { Nome = "Novo produto", Estoque = 1, Valor = 100 };
            var resultado = await controller.CadastrarProduto(novoProduto);

            var actionResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var produto = Assert.IsType<Produto>(actionResult.Value);
            Assert.Equal("Novo produto", produto.Nome);
            Assert.Equal(1, produto.Estoque);
            Assert.Equal(100, produto.Valor);
        }

        [Fact]
        public async Task CadastrarProduto_ValorNegativo_DeveRetornarBadRequest()
        {
            var dbContext = CriarDbContextInMemory();
            var controller = new ProdutoController(dbContext);

            Produto novoProduto = new Produto { Nome = "Novo produto", Estoque = 1, Valor = -100 };
            var resultado = await controller.CadastrarProduto(novoProduto);

            var actionResult = Assert.IsType<BadRequestObjectResult>(resultado.Result);
            Assert.Equal("O valor do produto não pode ser negativo.", actionResult.Value);
        }

        [Fact]
        public async Task AtualizarProduto_DeveAtualizarProduto()
        {
            var dbContext = CriarDbContextInMemory();
            var controller = new ProdutoController(dbContext);

            Produto produto = new Produto { Nome = "Novo produto", Estoque = 1, Valor = 100 };
            dbContext.Produtos.Add(produto);
            await dbContext.SaveChangesAsync();

            Produto produtoAtualizado = new Produto { Nome = "Produto", Estoque = 5, Valor = 10 };

            var resultado = await controller.AtualizarProduto(produto.Id, produtoAtualizado);

            var actionResult = Assert.IsType<OkObjectResult>(resultado);
            var produtoRetornado = Assert.IsType<Produto>(actionResult.Value);
            Assert.Equal("Produto", produtoRetornado.Nome);
            Assert.Equal(5, produtoRetornado.Estoque);
            Assert.Equal(10, produtoRetornado.Valor);
        }

        [Fact]
        public async Task AtualizarProduto_IdInvalido_DeveRetornarNotFound()
        {
            var dbContext = CriarDbContextInMemory();
            var controller = new ProdutoController(dbContext);

            Produto produto = new Produto { Nome = "Novo produto", Estoque = 1, Valor = 100 };

            var resultado = await controller.AtualizarProduto(0, produto);

            Assert.IsType<NotFoundResult>(resultado);
        }

        [Fact]
        public async Task DeletarProduto_DeveDeletarProduto()
        {
            var dbContext = CriarDbContextInMemory();
            var controller = new ProdutoController(dbContext);

            Produto produto = new Produto { Nome = "Novo produto", Estoque = 1, Valor = 100 };
            dbContext.Produtos.Add(produto);
            await dbContext.SaveChangesAsync();

            var resultado = await controller.DeletarProduto(produto.Id);

            Assert.IsType<OkResult>(resultado);
            Assert.Null(await dbContext.Produtos.FindAsync(produto.Id));
        }

        [Fact]
        public async Task DeletarProduto_IdInvalido_DeveRetornarNotFound()
        {
            var dbContext = CriarDbContextInMemory();
            var controller = new ProdutoController(dbContext);

            var resultado = await controller.DeletarProduto(0);

            Assert.IsType<NotFoundResult>(resultado);
        }
    }
}
