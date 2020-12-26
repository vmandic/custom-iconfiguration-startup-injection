using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace StartupIConfigurationInjectionProblem
{
  public class Program
  {
    private static IConfigurationBuilder ConfigurationBuilder;
    public static async Task Main(string[] args)
    {
      ConfigurationBuilder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional : false)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional : true)
        .AddJsonFile("appsettings.localhost.json", optional : true)
        .AddEnvironmentVariables();

      await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
      return Host.CreateDefaultBuilder(args)
        .UseContentRoot(Directory.GetCurrentDirectory())
        .ConfigureAppConfiguration(configBuilder =>
        {
          Console.WriteLine("ConfigureAppConfiguration called...");

          // NOTE: this does not work, we will have to reassign one by one in foreach below
          //configBuilder = ConfigurationBuilder; 
          
          // NOTE: clear existing stack built by CreateDefaultBuilder method
          configBuilder.Sources.Clear();

          foreach (var configSource in ConfigurationBuilder.Sources)
          {
            configBuilder.Add(configSource);
          }
        })
        .ConfigureWebHostDefaults(webBuilder =>
          webBuilder.UseStartup<Startup>());
    }
  }
}
