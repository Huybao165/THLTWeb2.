using WebAPI_simple.Models.Domain;
using WebAPI_simple.Models.DTO;

namespace WebAPI_simple.Repositories
{
    public interface IBookRepository
    {
        // ✅ GetAllBooks: có hỗ trợ lọc (Filter) + sắp xếp (Sort)
        List<BookWithAuthorAndPublisherDTO> GetAllBooks(
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            bool isAscending = true
        );

        // ✅ Lấy 1 sách theo Id
        BookWithAuthorAndPublisherDTO GetBookById(int id);

        // ✅ Thêm mới 1 sách
        AddBookRequestDTO AddBook(AddBookRequestDTO addBookRequestDTO);

        // ✅ Cập nhật thông tin sách theo Id
        AddBookRequestDTO? UpdateBookById(int id, AddBookRequestDTO bookDTO);

        // ✅ Xóa sách theo Id
        Book? DeleteBookById(int id);
    }
}
