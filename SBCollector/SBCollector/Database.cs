using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SQLite;

namespace SBCollector
{
    class Database
    {
        public string Source { get; private set; }

        private SQLiteConnection _connection = null;

        public void Open(string source)
        {
            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = source;
            _connection = new SQLiteConnection(builder.ToString());
            _connection.Open();

            Source = source;
        }

        public bool Find(string ID)
        {
            return true;
        }
    }
}
