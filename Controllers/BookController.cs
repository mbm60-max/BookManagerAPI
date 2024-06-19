using BManagerAPi.Entities;
using BManagerAPi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BManagerAPi.Controllers
{
    [ApiController]
    [Route("books")]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book book)
        {
            try
            {
                if(book.OwnerId != null){
                    await _bookService.CreateBookAsync(book);
                    var createdBook = await _bookService.GetBookAsync(book.Id);
                    // Return the entire book object as the response
                    return Ok(createdBook);
                }
                return StatusCode(500, "OwnerId not present so cannot create a book entry");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook([FromRoute] Guid id)
        {
            try
            {
                var book = await _bookService.GetBookAsync(id);
                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("ownedBy/{OwnerId}")]
        public async Task<IActionResult> GetBooks(string OwnerId)
        {
            try
            {
                var books = await _bookService.GetBooksAsync(OwnerId);
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook([FromRoute] Guid id, [FromBody]Book book)
        {
            try
            {
                await _bookService.UpdateBookAsync(id,book);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute] Guid id)
        {
            try
            {
                await _bookService.DeleteBookAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
