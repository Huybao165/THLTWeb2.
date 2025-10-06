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

        // GET: api/publishers
        [HttpGet]
        public IActionResult GetAllPublisher()
        {
            var publishers = publisherRepository.GetAllPublishers();
            return Ok(publishers);
        }

        // GET: api/publishers/{id}
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

        // POST: api/publishers
        [HttpPost]
        public IActionResult AddPublisher([FromBody] AddPublisherRequestDTO addPublisherRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var publisher = publisherRepository.AddPublisher(addPublisherRequestDTO);
            // Vì AddPublisher trả về AddPublisherRequestDTO (không có Id),
            // nên chỉ trả về Ok thay vì CreatedAtAction
            return Ok(publisher);
        }

        // PUT: api/publishers/{id}
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

        // DELETE: api/publishers/{id}
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
