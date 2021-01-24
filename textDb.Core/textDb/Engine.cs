using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace textDb
{
    sealed class Engine
    {
        private IConfiguration _configuration;

        public Settings Config { get; }

        private Engine()
        {
            Config = new Settings
            {
                FilePath =  System.IO.Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "textdb")
            };
            Directory.CreateDirectory(Config.FilePath);
        }

        private void SetConfig(IConfiguration configuration)
        {
            _configuration = configuration;
            if (_configuration != null && !string.IsNullOrWhiteSpace(_configuration["textdbpath"]))
            {
                Config.FilePath = _configuration["textdbpath"];
                Directory.CreateDirectory(Config.FilePath);
            }
        }

        public static Engine Instance { get; } = new Engine();
    }
}
