using MeetupProject.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MeetupProject.DAL.Contextes
{
    public sealed class EventDbContext : DbContext
    {
        public DbSet<EventEntity> Events { get; set; }

        public EventDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(EventDbContext).Assembly);
        }
    }
}
