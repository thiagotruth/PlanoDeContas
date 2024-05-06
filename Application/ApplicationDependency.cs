using Application.Services;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class ApplicationDependency
    {
        public static void AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddScoped<IContaService, ContaService>();
        }
    }
}
