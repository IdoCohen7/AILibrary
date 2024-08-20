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
            return AILibrary.Models.User.Register(user.Name, user.Email, user.Password, user.ProfilePic);
            
        }

        // POST api/<UserController>
        [HttpPost("Favorite")]
        public int AddToFavorites(int userId, string bookId)
        {
            return AILibrary.Models.User.AddToFavorites(userId, bookId);

        }

        // DELETE api/<UserController>/5
        [HttpDelete("RemoveFavorite")]
        public int RemoveFromFavorites(int userId, string bookId)
        {
            return AILibrary.Models.User.RemoveFromFavorites(userId, bookId);
        }

        // POST api/<UserController>
        [HttpPost("Mark")]
        public int MarkAsRead(int userId, string bookId)
        {
            return AILibrary.Models.User.MarkAsRead(userId, bookId);

        }

        // POST api/<UserController>
        [HttpPost("SendRequest")]
        public int SendBookRequest(int buyerId, int sellerId, string bookId)
        {
            return AILibrary.Models.User.SendBookRequest(buyerId, sellerId, bookId);

        }

        // PUT api/<UserController>/5
        [HttpPut("AcceptRequest")]
        public int AcceptBookRequest(int buyerId, int sellerId, string bookId)
        {
            return AILibrary.Models.User.AcceptBookRequest(buyerId, sellerId, bookId);
        }

        // PUT api/<UserController>/5
        [HttpPut("CancelRequest")]
        public int CancelBookRequest (int buyerId, int sellerId, string bookId)
        {
            return AILibrary.Models.User.CancelBookRequest(buyerId, sellerId, bookId);
        }

        // PUT api/<UserController>/5
        [HttpPut("ChangePassword")]
        public int ChangePassword (int userId, string password)
        {
            return AILibrary.Models.User.ChangePassword(userId, password);
        }

        // PUT api/<UserController>/5
        [HttpPut("ChangeUsername")]
        public int ChangeUsername(int userId, string name)
        {
            return AILibrary.Models.User.ChangeUsername(userId, name);
        }

        // PUT api/<UserController>/5
        [HttpPut("ChangeProfilePicture")]
        public int ChangeProfilePicture(int userId, string picture)
        {
            return AILibrary.Models.User.ChangeProfilePicture(userId, picture);
        }

        [HttpGet("GetAccepted")]
        public List<Book> GetAcceptedBooks(int userId)
        {
            return AILibrary.Models.User.GetAcceptedBooks(userId);
        }

        [HttpGet("GetPending")]
        public List<Object> GetPendingRequests(int userId)
        {
            return AILibrary.Models.User.GetPendingRequests(userId);
        }

        // GET: api/<UserController>
        [HttpGet("Notification")]
        public int GetNotificationCount(int userId)
        {
            return AILibrary.Models.User.GetNotificationCount(userId);
        }

        [HttpPost("AddReview")]
        public int AddReview(int userId, string bookId, string text, int rating)
        {
            return AILibrary.Models.User.AddReview(userId, bookId, text, rating);

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

        // GET: api/User/LibraryDetails
        [HttpGet("LibraryDetails")]
        public IActionResult GetUserLibraryDetails(int userId)
        {
            try
            {
                var details = AILibrary.Models.User.GetUserLibraryDetails(userId);
                return Ok(details);
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return StatusCode(500, "Internal server error");
            }
        }
        // GET: api/User/ThisWeeksRevenue
        [HttpGet("ThisWeeksRevenue")]
        public IActionResult GetThisWeeksRevenue()
        {
            try
            {
                float revenue = AILibrary.Models.User.GetThisWeeksRevenue();
                return Ok(revenue);
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return StatusCode(500, "Internal server error");
            }
        }
        // GET: api/User/AllUsersWithBookCount
        [HttpGet("AllUsersWithBookCount")]
        public IActionResult GetAllUsersWithBookCount()
        {
            try
            {
                DBservices dbs = new DBservices();
                List<object> users = dbs.GetAllUsersWithBookCount();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("BanUser")]
        public int BanUser(int userId)
        {
            return AILibrary.Models.User.BanUser(userId);
        }

        // PUT api/<UserController>/5
        [HttpPut("UnbanUser")]
        public int UnbanUser(int userId)
        {
            return AILibrary.Models.User.UnbanUser(userId);
        }



    }
}
