using backend_challenge;
using Microsoft.AspNetCore.Mvc;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Sample data for clients
Client[] clients = Enumerable.Range(0, 4)
    .Select(index => new Client(index))
    .ToArray();

// Sample data for users
User[] users = Enumerable.Range(0, 4)
    .Select(index => new User(index,
        Enumerable.Range(0, 2)
            .Select(clientIndex => clients[clientIndex])
            .ToArray(), []))
    .ToArray();

Session[] sessions = [];

int nextSessionID = 0;

app.MapGet("/users", () => users)
    .WithName("GetUsers")
    .WithOpenApi();

app.MapGet("/clients", () => clients)
    .WithName("GetClients")
    .WithOpenApi();

app.MapGet("/users/{userID}/clients", (int userID) => users[userID].clients)
    .WithName("GetClientsByUserID")
    .WithOpenApi();

app.MapGet("/sessions", () => sessions)
    .WithName("GetSessions")
    .WithOpenApi();

app.MapPost("/sessions", (int userID, int clientID) =>
    {
        Session newSession = new(nextSessionID++, userID, clientID);
        sessions = sessions.Append(newSession).ToArray();
        return Results.Created($"/sessions/{newSession.sessionID}", newSession);
    })
    .WithName("CreateSession")
    .WithOpenApi();

// TODO: Implement file upload handling
// app.MapPost("/sessions/{sessionID}/files", async (int sessionID, IFormFile file) =>
// {
//     string tempFile = Path.GetTempFileName();
//     app.Logger.LogInformation(tempFile);
//     await using FileStream stream = File.OpenWrite(tempFile);
//     await file.CopyToAsync(stream);
// })
// .WithName("UploadFile")
// .DisableAntiforgery();

app.Run();