using System;
using System.Collections.Generic;

namespace ecommerce.Models
{
    public class User: BaseEntity
    {
        public int userid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public List<Message> Messages { get; set; }
        public DateTime Logged { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public int interestid { get; set; }
        public Interest Interests { get; set; }
        // public int imageid { get; set; }
        // public Image ProfileImage { get; set; }

        public User()
        {
            Messages = new List<Message>();
            Interests = new Interest();
        }
    }

}