using AILibrary.DAL;
using System;
using System.Collections.Generic;

namespace AILibrary.Models
{
    public class Book
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string[] Authors { get; set; }
        public string PublishedDate { get; set; }
        public int PageCount { get; set; }
        public bool IsMagazine { get; set; }
        public bool IsMature { get; set; }
        public bool IsEbook { get; set; }
        public string Language { get; set; }
        public float Price { get; set; }
        public string Thumbnail { get; set; }
        public string PreviewLink { get; set; }
        public string InfoLink { get; set; }
        public string EpubLink { get; set; }
        public string PdfLink { get; set; }
        public float RatingAverage { get; set; }
        public int RatingCount { get; set; }
        public string Description { get; set; } // New property
        public string TextSnippet { get; set; } // New property
        public string Category { get; set; } // New property

        public Book(string id, string title, string subtitle, string[] authors, string publishedDate, int pageCount, bool isMagazine, bool isMature, bool isEbook, string language, float price, string thumbnail, string previewLink, string infoLink, string epubLink, string pdfLink, float ratingAverage, int ratingCount, string description, string textSnippet, string category)
        {
            Id = id;
            Title = title;
            Subtitle = subtitle;
            Authors = authors;
            PublishedDate = publishedDate;
            PageCount = pageCount;
            IsMagazine = isMagazine;
            IsMature = isMature;
            IsEbook = isEbook;
            Language = language;
            Price = SetRandomPrice();
            Thumbnail = thumbnail;
            PreviewLink = previewLink;
            InfoLink = infoLink;
            EpubLink = epubLink;
            PdfLink = pdfLink;
            RatingAverage = ratingAverage;
            RatingCount = ratingCount;
            Description = description; // Initialize new property
            TextSnippet = textSnippet; // Initialize new property
            Category = category; // Initialize new property
        }

        // Updated copy constructor
        public Book(Book other)
        {
            this.Id = other.Id;
            this.Title = other.Title;
            this.Subtitle = other.Subtitle;
            this.Authors = other.Authors != null ? (string[])other.Authors.Clone() : null;
            this.PublishedDate = other.PublishedDate;
            this.PageCount = other.PageCount;
            this.IsMagazine = other.IsMagazine;
            this.IsMature = other.IsMature;
            this.IsEbook = other.IsEbook;
            this.Language = other.Language;
            this.Price = SetRandomPrice();
            this.Thumbnail = other.Thumbnail;
            this.PreviewLink = other.PreviewLink;
            this.InfoLink = other.InfoLink;
            this.EpubLink = other.EpubLink;
            this.PdfLink = other.PdfLink;
            this.RatingAverage = other.RatingAverage;
            this.RatingCount = other.RatingCount;
            this.Description = other.Description; // Copy new property
            this.TextSnippet = other.TextSnippet; // Copy new property
            this.Category = other.Category; // Copy new property
        }

        public Book()
        {

        }

        public static List<Book> GetBooks()
        {
            DBservices dbs = new DBservices();
            return dbs.GetBooks();
        }

        private float SetRandomPrice()
        {
            Random random = new Random();
            // Generate a random float between 0 (inclusive) and 20 (exclusive) by scaling a random integer.
            float randomPrice = (float)(random.NextDouble() * 20);
            // Round to one decimal place
            float roundedPrice = (float)Math.Round(randomPrice, 1);
            return roundedPrice;
        }

        public static int InsertToTable(Book book)
        {
            DBservices dbs = new DBservices();
            return dbs.CreateBook(book);
        }

        public int AssignAuthorToBook(string authorName)
        {
            DBservices dbs = new DBservices();
            return dbs.AssignBookToAuthor(authorName, this.Id);
        }

        static public Book GetRandomBook()
        {
            DBservices dBservices = new DBservices();
            return dBservices.GetRandomBook();
        }

        static public List<Book> SearchBooksByParameter(string parameter, string text)
        {
            DBservices dbs = new DBservices();
            return dbs.SearchBooksByParameter(parameter, text);
        }

        static public List<Object> GetMarketPlace()
        {
            DBservices dbs = new DBservices();
            return dbs.GetMarketplace();
        }

        static public List<Object> GetBookReviews(string bookId)
        {
            DBservices dbs = new DBservices();
            return dbs.GetBookReviews(bookId);
        }

        public static int GetPurchasesThisWeek()
        {
            DBservices dbs = new DBservices();
            return dbs.GetPurchasesThisWeek();
        }
        public static List<Book> GetTopRatedBooks()
        {
            DBservices dbs = new DBservices();
            return dbs.GetTopRatedBooks();
        }
        public static List<Book> GetMostNewBooks()
        {
            DBservices dbs = new DBservices();
            return dbs.GetMostNewBooks();
        }
        public static List<object> GetBookPopularity()
        {
            DBservices dbs = new DBservices();
            return dbs.GetBookPopularity();
        }
        public static List<Book> GetAvailableBooks()
        {
            DBservices dbs = new DBservices();
            return dbs.GetAvailableBooks();
        }
        public static int GetAvailableBooksCount()
        {
            DBservices dbs = new DBservices();
            return dbs.GetAvailableBooksCount();
        }
        public static int GetTotalBooksRead()
        {
            DBservices dbs = new DBservices();
            return dbs.GetTotalBooksRead();
        }

        public static int GetTotalBooksPurchased()
        {
            DBservices dbs = new DBservices();
            return dbs.GetTotalBooksPurchased();
        }

        public static int GetTotalBooksExchanged()
        {
            DBservices dbs = new DBservices();
            return dbs.GetTotalBooksExchanged();
        }
        public static List<object> GetBooksSummary()
        {
            DBservices dbs = new DBservices();
            return dbs.GetBooksSummary();
        }

        static public List<string> Get16ThumbnailRandomBooks()
        {
            DBservices dBservices = new DBservices();
            return dBservices.Get16ThumbnailRandomBooks();
        }

        static public List<object> GetBookLibraryDetails(string bookId)
        {
            DBservices dBservices = new DBservices();   
            return dBservices.GetBookLibraryDetails(bookId);
        }


    }
}
