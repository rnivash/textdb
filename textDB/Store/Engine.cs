using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace TextDB.Store
{
    internal class Engine
    {
        private static volatile Engine _dbEgine;

        private static object syncRoot = new Object();

        public Settings CurrentConfig { get; private set; }

        private Engine()
        {
            LoadConfig();
        }

        public static Engine Instance
        {
            get
            {
                if (_dbEgine == null)
                {
                    lock (syncRoot)
                    {
                        if (_dbEgine == null)
                            _dbEgine = new Engine();
                    }
                }
                return _dbEgine;
            }
        }

        public void LoadConfig()
        {
            CurrentConfig = new Settings();

            if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["textdbpath"]))
            {
                CurrentConfig.DbFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\textdb\\";
            }
            else
            {
                CurrentConfig.DbFilePath = ConfigurationManager.AppSettings["textdbpath"];
            }
        }
    }
}
