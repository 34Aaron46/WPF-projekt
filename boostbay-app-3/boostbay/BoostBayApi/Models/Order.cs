using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoostBayApi.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        public string CustomerName { get; set; } = "";

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "Új";

        public Product? Product { get; set; }
    }
}
