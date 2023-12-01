using System.Threading.Tasks;
using aow3.Configuration.Dto;

namespace aow3.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
