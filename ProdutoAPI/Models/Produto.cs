using System.ComponentModel.DataAnnotations;

namespace ProdutoAPI.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public int Estoque { get; set; }

        [Required]
        public decimal Valor { get; set; }
    }
}
