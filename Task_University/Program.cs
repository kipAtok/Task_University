
using System.Data.SQLite;

class Program
{
    static void Main(string[] args)
    {
        string filePath = @"..\..\..\Files\University.db";
        string connectionString = $@"Data Source={filePath};Version=3";
        SQLiteConnection connection = new SQLiteConnection(connectionString);
        DatabaseHelper dbHelper = new DatabaseHelper(connection, filePath);
        UserHelper uHelper = new UserHelper(dbHelper, true);
        uHelper.Start();
        dbHelper.Kill();
    }
}
