using MeetupProject.API.Extensions;
using MeetupProject.API.Middlewares;
using MeetupProject.BLL.MappingProfiles;
using MeetupProject.BLL.Services.EventService;
using MeetupProject.DAL.Repositories;
using MeetupProject.DAL.Repositories.EventDbRepositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

IConfiguration configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
var EventsDatabaseConnectionString = configuration.GetSection("EVENTS_DATABASE_CONNECTION_STRING").Value;

builder.Services.AddDbCollectionAsync(EventsDatabaseConnectionString);

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IEventRepository, EventRepository>();

builder.Services.AddAutoMapper(typeof(BllMappingProfile));

builder.Services.AddScoped<IEventService, EventService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase();

app.UseMiddleware<ExceptionMiddleware>();

app.Run();
