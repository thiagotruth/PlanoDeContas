using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class InfrastructureDependency
    {
        public static void AddInfrastructureDependencies(this IServiceCollection services)
        {
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

            var connectionString = @"Data Source=(localdb)\Local;Initial Catalog=PlanoDeContas;User ID=sa;Password=pa$$word123#";//$"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword}";

            services.AddDbContext<PlanoDeContasContext>(opt => opt.UseSqlServer(connectionString));
            services.AddScoped<IContaRepository, ContaRepository>();
        }
    }
}
