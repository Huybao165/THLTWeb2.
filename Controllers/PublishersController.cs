using Microsoft.AspNetCore.Mvc;
using WebAPI_simple.Repositories;
using WebAPI_simple.Models.DTO;

namespace WebAPI_simple.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherRepository publisherRepository;

        public PublishersController(IPublisherRepository publisherRepository)
        {
            this.publisherRepository = publisherRepository;
        }

        // -------------------- GET: api/publishers --------------------
        [HttpGet]
        public IActionResult GetAllPublisher()
        {
            var publishers = publisherRepository.GetAllPublishers();
            return Ok(publishers);
        }

        // -------------------- GET: api/publishers/{id} --------------------
        [HttpGet("{id}")]
        public IActionResult GetPublisherById(int id)
        {
            var publisher = publisherRepository.GetPublisherById(id);
            if (publisher == null)
            {
                return NotFound($"Publisher with id {id} not found.");
            }
            return Ok(publisher);
        }

        // -------------------- POST: api/publishers --------------------
        [HttpPost]
        public IActionResult AddPublisher([FromBody] AddPublisherRequestDTO addPublisherRequestDTO)
        {
            // 1️⃣ Kiểm tra dữ liệu đầu vào
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 2️⃣ Kiểm tra trùng tên (theo yêu cầu bài tập 3)
            var existingPublisher = publisherRepository
                .GetAllPublishers()
                .FirstOrDefault(p =>
                    string.Equals(p.Name, addPublisherRequestDTO.Name, StringComparison.OrdinalIgnoreCase));

            if (existingPublisher != null)
            {
                return BadRequest($"Publisher name '{addPublisherRequestDTO.Name}' already exists.");
            }

            // 3️⃣ Thêm mới
            var publisher = publisherRepository.AddPublisher(addPublisherRequestDTO);
            return Ok(publisher);
        }

        // -------------------- PUT: api/publishers/{id} --------------------
        [HttpPut("{id}")]
        public IActionResult UpdatePublisherById(int id, [FromBody] PublisherNoIdDTO publisherNoIdDTO)
        {
            var publisher = publisherRepository.UpdatePublisherById(id, publisherNoIdDTO);
            if (publisher == null)
            {
                return NotFound($"Publisher with id {id} not found.");
            }
            return Ok(publisher);
        }

        // -------------------- DELETE: api/publishers/{id} --------------------
        [HttpDelete("{id}")]
        public IActionResult DeletePublisherById(int id)
        {
            var publisher = publisherRepository.DeletePublisherById(id);
            if (publisher == null)
            {
                return NotFound($"Publisher with id {id} not found.");
            }
            return Ok(publisher);
        }
    }
}
