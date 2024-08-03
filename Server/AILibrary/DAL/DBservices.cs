﻿using AILibrary.Models;
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
                        Description = dataReader["description"].ToString(), // New property
                        TextSnippet = dataReader["textSnippet"].ToString() // New property
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

            catch (Exception ex)
            {
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

            catch (Exception ex)
            {
                throw new Exception("Couldn't mark as read", ex);
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
                        Description = dataReader["description"].ToString(), // New property
                        TextSnippet = dataReader["textSnippet"].ToString() // New property
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
                        Description = dataReader["description"].ToString(), // New property
                        TextSnippet = dataReader["textSnippet"].ToString() // New property
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
                        Description = dataReader["description"].ToString(), // New property
                        TextSnippet = dataReader["textSnippet"].ToString() // New property
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
                        Description = dataReader["description"].ToString(), // New property
                        TextSnippet = dataReader["textSnippet"].ToString() // New property
                    };

                    if (parameter=="Content")
                    {
                        // Check if the PDF contains the search text
                        if ((b.Description != null && b.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase)))
                        {
                            books.Add(b); // Add the book to the list if it contains the text
                        }
                    }

                    if (parameter=="Author")
                    {
                        if (b.Authors != null)
                        {
                            foreach(var author in b.Authors)
                            {
                                if (author.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                                {
                                    books.Add(b); 
                                    break;
                                }
                            }
                        }
                    }

                    if (parameter=="Title")
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
                        Description = dataReader["description"].ToString(), // New property
                        TextSnippet = dataReader["textSnippet"].ToString(), // New property
                        Username = dataReader["Username"].ToString(),   
                        finishedDate = dataReader["finishReadingDate"].ToString(),
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



    }


}
