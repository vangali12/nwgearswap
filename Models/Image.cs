using System;
using System.Collections.Generic;

namespace ecommerce.Models
{
    public class Image: BaseEntity
    {
        public int imageid { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string ContentType { get; set; }
    }
}