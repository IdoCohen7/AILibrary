using Microsoft.AspNetCore.Mvc;
using AILibrary.Models;
using AILibrary.DAL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AILibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        // GET: api/<BookController>
        [HttpGet]
        public List<Book> Get()
        {
            return Book.GetBooks();
        }

        // GET: api/<BookController>
        [HttpGet("Text")]
        public List<Book> SearchBooksByParameter(string parameter, string text)
        {
            return Book.SearchBooksByParameter(parameter, text);
        }

        // GET api/<BookController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("Random")]
        public Book GetRandomBook()
        {
            return Book.GetRandomBook();
        }

        // POST api/<BookController>
        [HttpPost]
        public int Post([FromBody] Book book)
        {
            Book newBook = new Book(book);
            int val = Book.InsertToTable(newBook);
            if (newBook != null && newBook.Authors.Length != 0)
            {
                for (int i = 0; i<newBook.Authors.Length; i++)
                {
                    newBook.AssignAuthorToBook(newBook.Authors[i]);
                }
            }
            return val;
        }

        // PUT api/<BookController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BookController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
