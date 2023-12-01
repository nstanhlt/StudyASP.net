using Abp.Application.Services;
using aow3.MultiTenancy.Dto;

namespace aow3.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

