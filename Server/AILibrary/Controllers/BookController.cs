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

        [HttpGet("16Random")]
        public List<string> Get16ThumbnailRandomBooks()
        {
            return Book.Get16ThumbnailRandomBooks();

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
<<<<<<< Updated upstream
=======
        //// GET: api/Book/Popularity
        //[HttpGet("Popularity")]
        //public List<Book> GetBookPopularity()
        //{
        //    return Book.GetBookPopularity();
        //}
>>>>>>> Stashed changes
        // GET: api/Book/PurchasesThisWeek
        [HttpGet("PurchasesThisWeek")]
        public int GetPurchasesThisWeek()
        {
            return Book.GetPurchasesThisWeek();
        }
<<<<<<< Updated upstream
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

        // GET: api/Book/Popularity
        [HttpGet("Popularity")]
        public IActionResult GetBookPopularity()
        {
            try
            {
                var bookPopularityList = Book.GetBookPopularity();
                return Ok(bookPopularityList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        // GET: api/Book/Available
        [HttpGet("Available")]
        public IActionResult GetAvailableBooks()
        {
            try
            {
                var availableBooks = Book.GetAvailableBooks();
                return Ok(availableBooks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        // GET: api/Book/AvailableCount
        [HttpGet("AvailableCount")]
        public IActionResult GetAvailableBooksCount()
        {
            try
            {
                var availableBooksCount = Book.GetAvailableBooksCount();
                return Ok(availableBooksCount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Book/TotalBooksRead
        [HttpGet("TotalBooksRead")]
        public IActionResult GetTotalBooksRead()
        {
            try
            {
                var totalBooksRead = Book.GetTotalBooksRead();
                return Ok(totalBooksRead);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Book/TotalBooksPurchased
        [HttpGet("TotalBooksPurchased")]
        public IActionResult GetTotalBooksPurchased()
        {
            try
            {
                var totalBooksPurchased = Book.GetTotalBooksPurchased();
                return Ok(totalBooksPurchased);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Book/TotalBooksExchanged
        [HttpGet("TotalBooksExchanged")]
        public IActionResult GetTotalBooksExchanged()
        {
            try
            {
                var totalBooksExchanged = Book.GetTotalBooksExchanged();
                return Ok(totalBooksExchanged);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        // GET: api/Book/Summary
        [HttpGet("Summary")]
        public IActionResult GetBooksSummary()
        {
            try
            {
                var booksSummary = Book.GetBooksSummary();
                return Ok(booksSummary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

=======
>>>>>>> Stashed changes
    }

}

