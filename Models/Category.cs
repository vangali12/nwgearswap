using System;
using System.Collections.Generic;

namespace ecommerce.Models
{
    public class Category: BaseEntity
    {
        public int categoryid { get; set; }
        public Boolean CampHike { get; set; }
        public Boolean Climb { get; set; }
        public Boolean Cycle { get; set; }
        public Boolean Paddle { get; set; }
        public Boolean Run { get; set; }
        public Boolean Snow { get; set; }
        public Boolean Travel { get; set; }
        public Boolean Men { get; set; }
        public Boolean Women { get; set; }
        public Boolean Kids { get; set; }
        public Boolean BooksMaps { get; set; }
        public Boolean ArtPhotography { get; set; }
        public Category()
        {  }
    }

}