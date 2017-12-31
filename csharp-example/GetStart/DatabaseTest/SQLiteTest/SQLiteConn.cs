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
            dbConnection.ConnectionString = @"data source=test.s3db;version=3;";
            dbConnection.Open();

            /*
            string sql = "create table highscores (name varchar(20), score int)";
            new SQLiteCommand(sql, dbConnection).ExecuteNonQuery();
             */

            string createFileTable =
@"CREATE TABLE file (
  id int(7) NOT NULL,
  path varchar(260) DEFAULT NULL,
  description longtext,
  name varchar(50) NOT NULL,
  hardcopy varchar(45) DEFAULT NULL,
  PRIMARY KEY (id)
)";
            try
            {
                new SQLiteCommand(createFileTable, dbConnection).ExecuteNonQuery();
            }
            catch(SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }

            /*
            sql = "insert into highscores (name, score) values ('Me', 3000)";
            new SQLiteCommand(sql, dbConnection).ExecuteNonQuery();
            */

            string insertRecord = 
@"INSERT INTO file (id, name) VALUES (
  1, 'test'
)";

            /*
            sql = "select * from highscores order by score desc";
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine("Name: " + reader["name"] + "\tScore: " + reader["score"]);
             */

            /*
  `id` int(7) unsigned zerofill NOT NULL AUTO_INCREMENT,
  `path` varchar(260) DEFAULT NULL,
  `description` longtext,
  `name` varchar(50) NOT NULL,
  `hardcopy` varchar(45) DEFAULT NULL,
*/
            string sql = "SELECT * FROM file";
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine("ID: " + reader["id"] + "\tPath: " + reader["path"]);
        }
    }
}
