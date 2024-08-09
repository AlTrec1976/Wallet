using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.CookiePolicy;
using Wallet.Api.Extensions;
using Wallet.Common.Entities.Auth;
using Wallet.DAL.Repository.EF;
using Wallet.Common.Entities.KafkaModels;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
builder.Services.Configure<Kafka>(configuration.GetSection(nameof(Kafka)));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureBLLDependencies();
builder.Services.ConfigureDALDependencies();
builder.Services.ConfigureHttpClients();
builder.Services.ConfigureAuth();
builder.Services.AddApiAuthentification(configuration);

builder.Services.AddDbContext<DBContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DBConnection")));

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
