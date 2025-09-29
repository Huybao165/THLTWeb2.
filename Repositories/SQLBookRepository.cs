using Microsoft.EntityFrameworkCore;
using WebAPI_simple.Data;
using WebAPI_simple.Models.Domain;
using WebAPI_simple.Models.DTO;
using System.Linq;

namespace WebAPI_simple.Repositories
{
    public class SQLBookRepository : IBookRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLBookRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<BookWithAuthorAndPublisherDTO> GetAllBooks()
        {
            var allBooks = _dbContext.Books
                .Include(b => b.Publisher)
                .Include(b => b.Book_Authors).ThenInclude(ba => ba.Author)
                .Select(Books => new BookWithAuthorAndPublisherDTO()
                {
                    Id = Books.Id,
                    Title = Books.Title,
                    Description = Books.Description,
                    IsRead = Books.IsRead,
                    DateRead = Books.IsRead ? Books.DateRead : null,
                    Rate = Books.IsRead ? Books.Rate : null,
                    Genre = Books.Genre,
                    CoverUrl = Books.CoverUrl,
                    // Đã sửa lỗi: Sử dụng biểu thức điều kiện thay vì ?. để khắc phục lỗi "expression tree lambda"
                    PublisherName = Books.Publisher == null ? null : Books.Publisher.Name,
                    AuthorNames = Books.Book_Authors.Select(n => n.Author.FullName).ToList()
                }).ToList();
            return allBooks;
        }

        public BookWithAuthorAndPublisherDTO? GetBookById(int id)
        {
            var bookWithDomain = _dbContext.Books
                .Include(b => b.Book_Authors).ThenInclude(ba => ba.Author)
                .Include(b => b.Publisher)
                .Where(n => n.Id == id).FirstOrDefault();

            if (bookWithDomain == null)
            {
                return null;
            }

            // Map Domain Model to DTO
            var bookWithIdDTO = new BookWithAuthorAndPublisherDTO()
            {
                Id = bookWithDomain.Id,
                Title = bookWithDomain.Title,
                Description = bookWithDomain.Description,
                IsRead = bookWithDomain.IsRead,
                DateRead = bookWithDomain.IsRead ? bookWithDomain.DateRead : null,
                Rate = bookWithDomain.IsRead ? bookWithDomain.Rate : null,
                Genre = bookWithDomain.Genre,
                CoverUrl = bookWithDomain.CoverUrl,
                PublisherName = bookWithDomain.Publisher?.Name,
                AuthorNames = bookWithDomain.Book_Authors.Select(n => n.Author.FullName).ToList()
            };

            return bookWithIdDTO;
        }

        public AddBookRequestDTO AddBook(AddBookRequestDTO addBookRequestDTO)
        {
            var bookDomainModel = new Book
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

            _dbContext.Books.Add(bookDomainModel);
            _dbContext.SaveChanges();

            foreach (var id in addBookRequestDTO.AuthorIds)
            {
                var _book_author = new Book_Author()
                {
                    BookId = bookDomainModel.Id,
                    AuthorId = id
                };
                _dbContext.Books_Authors.Add(_book_author);
                _dbContext.SaveChanges();
            }
            return addBookRequestDTO;
        }

        public AddBookRequestDTO? UpdateBookById(int id, AddBookRequestDTO bookDTO)
        {
            var bookDomain = _dbContext.Books.FirstOrDefault(n => n.Id == id);

            if (bookDomain == null)
            {
                return null;
            }

            bookDomain.Title = bookDTO.Title;
            bookDomain.Description = bookDTO.Description;
            bookDomain.IsRead = bookDTO.IsRead;
            bookDomain.DateRead = bookDTO.DateRead;
            bookDomain.Rate = bookDTO.Rate;
            bookDomain.Genre = bookDTO.Genre;
            bookDomain.CoverUrl = bookDTO.CoverUrl;
            bookDomain.DateAdded = bookDTO.DateAdded;
            bookDomain.PublisherID = bookDTO.PublisherID;
            _dbContext.SaveChanges();

            var authorDomain = _dbContext.Books_Authors.Where(a => a.BookId == id).ToList();
            if (authorDomain.Any())
            {
                _dbContext.Books_Authors.RemoveRange(authorDomain);
                _dbContext.SaveChanges();
            }

            foreach (var authorId in bookDTO.AuthorIds)
            {
                var _book_author = new Book_Author()
                {
                    BookId = id,
                    AuthorId = authorId
                };
                _dbContext.Books_Authors.Add(_book_author);
                _dbContext.SaveChanges();
            }

            return bookDTO;
        }

        public Book? DeleteBookById(int id)
        {
            var bookDomain = _dbContext.Books.FirstOrDefault(n => n.Id == id);
            if (bookDomain != null)
            {
                _dbContext.Books.Remove(bookDomain);
                _dbContext.SaveChanges();
            }
            return bookDomain;
        }
    }
}
