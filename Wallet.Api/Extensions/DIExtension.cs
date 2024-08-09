using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Wallet.BLL.Logic.Contracts.Kafka;
using Wallet.BLL.Logic.Contracts.Notififcation;
using Wallet.BLL.Logic.Contracts.Users;
using Wallet.BLL.Logic.Notification;
using Wallet.BLL.Logic.Users;
using Wallet.Common.Entities.HttpClientts;
using Wallet.DAL.Repository;
using Wallet.DAL.Repository.Contracts;
using Wallet.DAL.Repository.EF;
using Wallet.BLL.Logic.Contracts.Auth;
using Wallet.BLL.Logic.Contracts.Permissions;
using Wallet.BLL.Logic.Authz;
using Wallet.BLL.Logic.Auth;
using Wallet.Common.Entities.Auth;

using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using Wallet.BLL.Logic.KafkaService;

namespace Wallet.Api.Extensions
{
    public static class DIExtension
    {
        public static IServiceCollection ConfigureBLLDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserLogic, UserLogic>();
            services.AddScoped<INotificationLogic, NotificationLogic>();
            services.AddScoped<IKafkaProducer, KafkaProducer>();

            return services;
        }

        public static IServiceCollection ConfigureDALDependencies(this IServiceCollection services)
        {
            services.AddScoped<IEFUserRepository, EFUserRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }

        public static IServiceCollection ConfigureHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddHttpClient(
                HttpClientNames.EMAIL_SERVICE,
                client =>
                {  
                    client.BaseAddress = new Uri("https://localhost:7212/api/EmailSender/send");
                });
            return services;
        }

        public static IServiceCollection ConfigureAuth(this IServiceCollection services)
        {
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtProvider, JwtProvider>();

            return services;
        }

        public static void AddApiAuthentification(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;

                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                                   Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
                    };
                    //cookies не из запроса

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Request.Cookies.TryGetValue("wallet-sec-cookies", out var accessToken);
                            context.Token = accessToken;

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddScoped<PermissionService, PermissionService>();
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

            services.AddAuthorization();
        }
    }
}
