using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ecommerce.Models {
    public class ProductEditModel {

        [Required(ErrorMessage = "Name is Required.")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
        public string Name { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }

        public Category Categories { get; set; }

        [Required(ErrorMessage = "Quantity is Required.")]
        [Range(0, 100000, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is Required.")]
        [Range(0, float.MaxValue, ErrorMessage = "Price must be more than $0.00")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Description is Required.")]
        [MinLength(6, ErrorMessage = "Description must be at least 6 characters.")]
        public string Description { get; set; }
    }
}