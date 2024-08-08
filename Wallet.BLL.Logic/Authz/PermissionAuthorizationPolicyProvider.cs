using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Wallet.Common.Entities.Authz;

namespace Wallet.BLL.Logic.Authz
{
    public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly ILogger<PermissionAuthorizationPolicyProvider> _logger;

        public PermissionAuthorizationPolicyProvider(IOptions<Microsoft.AspNetCore.Authorization.AuthorizationOptions> options, ILogger<PermissionAuthorizationPolicyProvider> logger) : base(options)
        {
            _logger = logger;
        }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            PermissionRequirement prm;

            try
            {
                AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName);


                if (policy is not null)
                {
                    return policy;
                }

                string[] groups = policyName.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                Permission[] enams = new Permission[groups.Length];

                for (int i = 0; i < groups.Length; i++)
                {
                    enams[i] = (Permission)Enum.Parse(typeof(Permission), groups[i].ToString());
                }

                prm = new PermissionRequirement(enams);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в GetPolicyAsync");
                throw;
            }

            return new AuthorizationPolicyBuilder()
                .AddRequirements(prm)
                .Build();
        }
    }
}
