
namespace koanvi.tradeAPIPlatform.BitfinexAPI {

  //get data from api
  public class DataGetter {

    private static HttpClient client = new HttpClient();
    public static string request = "https://api-pub.bitfinex.com/v2/candles/trade:1m:tBTCUSD/hist";

    async public static Task<List<Candel>> GetData() {
      List<Candel> result = new List<Candel>();

      var stringTask = client.GetStringAsync(request);
      var msg = await stringTask;

      var parseResult = System.Text.Json.JsonSerializer.Deserialize<object[][]>(msg);
      (parseResult.ToList<object[]>()).ForEach(row => {

        var MTS = row[0].ToString();
        var OPEN = row[1].ToString();
        var CLOSE = row[2].ToString();
        var HIGH = row[3].ToString();
        var LOW = row[4].ToString();
        var VOLUME = row[5].ToString();

        var candel = new Candel() {
          MTS = Convert.ToInt64(MTS, System.Globalization.CultureInfo.InvariantCulture),
          OPEN = Convert.ToSingle(OPEN, System.Globalization.CultureInfo.InvariantCulture),
          CLOSE = Convert.ToSingle(CLOSE, System.Globalization.CultureInfo.InvariantCulture),
          HIGH = Convert.ToSingle(HIGH, System.Globalization.CultureInfo.InvariantCulture),
          LOW = Convert.ToSingle(LOW, System.Globalization.CultureInfo.InvariantCulture),
          VOLUME = Convert.ToSingle(VOLUME, System.Globalization.CultureInfo.InvariantCulture),

        };

        result.Add(candel);

      });

      return result;

    }


  }

  public class Candel {
    public long MTS { get; set; }
    public float OPEN { get; set; }
    public float CLOSE { get; set; }
    public float HIGH { get; set; }
    public float LOW { get; set; }
    public float VOLUME { get; set; }// - объем 
  }

}



// TODO:
// add params:

//Path Params
// TimeFrame *
// string

// Available values: '1m', '5m', '15m', '30m', '1h', '3h', '6h', '12h', '1D', '1W', '14D', '1M'
// Symbol *
// string

// The symbol you want information about. (e.g. tBTCUSD, tETHUSD, fUSD, fBTC)
// Section *
// string

// Available values: "last", "hist"
// Period *
// string

// Funding period.Only required for funding candles. Enter after the symbol (trade:1m:fUSD:p30/hist).




// Query Params
// limit
// int32

// Number of candles requested (Max: 10000)
// start
// string

// Filter start (ms)
// end
// string

// Filter end (ms)
// sort
// int32

// if = 1 it sorts results returned with old > new