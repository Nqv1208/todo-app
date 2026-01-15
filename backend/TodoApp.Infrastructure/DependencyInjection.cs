using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace TodoApp.Infrastructure
{
    public class DependencyInjection
    {
        public static void AddInfrastructureServices(IHostApplicationBuilder builder)
        {
            var connectionStrng = builder.Configuration.GetConnectionString("TodoAppDb");
        }
    }
}