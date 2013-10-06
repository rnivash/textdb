using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TestDB.Bot
{
    internal class textDbEngine
    {
        private static volatile textDbEngine _dbEgine;

        private static object syncRoot = new Object();

        public textDbConfig CurrentConfig { get; private set; }

        private textDbEngine()
        {
            LoadConfig();
        }

        public static textDbEngine Instance
        {
            get
            {
                if (_dbEgine == null)
                {
                    lock (syncRoot)
                    {
                        if (_dbEgine == null)
                            _dbEgine = new textDbEngine();
                    }
                }
                return _dbEgine;
            }
        }

        public void LoadConfig()
        {
            CurrentConfig = new textDbConfig();

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
