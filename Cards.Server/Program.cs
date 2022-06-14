using Cards.Server.Services;
using Cards.Server.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IGameProvider, GameProvider>();
builder.Services.AddSignalR();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapHub<GameHub>("/game");


app.Run();
