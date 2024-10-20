
using System.Data.SQLite;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = @"Data Source=..\..\..\Files\University.db;Version=3";
        SQLiteConnection connection = new SQLiteConnection(connectionString);
        DatabaseHelper dbHelper = new DatabaseHelper(connection);
        UserHelper uHelper = new UserHelper(dbHelper);
        uHelper.Start();
        dbHelper.Kill();
    }
}
