using Core;
using SearchAPI.Logic;

namespace SearchAPI2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Register ISearchLogic
            builder.Services.AddScoped<ISearchLogic, SearchLogic>();
                     builder.Services.AddHttpClient<ISearchLogic, SearchProxy>();
            builder.Services.AddSingleton<IDatabase, Database>(); // Ensure IDatabase is registered
            builder.Services.AddSingleton<ISearchLogic, SearchLogic>(); // Register SearchLogic

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
