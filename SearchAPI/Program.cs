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
            builder.Services.AddSingleton<IDatabase, Database>(); // Ensure IDatabase is registered
            builder.Services.AddSingleton<ISearchLogic, SearchLogic>(); // Register SearchLogic
            builder.Services.AddControllers();

            var app = builder.Build();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}



/*
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<ISearchLogic, SearchProxy>();
builder.Services.AddSingleton<IDatabase, Database>(); // Ensure IDatabase is registered
builder.Services.AddSingleton<ISearchLogic, SearchLogic>(); // Register SearchLogic
builder.Services.AddControllers();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
*/


/*
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHttpClient<ISearchLogic, SearchProxy>();
        builder.Services.AddControllers();

        var app = builder.Build();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();

    }
}
*/