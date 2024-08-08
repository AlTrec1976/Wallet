using Microsoft.AspNetCore.Authorization;
using System.Text;
using Wallet.Common.Entities.Authz;

namespace Wallet.BLL.Logic.Authz
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission[] permission)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Permission permissionItem in permission)
            {
                sb.Append("," + permissionItem.ToString());
            }

            sb.Remove(0, 1);
            base.Policy = sb.ToString();
        }
    }
}
