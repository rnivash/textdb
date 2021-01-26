using System;
using System.Collections.ObjectModel;
using System.IO;

namespace textDb.Base
{
    public static class Read
    {
        public static Collection<string[]> ReadData(Type entityType)
        {
            Collection<string[]> result = new Collection<string[]>();
            FileStream fs = null;
            try
            {
                fs = new FileStream(BaseFile.GetFullName(entityType), FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                using(StreamReader sr = new StreamReader(fs))
                {
                    fs = null;
                    while (!sr.EndOfStream)
                    {
                        result.Add(BaseFile.Decode(sr.ReadLine()));
                    }
                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
            return result;
        }
    }
}
