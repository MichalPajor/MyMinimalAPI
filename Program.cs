using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyMinimalAPI.Data;
using MyMinimlAPI.DTOs;
using MyMinimalAPI.Models;
using MyMinimalAPI.Middleware;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "MyMinimalAPI",
        Description = "My simple example of MinimalAPI",
        Contact = new OpenApiContact
        {
            Name = "Michal",
            Email = "myExample@email.com",
            Url = new Uri("https://google.pl/")

        }
    });
    options.AddSecurityDefinition("APIKEY", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Name = "X-API-KEY", //header with api key
        Type = SecuritySchemeType.ApiKey,
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "APIKEY"
                }
            },
            new string[] {}
        }
    });
});


//Build docker connection string with secrets
var sqlConnBuilder = new SqlConnectionStringBuilder();
sqlConnBuilder.ConnectionString = builder.Configuration.GetConnectionString("DockerConnection");
sqlConnBuilder.UserID = builder.Configuration["UserId"];
sqlConnBuilder.Password = builder.Configuration["Password"];

//Add db context
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(sqlConnBuilder.ConnectionString))
//Register repository interface
builder.Services.AddScoped<ICommandRepo, CommandRepo>();
//Register mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

//Config Swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
//Use Api key middleware
app.UseMiddleware<ApiKeyMiddleware>();

//***ENDPOINTS***

//Get all commands
app.MapGet("api/v1/commands",async (ICommandRepo repo, IMapper mapper) =>{
    var commands = await repo.GetAllCommandsAsync();
    if(commands.Count() == 0)
        return Results.NotFound();
    return Results.Ok(mapper.Map<IEnumerable<CommandReadDto>>(commands));
});

//Get command by ID
app.MapGet("api/v1/commands/{id}", async (ICommandRepo repo, IMapper mapper, int id) =>{
    var command = await repo.GetCommandByIdAsync(id);
    if(command == null)
        return Results.NotFound();
    return Results.Ok(mapper.Map<CommandReadDto>(command));
});

//Create new command
app.MapPost("api/v1/commands", async (ICommandRepo repo, IMapper mapper, CommandCreateDto command) =>{
    var commandModel = mapper.Map<Command>(command);
    await repo.CreateCommandAsync(commandModel);
    await repo.SaveChangesAsync();

    var cmdReadDto = mapper.Map<CommandReadDto>(commandModel);

    return Results.Created($"api/v1/commands/{cmdReadDto.Id}",cmdReadDto);
});

//Update command
app.MapPut("api/v1/commands/{id}", async(ICommandRepo repo, IMapper mapper, int id, CommandUpdateDto command) =>{
    var commandFromDb = await repo.GetCommandByIdAsync(id);
    if(commandFromDb == null)
        return Results.NotFound();
    mapper.Map(command,commandFromDb);
    await repo.SaveChangesAsync();
    return Results.NoContent();
} );

//Delete command
app.MapDelete("api/v1/commands/{id}", async(ICommandRepo repo, int id) => {
    var commandFromDb = await repo.GetCommandByIdAsync(id);
    if(commandFromDb == null)
        return Results.NotFound();
    repo.DeleteCommand(commandFromDb);
    await repo.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();

