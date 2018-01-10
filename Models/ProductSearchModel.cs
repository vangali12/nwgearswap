using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models {
    public class ProductSearchModel
    {
        public string Name { get; set; }
        public int? minPrice { get; set; }
        public int? maxPrice { get; set; }
        public string Category { get; set; }
        public string Region { get; set; }
    }
}