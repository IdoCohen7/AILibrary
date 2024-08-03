using Microsoft.AspNetCore.Mvc;
using AILibrary.Models;
using AILibrary.DAL;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AILibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/<UserController>
        [HttpGet]
        public List<User> Get()
        {
            return AILibrary.Models.User.GetUsers();
        }

        [HttpGet("Login")]
        public User Get(string email, string password)
        {
            return AILibrary.Models.User.Login(email, password);
        }

        [HttpGet("GetFavorites")]
        public List<Book> GetUsersFavorites(int userId)
        {
            return AILibrary.Models.User.GetUsersFavorites(userId);
        }

        [HttpGet("GetHistory")]
        public List<Book> GetUsersHistory(int userId)
        {
            return AILibrary.Models.User.GetUsersHistory(userId);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost("Register")]
        public int Post([FromBody] User user)
        {
            return AILibrary.Models.User.Register(user.Name, user.Email, user.Password);
            
        }

        // POST api/<UserController>
        [HttpPost("Favorite")]
        public int AddToFavorites(int userId, string bookId)
        {
            return AILibrary.Models.User.AddToFavorites(userId, bookId);

        }

        // POST api/<UserController>
        [HttpPost("Mark")]
        public int MarkAsRead(int userId, string bookId)
        {
            return AILibrary.Models.User.MarkAsRead(userId, bookId);

        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
