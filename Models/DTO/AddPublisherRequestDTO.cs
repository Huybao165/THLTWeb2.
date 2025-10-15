using System.ComponentModel.DataAnnotations;

namespace WebAPI_simple.Models.DTO
{
    public class AddPublisherRequestDTO
    {
        [Required(ErrorMessage = "Publisher name is required.")]
        [MinLength(3, ErrorMessage = "Publisher name must be at least 3 characters long.")]
        public string? Name { get; set; }
    }
}
