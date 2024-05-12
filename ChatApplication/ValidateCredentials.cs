using System.Data.SQLite;
using System.Windows.Forms;

namespace ChatApplication
{
    internal class CredentialValidator 
    {
        public static bool ValidateCredentials(string username, string passwordHash)
        {
            try
            {
                string query = "SELECT * FROM Login WHERE username = @username AND passwordHash = @password";
                using (SQLiteConnection connection = GetSqlConnection())
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", passwordHash);

                    connection.Open();
                    SQLiteDataReader reader = command.ExecuteReader();
                    return reader.HasRows;
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show($"Login Error: {ex.Message}");
                return false;
            }
        }

        public static SQLiteConnection GetSqlConnection()
        {
            return new SQLiteConnection("Data Source=ChatAppDB.db;Version=3;");
        }
    }
}
