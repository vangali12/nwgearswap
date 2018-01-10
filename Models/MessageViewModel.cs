using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models {
    public class MessageViewModel {

        [Required(ErrorMessage = "Title is Required.")]
        [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Message is Required.")]
        [MinLength(6, ErrorMessage = "Message must be at least 6 characters")]
        public string Content { get; set; }
    }
}