using Abp.Authorization;
using aow3.Authorization.Roles;
using aow3.Authorization.Users;

namespace aow3.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
