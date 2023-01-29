using Dapper;
using Microsoft.Data.Sqlite;

namespace TelegramBot_Timetable;

public static class DbCommands
{
    private static readonly string path = "";
    private static readonly string pathToUsers = path + "users.db";
    private static readonly string pathToTasks = path + "tasks.db";
    
    static DbCommands()
    {
        if (!File.Exists(pathToUsers))
        {
            using (var connection = new SqliteConnection($"Data Source={pathToUsers}"))
            {
                connection.Open();
                var command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = "CREATE TABLE Users(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                      "userId BIGINT NOT NULL," +
                                      "chatId BIGINT NOT NULL)";
                command.ExecuteNonQuery();
            }
        }
        
        if (!File.Exists(pathToTasks))
        {
            using (var connection = new SqliteConnection($"Data Source={pathToTasks}"))
            {
                connection.Open();
                var command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = "CREATE TABLE Tasks(taskId INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
                                      "userId INTEGER NOT NULL," +
                                      "status INTEGER NOT NULL," +
                                      "heading TINYTEXT NOT NULL," +
                                      "description TINYTEXT)";
                command.ExecuteNonQuery();
            }
        }
    }
    
    public static bool CheckUser(long userid)
    {
        using (var connection = new SqliteConnection($"Data Source={pathToUsers}"))
        {
            connection.Open();
            var command = new SqliteCommand();
            command.Connection = connection;
            command.CommandText = $"SELECT * FROM Users WHERE userId = {userid}";
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                    return true;
                return false;
            }
        }
    }
    
    public static void AddUser(long userid, long chatid)
    {
        using (var connection = new SqliteConnection($"Data Source={pathToUsers}"))
        {
            connection.Open();
            var command = new SqliteCommand();
            command.Connection = connection;
            command.CommandText = $"INSERT INTO Users (userId, chatId) VALUES ({userid}, {chatid})";
            command.ExecuteNonQuery();
        }
    }
    
    public static int GetUser(long userid)
    {
        using (var connection = new SqliteConnection($"Data Source={pathToUsers}"))
        {
            connection.Open();
            var command = new SqliteCommand();
            command.Connection = connection;
            command.CommandText = $"SELECT * FROM Users WHERE userId = {userid}";
            using (var reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                    throw new Exception("Fatal db exception. Cant find user.");
                
                while (reader.Read())
                {
                    return (int)(long)reader.GetValue(0);
                }
                
                throw new Exception("Fatal db exception. Cant find user.");
            }
        }
    }
    
    public static User[] GetUsers()
    {
        using (var connection = new SqliteConnection($"Data Source={pathToUsers}"))
        {
            return connection.Query<User>($"SELECT * FROM Users").ToArray();
        }
    }

    public static void AddTask(TimeTask t)
    {
        using (var connection = new SqliteConnection($"Data Source={pathToTasks}"))
        {
            connection.Open();
            var command = new SqliteCommand();
            command.Connection = connection;
            command.CommandText = $"INSERT INTO Tasks (userId, status, heading, description) VALUES" +
                                  $"({t.userId}, {t.status}, \"{t.heading}\", \"{t.description ?? ""}\")";
            command.ExecuteNonQuery();
        }
    }

    public static TimeTask[] GetTasks(int userid)
    {
        using (var connection = new SqliteConnection($"Data Source={pathToTasks}"))
        {
            return connection.Query<TimeTask>($"SELECT * FROM Tasks WHERE userId = {userid} AND status != 2").ToArray();
        }
    }

    public static void ModifyTask(TimeTask t, int newStatus)
    {
        using (var connection = new SqliteConnection($"Data Source={pathToTasks}"))
        {
            connection.Open();
            var command = new SqliteCommand();
            command.Connection = connection;
            command.CommandText = $"UPDATE Tasks SET status = {newStatus} WHERE taskId = {t.taskId}";
            command.ExecuteNonQuery();
        }
    }
}