using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using aow3.Configuration.Dto;

namespace aow3.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : aow3AppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
