using AILibrary.DAL;
using System.Globalization;
using static System.Reflection.Metadata.BlobBuilder;

namespace AILibrary.Models
{
    public class Author
    {
        int id;
        string name;


        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }

        public Author() { }
        public Author(string name)
        {
            this.name = name;
        }

        public Author(Author other)
        {
            this.name = other.Name;
        }

        public static List<Author> GetAuthors()
        {
            DBservices dbs = new DBservices();
            return dbs.GetAuthors();
        }


        public static int InsertToTable(Author author)
        {
            DBservices dbs = new DBservices();
            return dbs.CreateAuthor(author);
        }

        public static List<Book> GetAuthorsBooks(int id)
        {
            DBservices dbs = new DBservices();
            return dbs.GetAuthorsBooks(id);
        }
<<<<<<< Updated upstream
        public static List<object> GetAuthorPopularity()
        {
            DBservices dbs = new DBservices();
            return dbs.GetAuthorPopularity();
        }
=======
      public static List<Author> GetAuthorPopularity(int id)
        {
            DBservices dbs = new DBservices();
            return dbs.GetAuthorPopularity(id);
        }

>>>>>>> Stashed changes

    }

    

    

   
}
