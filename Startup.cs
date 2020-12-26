using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace StartupIConfigurationInjectionProblem
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Console.WriteLine("");
      var root = (IConfigurationRoot) configuration;

      Console.WriteLine("Displaying loaded config providers of incjected IConfiguration in Startup.cs CTOR:");

      var loadedAppsettingsLocalhostJson = false;
      foreach (var provider in root.Providers)
      {
        var providerName = provider.ToString();
        Console.WriteLine("Provider: {0}", providerName);

        if (providerName.Contains("appsettings.localhost.json"))
        {
          loadedAppsettingsLocalhostJson = true;
        }
      }

      Console.WriteLine($"\nWas appsettings.localhost.json loaded? {loadedAppsettingsLocalhostJson}\n");

      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

      services.AddControllers();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "StartupIConfigurationInjectionProblem", Version = "v1" });
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StartupIConfigurationInjectionProblem v1"));
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
