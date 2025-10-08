using Microsoft.EntityFrameworkCore;
using WebAPI_simple.Data;
using WebAPI_simple.Models.Domain;
using WebAPI_simple.Models.DTO;

namespace WebAPI_simple.Repositories
{
    public class SQLBookRepository : IBookRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLBookRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ✅ GetAllBooks – có hỗ trợ Filter + Sort
        public List<BookWithAuthorAndPublisherDTO> GetAllBooks(
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            bool isAscending = true)
        {
            // Lấy danh sách sách và ánh xạ sang DTO
            var allBooks = _dbContext.Books
                .Include(b => b.Publisher)
                .Include(b => b.Book_Authors).ThenInclude(ba => ba.Author)
                .Select(b => new BookWithAuthorAndPublisherDTO()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    IsRead = b.IsRead,
                    DateRead = b.IsRead ? b.DateRead : null,
                    Rate = b.IsRead ? b.Rate : null,
                    Genre = b.Genre,
                    CoverUrl = b.CoverUrl,
                    PublisherName = b.Publisher == null ? null : b.Publisher.Name,
                    AuthorNames = b.Book_Authors.Select(a => a.Author.FullName).ToList()
                })
                .AsQueryable();

            // 🧩 FILTERING
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if (filterOn.Equals("title", StringComparison.OrdinalIgnoreCase))
                {
                    allBooks = allBooks.Where(x => x.Title.Contains(filterQuery));
                }
                else if (filterOn.Equals("genre", StringComparison.OrdinalIgnoreCase))
                {
                    allBooks = allBooks.Where(x => x.Genre.Contains(filterQuery));
                }
                else if (filterOn.Equals("publisher", StringComparison.OrdinalIgnoreCase))
                {
                    allBooks = allBooks.Where(x => x.PublisherName.Contains(filterQuery));
                }
            }

            // 🧭 SORTING
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("title", StringComparison.OrdinalIgnoreCase))
                {
                    allBooks = isAscending
                        ? allBooks.OrderBy(x => x.Title)
                        : allBooks.OrderByDescending(x => x.Title);
                }
                else if (sortBy.Equals("genre", StringComparison.OrdinalIgnoreCase))
                {
                    allBooks = isAscending
                        ? allBooks.OrderBy(x => x.Genre)
                        : allBooks.OrderByDescending(x => x.Genre);
                }
                else if (sortBy.Equals("publisher", StringComparison.OrdinalIgnoreCase))
                {
                    allBooks = isAscending
                        ? allBooks.OrderBy(x => x.PublisherName)
                        : allBooks.OrderByDescending(x => x.PublisherName);
                }
            }

            // ✅ Trả kết quả ra danh sách
            return allBooks.ToList();
        }

        // ✅ Lấy 1 sách theo Id
        public BookWithAuthorAndPublisherDTO GetBookById(int id)
        {
            var bookDomain = _dbContext.Books
                .Include(b => b.Book_Authors).ThenInclude(ba => ba.Author)
                .Include(b => b.Publisher)
                .FirstOrDefault(b => b.Id == id);

            if (bookDomain == null) return null!;

            var bookDTO = new BookWithAuthorAndPublisherDTO()
            {
                Id = bookDomain.Id,
                Title = bookDomain.Title,
                Description = bookDomain.Description,
                IsRead = bookDomain.IsRead,
                DateRead = bookDomain.IsRead ? bookDomain.DateRead : null,
                Rate = bookDomain.IsRead ? bookDomain.Rate : null,
                Genre = bookDomain.Genre,
                CoverUrl = bookDomain.CoverUrl,
                PublisherName = bookDomain.Publisher?.Name,
                AuthorNames = bookDomain.Book_Authors.Select(a => a.Author.FullName).ToList()
            };

            return bookDTO;
        }

        // ✅ Thêm mới 1 sách
        public AddBookRequestDTO AddBook(AddBookRequestDTO addBookRequestDTO)
        {
            var book = new Book
            {
                Title = addBookRequestDTO.Title,
                Description = addBookRequestDTO.Description,
                IsRead = addBookRequestDTO.IsRead,
                DateRead = addBookRequestDTO.DateRead,
                Rate = addBookRequestDTO.Rate,
                Genre = addBookRequestDTO.Genre,
                CoverUrl = addBookRequestDTO.CoverUrl,
                DateAdded = addBookRequestDTO.DateAdded,
                PublisherID = addBookRequestDTO.PublisherID
            };

            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();

            foreach (var authorId in addBookRequestDTO.AuthorIds)
            {
                var bookAuthor = new Book_Author()
                {
                    BookId = book.Id,
                    AuthorId = authorId
                };
                _dbContext.Books_Authors.Add(bookAuthor);
                _dbContext.SaveChanges();
            }

            return addBookRequestDTO;
        }

        // ✅ Cập nhật 1 sách
        public AddBookRequestDTO? UpdateBookById(int id, AddBookRequestDTO bookDTO)
        {
            var book = _dbContext.Books.FirstOrDefault(b => b.Id == id);
            if (book == null) return null;

            book.Title = bookDTO.Title;
            book.Description = bookDTO.Description;
            book.IsRead = bookDTO.IsRead;
            book.DateRead = bookDTO.DateRead;
            book.Rate = bookDTO.Rate;
            book.Genre = bookDTO.Genre;
            book.CoverUrl = bookDTO.CoverUrl;
            book.DateAdded = bookDTO.DateAdded;
            book.PublisherID = bookDTO.PublisherID;

            _dbContext.SaveChanges();

            // Cập nhật danh sách tác giả
            var oldAuthors = _dbContext.Books_Authors.Where(a => a.BookId == id).ToList();
            if (oldAuthors.Any())
            {
                _dbContext.Books_Authors.RemoveRange(oldAuthors);
                _dbContext.SaveChanges();
            }

            foreach (var authorId in bookDTO.AuthorIds)
            {
                var bookAuthor = new Book_Author()
                {
                    BookId = id,
                    AuthorId = authorId
                };
                _dbContext.Books_Authors.Add(bookAuthor);
                _dbContext.SaveChanges();
            }

            return bookDTO;
        }

        // ✅ Xóa 1 sách
        public Book? DeleteBookById(int id)
        {
            var book = _dbContext.Books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                _dbContext.Books.Remove(book);
                _dbContext.SaveChanges();
            }
            return book;
        }
    }
}
