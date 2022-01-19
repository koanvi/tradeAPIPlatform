using Microsoft.Data.Sqlite;

namespace koanvi.tradeAPIPlatform.DB {

  public class DbResolver {

    private string connectionString = "Data Source=localStorage.db";
    private string tableName = "candels";

    private SqliteConnection connection;

    public DbResolver() {
      this.connection = new SqliteConnection(connectionString);
      this.connection.Open();
      Console.WriteLine(@$"🚀{connection.State}");
    }

    public void GenerateDB() {
      SqliteCommand command = new SqliteCommand();

      try {

        this.connection.Open();

        command.Connection = this.connection;

        command.CommandText = $@"

        CREATE TABLE IF NOT EXISTS {this.tableName}(
        
          id      integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
          date    integer NOT NULL, -- as Unix Time, the number of seconds since 1970-01-01 00:00:00 UTC.
          sourse  integer NOT NULL, -- 0-Bitfinex,1-Kraken
          open    real NOT NULL,
          close   real NOT NULL,
          high    real NOT NULL,
          low     real NOT NULL,
          volume  real NOT NULL

        )

      ";

        command.ExecuteNonQuery();

      } catch (System.Exception) { throw; }
      finally { command.Connection.Close(); }

    }

    public void SaveAPItoDB(List<BitfinexAPI.Candel> input, Sourse sourse) {

      this.connection.Open();

      using (var transaction = this.connection.BeginTransaction()) {
        int i = 0;
        try {

          string CommandText = $@"
        INSERT INTO {this.tableName}(

          date,
          sourse,
          open,
          close,
          high,
          low,
          volume

        )
        VALUES (

          $date,
          $sourse,
          $open,
          $close,
          $high,
          $low,
          $volume

        )
    ";

          input.ForEach(row => {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = CommandText;
            i++;
            // date
            var dateParameter = command.CreateParameter();
            dateParameter.ParameterName = "$date";
            dateParameter.Value = row.MTS;
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
        } catch (System.Exception ex) {
          transaction.Rollback();
          Console.WriteLine(ex.Message);
          throw;
        }
        finally { this.connection.Close(); }
      }//transaction

    }

    public List<ReportRowResult> GetReport(ReportParams reportParams) {

      this.connection.Open();
      SqliteCommand command = new SqliteCommand();
      command.Connection = this.connection;
      command.CommandText = $@"

        select 
        
          id,
          date,
          sourse,
          open,
          close,
          high,
          low,
          volume

        from  {this.tableName}

      ";

      var result = new List<ReportRowResult>();

      using (SqliteDataReader reader = command.ExecuteReader()) {
        if (reader.HasRows) {
          while (reader.Read()) {

            var date = new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(double.Parse(reader.GetValue(1).ToString()));
            var tmpResult = new ReportRowResult() {

              id = Convert.ToInt32(reader.GetValue(0)),//0
              date = date,//1
              sourse = (Sourse)Convert.ToInt32(reader.GetValue(2)),//2
              open = Convert.ToSingle(reader.GetValue(3)),//3
              close = Convert.ToSingle(reader.GetValue(4)),//4
              high = Convert.ToSingle(reader.GetValue(4)),//5
              low = Convert.ToSingle(reader.GetValue(4)),//6
              volume = Convert.ToSingle(reader.GetValue(4)),//7

            };

            result.Add(tmpResult);

          }
        }
      }





      // throw new Exception("GenerateReport not implementred");
      return result;
    }

  }

  public class APIInput { }

  public class ReportParams { }

  public class ReportRowResult {

    public int id { get; set; }
    public DateTime date { get; set; }
    public Sourse sourse { get; set; }
    public float open { get; set; }
    public float close { get; set; }
    public float high { get; set; }
    public float low { get; set; }
    public float volume { get; set; }// - объем 

  }

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



// double ticks = double.Parse(startdatetime);
// TimeSpan time = TimeSpan.FromMilliseconds(ticks);
// DateTime startdate = new DateTime(1970, 1, 1) + time; 


// DateTime startdate = new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(double.Parse(startdatetime));