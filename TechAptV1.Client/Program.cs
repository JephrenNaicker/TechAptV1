// Copyright © 2025 Always Active Technologies PTY Ltd

using Microsoft.EntityFrameworkCore;
using Serilog;
using TechAptV1.Client.Components;
using TechAptV1.Client.DatabaseContext;
using TechAptV1.Client.Interface;
using TechAptV1.Client.Services;

namespace TechAptV1.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.Title = "Tech Apt V1";

                var builder = WebApplication.CreateBuilder(args);

                builder.Services.AddSerilog(lc => lc
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
                    .ReadFrom.Configuration(builder.Configuration));

                // Add services to the container.
                builder.Services.AddRazorComponents().AddInteractiveServerComponents();
                builder.Services.AddDbContext<DataContext>(options =>options.UseSqlite(builder.Configuration.GetConnectionString("Default")));
                builder.Services.AddScoped<IDataService, DataService>();
                builder.Services.AddScoped<IThreadingService, ThreadingService>();
                builder.Services.AddScoped<NotificationService>();

                var app = builder.Build();

                // Ensure the database is created and migrations are applied
                using (var scope = app.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
                    try
                    {
                        db.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Database migration failed: {ex.Message}");
                    }
                }

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Error");
                }

                app.UseStaticFiles();
                app.UseAntiforgery();

                app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

                app.Run();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Fatal exception in Program");
                Console.WriteLine(exception);
            }
        }
    }
}
