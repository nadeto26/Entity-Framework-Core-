namespace BookShop
{
    using BookShop.Models;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);


          
            //DbInitializer.ResetDatabase(dbContext);

            //string input = Console.ReadLine();
            Stopwatch sw = Stopwatch.StartNew();
            IncreasePrices(db);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        //Problem4
        public static string GetBooksByPrice(BookShopContext context)
        {
            string[] books = context.Books
                .Where(x => x.Price > 40)
                .OrderByDescending(x => x.Price)
                .Select(a=> $"{a.Title} - ${a.Price:f2}")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //Problem5

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            string[] books = context.Books
                .Where(a => a.ReleaseDate.Value.Year != year)
                .OrderBy(a => a.BookId)
                .Select(a=>a.Title) .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //Problem7

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime convertedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var books = context.Books
                .Where(b => b.ReleaseDate < convertedDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:F2}")
                .ToList();

            var result = String.Join(Environment.NewLine, books);
            return result;
        }

        //Problem9

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
            .Where(a=>a.Title.Contains(input, StringComparison.OrdinalIgnoreCase))
            .OrderBy(a=>a.Title)
            .Select(a=>a.Title)
            .ToArray();

            var result = String.Join(Environment.NewLine, books);
            return result;
        }

        //Problem10

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            string[] authors =
                context.Books
                .OrderBy(b => b.BookId)
                 .Where(a => a.Author.LastName.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                 .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
                 .ToArray();


            var result = String.Join(Environment.NewLine, authors);
            return result;




        }

        //Problem11
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {

          
               return  context.Books
                .Count(a => a.Title.Length > lengthCheck);
                 
        }

        //problem12
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var authorsWithBookCopies = context.Authors
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName,
                    TotalCopies = a.Books
                        .Sum(b => b.Copies)
                })
                .ToArray()
                .OrderByDescending(b => b.TotalCopies); // This is optimization

            foreach(var book in authorsWithBookCopies)
            {
                sb.AppendLine($"{book.FullName} - {book.TotalCopies}");
            }

            return sb.ToString();



        }

        //Problem 13

        public static string GetTotalProfitByCategory(BookShopContext context)
        {

            StringBuilder sb = new StringBuilder();
            var categoriesWithProfit = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    TotalProfit = c.CategoryBooks
                        .Sum(cb => cb.Book.Copies * cb.Book.Price)
                })
                .ToArray()
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.CategoryName);

            foreach(var c in categoriesWithProfit)
            {
                sb.AppendLine($"{c.CategoryName} ${c.TotalProfit:F2}");
            }

            return sb.ToString().TrimEnd();

        }

        //Problem14

        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var categoriesWithMostRecentBooks = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    CategoryName = c.Name,
                    MostRecentBooks = c.CategoryBooks
                        .OrderByDescending(cb => cb.Book.ReleaseDate)
                        .Take(3) // This can lower network load
                        .Select(cb => new
                        {
                            BookTitle = cb.Book.Title,
                            ReleaseYear = cb.Book.ReleaseDate.Value.Year
                        })
                        .ToArray()
                })
                .ToArray();


            foreach (var c in categoriesWithMostRecentBooks)
            {
                sb.AppendLine($"--{c.CategoryName}");

                foreach (var b in c.MostRecentBooks/*.Take(3) This is lowering query complexity*/)
                {
                    sb.AppendLine($"{b.BookTitle} ({b.ReleaseYear})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //problem15

        public static void IncreasePrices(BookShopContext context)
        {
            Book[] bookReleasedBefore2010 = context
               .Books
               .Where(b => b.ReleaseDate.HasValue &&
                           b.ReleaseDate.Value.Year < 2010)
               .ToArray(); 

            foreach(var b in bookReleasedBefore2010)
            {
                b.Price += 5;
            }

            context.SaveChanges();

        }
    }
}


