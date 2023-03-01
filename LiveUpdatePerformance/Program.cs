using LiveUpdatePerformance;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPermission", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:3000")
            .AllowCredentials();
    });
});

builder.Services.AddControllers();

builder.Services.AddSignalR();

builder.Services.Configure<HostOptions>(hostOptions =>
{
    hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});
builder.Services.AddHostedService<Sender>();

var app = builder.Build();

app.UseCors("ClientPermission");

app.MapHub<LiveUpdateHub>("/liveupdate");

app.MapControllers();

app.Run();
