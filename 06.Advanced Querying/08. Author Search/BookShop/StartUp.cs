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
            
            string input = Console.ReadLine();
            string resuler = GetAuthorNamesEndingIn(db, input);
        }

        public static string GetAuthorNamesEndingIn(BookShopContext dbContext, string input)
        {
            string[] authorNames = dbContext.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .Select(a => $"{a.FirstName} {a.LastName}")
                .ToArray();

            return string.Join(Environment.NewLine, authorNames);
        }

    }
}


