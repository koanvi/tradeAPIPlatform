using Microsoft.Data.Sqlite;

namespace koanvi.apiGetter.DB {

  public class DbResolver {

    private string connectionString = "Data Source=usersdata.db";
    private SqliteConnection? connection = null;

    public DbResolver() {
      this.connection = new SqliteConnection(connectionString);
      this.connection.Open();
      Console.WriteLine(connection.State);
    }

    public void GenerateDB() {
      SqliteCommand command = new SqliteCommand();

      string tableName = "apiresult";
      string commandText =
            $@"

      CREATE TABLE {tableName}(
      id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
      date date NOT NULL,
      type integer NOT NULL--

      ";
      
      // command.ExecuteNonQuery();


    }

    public void SaveAPItoDB() {

    }

    public ReportResult GenerateReport(ReportParams reportParams) {
      return null;
    }
    
  }

  public class APIInput { }

  public class ReportParams { }

  public class ReportResult { }



}


