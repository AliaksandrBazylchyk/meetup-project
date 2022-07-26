using MeetupProject.DAL.Contextes;
using Microsoft.EntityFrameworkCore;

namespace MeetupProject.API.Extensions
{
    public static class DatabaseSeedExtension
    {
        public static async void MigrateDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();
            await serviceScope.ServiceProvider.GetRequiredService<EventDbContext>().Database.MigrateAsync();
        }
    }
}
