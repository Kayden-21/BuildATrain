using Microsoft.EntityFrameworkCore;
using BuildATrain.Database.Repositories;
using BuildATrain.Services;
using Lib.AspNetCore.ServerSentEvents;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = new ConfigurationBuilder()
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        ConfigureServices(builder.Services, configuration);
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        
        app.UseRouting()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapServerSentEvents("/sse-heartbeat");
        endpoints.MapServerSentEvents<EventsService>("/sse-events");

        endpoints.MapControllerRoute("default", "{controller=EventsController}/{action=sse-events-receiver}");
    });

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddServerSentEvents();

        services.AddServerSentEvents<IEventsService, EventsService>(options =>
        {
            options.ReconnectInterval = 5000;
        });

        services.AddHostedService<Heartbeat>();

        services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("BuildATrain")));

        // Configure repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}
