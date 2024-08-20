using AILibrary.Models;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;


namespace AILibrary.DAL
{
    public class DBservices
    {
        public SqlDataAdapter da;
        public DataTable dt;

        public DBservices()
        {
        }
        public SqlConnection connect(String conString)
        {

            // read the connection string from the configuration file
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("myProjDB");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        private SqlCommand CreateCommandWithStoredProcedureNoParameters(String spName, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            return cmd;
        }

        private SqlCommand CreateCommandWithStoredProcedure_Book(String spName, SqlConnection con, string bookId)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            cmd.Parameters.AddWithValue("@bookId", bookId);

            return cmd;
        }

        public List<Book> GetBooks()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<Book> books = new List<Book>(); // Initialize the list of books

            try
            {
                con = connect("myProjDB"); // Create the connection
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetBooks", con); // Create the command for reading books

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Book b = new Book
                    {
                        Id = dataReader["id"].ToString(),
                        Title = dataReader["title"].ToString(),
                        Subtitle = dataReader["subtitle"].ToString(),
                        Authors = dataReader["authors"].ToString().Split(new[] { ", " }, StringSplitOptions.None),
                        PublishedDate = dataReader["publishedDate"].ToString(),
                        PageCount = Convert.ToInt32(dataReader["pageCount"]),
                        IsMagazine = Convert.ToBoolean(dataReader["isMagazine"]),
                        IsMature = Convert.ToBoolean(dataReader["isMature"]),
                        IsEbook = Convert.ToBoolean(dataReader["isEbook"]),
                        Language = dataReader["language"].ToString(),
                        Price = Convert.ToSingle(dataReader["price"]),
                        Thumbnail = dataReader["thumbnail"].ToString(),
                        PreviewLink = dataReader["previewLink"].ToString(),
                        InfoLink = dataReader["infoLink"].ToString(),
                        EpubLink = dataReader["epubLink"].ToString(),
                        PdfLink = dataReader["pdfLink"].ToString(),
                        RatingAverage = Convert.ToSingle(dataReader["ratingAvg"]),
                        RatingCount = Convert.ToInt32(dataReader["ratingCount"]),
                        Description = dataReader["description"].ToString(),
                        TextSnippet = dataReader["textSnippet"].ToString(),
                        Category = dataReader["category"].ToString()
                    };

                    books.Add(b); // Add the book to the list
                }

                return books; // Return the list of books
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving books", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

        public List<Author> GetAuthors()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<Author> authors = new List<Author>(); // Initialize the list of authors

            try
            {
                con = connect("myProjDB"); // Create the connection
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetAuthors", con); // Create the command for reading authors

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Author author = new Author
                    {
                        Id = Convert.ToInt32(dataReader["id"]),
                        Name = dataReader["name"].ToString()
                    };

                    authors.Add(author); // Add the author to the list
                }

                return authors; // Return the list of authors
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving authors", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }



        private SqlCommand CreateCommandWithStoredProcedure_CreateBook(string spName, SqlConnection con, Book book)
        {
            SqlCommand cmd = new SqlCommand(); // Create the command object

            cmd.Connection = con;              // Assign the connection to the command object

            cmd.CommandText = spName;          // Specify the stored procedure name

            cmd.CommandTimeout = 10;           // Time to wait for execution

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // Type of command

            // Add parameters with values from the Book object
            cmd.Parameters.AddWithValue("@id", book.Id ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@title", book.Title ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@subtitle", book.Subtitle ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@authors", string.Join(", ", book.Authors ?? Array.Empty<string>()) ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@publishedDate", book.PublishedDate ?? (object)DBNull.Value); // Keep as string
            cmd.Parameters.AddWithValue("@description", book.Description ?? (object)DBNull.Value); // Added parameter
            cmd.Parameters.AddWithValue("@textSnippet", book.TextSnippet ?? (object)DBNull.Value); // Added parameter
            cmd.Parameters.AddWithValue("@pageCount", book.PageCount);
            cmd.Parameters.AddWithValue("@thumbnail", book.Thumbnail ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@language", book.Language ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@previewLink", book.PreviewLink ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@infoLink", book.InfoLink ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@epubLink", book.EpubLink ?? (object)DBNull.Value); // Updated parameter name
            cmd.Parameters.AddWithValue("@pdfLink", book.PdfLink ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@isMagazine", book.IsMagazine);
            cmd.Parameters.AddWithValue("@isMature", book.IsMature);
            cmd.Parameters.AddWithValue("@isEbook", book.IsEbook);
            cmd.Parameters.AddWithValue("@price", book.Price);
            cmd.Parameters.AddWithValue("@ratingAvg", book.RatingAverage);
            cmd.Parameters.AddWithValue("@ratingCount", book.RatingCount);

            return cmd;
        }



        public int CreateBook(Book newBook)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // Create the connection

                cmd = CreateCommandWithStoredProcedure_CreateBook("SP_CreateBook", con, newBook); // Create the command

                int numEffected = cmd.ExecuteNonQuery(); // Execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't create book", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

        private SqlCommand CreateCommandWithStoredProcedure_AssignBookToAuthor(string spName, SqlConnection con, string bookId, string authorName)
        {
            SqlCommand cmd = new SqlCommand(); // Create the command object

            cmd.Connection = con;              // Assign the connection to the command object

            cmd.CommandText = spName;          // Specify the stored procedure name

            cmd.CommandTimeout = 10;           // Time to wait for execution

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // Type of command

            // Add parameters with values from the Book object
            cmd.Parameters.AddWithValue("@bookId", bookId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@authorName", authorName ?? (object)DBNull.Value);

            return cmd;
        }

        private SqlCommand CreateCommandWithStoredProcedure_CreateAuthor(string spName, SqlConnection con, Author author)
        {
            SqlCommand cmd = new SqlCommand(); // Create the command object

            cmd.Connection = con;              // Assign the connection to the command object

            cmd.CommandText = spName;          // Specify the stored procedure name

            cmd.CommandTimeout = 10;           // Time to wait for execution

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // Type of command

            // Add parameters with values from the Book object
            cmd.Parameters.AddWithValue("@id", author.Id);
            cmd.Parameters.AddWithValue("@name", author.Name);

            return cmd;
        }

        public int CreateAuthor(Author author)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // Create the connection

                cmd = CreateCommandWithStoredProcedure_CreateAuthor("SP_CreateAuthor", con, author); // Create the command

                int numEffected = cmd.ExecuteNonQuery(); // Execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't create author", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

        public int AssignBookToAuthor(string authorName, string bookId)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // Create the connection

                cmd = CreateCommandWithStoredProcedure_AssignBookToAuthor("SP_AssignBookToAuthor", con, bookId, authorName); // Create the command

                int numEffected = cmd.ExecuteNonQuery(); // Execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't assign book to author", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

        public List<User> GetUsers()
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetUsers", con); // create the command

                List<User> users = new List<User>();

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    User u = new User();
                    u.Id = Convert.ToInt32(dataReader["id"]);
                    u.Name = dataReader["name"].ToString();
                    u.Email = dataReader["email"].ToString();
                    u.Password = dataReader["password"].ToString();
                    u.IsAdmin = Convert.ToBoolean(dataReader["isAdmin"]);
                    u.IsActive = Convert.ToBoolean(dataReader["isActive"]);
                    u.ProfilePic = dataReader["profilePic"].ToString();
                    u.RegistrationDate = Convert.ToDateTime(dataReader["registrationDate"]).ToString("dd/MM/yyyy");
                    users.Add(u);
                }

                return users;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        private SqlCommand CreateCommandWithStoredProcedure_Login(String spName, SqlConnection con, string email, string password)
        {
            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;          // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            // Add parameters with values
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", password);

            return cmd;
        }

        public User Login(string email, string password)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
                cmd = CreateCommandWithStoredProcedure_Login("SP_Login", con, email, password); // create the command

                User user = new User();

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    user.Id = Convert.ToInt32(dataReader["id"]);
                    user.Name = dataReader["name"].ToString();
                    user.Email = dataReader["email"].ToString();
                    user.Password = dataReader["password"].ToString();
                    user.ProfilePic = dataReader["profilePic"].ToString();
                    user.RegistrationDate = Convert.ToDateTime(dataReader["registrationDate"]).ToString("dd/MM/yyyy");
                    int adminStatus = Convert.ToInt32(dataReader["isAdmin"]);
                    if (adminStatus == 1)
                    {
                        user.IsAdmin = true;
                    }
                    else
                    {
                        user.IsAdmin = false;
                    }
                    int activeStatus = Convert.ToInt32(dataReader["isActive"]);
                    if (activeStatus == 1)
                    {
                        user.IsActive = true;
                    }
                    else
                    {
                        user.IsActive = false;
                    }
                }

