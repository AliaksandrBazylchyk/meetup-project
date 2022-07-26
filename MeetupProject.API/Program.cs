using MeetupProject.API.Extensions;
using MeetupProject.API.MappingProfiles;
using MeetupProject.API.Middlewares;
using MeetupProject.BLL.MappingProfiles;
using MeetupProject.BLL.Services.EventService;
using MeetupProject.DAL.Repositories;
using MeetupProject.DAL.Repositories.EventDbRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Meetup API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

IConfiguration configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
var EventsDatabaseConnectionString = configuration.GetSection("EVENTS_DATABASE_CONNECTION_STRING").Value;
var identityServerConnectionString = configuration.GetSection("IDENTITY_SERVER_CONNECTION_STRING").Value;

builder.Services.AddDbCollection(EventsDatabaseConnectionString);

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IEventRepository, EventRepository>();

builder.Services.AddAutoMapper(typeof(BllMappingProfile));
builder.Services.AddAutoMapper(typeof(ApiMappingProfile));

builder.Services.AddScoped<IEventService, EventService>();

builder.Services.AddAuthentication(s =>
{
    s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = identityServerConnectionString;
    /*********************************************************/
    /*          TODO Comment this code on Release            */
    /*********************************************************/
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters.ValidateAudience = false;
    options.TokenValidationParameters.ValidateIssuer = false;
    /*********************************************************/
});

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
