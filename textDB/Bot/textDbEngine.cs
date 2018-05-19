using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace TextDB.Bot
{
    internal class TextDbEngine
    {
        private static volatile TextDbEngine _dbEgine;

        private static object syncRoot = new Object();

        public TextDbConfig CurrentConfig { get; private set; }

        private TextDbEngine()
        {
            LoadConfig();
        }

        public static TextDbEngine Instance
        {
            get
            {
                if (_dbEgine == null)
                {
                    lock (syncRoot)
                    {
                        if (_dbEgine == null)
                            _dbEgine = new TextDbEngine();
                    }
                }
                return _dbEgine;
            }
        }

        public void LoadConfig()
        {
            CurrentConfig = new TextDbConfig();

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
