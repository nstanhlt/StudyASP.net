﻿using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using aow3.Authorization;

namespace aow3
{
    [DependsOn(
        typeof(aow3CoreModule), 
        typeof(AbpAutoMapperModule))]
    public class aow3ApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<aow3AuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(aow3ApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
