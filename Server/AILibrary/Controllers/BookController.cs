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

        // GET: api/<BookController>
        [HttpGet("Market")]
        public List<Object> GetMarketplace()
        {
            return Book.GetMarketPlace();
        }

        // GET api/<BookController>/5
        [HttpGet("Reviews")]
        public List<Object> GetBookReviews(string bookId)
        {
            return Book.GetBookReviews(bookId);
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



        // GET: api/Book/TopRated
        [HttpGet("TopRated")]
        public List<Book> GetTopRatedBooks()
        {
            return Book.GetTopRatedBooks();
        }
        // GET: api/Book/MostNew
        [HttpGet("MostNew")]
        public List<Book> GetMostNewBooks()
        {
            return Book.GetMostNewBooks();
        }

        
       

        // GET: api/Book/BookLibraryDetails
        [HttpGet("BookLibraryDetails")]
        public List<object> GetBookLibraryDetails(string bookId)
        {
            return Book.GetBookLibraryDetails(bookId);  
        }

        




    }
}
