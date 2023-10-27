using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace P02_FootballBetting.Data
{
    public class FootballBettingContext:DbContext
    {
        // When we test the application locally on our PC
         public FootballBettingContext()
        {

        }

        // Used by Judge
        // Loading of the DbContext with DI -> In real application it is useful
        public FootballBettingContext(DbContextOptions options)
            : base(options)
        {

        }
    }
}