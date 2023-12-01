﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using aow3.Configuration;
using aow3.Web;

namespace aow3.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class aow3DbContextFactory : IDesignTimeDbContextFactory<aow3DbContext>
    {
        public aow3DbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<aow3DbContext>();
            
            /*
             You can provide an environmentName parameter to the AppConfigurations.Get method. 
             In this case, AppConfigurations will try to read appsettings.{environmentName}.json.
             Use Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") method or from string[] args to get environment if necessary.
             https://docs.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#args
             */
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            aow3DbContextConfigurer.Configure(builder, configuration.GetConnectionString(aow3Consts.ConnectionStringName));

            return new aow3DbContext(builder.Options);
        }
    }
}
