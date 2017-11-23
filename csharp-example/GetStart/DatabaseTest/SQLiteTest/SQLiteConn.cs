using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace SQLiteTest
{
    public class SQLiteConn
    {
        public static void TestMain()
        {
            var dbConnection = new SQLiteConnection();
            dbConnection.Open();

            string sql = "create table highscores (name varchar(20), score int)";
            new SQLiteCommand(sql, dbConnection).ExecuteNonQuery();
            sql = "insert into highscores (name, score) values ('Me', 3000)";
            new SQLiteCommand(sql, dbConnection).ExecuteNonQuery();

            sql = "select * from highscores order by score desc";
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine("Name: " + reader["name"] + "\tScore: " + reader["score"]);
        }
    }
}
