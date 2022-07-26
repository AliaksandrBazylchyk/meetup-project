using MeetupProject.DAL.Contextes;
using Microsoft.EntityFrameworkCore;

namespace MeetupProject.API.Extensions
{
    public static class DatabaseExtension
    {
        public static async Task<IServiceCollection> AddDbCollectionAsync(
            this IServiceCollection services,
            string connectionString
        )
        {
            services.AddDbContext<EventDbContext>(s =>
            {
                s.UseNpgsql(connectionString);
            });

            return services;
        }
    }
}
