using System;
using System.Collections.Generic;

namespace ecommerce.Models
{
    public class Product: BaseEntity
    {
        public int productid { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public int userid { get; set; }
        public User Seller { get; set; }
        public int categoryid { get; set; }
        public Category Categories { get; set; }
        public Product()
        { 
            Categories = new Category();
        }
    }

}