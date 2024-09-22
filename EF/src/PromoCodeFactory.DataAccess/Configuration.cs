﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PromoCodeFactory.DataAccess.Data;

namespace PromoCodeFactory.DataAccess
{
    public static class Configuration
    {
        public static IServiceCollection ConfigureContext(this IServiceCollection services)
        {
            services.AddDbContext<SQLiteDatabaseContext>(x => x.UseSqlite("Data Source = SQLite.db"));
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<SQLiteDatabaseContext>();
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }
            return services;
        }
    }
}
