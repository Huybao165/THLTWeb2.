using WebAPI_simple.Data;
using WebAPI_simple.Models.Domain;
using WebAPI_simple.Models.DTO;
using System.Collections.Generic;
using System.Linq;

namespace WebAPI_simple.Repositories
{
    public class SQLPublisherRepository : IPublisherRepository
    {
        private readonly AppDbContext _context;

        public SQLPublisherRepository(AppDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả Publisher
        public List<PublisherDTO> GetAllPublishers()
        {
            return _context.Publishers
                .Select(p => new PublisherDTO
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToList();
        }

        // Lấy Publisher theo Id
        public PublisherNoIdDTO GetPublisherById(int id)
        {
            var publisher = _context.Publishers.Find(id);
            if (publisher == null) return null;

            return new PublisherNoIdDTO
            {
                Name = publisher.Name
            };
        }

        // Thêm Publisher mới
        public AddPublisherRequestDTO AddPublisher(AddPublisherRequestDTO addPublisherRequestDTO)
        {
            var publisher = new Publisher
            {
                Name = addPublisherRequestDTO.Name
            };

            _context.Publishers.Add(publisher);
            _context.SaveChanges();

            // Trả về lại DTO yêu cầu ban đầu
            return addPublisherRequestDTO;
        }

        // Cập nhật Publisher theo Id
        public PublisherNoIdDTO UpdatePublisherById(int id, PublisherNoIdDTO publisherNoIdDTO)
        {
            var publisher = _context.Publishers.Find(id);
            if (publisher == null) return null;

            publisher.Name = publisherNoIdDTO.Name;
            _context.SaveChanges();

            return publisherNoIdDTO;
        }
        public bool PublisherExists(string name)
        {
            return _context.Publishers
                .Any(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        // Xóa Publisher theo Id
        public Publisher? DeletePublisherById(int id)
        {
            var publisher = _context.Publishers.Find(id);
            if (publisher == null) return null;

            _context.Publishers.Remove(publisher);
            _context.SaveChanges();

            return publisher;
        }
    }
}
