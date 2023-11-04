namespace BookShop
{
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            int year = int.Parse(Console.ReadLine());
            string result = GetBooksNotReleasedIn(db,year);
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

         
    }
}


