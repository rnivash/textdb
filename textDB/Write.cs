using System;
using System.IO;

namespace TextDB
{
    public static class Write
    {
        public static void WriteData(Type entityType, string[] values)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(BaseFile.GetFullName(entityType), FileMode.Append);
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    fs = null;
                    sw.WriteLine(BaseFile.Encode(values));
                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
            
        }

        public static void WriteData(Type entityType, string[][] values)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(BaseFile.GetFullName(entityType), FileMode.Append);
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    fs = null;
                    foreach (string[] lin in values)
                    {
                        sw.WriteLine(BaseFile.Encode(lin));
                    }
                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }
    }
}
