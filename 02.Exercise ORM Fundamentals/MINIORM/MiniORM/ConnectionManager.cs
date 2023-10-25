using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINIORM.MiniORM
{
    internal class ConnectionManager : IDisposable
    {
        private readonly DatabaseConnection connection;

        public ConnectionManager(DatabaseConnection connection)
        {
            this.connection = connection;

            this.connection.Open();
        }

        public void Dispose()
        {
            this.connection.Close();
        }
    }
}
