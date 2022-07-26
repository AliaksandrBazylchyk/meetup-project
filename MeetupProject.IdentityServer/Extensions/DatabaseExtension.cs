using Duende.IdentityServer.EntityFramework.Mappers;
using MeetupProject.IdentityServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MeetupProject.IdentityServer.Extensions
{
    public static class DatabaseExtension
    {
        /// <summary>
        /// Method create> migrate and seed 3 databases for Identity Server and ASP.NET Identity
        /// </summary>
        /// <param name="app">web-application "MeetupProject.IdentityServer"</param>
        public static async void InitializeDatabase(this IApplicationBuilder app)
        {
            // Getting services factory
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();
                
            // Getting the Persisted Grants Database context and migrate it
            await serviceScope.ServiceProvider.GetRequiredService<Duende.IdentityServer.EntityFramework.DbContexts.PersistedGrantDbContext>().Database.MigrateAsync();

            // Getting the Identity Server Configuration Database context and introduce it for seeds
            var context = serviceScope.ServiceProvider.GetRequiredService<Duende.IdentityServer.EntityFramework.DbContexts.ConfigurationDbContext>();

            // Migrate Identity Server configurations Database
            await context.Database.MigrateAsync();

            // Seed configurations for identity Server from Config.cs
            if (!await context.Clients.AnyAsync())
            {
                foreach (var client in Config.Clients)
                {
                    await context.Clients.AddAsync(client.ToEntity());
                }

                await context.SaveChangesAsync();
            }
            if (!await context.IdentityResources.AnyAsync())
            {
                foreach (var resource in Config.IdentityResources)
                {
                    await context.IdentityResources.AddAsync(resource.ToEntity());
                }

                await context.SaveChangesAsync();
            }
            if (!await context.ApiScopes.AnyAsync())
            {
                foreach (var resource in Config.ApiScopes)
                {
                    await context.ApiScopes.AddAsync(resource.ToEntity());
                }

                await context.SaveChangesAsync();
            }

            // Getting the ASP.NET Identity core database context and introduce it for seeds
            var identityContext = serviceScope.ServiceProvider.GetRequiredService<IdentityServerDbContext>();

            // Getting ASP.NET Identity core User Manager service
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser<Guid>>>();

            // Migrate ASP.NET Identity Database
            await identityContext.Database.MigrateAsync();

            // Seed users for ASP.NET Identity
            if (!await identityContext.Users.AnyAsync())
            {
                var hasher = new PasswordHasher<IdentityUser<Guid>>();
                var userEntity = new IdentityUser<Guid>
                {
                    Id = Guid.NewGuid(),
                    Email = "test@gmail.com",
                    NormalizedEmail = "TEST@gmail.com",
                    UserName = "test",
                    NormalizedUserName = "TEST"
                };
                userEntity.PasswordHash = hasher.HashPassword(userEntity, "root");
                await userManager.CreateAsync(userEntity);

                await context.SaveChangesAsync();
            }
        }
    }
}