                return user;
            }
            catch (Exception ex)
            {
                // write to log
                throw new Exception("Couldn't retrieve any user", ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        private SqlCommand CreateCommandWithStoredProcedure_Register(String spName, SqlConnection con, string name, string email, string password, string profilePic)
        {
            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;          // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            // Add parameters with values
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@profilePic", profilePic);

            return cmd;
        }

        public int Register(string name, string email, string password, string profilePic)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection

                cmd = CreateCommandWithStoredProcedure_Register("SP_Register", con, name, email, password, profilePic); // create the command

                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }

            catch (Exception ex)
            {
                throw new Exception("Couldn't register", ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        private SqlCommand CreateCommandWithStoredProcedure_UserAndBook(String spName, SqlConnection con, int userId, string bookId)
        {
            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;          // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            // Add parameters with values
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@bookId", bookId);

            return cmd;
        }

        public int AddToFavorites(int userId, string bookId)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection

                cmd = CreateCommandWithStoredProcedure_UserAndBook("SP_AddToFavorites", con, userId, bookId); // create the command

                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }

            catch (SqlException ex)
            {
                if (ex.Number == 2627) // SQL Server error code for a primary key violation
                {
                    return -1; // Return a specific value for duplicate entries
                }
                throw new Exception("Couldn't add to favorites", ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public int ReomveFromFavorites(int userId, string bookId)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection

                cmd = CreateCommandWithStoredProcedure_UserAndBook("SP_RemoveFromFavorites", con, userId, bookId); // create the command

                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }

            catch (SqlException ex)
            {
                if (ex.Number == 2627) // SQL Server error code for a primary key violation
                {
                    return -1; // Return a specific value for duplicate entries
                }
                throw new Exception("Couldn't remove from favorites", ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public int MarkAsRead(int userId, string bookId)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection

                cmd = CreateCommandWithStoredProcedure_UserAndBook("SP_MarkAsRead", con, userId, bookId); // create the command

                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }

            catch (SqlException ex)
            {
                if (ex.Number == 2627) // SQL Server error code for a primary key violation
                {
                    return -1; // Return a specific value for duplicate entries
                }
                throw new Exception("Couldn't add to history", ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public int BanUser(int userId)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection

                cmd = CreateCommandWithStoredProcedure_User("SP_BanUser", con, userId); // create the command

                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }

            catch (SqlException ex)
            {
                if (ex.Number == 2627) // SQL Server error code for a primary key violation
                {
                    return -1; // Return a specific value for duplicate entries
                }
                throw new Exception("Couldn't ban user", ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public int UnbanUser(int userId)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection

                cmd = CreateCommandWithStoredProcedure_User("SP_UnbanUser", con, userId); // create the command

                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }

            catch (SqlException ex)
            {
                if (ex.Number == 2627) // SQL Server error code for a primary key violation
                {
                    return -1; // Return a specific value for duplicate entries
                }
                throw new Exception("Couldn't unban user", ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public int ChangePassword(int userId, string password)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection

                cmd = CreateCommandWithStoredProcedure_UserPassword("SP_ChangePassword", con, userId, password); // create the command

                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }

            catch (SqlException ex)
            {
                if (ex.Number == 2627) // SQL Server error code for a primary key violation
                {
                    return -1; // Return a specific value for duplicate entries
                }
                throw new Exception("Couldn't change password", ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public int ChangeUsername(int userId, string name)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection

                cmd = CreateCommandWithStoredProcedure_UserName("SP_ChangeUsername", con, userId, name); // create the command

                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }

            catch (SqlException ex)
            {
                if (ex.Number == 2627) // SQL Server error code for a primary key violation
                {
                    return -1; // Return a specific value for duplicate entries
                }
                throw new Exception("Couldn't change username", ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public int ChangeProfilePicture(int userId, string pictureSrc)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection

                cmd = CreateCommandWithStoredProcedure_UserPicture("SP_ChangeProfilePicture", con, userId, pictureSrc); // create the command

                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }

            catch (SqlException ex)
            {
                if (ex.Number == 2627) // SQL Server error code for a primary key violation
                {
                    return -1; // Return a specific value for duplicate entries
                }
                throw new Exception("Couldn't change picture", ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        private SqlCommand CreateCommandWithStoredProcedure_GetAuthorsBooks(String spName, SqlConnection con, int id)
        {
            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;          // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            // Add parameters with values
            cmd.Parameters.AddWithValue("@authorId", id);

            return cmd;
        }

        public List<Book> GetAuthorsBooks(int id)
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<Book> books = new List<Book>(); // Initialize the list of books

            try
            {
                con = connect("myProjDB"); // Create the connection
                cmd = CreateCommandWithStoredProcedure_GetAuthorsBooks("SP_GetAuthorsBooks", con, id); // Create the command for reading books

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Book b = new Book
                    {
                        Id = dataReader["id"].ToString(),
                        Title = dataReader["title"].ToString(),
                        Subtitle = dataReader["subtitle"].ToString(),
                        Authors = dataReader["authors"].ToString().Split(new[] { ", " }, StringSplitOptions.None),
                        PublishedDate = dataReader["publishedDate"].ToString(),
                        PageCount = Convert.ToInt32(dataReader["pageCount"]),
                        IsMagazine = Convert.ToBoolean(dataReader["isMagazine"]),
                        IsMature = Convert.ToBoolean(dataReader["isMature"]),
                        IsEbook = Convert.ToBoolean(dataReader["isEbook"]),
                        Language = dataReader["language"].ToString(),
                        Price = Convert.ToSingle(dataReader["price"]),
                        Thumbnail = dataReader["thumbnail"].ToString(),
                        PreviewLink = dataReader["previewLink"].ToString(),
                        InfoLink = dataReader["infoLink"].ToString(),
                        EpubLink = dataReader["epubLink"].ToString(),
                        PdfLink = dataReader["pdfLink"].ToString(),
                        RatingAverage = Convert.ToSingle(dataReader["ratingAvg"]),
                        RatingCount = Convert.ToInt32(dataReader["ratingCount"]),
                        Description = dataReader["description"].ToString(),
                        TextSnippet = dataReader["textSnippet"].ToString(),
                        Category = dataReader["category"].ToString()
                    };

                    books.Add(b); // Add the book to the list
                }

                return books; // Return the list of books
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving books", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

        private SqlCommand CreateCommandWithStoredProcedure_User(String spName, SqlConnection con, int userId)
        {
            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;          // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            // Add parameters with values
            cmd.Parameters.AddWithValue("@userId", userId);

            return cmd;
        }

        private SqlCommand CreateCommandWithStoredProcedure_Author(String spName, SqlConnection con, int authorId)
        {
            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;          // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            // Add parameters with values
            cmd.Parameters.AddWithValue("@authorId", authorId);

            return cmd;
        }



        private SqlCommand CreateCommandWithStoredProcedure_UserPassword(String spName, SqlConnection con, int userId, string password)
        {
            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;          // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            // Add parameters with values
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@newPassword", password);

            return cmd;
        }

        private SqlCommand CreateCommandWithStoredProcedure_UserName(String spName, SqlConnection con, int userId, string name)
        {
            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;          // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            // Add parameters with values
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@newName", name);

            return cmd;
        }

        private SqlCommand CreateCommandWithStoredProcedure_UserPicture(String spName, SqlConnection con, int userId, string pictureSrc)
        {
            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;          // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            // Add parameters with values
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@pictureSrc", pictureSrc);

            return cmd;
        }

        public List<Book> GetUsersFavorites(int id)
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<Book> books = new List<Book>(); // Initialize the list of books

            try
            {
                con = connect("myProjDB"); // Create the connection
                cmd = CreateCommandWithStoredProcedure_User("SP_GetUsersFavorites", con, id); // Create the command for reading books

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Book b = new Book
                    {
                        Id = dataReader["id"].ToString(),
                        Title = dataReader["title"].ToString(),
                        Subtitle = dataReader["subtitle"].ToString(),
                        Authors = dataReader["authors"].ToString().Split(new[] { ", " }, StringSplitOptions.None),
                        PublishedDate = dataReader["publishedDate"].ToString(),
                        PageCount = Convert.ToInt32(dataReader["pageCount"]),
                        IsMagazine = Convert.ToBoolean(dataReader["isMagazine"]),
                        IsMature = Convert.ToBoolean(dataReader["isMature"]),
                        IsEbook = Convert.ToBoolean(dataReader["isEbook"]),
                        Language = dataReader["language"].ToString(),
                        Price = Convert.ToSingle(dataReader["price"]),
                        Thumbnail = dataReader["thumbnail"].ToString(),
                        PreviewLink = dataReader["previewLink"].ToString(),
                        InfoLink = dataReader["infoLink"].ToString(),
                        EpubLink = dataReader["epubLink"].ToString(),
                        PdfLink = dataReader["pdfLink"].ToString(),
                        RatingAverage = Convert.ToSingle(dataReader["ratingAvg"]),
                        RatingCount = Convert.ToInt32(dataReader["ratingCount"]),
                        Description = dataReader["description"].ToString(),
                        TextSnippet = dataReader["textSnippet"].ToString(),
                        Category = dataReader["category"].ToString()
                    };

                    books.Add(b); // Add the book to the list
                }

                return books; // Return the list of books
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving books", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

        public List<Book> GetUsersHistory(int id)
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<Book> books = new List<Book>(); // Initialize the list of books

            try
            {
                con = connect("myProjDB"); // Create the connection
                cmd = CreateCommandWithStoredProcedure_User("SP_GetUsersHistory", con, id); // Create the command for reading books

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Book b = new Book
                    {
                        Id = dataReader["id"].ToString(),
                        Title = dataReader["title"].ToString(),
                        Subtitle = dataReader["subtitle"].ToString(),
                        Authors = dataReader["authors"].ToString().Split(new[] { ", " }, StringSplitOptions.None),
                        PublishedDate = dataReader["publishedDate"].ToString(),
                        PageCount = Convert.ToInt32(dataReader["pageCount"]),
                        IsMagazine = Convert.ToBoolean(dataReader["isMagazine"]),
                        IsMature = Convert.ToBoolean(dataReader["isMature"]),
                        IsEbook = Convert.ToBoolean(dataReader["isEbook"]),
                        Language = dataReader["language"].ToString(),
                        Price = Convert.ToSingle(dataReader["price"]),
                        Thumbnail = dataReader["thumbnail"].ToString(),
                        PreviewLink = dataReader["previewLink"].ToString(),
                        InfoLink = dataReader["infoLink"].ToString(),
                        EpubLink = dataReader["epubLink"].ToString(),
                        PdfLink = dataReader["pdfLink"].ToString(),
                        RatingAverage = Convert.ToSingle(dataReader["ratingAvg"]),
                        RatingCount = Convert.ToInt32(dataReader["ratingCount"]),
                        Description = dataReader["description"].ToString(),
                        TextSnippet = dataReader["textSnippet"].ToString(),
                        Category = dataReader["category"].ToString()
                    };

                    books.Add(b); // Add the book to the list
                }

                return books; // Return the list of books
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving books", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

        public Book GetRandomBook()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            Book book = null; // Initialize the book object

            try
            {
                con = connect("myProjDB"); // Create the connection
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetRandomBook", con); // Create the command for reading books

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dataReader.Read())
                {
                    book = new Book
                    {
                        Id = dataReader["id"].ToString(),
                        Title = dataReader["title"].ToString(),
                        Subtitle = dataReader["subtitle"].ToString(),
                        Authors = dataReader["authors"].ToString().Split(new[] { ", " }, StringSplitOptions.None),
                        PublishedDate = dataReader["publishedDate"].ToString(),
                        PageCount = Convert.ToInt32(dataReader["pageCount"]),
                        IsMagazine = Convert.ToBoolean(dataReader["isMagazine"]),
                        IsMature = Convert.ToBoolean(dataReader["isMature"]),
                        IsEbook = Convert.ToBoolean(dataReader["isEbook"]),
                        Language = dataReader["language"].ToString(),
                        Price = Convert.ToSingle(dataReader["price"]),
                        Thumbnail = dataReader["thumbnail"].ToString(),
                        PreviewLink = dataReader["previewLink"].ToString(),
                        InfoLink = dataReader["infoLink"].ToString(),
                        EpubLink = dataReader["epubLink"].ToString(),
                        PdfLink = dataReader["pdfLink"].ToString(),
                        RatingAverage = Convert.ToSingle(dataReader["ratingAvg"]),
                        RatingCount = Convert.ToInt32(dataReader["ratingCount"]),
                        Description = dataReader["description"].ToString(), // New property
                        TextSnippet = dataReader["textSnippet"].ToString() // New property
                    };
                }

                return book; // Return the single book
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving book", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }
        public List<string> Get16ThumbnailRandomBooks()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<string> thumbnails = new List<string>(); // List of thumbnails

            try
            {
                con = connect("myProjDB"); // Create the connection
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_Get16ThumbnailRandomBooks", con); // Create the command for the stored procedure

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    string thumbnail = dataReader["thumbnail"].ToString();
                    thumbnails.Add(thumbnail); // Add the thumbnail to the list
                }

                return thumbnails; // Return the list of thumbnails
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving thumbnails", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }


        public List<Question> GetRandomQuestions()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<Question> questions = new List<Question>(); // Initialize the list of questions

            try
            {
                con = connect("myProjDB"); // Create the connection
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetRandomQuestions", con); // Adjusted stored procedure name

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Question q = new Question
                    {
                        Id = Convert.ToInt32(dataReader["id"]),
                        QuestionText = dataReader["questionText"].ToString(),
                        Answer1 = dataReader["answer1"].ToString(),
                        Answer2 = dataReader["answer2"].ToString(),
                        Answer3 = dataReader["answer3"].ToString(),
                        Answer4 = dataReader["answer4"].ToString(),
                        CorrectAnswer = dataReader["correctAnswer"].ToString()
                    };

                    questions.Add(q); // Add the question to the list
                }

                return questions; // Return the list of questions
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving questions", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

        public List<Book> SearchBooksByParameter(string parameter, string searchText)
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<Book> books = new List<Book>(); // Initialize the list of books

            try
            {
                con = connect("myProjDB"); // Create the connection
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetBooks", con); // Create the command for reading books

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Book b = new Book
                    {
                        Id = dataReader["id"].ToString(),
                        Title = dataReader["title"].ToString(),
                        Subtitle = dataReader["subtitle"].ToString(),
                        Authors = dataReader["authors"].ToString().Split(new[] { ", " }, StringSplitOptions.None),
                        PublishedDate = dataReader["publishedDate"].ToString(),
                        PageCount = Convert.ToInt32(dataReader["pageCount"]),
                        IsMagazine = Convert.ToBoolean(dataReader["isMagazine"]),
                        IsMature = Convert.ToBoolean(dataReader["isMature"]),
                        IsEbook = Convert.ToBoolean(dataReader["isEbook"]),
                        Language = dataReader["language"].ToString(),
                        Price = Convert.ToSingle(dataReader["price"]),
                        Thumbnail = dataReader["thumbnail"].ToString(),
                        PreviewLink = dataReader["previewLink"].ToString(),
                        InfoLink = dataReader["infoLink"].ToString(),
                        EpubLink = dataReader["epubLink"].ToString(),
                        PdfLink = dataReader["pdfLink"].ToString(),
                        RatingAverage = Convert.ToSingle(dataReader["ratingAvg"]),
                        RatingCount = Convert.ToInt32(dataReader["ratingCount"]),
                        Description = dataReader["description"].ToString(),
                        TextSnippet = dataReader["textSnippet"].ToString(),
                        Category = dataReader["category"].ToString()
                    };

                    if (parameter == "Content")
                    {
                        // Check if the PDF contains the search text
                        if ((b.Description != null && b.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase)))
                        {
                            books.Add(b); // Add the book to the list if it contains the text
                        }
                    }

                    if (parameter == "Author")
                    {
                        if (b.Authors != null)
                        {
                            foreach (var author in b.Authors)
                            {
                                if (author.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                                {
                                    books.Add(b);
                                    break;
                                }
                            }
                        }
                    }

                    if (parameter == "Title")
                    {
                        if (b.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                        {
                            books.Add(b);
                        }
                    }

                }

                return books; // Return the list of books that contain the search text in their PDF
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving books", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

        public List<Object> GetMarketplace()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<Object> books = new List<Object>(); // Initialize the list of books

            try
            {
                con = connect("myProjDB"); // Create the connection
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetAllHistories", con); // Create the command for reading books

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    books.Add(new
                    {
                        Id = dataReader["BookId"].ToString(),
                        Title = dataReader["title"].ToString(),
                        Subtitle = dataReader["subtitle"].ToString(),
                        Authors = dataReader["authors"].ToString().Split(new[] { ", " }, StringSplitOptions.None),
                        PublishedDate = dataReader["publishedDate"].ToString(),
                        PageCount = Convert.ToInt32(dataReader["pageCount"]),
                        IsMagazine = Convert.ToBoolean(dataReader["isMagazine"]),
                        IsMature = Convert.ToBoolean(dataReader["isMature"]),
                        IsEbook = Convert.ToBoolean(dataReader["isEbook"]),
                        Language = dataReader["language"].ToString(),
                        Price = Convert.ToSingle(dataReader["price"]),
                        Thumbnail = dataReader["thumbnail"].ToString(),
                        PreviewLink = dataReader["previewLink"].ToString(),
                        InfoLink = dataReader["infoLink"].ToString(),
                        EpubLink = dataReader["epubLink"].ToString(),
                        PdfLink = dataReader["pdfLink"].ToString(),
                        RatingAverage = Convert.ToSingle(dataReader["ratingAvg"]),
                        RatingCount = Convert.ToInt32(dataReader["ratingCount"]),
                        Description = dataReader["description"].ToString(),
                        TextSnippet = dataReader["textSnippet"].ToString(),
                        Category = dataReader["category"].ToString(),
                        Username = dataReader["Username"].ToString(),
                       finishedDate = ((DateTime)dataReader["finishReadingDate"]).ToString("dd/MM/yyyy"),
                    UserId = dataReader["userId"].ToString()
                    });

                }

                return books; // Return the list of books
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving books", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }

        }
        public int GetPurchasesThisWeek()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            int totalBooksPurchased = 0; // Initialize the result

            try
            {
                con = connect("myProjDB"); // Create the connection
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetPurchasesThisWeek", con); // Create the command

                con.Open(); 
                totalBooksPurchased = Convert.ToInt32(cmd.ExecuteScalar());

                return totalBooksPurchased; // Return the count
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error retrieving the number of books purchased this week", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

        private SqlCommand CreateCommandWithStoredProcedure_BuyerSeller(string spName, SqlConnection con, int buyerId, int sellerId, string bookId)
        {
            SqlCommand cmd = new SqlCommand(); // Create the command object

            cmd.Connection = con;              // Assign the connection to the command object

            cmd.CommandText = spName;          // Specify the stored procedure name

            cmd.CommandTimeout = 10;           // Time to wait for execution

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // Type of command

            // Add parameters with values from the Book object
            cmd.Parameters.AddWithValue("@buyerId", buyerId);
            cmd.Parameters.AddWithValue("@sellerId", sellerId);
            cmd.Parameters.AddWithValue("@bookId", bookId);

            return cmd;
        }

        public int SendBookRequest(int buyerId, int sellerId, string bookId)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // Create the connection

                cmd = CreateCommandWithStoredProcedure_BuyerSeller("SP_SendBookRequest", con, buyerId, sellerId, bookId); // Create the command

                int numEffected = cmd.ExecuteNonQuery(); // Execute the command
                return numEffected;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // SQL Server error code for a primary key violation
                {
                    return -1; // Return a specific value for duplicate entries
                }
                throw new Exception("Couldn't send request", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

        public int CancelBookRequest(int buyerId, int sellerId, string bookId)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // Create the connection

                cmd = CreateCommandWithStoredProcedure_BuyerSeller("SP_CancelBookRequest", con, buyerId, sellerId, bookId); // Create the command

                int numEffected = cmd.ExecuteNonQuery(); // Execute the command
                return numEffected;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // SQL Server error code for a primary key violation
                {
                    return -1; // Return a specific value for duplicate entries
                }
                throw new Exception("Couldn't cancel request", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

        public List<Book> GetUserAcceptedBooks(int userId)
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<Book> books = new List<Book>(); // Initialize the list of books

            try
            {
                con = connect("myProjDB"); // Create the connection
                cmd = CreateCommandWithStoredProcedure_User("SP_GetAcceptedBooks", con, userId); // Create the command for reading books

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Book b = new Book
                    {
                        Id = dataReader["id"].ToString(),
                        Title = dataReader["title"].ToString(),
                        Subtitle = dataReader["subtitle"].ToString(),
                        Authors = dataReader["authors"].ToString().Split(new[] { ", " }, StringSplitOptions.None),
                        PublishedDate = dataReader["publishedDate"].ToString(),
                        PageCount = Convert.ToInt32(dataReader["pageCount"]),
                        IsMagazine = Convert.ToBoolean(dataReader["isMagazine"]),
                        IsMature = Convert.ToBoolean(dataReader["isMature"]),
                        IsEbook = Convert.ToBoolean(dataReader["isEbook"]),
                        Language = dataReader["language"].ToString(),
                        Price = Convert.ToSingle(dataReader["price"]),
                        Thumbnail = dataReader["thumbnail"].ToString(),
                        PreviewLink = dataReader["previewLink"].ToString(),
                        InfoLink = dataReader["infoLink"].ToString(),
                        EpubLink = dataReader["epubLink"].ToString(),
                        PdfLink = dataReader["pdfLink"].ToString(),
                        RatingAverage = Convert.ToSingle(dataReader["ratingAvg"]),
                        RatingCount = Convert.ToInt32(dataReader["ratingCount"]),
                        Description = dataReader["description"].ToString(),
                        TextSnippet = dataReader["textSnippet"].ToString(),
                        Category = dataReader["category"].ToString()
                    };

                    books.Add(b); // Add the book to the list
                }

                return books; // Return the list of books
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving books", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

        public List<Object> GetPendingRequests(int userId)
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<Object> books = new List<Object>(); // Initialize the list of books

            try
            {
                con = connect("myProjDB"); // Create the connection
                cmd = CreateCommandWithStoredProcedure_User("SP_GetUserPendingRequests", con, userId); // Create the command for reading books

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    books.Add(new
                    {
                        BuyerId = dataReader["buyerId"].ToString(),
                        BuyerName = dataReader["name"].ToString(),
                        BuyerProfilePic = dataReader["profilePic"].ToString(),
                        SellerId = dataReader["sellerId"].ToString(),
                        Id = dataReader["bookId"].ToString(),
                        Title = dataReader["title"].ToString(),
                        RequestDate = dataReader["requestDate"].ToString(),
                        Price = Convert.ToSingle(dataReader["price"]),
                        Thumbnail = dataReader["thumbnail"].ToString()
                    });

                }

                return books; // Return the list of books
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving requests", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }


        }

        public int GetNotificationCount(int userId)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // Create the connection

                cmd = CreateCommandWithStoredProcedure_User("SP_GetNotificationCount", con, userId); // Create the command

                int notificationCount = (int)cmd.ExecuteScalar(); // Execute the command and get the count
                return notificationCount;
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't get number", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

        public int AcceptBookRequest(int buyerId, int sellerId, string bookId)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // Create the connection

                cmd = CreateCommandWithStoredProcedure_BuyerSeller("SP_AcceptBookRequest", con, buyerId, sellerId, bookId); // Create the command

                int numEffected = cmd.ExecuteNonQuery(); // Execute the command
                return numEffected;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // SQL Server error code for a primary key violation
                {
                    return -1; // Return a specific value for duplicate entries
                }
                throw new Exception("Couldn't accept request", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }
        public int GetPurchasesThisWeek()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            int purchasesThisWeek = 0;

            try
            {
                con = connect("myProjDB"); // יצירת החיבור ל-DB
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetPurchasesThisWeek", con); // יצירת הפקודה לקריאה ל-SP

                // הוצאת התוצאה כערך סקלרי
                purchasesThisWeek = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                // טיפול בשגיאות
                throw new Exception("Error retrieving the number of purchases this week", ex);
            }
            finally
            {
                if (con != null)
                {
                    // סגירת החיבור ל-DB
                    con.Close();
                }
            }

            return purchasesThisWeek;
        }
        public List<Book> GetTopRatedBooks()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<Book> books = new List<Book>();

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetTopRatedBooks", con);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Book book = new Book
                    {
                        Id = dataReader["id"].ToString(),
                        Title = dataReader["title"].ToString(),
                        Subtitle = dataReader["subtitle"].ToString(),
                        Authors = dataReader["authors"].ToString().Split(new[] { ", " }, StringSplitOptions.None),
                        PublishedDate = dataReader["publishedDate"].ToString(),
                        PageCount = Convert.ToInt32(dataReader["pageCount"]),
                        IsMagazine = Convert.ToBoolean(dataReader["isMagazine"]),
                        IsMature = Convert.ToBoolean(dataReader["isMature"]),
                        IsEbook = Convert.ToBoolean(dataReader["isEbook"]),
                        Language = dataReader["language"].ToString(),
                        Price = Convert.ToSingle(dataReader["price"]),
                        Thumbnail = dataReader["thumbnail"].ToString(),
                        PreviewLink = dataReader["previewLink"].ToString(),
                        InfoLink = dataReader["infoLink"].ToString(),
                        EpubLink = dataReader["epubLink"].ToString(),
                        PdfLink = dataReader["pdfLink"].ToString(),
                        RatingAverage = Convert.ToSingle(dataReader["ratingAvg"]),
                        RatingCount = Convert.ToInt32(dataReader["ratingCount"]),
                        Description = dataReader["description"].ToString(),
                        TextSnippet = dataReader["textSnippet"].ToString(),
                    };
                    books.Add(book);
                }

                return books;
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving books", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }
        public List<Book> GetMostNewBooks()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<Book> books = new List<Book>();

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetMostNewBooks", con);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Book book = new Book
                    {
                        Id = dataReader["id"].ToString(),
                        Title = dataReader["title"].ToString(),
                        Subtitle = dataReader["subtitle"].ToString(),
                        Authors = dataReader["authors"].ToString().Split(new[] { ", " }, StringSplitOptions.None),
                        PublishedDate = dataReader["publishedDate"].ToString(),
                        PageCount = Convert.ToInt32(dataReader["pageCount"]),
                        IsMagazine = Convert.ToBoolean(dataReader["isMagazine"]),
                        IsMature = Convert.ToBoolean(dataReader["isMature"]),
                        IsEbook = Convert.ToBoolean(dataReader["isEbook"]),
                        Language = dataReader["language"].ToString(),
                        Price = Convert.ToSingle(dataReader["price"]),
                        Thumbnail = dataReader["thumbnail"].ToString(),
                        PreviewLink = dataReader["previewLink"].ToString(),
                        InfoLink = dataReader["infoLink"].ToString(),
                        EpubLink = dataReader["epubLink"].ToString(),
                        PdfLink = dataReader["pdfLink"].ToString(),
                        RatingAverage = Convert.ToSingle(dataReader["ratingAvg"]),
                        RatingCount = Convert.ToInt32(dataReader["ratingCount"]),
                        Description = dataReader["description"].ToString(),
                        TextSnippet = dataReader["textSnippet"].ToString(),
                    };
                    books.Add(book);
                }

                return books;
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving books", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }
        public List<object> GetUserLibraryDetails()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<object> libraryDetails = new List<object>();

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetUserLibraryDetails", con);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    libraryDetails.Add(new
                    {
                        UserId = dataReader["UserId"].ToString(),
                        UserName = dataReader["UserName"].ToString(),
                        Email = dataReader["email"].ToString(),
                        BookId = dataReader["BookId"].ToString(),
                        Title = dataReader["title"].ToString(),
                        Authors = dataReader["authors"].ToString().Split(new[] { ", " }, StringSplitOptions.None),
                        Status = dataReader["Status"].ToString()
                    });
                }

                return libraryDetails;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving user library details", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public List<object> GetAuthorPopularity()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<object> authorPopularity = new List<object>();

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetAuthorPopularity", con);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    authorPopularity.Add(new
                    {
                        AuthorId = dataReader["AuthorId"].ToString(),
                        Name = dataReader["name"].ToString(),
                        NumberOfAppearances = Convert.ToInt32(dataReader["NumberOfAppearances"])
                    });
                }

                return authorPopularity;
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving author popularity", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public List<object> GetBookPopularity()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<object> bookPopularity = new List<object>();

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetBookPopularity", con);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    bookPopularity.Add(new
                    {
                        BookId = dataReader["BookId"].ToString(),
                        Title = dataReader["title"].ToString(),
                        NumberOfAppearances = Convert.ToInt32(dataReader["NumberOfAppearances"])
                    });
                }

                return bookPopularity;
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving book popularity", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public List<Book> GetAvailableBooks()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<Book> availableBooks = new List<Book>();

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetAvailableBooks", con);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    availableBooks.Add(new Book
                    {
                        Id = dataReader["id"].ToString(),
                        Title = dataReader["title"].ToString(),
                        Subtitle = dataReader["subtitle"].ToString(),
                        Authors = dataReader["authors"].ToString().Split(','), // Assuming authors are stored as a comma-separated string
                        PublishedDate = dataReader["publishedDate"].ToString(),
                        PageCount = Convert.ToInt32(dataReader["pageCount"]),
                        IsMagazine = Convert.ToBoolean(dataReader["isMagazine"]),
                        IsMature = Convert.ToBoolean(dataReader["isMature"]),
                        IsEbook = Convert.ToBoolean(dataReader["isEbook"]),
                        Language = dataReader["language"].ToString(),
                        Price = Convert.ToSingle(dataReader["price"]),
                        Thumbnail = dataReader["thumbnail"].ToString(),
                        PreviewLink = dataReader["previewLink"].ToString(),
                        InfoLink = dataReader["infoLink"].ToString(),
                        EpubLink = dataReader["epubLink"].ToString(),
                        PdfLink = dataReader["pdfLink"].ToString(),
                        RatingAverage = Convert.ToSingle(dataReader["ratingAvg"]),
                        RatingCount = Convert.ToInt32(dataReader["ratingCount"]),
                        Description = dataReader["description"].ToString(),
                        TextSnippet = dataReader["textSnippet"].ToString(),
                        Category = dataReader["category"].ToString()
                    });
                }

                return availableBooks;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving available books", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }
        public int GetAvailableBooksCount()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            int availableBooksCount = 0;

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetAvailableBooksCount", con);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dataReader.Read())
                {
                    availableBooksCount = Convert.ToInt32(dataReader["AvailableBooksCount"]);
                }

                return availableBooksCount;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving available books count", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public float GetThisWeeksRevenue()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            float revenue = 0;

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetThisWeeksRevenue", con);

                // Execute the command and read the result
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dataReader.Read())
                {
                    revenue = Convert.ToSingle(dataReader["Revenue"]);
                }

                return revenue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving this week's revenue", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public List<Object> GetAllUsersWithBookCount()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<Object> users = new List<Object>(); // Initialize the list of books

            try
            {
                con = connect("myProjDB"); // Create the connection
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetAllUsersWithBookCount", con); // Create the command for reading books

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    users.Add(new
                    {
                        Id = Convert.ToInt32(dataReader["Id"]),
                        Name = dataReader["Name"].ToString(),
                        Email = dataReader["Email"].ToString(),
                        ProfilePic = dataReader["ProfilePic"].ToString(),
                        IsActive = Convert.ToBoolean(dataReader["IsActive"]),
                        TotalBooksPurchased = Convert.ToInt32(dataReader["TotalBooksPurchased"])
                    });

                }

                return users; // Return the list of books
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving books", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }
        public int GetTotalBooksRead()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            int totalBooksRead = 0;

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetTotalBooksRead", con);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dataReader.Read())
                {
                    totalBooksRead = Convert.ToInt32(dataReader["TotalBooksRead"]);
                }

                return totalBooksRead;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving total books read", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public int GetTotalBooksPurchased()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            int totalBooksPurchased = 0;

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetTotalBooksPurchased", con);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dataReader.Read())
                {
                    totalBooksPurchased = Convert.ToInt32(dataReader["TotalBooksPurchased"]);
                }

                return totalBooksPurchased;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving total books purchased", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public int GetTotalBooksExchanged()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            int totalBooksExchanged = 0;

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetTotalBooksExchanged", con);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dataReader.Read())
                {
                    totalBooksExchanged = Convert.ToInt32(dataReader["TotalBooksExchanged"]);
                }

                return totalBooksExchanged;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving total books exchanged", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public List<object> GetBooksSummary()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<object> bookSummaries = new List<object>(); // Initialize the list of book summaries

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetBooksSummary", con); // Create the command for reading books
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    bookSummaries.Add(new
                    {
                        Title = dataReader["BookTitle"].ToString(),
                        Author = dataReader["AuthorName"].ToString(),
                        PublishedDate = Convert.ToDateTime(dataReader["PublishedDate"]),
                        BookType = dataReader["BookType"].ToString(),
                    });
                }

