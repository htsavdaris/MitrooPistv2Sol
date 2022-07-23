using Microsoft.Extensions.Configuration;
using System;

namespace MitrooPistv2.API

{
    public class DbConnectionStringResolver
    {

        private readonly IConfiguration configuration;

        public DbConnectionStringResolver(IConfiguration config)
        {
            this.configuration = config;

        }

        public string GetNameOrConnectionString()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var use_docker = Environment.GetEnvironmentVariable("USE_DOCKER");            
            switch (environment)
            {
                case "Development":
                    {
                        if (use_docker == "False")
                        {
                            return configuration["ConnectionStrings:LocalDBConnection"]; 
                        }
                        else
                        {
                            return configuration["ConnectionStrings:LocalDockerConnection"]; 
                        }
                    }                                                                                                                     
                case "Production": return null;
                default:
                    return configuration.GetConnectionString("ConnectionStrings:LocalDBConnection");
            }
        }

    }
}
