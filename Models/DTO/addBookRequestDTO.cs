using System.ComponentModel.DataAnnotations;   // 💡 Thêm dòng này ở đầu file
using WebAPI_simple.Models.Domain;

namespace WebAPI_simple.Models.DTO
{
    public class AddBookRequestDTO
    {
        // ✅ Bài tập 1: Title không được rỗng, tối thiểu 10 ký tự, không chứa ký tự đặc biệt
        [Required(ErrorMessage = "Title is required.")]
        [MinLength(10, ErrorMessage = "Title must be at least 10 characters long.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Title cannot contain special characters.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string? Description { get; set; }

        public bool IsRead { get; set; }

        public DateTime? DateRead { get; set; }

        [Range(0, 5, ErrorMessage = "Rate must be between 0 and 5.")]
        public int? Rate { get; set; }

        public string? Genre { get; set; }

        public string? CoverUrl { get; set; }

        public DateTime DateAdded { get; set; }

        [Required(ErrorMessage = "PublisherID is required.")]
        public int PublisherID { get; set; }

        [Required(ErrorMessage = "At least one author ID is required.")]
        [MinLength(1, ErrorMessage = "At least one author ID is required.")]
        public List<int> AuthorIds { get; set; } = new List<int>();
    }
}
