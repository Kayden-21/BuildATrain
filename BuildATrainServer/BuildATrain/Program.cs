using BuildATrain.Services;
using Lib.AspNetCore.ServerSentEvents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddServerSentEvents();

builder.Services.AddServerSentEvents<IEventsService, EventsService>(options =>
{
    options.ReconnectInterval = 5000;
});

builder.Services.AddHostedService<Heartbeat>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