                return bookSummaries;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving book summaries", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }




        private SqlCommand CreateCommandWithStoredProcedure_UserBookReview(string spName, SqlConnection con, int userId, string bookId, string text, int rating)
        {
            SqlCommand cmd = new SqlCommand(); // Create the command object

            cmd.Connection = con;              // Assign the connection to the command object

            cmd.CommandText = spName;          // Specify the stored procedure name

            cmd.CommandTimeout = 10;           // Time to wait for execution

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // Type of command

            // Add parameters with values from the Book object
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@bookId", bookId);
            cmd.Parameters.AddWithValue("@reviewText", text);
            cmd.Parameters.AddWithValue("@rating", rating);

            return cmd;
        }

        public int AddReview(int userId, string bookId, string text, int rating)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // Create the connection

                cmd = CreateCommandWithStoredProcedure_UserBookReview("SP_AddReview", con, userId, bookId, text, rating); // Create the command

                int numEffected = cmd.ExecuteNonQuery(); // Execute the command
                return numEffected;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // SQL Server error code for a primary key violation
                {
                    return -1; // Return a specific value for duplicate entries
                }
                throw new Exception("Couldn't add review", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

        public List<Object> GetBookReviews(string bookId)
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<Object> reviews = new List<Object>();

            try
            {
                con = connect("myProjDB"); // Create the connection
                cmd = CreateCommandWithStoredProcedure_Book("SP_GetBookReviews", con, bookId); // Create the command for reading reviews

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    reviews.Add(new
                    {
                        BookId = dataReader["bookId"].ToString(),
                        UserId = dataReader["userId"].ToString(),
                        Date = Convert.ToDateTime(dataReader["date"]).ToString("yyyy-MM-dd"), // Convert date to string with format 'yyyy-MM-dd'
                        Rating = dataReader["rating"] != DBNull.Value ? Convert.ToInt32(dataReader["rating"]) : (int?)null, // Handle potential NULL values for rating
                        Text = dataReader["reviewText"].ToString(),
                        UserName = dataReader["name"].ToString(),
                        ProfilePic = dataReader["profilePic"] != DBNull.Value ? dataReader["profilePic"].ToString() : null // Handle potential NULL values for profilePic
                    });
                }

                return reviews; // Return the list of reviews
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving reviews", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

        public int GetPurchasesThisWeek()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            int totalBooksPurchased = 0; // Initialize the result

            try
            {
                con = connect("myProjDB"); // Create the connection
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetPurchasesThisWeek", con); // Create the command

                con.Open();
                totalBooksPurchased = Convert.ToInt32(cmd.ExecuteScalar());

                return totalBooksPurchased; // Return the count
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error retrieving the number of books purchased this week", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

        public List<Book> GetTopRatedBooks()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<Book> books = new List<Book>();

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetTopRatedBooks", con);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Book book = new Book
                    {
                        Id = dataReader["id"].ToString(),
                        Title = dataReader["title"].ToString(),
                        Subtitle = dataReader["subtitle"].ToString(),
                        Authors = dataReader["authors"].ToString().Split(new[] { ", " }, StringSplitOptions.None),
                        PublishedDate = dataReader["publishedDate"].ToString(),
                        PageCount = Convert.ToInt32(dataReader["pageCount"]),
                        IsMagazine = Convert.ToBoolean(dataReader["isMagazine"]),
                        IsMature = Convert.ToBoolean(dataReader["isMature"]),
                        IsEbook = Convert.ToBoolean(dataReader["isEbook"]),
                        Language = dataReader["language"].ToString(),
                        Price = Convert.ToSingle(dataReader["price"]),
                        Thumbnail = dataReader["thumbnail"].ToString(),
                        PreviewLink = dataReader["previewLink"].ToString(),
                        InfoLink = dataReader["infoLink"].ToString(),
                        EpubLink = dataReader["epubLink"].ToString(),
                        PdfLink = dataReader["pdfLink"].ToString(),
                        RatingAverage = Convert.ToSingle(dataReader["ratingAvg"]),
                        RatingCount = Convert.ToInt32(dataReader["ratingCount"]),
                        Description = dataReader["description"].ToString(),
                        TextSnippet = dataReader["textSnippet"].ToString(),
                    };
                    books.Add(book);
                }

                return books;
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving books", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

        public List<Book> GetMostNewBooks()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<Book> books = new List<Book>();

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetMostNewBooks", con);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Book book = new Book
                    {
                        Id = dataReader["id"].ToString(),
                        Title = dataReader["title"].ToString(),
                        Subtitle = dataReader["subtitle"].ToString(),
                        Authors = dataReader["authors"].ToString().Split(new[] { ", " }, StringSplitOptions.None),
                        PublishedDate = dataReader["publishedDate"].ToString(),
                        PageCount = Convert.ToInt32(dataReader["pageCount"]),
                        IsMagazine = Convert.ToBoolean(dataReader["isMagazine"]),
                        IsMature = Convert.ToBoolean(dataReader["isMature"]),
                        IsEbook = Convert.ToBoolean(dataReader["isEbook"]),
                        Language = dataReader["language"].ToString(),
                        Price = Convert.ToSingle(dataReader["price"]),
                        Thumbnail = dataReader["thumbnail"].ToString(),
                        PreviewLink = dataReader["previewLink"].ToString(),
                        InfoLink = dataReader["infoLink"].ToString(),
                        EpubLink = dataReader["epubLink"].ToString(),
                        PdfLink = dataReader["pdfLink"].ToString(),
                        RatingAverage = Convert.ToSingle(dataReader["ratingAvg"]),
                        RatingCount = Convert.ToInt32(dataReader["ratingCount"]),
                        Description = dataReader["description"].ToString(),
                        TextSnippet = dataReader["textSnippet"].ToString(),
                    };
                    books.Add(book);
                }

