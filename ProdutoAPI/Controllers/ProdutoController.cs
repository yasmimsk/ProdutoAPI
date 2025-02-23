using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdutoAPI.Database;
using ProdutoAPI.Models;
using System;
using System.Xml.Linq;

namespace ProdutoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoDbContext _dbContext;

        public ProdutoController(ProdutoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> ObterProdutos(string ordenacao = "Nome", bool descendente = false)
        {
            var parametro = typeof(Produto).GetProperty(ordenacao);
            if (parametro == null)
                return BadRequest($"A propriedade {ordenacao} não existe.");

            if (descendente)
                return await _dbContext.Produtos.OrderByDescending(x => EF.Property<object>(x, ordenacao)).ToListAsync();
            return await _dbContext.Produtos.OrderBy(x => EF.Property<object>(x, ordenacao)).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> ObterProduto(int id)
        {
            Produto produto = await _dbContext.Produtos.FindAsync(id);

            if (produto == null)
                return NotFound();

            return produto;
        }

        [HttpGet("por-nome")]
        public async Task<ActionResult<IEnumerable<Produto>>> ObterProdutoPorNome(string nome)
        {
            return await _dbContext.Produtos.Where(x => x.Nome == nome).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> CadastrarProduto(Produto produto)
        {
            if (produto.Valor < 0)
                return BadRequest("O valor do produto não pode ser negativo.");

            _dbContext.Produtos.Add(produto);
            await _dbContext.SaveChangesAsync();

            return Ok(produto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarProduto(int id, Produto produtoAtualizado)
        {
            Produto produto = await _dbContext.Produtos.FindAsync(id);

            if (produto == null)
                return NotFound();

            produto.Nome = produtoAtualizado.Nome;
            produto.Estoque = produtoAtualizado.Estoque;
            produto.Valor = produtoAtualizado.Valor;

            await _dbContext.SaveChangesAsync();

            return Ok(produto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarProduto(int id)
        {
            Produto produto = await _dbContext.Produtos.FindAsync(id);

            if (produto == null) 
                return NotFound();

            _dbContext.Produtos.Remove(produto);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}