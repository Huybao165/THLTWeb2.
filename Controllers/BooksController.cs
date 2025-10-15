using Microsoft.AspNetCore.Mvc;
using WebAPI_simple.Models.DTO;
using WebAPI_simple.Repositories;

namespace WebAPI_simple.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        // ✅ Inject Repository vào Controller
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // ✅ GetAllBooks có hỗ trợ Filter + Sort
        [HttpGet("get-all-books")]
        public IActionResult GetAll(
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool isAscending = true)
        {
            // 🔹 Sử dụng Repository Pattern để lấy dữ liệu
            var allBooks = _bookRepository.GetAllBooks(filterOn, filterQuery, sortBy, isAscending);

            // 🔹 Trả về dữ liệu cho client
            return Ok(allBooks);
        }

        // ✅ Get book by Id
        [HttpGet("get-book-by-id/{id}")]
        public IActionResult GetBookById([FromRoute] int id)
        {
            var book = _bookRepository.GetBookById(id);

            if (book == null)
            {
                return NotFound("❌ Book not found!");
            }

            return Ok(book);
        }

        // ✅ Add new book
        [HttpPost("add-book")]
        public IActionResult AddBook([FromBody] AddBookRequestDTO addBookRequestDTO)
        {
            if (addBookRequestDTO == null)
            {
                return BadRequest("❌ Invalid book data");
            }

            var newBook = _bookRepository.AddBook(addBookRequestDTO);
            return Ok(newBook);
        }

        // ✅ Update book by Id
        [HttpPut("update-book-by-id/{id}")]
        public IActionResult UpdateBookById([FromRoute] int id, [FromBody] AddBookRequestDTO bookDTO)
        {
            var updatedBook = _bookRepository.UpdateBookById(id, bookDTO);

            if (updatedBook == null)
            {
                return NotFound("❌ Book not found to update!");
            }

            return Ok(updatedBook);
        }

        // ✅ Delete book by Id
        [HttpDelete("delete-book-by-id/{id}")]
        public IActionResult DeleteBookById([FromRoute] int id)
        {
            var deletedBook = _bookRepository.DeleteBookById(id);

            if (deletedBook == null)
            {
                return NotFound("❌ Book not found to delete!");
            }

            return Ok($"✅ Book '{deletedBook.Title}' has been deleted successfully.");
        }
// 🧩 Hàm Validate ở Controller
#region Private methods
private bool ValidateAddBook(AddBookRequestDTO addBookRequestDTO)
        {
            if (addBookRequestDTO == null)
            {
                ModelState.AddModelError(nameof(addBookRequestDTO), "Please add book data");
                return false;
            }

            // 🔸 Kiểm tra Description NotNull
            if (string.IsNullOrEmpty(addBookRequestDTO.Description))
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.Description),
                    $"{nameof(addBookRequestDTO.Description)} cannot be null");
            }

            // 🔸 Kiểm tra rating (0,5)
            if (addBookRequestDTO.Rate < 0 || addBookRequestDTO.Rate > 5)
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.Rate),
                    $"{nameof(addBookRequestDTO.Rate)} cannot be less than 0 and more than 5");
            }

            // 🔸 Nếu có lỗi nào thì return false
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
