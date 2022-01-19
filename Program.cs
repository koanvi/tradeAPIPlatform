using System;
using System.Collections.Generic;
using System.Linq;

namespace koanvi.tradeAPIPlatform {
  public class Program {
    public static void Main(string[] args) {
      MainAsync().GetAwaiter().GetResult();
    }
    private static async Task MainAsync() {

      try {
        await GetDataFromDB();
        // await ImportFromBitfinexAPI();
        Console.WriteLine("✅ done");

      } catch (System.Exception ex) { Console.WriteLine(@$"❎ error: {ex.Message}"); }

    }

    private static async Task ImportFromBitfinexAPI() {
      var toImport = await BitfinexAPI.DataGetter.GetData();
      var dbResolver = new DB.DbResolver();
      dbResolver.GenerateDB();
      dbResolver.SaveAPItoDB(toImport, DB.Sourse.Bitfinex);

    }

    private static async Task GetDataFromDB() {
      var report = new DB.DbResolver().GetReport(new DB.ReportParams());
      Console.WriteLine(report);

    }

  }

}