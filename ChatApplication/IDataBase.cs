using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplication
{
    internal interface IDataBase
    {
        SQLiteConnection GetSqlConnection();
    }


    internal class SQLiteDatabase : IDataBase
    {
        private string connectionString;

        public SQLiteDatabase(string dbFilePath)
        {
            connectionString = $"Data Source={dbFilePath};Version=3;";
        }
        public SQLiteConnection GetSqlConnection()
        {
            return new SQLiteConnection(connectionString);
        }
    }
}
