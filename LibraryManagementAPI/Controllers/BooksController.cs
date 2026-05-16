using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;

        public BooksController(IBookService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var books = _service.GetAll();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var book = _service.GetById(id);
            if (book == null) return NotFound($"Book with ID {id} not found.");
            return Ok(book);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add([FromBody] BookDto bookDto)
        {
            var book = _service.Add(bookDto);
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Update(int id, [FromBody] BookDto bookDto)
        {
            var book = _service.Update(id, bookDto);
            if (book == null) return NotFound($"Book with ID {id} not found.");
            return Ok(book);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var result = _service.Delete(id);
            if (!result) return NotFound($"Book with ID {id} not found.");
            return NoContent();
        }
    }
}