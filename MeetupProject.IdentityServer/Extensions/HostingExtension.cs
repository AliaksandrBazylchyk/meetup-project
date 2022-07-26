using MeetupProject.IdentityServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

namespace MeetupProject.IdentityServer.Extensions;

internal static class HostingExtension
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        IConfiguration configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
        var identityServerDatabaseConnectionString = configuration.GetSection("IDENTITY_SERVER_DATABASE_CONNECTION_STRING").Value;
        var aspNetIdentityDatabaseConnectionString = configuration.GetSection("ASP_NET_IDENTITY_DATABASE_CONNECTION_STRING").Value;

        var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

        // Controllers for ASP.NET Identity endpoints
        builder.Services.AddControllers();

        // Database context for ASP.NET Identity
        builder.Services.AddDbContext<IdentityServerDbContext>(options =>
            options.UseNpgsql(aspNetIdentityDatabaseConnectionString));

        // Enabling ASP.NET Identity with extensible roles and users (UserEntity, RoleEntity)
        builder.Services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<IdentityServerDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthorization();

        builder.Services
            .AddIdentityServer(options => { options.EmitStaticAudienceClaim = true; })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = contextOptionsBuilder =>
                    contextOptionsBuilder.UseNpgsql(identityServerDatabaseConnectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = contextOptionsBuilder =>
                    contextOptionsBuilder.UseNpgsql(identityServerDatabaseConnectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddAspNetIdentity<IdentityUser<Guid>>()
            .AddDeveloperSigningCredential();

        builder.Services.AddCors();

        return builder.Build();
    }

    public static async Task<WebApplication> ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        // Database extension initializer
        app.InitializeDatabase();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseIdentityServer();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
        app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        return app;
    }
}
