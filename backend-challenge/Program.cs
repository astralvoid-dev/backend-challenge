using backend_challenge;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Could not implement because of time constraints
// builder.Services.AddDbContext<AppDBContext>(opt => opt.UseInMemoryDatabase("AppDBContext"));

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
            .ToArray()))
    .ToArray();

// File upload sessions
Session[] sessions = [];

// Counter for session IDs
int nextSessionID = 0;

app.MapGet("/users", () => users)
    .WithName("GetUsers")
    .WithOpenApi();

app.MapGet("/clients", () => clients)
    .WithName("GetClients")
    .WithOpenApi();

app.MapGet("/users/{userID}/clients", (int userID) =>
    {
        if(userID < 0 || userID >= users.Length)
        {
            return Results.BadRequest(new { message = "Invalid user ID." });
        }
        return Results.Ok(users[userID].clients);
    })
    .WithName("GetClientsByUserID")
    .WithOpenApi();

app.MapGet("/sessions", () => sessions)
    .WithName("GetSessions")
    .WithOpenApi();

app.MapGet("/clients/{clientID}/sessions", (int clientID) =>
    {
        if(clientID < 0 || clientID >= clients.Length)
        {
            return Results.BadRequest(new { message = "Invalid client ID." });
        }
        return Results.Ok(clients[clientID].sessions);
    })
    .WithName("GetSessionsByClientID")
    .WithOpenApi();

app.MapGet("/sessions/{sessionID}/status", (int sessionID) =>
    {
        if(sessionID < 0 || sessionID >= sessions.Length)
        {
            return Results.BadRequest(new { message = "Invalid session ID." });
        }
        
        switch (sessions[sessionID].status)
        {
            case UploadStatus.Pending:
                return Results.Ok(new { status = "Pending" });
            case UploadStatus.Completed:
                return Results.Ok(new { status = "Completed" });
            default:
                return Results.NotFound(new { message = "Status unknown." });
        }
    })
    .WithName("GetSessionStatus")
    .WithOpenApi();

app.MapPost("/sessions", (int userID, int clientID) =>
    {
        if (userID < 0 || userID >= users.Length)
        {
            return Results.BadRequest(new { message = "Invalid user ID." });
        }
        
        if (clientID < 0 || clientID >= clients.Length)
        {
            return Results.BadRequest(new { message = "Invalid client ID." });
        }
        
        if (!users[userID].clients.Contains(clients[clientID]))
        {
            return Results.BadRequest(new { message = "Client does not belong to the user." });
        }
        
        Session newSession = new(nextSessionID++, userID, clientID);
        sessions = sessions.Append(newSession).ToArray();
        clients[clientID].AddSession(newSession);
        return Results.Created($"/sessions/{newSession.sessionID}", newSession);
    })
    .WithName("CreateSession")
    .WithOpenApi();

app.MapPost("/sessions/{sessionID}/files", async (int sessionID, IFormFile file) =>
    {
        if (sessionID < 0 || sessionID >= sessions.Length)
        {
            return Results.BadRequest(new { message = "Invalid session ID." });
        }
        
        if (file.ContentType != "application/pdf")
        {
            return Results.BadRequest(new { message = "Only PDF files are allowed." });
        }

        sessions[sessionID].AddFile(file);

        return Results.Ok(new { message = "File uploaded successfully." });
    })
    .WithName("UploadFile")
    .DisableAntiforgery();

app.Run();