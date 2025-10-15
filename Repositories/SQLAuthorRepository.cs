using WebAPI_simple.Data;                // DbContext
using WebAPI_simple.Models.Domain;       // Author entity
using WebAPI_simple.Models.DTO;          // DTOs
using Microsoft.EntityFrameworkCore;     // EF Core
using System.Collections.Generic;
using System.Linq;

namespace WebAPI_simple.Repositories
{
    public class SQLAuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _context;

        // Inject DbContext
        public SQLAuthorRepository(AppDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả Authors
        public List<AuthorDTO> GetAllAuthor()
        {
            return _context.Authors
                .Select(a => new AuthorDTO
                {
                    Id = a.Id,
                    FullName = a.FullName
                })
                .ToList();
        }

        // Lấy Author theo Id
        public AuthorNoIdDTO? GetAuthorById(int id)
        {
            var author = _context.Authors.Find(id);
            if (author == null) return null;

            return new AuthorNoIdDTO
            {
                FullName = author.FullName
            };
        }

        // Thêm Author mới
        public AddAuthorRequestDTO AddAuthor(AddAuthorRequestDTO addAuthorRequestDTO)
        {
            var author = new Author
            {
                FullName = addAuthorRequestDTO.FullName
            };

            _context.Authors.Add(author);
            _context.SaveChanges();

            return addAuthorRequestDTO;
            // 👉 nếu muốn trả cả Id thì sửa trả về AuthorDTO thay vì AddAuthorRequestDTO
        }

        // Cập nhật Author theo Id
        public AuthorNoIdDTO? UpdateAuthorById(int id, AuthorNoIdDTO authorNoIdDTO)
        {
            var author = _context.Authors.Find(id);
            if (author == null) return null;

            author.FullName = authorNoIdDTO.FullName;
            _context.SaveChanges();

            return new AuthorNoIdDTO
            {
                FullName = author.FullName
            };
        }

        // Xóa Author theo Id
        public Author? DeleteAuthorById(int id)
        {
            var author = _context.Authors.Find(id);
            if (author == null) return null;

            _context.Authors.Remove(author);
            _context.SaveChanges();

            return author;
        }
    }
}
