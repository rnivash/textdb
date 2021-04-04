using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System;

namespace textDb
{
    sealed class Engine
    {
      
        public Settings Config { get; }

        private Engine()
        {
            
            var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json")
                                .Build();

            Config = new Settings
            {
                FilePath =  System.IO.Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "textdb")
            };

            SetConfig(configuration);

            Directory.CreateDirectory(Config.FilePath);
        }

        private void SetConfig(IConfiguration configuration)
        {
            if (configuration != null && !string.IsNullOrWhiteSpace(configuration["textdbpath"]))
            {
                Config.FilePath = configuration["textdbpath"];
            }
        }

        public static Engine Instance { get; } = new Engine();
    }
}