                return books;
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving books", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

        public List<object> GetUserLibraryDetails(int userId)
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<object> libraryDetails = new List<object>();

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedure_User("SP_GetUserLibraryDetails", con, userId);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    libraryDetails.Add(new
                    {
                        Id = dataReader["id"].ToString(),
                        Title = dataReader["title"].ToString(),
                        Subtitle = dataReader["subtitle"].ToString(),
                        Authors = dataReader["authors"].ToString().Split(new[] { ", " }, StringSplitOptions.None),
                        PublishedDate = dataReader["publishedDate"].ToString(),
                        PageCount = Convert.ToInt32(dataReader["pageCount"]),
                        IsMagazine = Convert.ToBoolean(dataReader["isMagazine"]),
                        IsMature = Convert.ToBoolean(dataReader["isMature"]),
                        IsEbook = Convert.ToBoolean(dataReader["isEbook"]),
                        Language = dataReader["language"].ToString(),
                        Price = Convert.ToSingle(dataReader["price"]),
                        Thumbnail = dataReader["thumbnail"].ToString(),
                        PreviewLink = dataReader["previewLink"].ToString(),
                        InfoLink = dataReader["infoLink"].ToString(),
                        EpubLink = dataReader["epubLink"].ToString(),
                        PdfLink = dataReader["pdfLink"].ToString(),
                        RatingAverage = Convert.ToSingle(dataReader["ratingAvg"]),
                        RatingCount = Convert.ToInt32(dataReader["ratingCount"]),
                        Description = dataReader["description"].ToString(),
                        TextSnippet = dataReader["textSnippet"].ToString(),
                        Category = dataReader["category"].ToString(),
                        FinishReadingDate = dataReader["finishReadingDate"] != DBNull.Value
    ? Convert.ToDateTime(dataReader["finishReadingDate"]).ToString("dd/MM/yyyy")
    : null
                });
                }

                return libraryDetails;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving user library details", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public List<object> GetBookLibraryDetails(string bookId)
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<object> libraryDetails = new List<object>();

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedure_Book("SP_GetBookLibraryDetails", con, bookId);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    libraryDetails.Add(new
                    {
                        Id = dataReader["UserId"].ToString(),
                        Name = dataReader["UserName"].ToString(),
                        ProfilePic = dataReader["profilePic"].ToString(),
                        Email = dataReader["Email"].ToString(),
                        FinishReadingDate = dataReader["finishReadingDate"] != DBNull.Value
    ? Convert.ToDateTime(dataReader["finishReadingDate"]).ToString("dd/MM/yyyy")
    : null
                    });
                }

                return libraryDetails;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving book library details", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public List<object> GetAuthorLibraryDetails(int authorId)
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<object> libraryDetails = new List<object>();

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedure_Author("SP_GetAuthorLibraryDetails", con, authorId);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    libraryDetails.Add(new
                    {
                        Id = dataReader["UserId"].ToString(),
                        Name = dataReader["UserName"].ToString(),
                        ProfilePic = dataReader["profilePic"].ToString(),
                        Email = dataReader["Email"].ToString(),
                        Title = dataReader["BookTitle"].ToString(),
                        Thumbnail = dataReader["BookThumbnail"].ToString(),
                        FinishReadingDate = dataReader["finishReadingDate"] != DBNull.Value
    ? Convert.ToDateTime(dataReader["finishReadingDate"]).ToString("dd/MM/yyyy")
    : null
                    });
                }

                return libraryDetails;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving author library details", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public List<object> GetAuthorPopularity()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<object> authorPopularity = new List<object>();

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetAuthorPopularity", con);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    authorPopularity.Add(new
                    {
                        AuthorId = dataReader["AuthorId"].ToString(),
                        Name = dataReader["name"].ToString(),
                        NumberOfAppearances = Convert.ToInt32(dataReader["NumberOfAppearances"])
                    });
                }

                return authorPopularity;
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving author popularity", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public List<object> GetBookPopularity()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<object> bookPopularity = new List<object>();

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetBookPopularity", con);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    bookPopularity.Add(new
                    {
                        BookId = dataReader["BookId"].ToString(),
                        Title = dataReader["title"].ToString(),
                        NumberOfAppearances = Convert.ToInt32(dataReader["NumberOfAppearances"])
                    });
                }

                return bookPopularity;
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving book popularity", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public List<Book> GetAvailableBooks()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<Book> availableBooks = new List<Book>();

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetAvailableBooks", con);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    availableBooks.Add(new Book
                    {
                        Id = dataReader["id"].ToString(),
                        Title = dataReader["title"].ToString(),
                        Subtitle = dataReader["subtitle"].ToString(),
                        Authors = dataReader["authors"].ToString().Split(','), // Assuming authors are stored as a comma-separated string
                        PublishedDate = dataReader["publishedDate"].ToString(),
                        PageCount = Convert.ToInt32(dataReader["pageCount"]),
                        IsMagazine = Convert.ToBoolean(dataReader["isMagazine"]),
                        IsMature = Convert.ToBoolean(dataReader["isMature"]),
                        IsEbook = Convert.ToBoolean(dataReader["isEbook"]),
                        Language = dataReader["language"].ToString(),
                        Price = Convert.ToSingle(dataReader["price"]),
                        Thumbnail = dataReader["thumbnail"].ToString(),
                        PreviewLink = dataReader["previewLink"].ToString(),
                        InfoLink = dataReader["infoLink"].ToString(),
                        EpubLink = dataReader["epubLink"].ToString(),
                        PdfLink = dataReader["pdfLink"].ToString(),
                        RatingAverage = Convert.ToSingle(dataReader["ratingAvg"]),
                        RatingCount = Convert.ToInt32(dataReader["ratingCount"]),
                        Description = dataReader["description"].ToString(),
                        TextSnippet = dataReader["textSnippet"].ToString(),
                        Category = dataReader["category"].ToString()
                    });
                }

                return availableBooks;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving available books", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }
        public int GetAvailableBooksCount()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            int availableBooksCount = 0;

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetAvailableBooksCount", con);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dataReader.Read())
                {
                    availableBooksCount = Convert.ToInt32(dataReader["AvailableBooksCount"]);
                }

                return availableBooksCount;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving available books count", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public float GetThisWeeksRevenue()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            float revenue = 0;

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetThisWeeksRevenue", con);

                // Execute the command and read the result
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dataReader.Read())
                {
                    revenue = Convert.ToSingle(dataReader["Revenue"]);
                }

                return revenue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving this week's revenue", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public List<Object> GetAllUsersWithBookCount()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<Object> users = new List<Object>(); // Initialize the list of books

            try
            {
                con = connect("myProjDB"); // Create the connection
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetAllUsersWithBookCount", con); // Create the command for reading books

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    users.Add(new
                    {
                        Id = Convert.ToInt32(dataReader["Id"]),
                        Name = dataReader["Name"].ToString(),
                        Email = dataReader["Email"].ToString(),
                        Password = dataReader["password"].ToString(),
                        ProfilePic = dataReader["ProfilePic"].ToString(),
                        IsActive = Convert.ToBoolean(dataReader["IsActive"]),
                        TotalBooksPurchased = Convert.ToInt32(dataReader["TotalBooksPurchased"])
                    });

                }

                return users; // Return the list of books
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving books", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }
        public int GetTotalBooksRead()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            int totalBooksRead = 0;

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetTotalBooksRead", con);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dataReader.Read())
                {
                    totalBooksRead = Convert.ToInt32(dataReader["TotalBooksRead"]);
                }

                return totalBooksRead;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving total books read", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public int GetTotalBooksPurchased()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            int totalBooksPurchased = 0;

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetTotalBooksPurchased", con);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dataReader.Read())
                {
                    totalBooksPurchased = Convert.ToInt32(dataReader["TotalBooksPurchased"]);
                }

                return totalBooksPurchased;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving total books purchased", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public int GetTotalBooksExchanged()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            int totalBooksExchanged = 0;

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetTotalBooksExchanged", con);

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dataReader.Read())
                {
                    totalBooksExchanged = Convert.ToInt32(dataReader["TotalBooksExchanged"]);
                }

                return totalBooksExchanged;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving total books exchanged", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public List<object> GetBooksSummary()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<object> bookSummaries = new List<object>(); // Initialize the list of book summaries

            try
            {
                con = connect("myProjDB");
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_GetBooksSummary", con); // Create the command for reading books
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    bookSummaries.Add(new
                    {
                        Title = dataReader["BookTitle"].ToString(),
                        Author = dataReader["AuthorName"].ToString(),
                        PublishedDate = Convert.ToDateTime(dataReader["PublishedDate"]),
                        BookType = dataReader["BookType"].ToString(),
                    });
                }

                return bookSummaries;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving book summaries", ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public List<string> Get16ThumbnailRandomBooks()
        {
            SqlConnection con = null;
            SqlCommand cmd;
            List<string> thumbnails = new List<string>(); // List of thumbnails

            try
            {
                con = connect("myProjDB"); // Create the connection
                cmd = CreateCommandWithStoredProcedureNoParameters("SP_Get16ThumbnailRandomBooks", con); // Create the command for the stored procedure

                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    string thumbnail = dataReader["thumbnail"].ToString();
                    thumbnails.Add(thumbnail); // Add the thumbnail to the list
                }

                return thumbnails; // Return the list of thumbnails
            }
            catch (Exception ex)
            {
                // Write to log
                throw new Exception("Error retrieving thumbnails", ex);
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

    }
}

