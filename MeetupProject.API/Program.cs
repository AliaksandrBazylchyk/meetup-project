using MeetupProject.API.Extensions;
using MeetupProject.API.MappingProfiles;
using MeetupProject.API.Middlewares;
using MeetupProject.BLL.MappingProfiles;
using MeetupProject.BLL.Services.EventService;
using MeetupProject.DAL.Repositories;
using MeetupProject.DAL.Repositories.EventDbRepositories;

var builder = WebApplication.CreateBuilder(args);

// Load controllers and endpoints
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Load Swagger with authorization
builder.Services.AddConfiguredSwaggerGen();

// Load configuration string from docker enviroment
IConfiguration configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
var EventsDatabaseConnectionString = configuration.GetSection("EVENTS_DATABASE_CONNECTION_STRING").Value;
var identityServerConnectionString = configuration.GetSection("IDENTITY_SERVER_CONNECTION_STRING").Value;

// Load Events database
builder.Services.AddDbCollection(EventsDatabaseConnectionString);

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IEventRepository, EventRepository>();

builder.Services.AddAutoMapper(typeof(BllMappingProfile));
builder.Services.AddAutoMapper(typeof(ApiMappingProfile));

builder.Services.AddScoped<IEventService, EventService>();

builder.Services.AddJWTAuthentication(identityServerConnectionString);

builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.MigrateDatabase();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.Run();
