using WebAPI_simple.Models.DTO;
using WebAPI_simple.Models.Domain;
using System.Collections.Generic;

namespace WebAPI_simple.Repositories
{
    public interface IAuthorRepository
    {
        // Lấy danh sách tất cả Author
        List<AuthorDTO> GetAllAuthor();

        // Lấy Author theo Id
        AuthorNoIdDTO? GetAuthorById(int id);

        // Thêm Author mới
        AddAuthorRequestDTO AddAuthor(AddAuthorRequestDTO addAuthorRequestDTO);

        // Cập nhật Author theo Id
        AuthorNoIdDTO? UpdateAuthorById(int id, AuthorNoIdDTO authorNoIdDTO);

        // Xóa Author theo Id
        Author? DeleteAuthorById(int id);
    }
}
