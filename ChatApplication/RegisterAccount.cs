using System.Windows.Forms;
using System.Data.SQLite;

namespace ChatApplication
{
    internal class RegisterAccount : IDataBase
    {

        public RegisterAccount(string username, string passwordHash, string email)
        {
            if (IsUsernameAvailable(username))
            {
                try
                {
                    string query = @"INSERT INTO Register (username, passwordHash, email) VALUES (@username, @passwordHash, @email)";
                    using (SQLiteConnection connection = GetSqlConnection())
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@passwordHash", passwordHash);
                        command.Parameters.AddWithValue("@email", email);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Registration Successful");

                    string insertLoginQuery = @"INSERT INTO Login (username, passwordHash) VALUES (@username, @passwordHash)";

                    using (SQLiteConnection connection = GetSqlConnection())
                    using (SQLiteCommand command = new SQLiteCommand(insertLoginQuery, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@passwordHash", passwordHash);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }

                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show($"Registration Error: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Username already chosen, Please choose something else!");
            }

        }
        public SQLiteConnection GetSqlConnection() 
        {
            return new SQLiteConnection("Data Source=ChatAppDB.db;Version=3;");
        }

        private bool IsUsernameAvailable(string username)
        {
            using (SQLiteConnection connection = GetSqlConnection())
            {
                connection.Open();

                string sql = @"SELECT 1 FROM Login WHERE username = @username";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@username", username);

                    object result = command.ExecuteScalar();

                    return result == null;
                }
            }
        }

    }
}
