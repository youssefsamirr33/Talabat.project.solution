using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class BasketItemsDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string PictureUrl { get; set; }

        [Required]
        [Range(0.1 , double.MaxValue , ErrorMessage ="The Price must be greater than 0.1 sent.")]
        public decimal Price { get; set; }

        [Required]
        [Range(1,double.MaxValue , ErrorMessage = "The Quantity must be 1 item at least.")]
        public int Quantity { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Brand { get; set; }
    }
}