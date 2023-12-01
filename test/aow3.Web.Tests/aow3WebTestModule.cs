using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using aow3.EntityFrameworkCore;
using aow3.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace aow3.Web.Tests
{
    [DependsOn(
        typeof(aow3WebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class aow3WebTestModule : AbpModule
    {
        public aow3WebTestModule(aow3EntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(aow3WebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(aow3WebMvcModule).Assembly);
        }
    }
}