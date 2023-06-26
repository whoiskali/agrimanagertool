using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Entities;
using Infrastructure.Models.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, string connectionString, string assembly, Jwt jwt, string key)
        {
            services.AddDbContext<ApplicationDbContext>(
            options =>
                options.UseSqlServer(
                        connectionString,
                        b => b.MigrationsAssembly(assembly)
                    )
                );
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<ICryptography>(x => ActivatorUtilities.CreateInstance<Cryptography>(x, key));
            services
                    .AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        options.SaveToken = true;
                        options.RequireHttpsMetadata = false;

                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = jwt.ValidateIssuer ?? true,
                            ValidateAudience = jwt.ValidateAudience ?? true,
                            ValidAudience = jwt.ValidAudience,
                            ValidIssuer = jwt.ValidIssuer,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret))
                        };
                    });

            services.AddScoped<ITokenClient>(x => ActivatorUtilities.CreateInstance<JwtToken>(x, jwt));
            //FirebaseApp.Create(new AppOptions()
            //{
            //    Credential = GoogleCredential.FromFile(Environment.CurrentDirectory + "/firebase-admin-sdk.json")
            //});
            //services.AddScoped<INotificationService>(x => ActivatorUtilities.CreateInstance<FirebaseService>(x, firebase));
            //services.AddTransient<INotificationService, FirebaseService>();
        }
    }
}
