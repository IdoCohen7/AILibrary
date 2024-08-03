using AILibrary.DAL;

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

        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
            IsAdmin = false;
            IsActive = true;
        }

        public User() { }


        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public bool IsAdmin { get => isAdmin; set => isAdmin = value; }
        public bool IsActive { get => isActive; set => isActive = value; }

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

        static public int Register(string name, string email, string password)
        {
            DBservices dbs = new DBservices();
            return dbs.Register(name, email, password);
        }

        static public int AddToFavorites(int userId, string bookId)
        {
            DBservices db = new DBservices();
            return db.AddToFavorites(userId, bookId);

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




    }
}
