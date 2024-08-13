using EmailServiceClientGrpcApp;
using Microsoft.Extensions.DependencyInjection;

namespace Wallet.BLL.Grpc
{
    public static class GrpcDIExtension
    {
        public static IServiceCollection ConfigureGrpc(this IServiceCollection services)
        {
            services.AddGrpcClient<EmailServiceGrpc.EmailServiceGrpcClient>(o =>
            {
                o.Address = new Uri("https://localhost:7212");
            });

            return services;
        }
    }
}
