using System.Threading.Tasks;
using Abp.Application.Services;
using aow3.Authorization.Accounts.Dto;

namespace aow3.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
