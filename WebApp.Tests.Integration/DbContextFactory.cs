﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApp.DataAccessLayer;

namespace WebApp.Tests.Integration
{
    public static class DbContextFactory
    {
        private static DataLayer dataLayerInstance;

        public static DataLayer DataAccessLayerInstance
        {
            get
            {
                if (DbContextFactory.dataLayerInstance == null)
                {
                    DbContextFactory.dataLayerInstance = DbContextFactory.CreateDataAccessLayerInstance();
                }
                return DbContextFactory.dataLayerInstance;
            }
        }

        private static DataLayer CreateDataAccessLayerInstance()
        {
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();
            var dbContextBuilder = new DbContextOptionsBuilder<DataDbContext>();
            dbContextBuilder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=WebAppSolution;Trusted_Connection=True;MultipleActiveResultSets=true")
                .UseInternalServiceProvider(serviceProvider);

            var dbContext = new DataDbContext(dbContextBuilder.Options);

            return new DataLayer(dbContext);
        }
    }
}
