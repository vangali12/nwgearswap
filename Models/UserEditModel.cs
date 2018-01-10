using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ecommerce.Models {
    public class UserEditModel {
        [Required(ErrorMessage = "First Name is Required.")]
        [MinLength(2, ErrorMessage = "First Name must be at least 2 characters long.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "First Name is Required.")]
        [MinLength(2, ErrorMessage = "Last Name must be at least 2 characters long.")]
        public string LastName { get; set; }

        public string City { get; set; }
        
        [DataType(DataType.Upload)]
        public IFormFile imageFile { get; set; }
        public string Region { get; set; }
        public string Description { get; set; }
        public Interest Interests { get; set; }
    }
}