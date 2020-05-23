using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace TextDB
{
    sealed class Engine
    {
        private static readonly Engine _dbEgine = new Engine();

        public Settings Config { get; private set; }

        private Engine()
        {
            LoadConfig();
        }

        public static Engine Instance
        {
            get
            {
                return _dbEgine;
            }
        }

        public void LoadConfig()
        {
            Config = new Settings();

            if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["textdbpath"]))
            {
                Config.FilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\textdb\\";
            }
            else
            {
                Config.FilePath = ConfigurationManager.AppSettings["textdbpath"];
            }

            Directory.CreateDirectory(Config.FilePath);
        }
    }
}
