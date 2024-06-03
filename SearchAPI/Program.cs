using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Core;
using SearchAPI.Logic;
using Shared;

namespace SearchAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpClient<ISearchLogic, SearchProxy>();
            builder.Services.AddSingleton<IDatabase, Database>(); // Sørger for at IDatabase er registreret 
            builder.Services.AddSingleton<ISearchLogic, SearchLogic>(); // Registrer SearchLogic
            builder.Services.AddControllers();

            var app = builder.Build();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}