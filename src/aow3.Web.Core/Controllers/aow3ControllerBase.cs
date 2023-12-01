using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace aow3.Controllers
{
    public abstract class aow3ControllerBase: AbpController
    {
        protected aow3ControllerBase()
        {
            LocalizationSourceName = aow3Consts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
