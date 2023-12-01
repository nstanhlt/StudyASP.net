using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace aow3.EntityFrameworkCore
{
    public static class aow3DbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<aow3DbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<aow3DbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
