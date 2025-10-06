using Microsoft.AspNetCore.Mvc;
using WebAPI_simple.Repositories;
using WebAPI_simple.Models.DTO;
using WebAPI_simple.Models.Domain;

namespace WebAPI_simple.Controllers
{
    [ApiController]
    [Route("api/[controller]")]  // => api/authors
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;

        // Inject IAuthorRepository qua constructor
        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        // GET: api/authors
        [HttpGet]
        public IActionResult GetAllAuthor()
        {
            var authors = _authorRepository.GetAllAuthor();
            return Ok(authors);
        }

        // GET: api/authors/1
        [HttpGet("{id}")]
        public IActionResult GetAuthorById(int id)
        {
            var author = _authorRepository.GetAuthorById(id);
            if (author == null)
                return NotFound($"Không tìm thấy Author với Id = {id}");

            return Ok(author);
        }

        // POST: api/authors
        [HttpPost]
        public IActionResult AddAuthor([FromBody] AddAuthorRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newAuthor = _authorRepository.AddAuthor(request);
            return Ok(newAuthor);
        }

        // PUT: api/authors/1
        [HttpPut("{id}")]
        public IActionResult UpdateAuthorById(int id, [FromBody] AuthorNoIdDTO request)
        {
            var updatedAuthor = _authorRepository.UpdateAuthorById(id, request);
            if (updatedAuthor == null)
                return NotFound($"Không tìm thấy Author với Id = {id}");

            return Ok(updatedAuthor);
        }

        // DELETE: api/authors/1
        [HttpDelete("{id}")]
        public IActionResult DeleteAuthorById(int id)
        {
            var deletedAuthor = _authorRepository.DeleteAuthorById(id);
            if (deletedAuthor == null)
                return NotFound($"Không tìm thấy Author với Id = {id}");

            return Ok(deletedAuthor);
        }
    }
}
