using AILibrary.DAL;
using System.Collections.Generic;

namespace AILibrary.Models
{
    public class User
    {
        int id;
        string name;
        string email;
        string password;
        bool isAdmin;
        bool isActive;
        string profilePic;
        string registrationDate;

        public User(string name, string email, string password, string profilePic, string registrationDate)
        {
            Name = name;
            Email = email;
            Password = password;
            IsAdmin = false;
            IsActive = true;
            ProfilePic = profilePic;
            RegistrationDate = registrationDate;
        }

        public User() { }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public bool IsAdmin { get => isAdmin; set => isAdmin = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public string ProfilePic { get => profilePic; set => profilePic = value; }
        public string RegistrationDate { get => registrationDate; set => registrationDate = value; }

    static public User Login(string email, string password)
        {
            DBservices dbs = new DBservices();
            return dbs.Login(email, password);
        }

        static public List<User> GetUsers()
        {
            DBservices db = new DBservices();
            return db.GetUsers();
        }

        static public int Register(string name, string email, string password, string profilePic)
        {
            DBservices dbs = new DBservices();
            return dbs.Register(name, email, password, profilePic);
        }

        static public int AddToFavorites(int userId, string bookId)
        {
            DBservices db = new DBservices();
            return db.AddToFavorites(userId, bookId);
        }

        static public int RemoveFromFavorites(int userId, string bookId)
        {
            DBservices db = new DBservices();
            return db.ReomveFromFavorites(userId, bookId);
        }

        static public int MarkAsRead(int userId, string bookId)
        {
            DBservices db = new DBservices();
            return db.MarkAsRead(userId, bookId);
        }

        static public List<Book> GetUsersFavorites(int userId)
        {
            DBservices db = new DBservices();
            return db.GetUsersFavorites(userId);
        }

        static public List<Book> GetUsersHistory(int userId)
        {
            DBservices db = new DBservices();
            return db.GetUsersHistory(userId);
        }

        static public int SendBookRequest(int buyerId, int sellerId, string bookId)
        {
            DBservices db = new DBservices();
            return db.SendBookRequest(buyerId, sellerId, bookId);
        }

        static public int AcceptBookRequest(int buyerId, int sellerId, string bookId)
        {
            DBservices db = new DBservices();
            return db.AcceptBookRequest(buyerId, sellerId, bookId);
        }

        static public int CancelBookRequest(int buyerId, int sellerId, string bookId)
        {
            DBservices db = new DBservices();
            return db.CancelBookRequest(buyerId, sellerId, bookId);
        }

        static public List<Book> GetAcceptedBooks(int userId)
        {
            DBservices db = new DBservices();
            return db.GetUserAcceptedBooks(userId);
        }

        static public List<Object> GetPendingRequests(int userId)
        {
            DBservices db = new DBservices();
            return db.GetPendingRequests(userId);
        }

        static public int GetNotificationCount(int userId)
        {
            DBservices db = new DBservices();
            return db.GetNotificationCount(userId);
        }

        static public int AddReview(int userId, string bookId, string text, int rating)
        {
            DBservices db = new DBservices();
            return (db.AddReview(userId, bookId, text, rating));
        }

        static public int ChangePassword(int userId, string password)
        {
            DBservices db = new DBservices();
            return (db.ChangePassword(userId, password));
        }

        static public int ChangeUsername(int userId, string name)
        {
            DBservices db = new DBservices();
            return (db.ChangeUsername(userId, name));   
        }

        static public int ChangeProfilePicture(int userId, string picture) { 
            DBservices db=new DBservices();
            return (db.ChangeProfilePicture(userId, picture));
        }

        public static List<object> GetUserLibraryDetails(int userId)
        {
            DBservices dbs = new DBservices();
            return dbs.GetUserLibraryDetails(userId);
        }

        public static List<object> GetAllUsersWithBookCount()
        {
            DBservices dbs = new DBservices();
            return dbs.GetAllUsersWithBookCount();
        }

        static public int BanUser(int userId)
        {
            DBservices dbs = new DBservices();
            return dbs.BanUser(userId);
        }

        static public int UnbanUser(int userId)
        {
            DBservices dbs = new DBservices();
            return dbs.UnbanUser(userId);
        }


    }
}
