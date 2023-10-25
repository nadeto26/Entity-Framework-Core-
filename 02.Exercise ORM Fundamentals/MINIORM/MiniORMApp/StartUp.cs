using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Data.Entities;

namespace MINIORM.MiniORM
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniDbContext dbContext = new SoftUniDbContext(Config.ConnectionString);

            Employee newEmployee = dbContext
                .Employees.First(e => e.FirstName == "Test");
            dbContext.Employees.Remove(newEmployee);

            dbContext.SaveChanges();
        }
    }
}
