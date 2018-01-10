using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce.Models
{
    public class Message: BaseEntity
    {
        public int messageid { get; set; }
        public int productid { get; set; }
        public Product Item { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Seen { get; set; }
        public int senderid { get; set; }
        [ForeignKey("senderid")]
        public User Sender { get; set; }
        public int recipientid { get; set; }
        
        public Message()
        {}
    }

}