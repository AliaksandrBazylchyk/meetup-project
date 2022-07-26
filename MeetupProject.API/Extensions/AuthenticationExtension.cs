using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MeetupProject.API.Extensions
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddJWTAuthentication(
            this IServiceCollection services,
            string identityServerConnectionString
        )
        {
            services.AddAuthentication(s =>
            {
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = identityServerConnectionString;
                options.TokenValidationParameters.ValidateAudience = false;
                options.TokenValidationParameters.ValidateIssuer = false;
            });

            return services;
        }
    }
}
