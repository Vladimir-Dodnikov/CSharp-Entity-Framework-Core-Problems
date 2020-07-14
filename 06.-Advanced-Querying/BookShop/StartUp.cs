namespace BookShop
{
    using BookShop.Models;
    using Data;
    using Initializer;
    using Microsoft.Data.SqlClient.Server;
    using Microsoft.EntityFrameworkCore.Internal;
    using Microsoft.VisualBasic;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.ConstrainedExecution;
    using System.Text;
    using Z.EntityFramework.Plus;

    public class StartUp
    {
        public static void Main()
        {
            using BookShopContext db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            // var input = Console.ReadLine();

            var result = RemoveBooks(db);//, input);

            Console.WriteLine(result);
        }
        //Problem - 01
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            //StringBuilder sb = new StringBuilder();

            List<string> bookTitles = context
                .Books
                .ToList() //materialize (.AsEnumerable()) or downgrade EFC to 2.2.0
                .Where(b => b.AgeRestriction.ToString().ToLower() == command.ToLower())
                .Select(b => b.Title)
                .OrderBy(bt => bt)
                .ToList();

            //return sb.ToString().TrimEnd();
            return string.Join(Environment.NewLine, bookTitles);
        }
        //Problem - 02
        public static string GetGoldenBooks(BookShopContext context)
        {
            var bookTitles = context
                .Books
                .Where(b => b.EditionType == Models.Enums.EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, bookTitles);
        }
        //Problem - 03
        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context
                .Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }
        //Problem - 04
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var booksNotReleasedIn = context
                .Books
                .Where(b => b.ReleaseDate.Value.Year != year) //ReleaseDate is 'DateTime?' !
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, booksNotReleasedIn);
        }
        //Problem - 05
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToArray();

            var booksByCategory = new List<string>();

            foreach (var category in categories)
            {
                var bookTitles = context
                    .Books
                    .Where(b => b.BookCategories.Any(bc => bc.Category.Name.ToLower() == category))
                    .OrderBy(b => b.Title)
                    .Select(b => b.Title)
                    .ToList();

                booksByCategory.AddRange(bookTitles);
            }

            booksByCategory = booksByCategory.OrderBy(bt => bt).ToList();

            return string.Join(Environment.NewLine, booksByCategory);

        }
        //Problem - 06
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder sb = new StringBuilder();

            DateTime formatDate = DateTime.ParseExact(date, "dd-MM-yyyy", null);

            var booksByDate = context
                .Books
                .Where(b => b.ReleaseDate < formatDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(x => new { x.Title, x.EditionType, x.Price })
                .ToList();

            foreach (var book in booksByDate)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }
        //Problem - 07
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authorsEndingIn = context
                .Authors
                .Where(a => a.FirstName.EndsWith(input.ToLower()))
                .Select(x => x.FirstName + " " + x.LastName)
                .OrderBy(a => a)
                .ToList();

            return string.Join(Environment.NewLine, authorsEndingIn);
        }
        //Problem - 08
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var booksTitles = context
                .Books
                .AsEnumerable()
                .Where(b => b.Title.Contains(input, StringComparison.CurrentCultureIgnoreCase))
                .Select(b => b.Title)
                .OrderBy(bt => bt)
                .ToList();

            return string.Join(Environment.NewLine, booksTitles);
        }
        //Problem - 09
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            var booksByAuthors = context
                .Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(b => new
                {
                    b.BookId,
                    b.Title,
                    FullName = b.Author.FirstName + " " + b.Author.LastName
                })
                .OrderBy(b => b.BookId)
                .ToList();

            foreach (var book in booksByAuthors)
            {
                sb.AppendLine($"{book.Title} ({book.FullName})");
            }
            return sb.ToString().TrimEnd();
        }
        //Problem - 10
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var booksCount = context
                .Books
                .Where(b => b.Title.Length > lengthCheck)
                .Select(b => b.BookId)
                .Count();

            return booksCount;
        }
        //Problem - 11
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var sb = new StringBuilder();
            var bookCopies = context
                .Authors
                .Select(a => new
                {
                    Author = a.FirstName + " " + a.LastName,
                    Copies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(b => b.Copies)
                .ToList();

            foreach (var b in bookCopies)
            {
                sb.AppendLine($"{b.Author} - {b.Copies}");
            }

            return sb.ToString().TrimEnd();
        }
        //Problem - 12
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var sb = new StringBuilder();
            var booksProfit = context
                .Categories
                .Select(c => new
                {
                    Category = c.Name,
                    Profit = c.CategoryBooks.Sum(b => b.Book.Price * b.Book.Copies)
                })
                .OrderByDescending(b => b.Profit)
                .ThenBy(b => b.Category)
                .ToList();

            foreach (var b in booksProfit)
            {
                sb.AppendLine($"{b.Category} ${b.Profit:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        //Problem - 13
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var sb = new StringBuilder();

            var booksByDate = context
                .Categories
                .Select(c => new
                {
                    Category = c.Name,
                    Books = c.CategoryBooks
                    .Where(b => b.Book.ReleaseDate != null)
                    .OrderByDescending(b => b.Book.ReleaseDate)
                    .Select(b => new
                    {
                        BookTitle = b.Book.Title,
                        dateYear = b.Book.ReleaseDate.Value.Year
                    })
                    .Take(3)
                })
                .OrderBy(b=>b.Category)
                .ToList();
            //--Action
            //Brandy ofthe Damned(2015)

            foreach (var b in booksByDate)
            {
                sb.AppendLine($"--{b.Category}");

                foreach (var book in b.Books)
                {
                    sb.AppendLine($"{book.BookTitle} ({book.dateYear})");
                }
            }

            return sb.ToString().TrimEnd();
        }
        //Problem - 14
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context
                .Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }
        //Problem - 15
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context
                .Books
                .Where(b => b.Copies < 4200)
                .ToList();

            context.RemoveRange(books);
            context.SaveChanges();

            return books.Count();
        }
    }
}
