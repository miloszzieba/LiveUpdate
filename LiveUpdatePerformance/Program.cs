using LiveUpdatePerformance;
using LiveUpdatePerformance.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPermission", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:3000", "http://localhost:8080", "http://127.0.0.1:5173")
            .AllowCredentials();
    });
});

builder.Services.AddSingleton<DataService>();

builder.Services.AddScoped<LiveUpdateHub>(serviceProvider => new LiveUpdateHub(serviceProvider.GetRequiredService<DataService>()));

builder.Services.AddSignalR();

builder.Services.Configure<HostOptions>(hostOptions =>
{
    hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});
builder.Services.AddHostedService<Sender>();

var app = builder.Build();

app.UseCors("ClientPermission");

app.MapHub<LiveUpdateHub>("/liveupdate");

app.Run();
