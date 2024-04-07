using RF_Redmine.Classes;
using Microsoft.AspNetCore.Cors;
using RF_Redmine.Classes.Db_Classes;
using System.Data.SQLite;

namespace RF_Redmine
{
    public class Program
    {
        Program(string[] args)
        {
            db.kapcsolodik();
            startup(args);
        }

        void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy", builder => {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
        }

        void startup(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            ConfigureServices(builder.Services);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(/*sui =>
                {
                    sui.SwaggerEndpoint("/swagger/v1/swagger.json", "YOLO");
                }*/);
            }
            app.UseCors("CorsPolicy");
            //app.UseRouting();
            //app.UseDefaultFiles();
            app.UseStaticFiles(); // Serve static files from the web root directory

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        public static void Main(string[] args)
        {
            new Program(args);
        }
    }
}
