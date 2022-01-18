using Microsoft.Data.Sqlite;

namespace koanvi.apiGetter.DB {

  public class DbResolver {

    private string connectionString = "Data Source=localStorage.db";
    private SqliteConnection? connection = null;
    public string tableName = "candels";

    public DbResolver() {
      this.connection = new SqliteConnection(connectionString);
      this.connection.Open();
      Console.WriteLine(connection.State);
    }

    public void GenerateDB() {
      SqliteCommand command = new SqliteCommand();
      command.Connection = this.connection;

      command.CommandText = $@"

        CREATE TABLE IF NOT EXISTS {this.tableName}(
        
        id      integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
        date    integer NOT NULL,--as Unix Time, the number of seconds since 1970-01-01 00:00:00 UTC.
        sourse  integer NOT NULL,--0-Bitfinex,1-Kraken
        OPEN    real NOT NULL,
        CLOSE   real NOT NULL,
        HIGH    real NOT NULL,
        LOW     real NOT NULL,
        VOLUME  real NOT NULL

      ";

      command.ExecuteNonQuery();


    }

    public void SaveAPItoDB(List<BitfinexAPI.Candel> input, Sourse sourse) {

      this.connection.Open();

      using (var transaction = this.connection.BeginTransaction()) {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = $@"
        INSERT INTO {this.tableName}(
          date,
          sourse,
          OPEN,
          CLOSE,
          HIGH,
          LOW,
          VOLUME

        )
        VALUES (
          $date,
          $sourse,
          $OPEN,
          $CLOSE,
          $HIGH,
          $LOW,
          $VOLUME
          )
    ";
        input.ForEach(row => {

          // date
          var dateParameter = command.CreateParameter();
          dateParameter.ParameterName = "$date";
          dateParameter.Value = row.CLOSE;
          command.Parameters.Add(dateParameter);

          // sourse
          var sourseParameter = command.CreateParameter();
          sourseParameter.ParameterName = "$sourse";
          sourseParameter.Value = sourse;
          command.Parameters.Add(sourseParameter);

          // OPEN
          var openParameter = command.CreateParameter();
          openParameter.ParameterName = "$open";
          openParameter.Value = row.OPEN;
          command.Parameters.Add(openParameter);

          // CLOSE
          var closeParameter = command.CreateParameter();
          closeParameter.ParameterName = "$close";
          closeParameter.Value = row.CLOSE;
          command.Parameters.Add(closeParameter);

          // HIGH
          var highParameter = command.CreateParameter();
          highParameter.ParameterName = "$high";
          highParameter.Value = row.HIGH;
          command.Parameters.Add(highParameter);

          // LOW
          var lowParameter = command.CreateParameter();
          lowParameter.ParameterName = "$low";
          lowParameter.Value = row.LOW;
          command.Parameters.Add(lowParameter);

          // VOLUME
          var volumeParameter = command.CreateParameter();
          volumeParameter.ParameterName = "$volume";
          volumeParameter.Value = row.VOLUME;
          command.Parameters.Add(volumeParameter);

          command.ExecuteNonQuery();
        });//row

        transaction.Commit();

      }//transaction


    }

    public ReportResult GenerateReport(ReportParams reportParams) {
      throw new Exception("GenerateReport not implementred");
      // return null;
    }

  }

  public class APIInput { }

  public class ReportParams { }

  public class ReportResult { }


  public enum Sourse {
    Bitfinex,
    Kraken,
  }

  public enum Peer {
    tBTCUSD,
    tETHUSD,
    fUSD,
    fBTC,
  }

}


