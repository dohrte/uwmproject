using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using DbUp;

namespace Garage.DbUp
{
  public static class Program
  {
    public static int Main(string[] args)
    {
      Console.WriteLine("Running DbUp for Garage Database.");

      Console.WriteLine("Waiting 30 seconds for SQL Server to start up.");
      Console.WriteLine("Counting down:");
      for (int i = 30; i >= 0; i--)
      {
        Console.WriteLine(i);
        Thread.Sleep(1_000);
      }

      var connectionString =
          args.FirstOrDefault()
          ?? Environment.GetEnvironmentVariable("CONNECTION_STRING");

      EnsureDatabase.For.SqlDatabase(connectionString);

      var upgrader =
          DeployChanges.To
              .SqlDatabase(connectionString)
              .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
              .LogToConsole()
              .Build();

      var result = upgrader.PerformUpgrade();

      if (!result.Successful)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(result.Error);
        Console.ResetColor();
#if DEBUG
        Console.ReadLine();
#endif
        return -1;
      }

      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("Success!");
      Console.ResetColor();
      return 0;
    }
  }
}
