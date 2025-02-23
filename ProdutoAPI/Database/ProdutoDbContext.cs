using Microsoft.EntityFrameworkCore;
using ProdutoAPI.Models;

namespace ProdutoAPI.Database
{
    public class ProdutoDbContext : DbContext
    {
        public ProdutoDbContext(DbContextOptions<ProdutoDbContext> options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; }
    }
}
