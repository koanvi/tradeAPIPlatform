using System;
using System.Collections.Generic;
using System.Linq;

namespace koanvi.apiGetter {
  public class Program {
    public static void Main(string[] args) {
      MainAsync().GetAwaiter().GetResult();
      // var asdasd = BitfinexAPI.DataGetter.GetData();
    }
    private static async Task MainAsync() {
      var toImport = await BitfinexAPI.DataGetter.GetData();

      Console.WriteLine(toImport);

    }
  }

}