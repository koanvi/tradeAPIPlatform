using Microsoft.Data.Sqlite;

namespace koanvi.apiGetter.BitfinexAPI {

  //get data from api
  public class DataGetter {
    string connectionString = "Data Source=usersdata.db";
    public BitfinexAPI() {

      using (var connection = new SqliteConnection(connectionString)) {
        connection.Open();
        Console.WriteLine(connection.State);
      }
      Console.Read();

    }

  }
  public class Candel {
    public int MTS { get; set; }
    public float OPEN { get; set; }
    public float CLOSE { get; set; }
    public float HIGH { get; set; }
    public float LOW { get; set; }
    public float VOLUME { get; set; }
  }

}


//   [
//   MTS,
//   OPEN,
//   CLOSE,
//   HIGH,
//   LOW,
//   VOLUME- объем 
// ]
// }

// MTS:
// double ticks = double.Parse(startdatetime);
// TimeSpan time = TimeSpan.FromMilliseconds(ticks);
// DateTime startdate = new DateTime(1970, 1, 1) + time;