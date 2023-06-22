using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            return services;
        }
    }
}
