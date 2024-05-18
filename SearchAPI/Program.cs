using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Core;
using SearchAPI.Logic;

namespace SearchAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting Search API...");
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("policy",
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin();
                                  });
            });

            // Register the SearchLogic implementation
            builder.Services.AddSingleton<ISearchLogic, SearchLogic>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();
            app.UseCors("policy");
            app.UseAuthorization();
            app.MapControllers();

            Console.WriteLine("Search API is running.");
            app.Run();
        }
    }
}
