using System.Collections.Generic;
using System.IO;

namespace TextDB.Store
{
    public class Read : BaseFile
    {
        public static List<string[]> ReadData(string tableName)
        {
            List<string[]> result = new List<string[]>();

            using (FileStream fs = new FileStream(GetFullName(tableName), FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        result.Add(Decode(sr.ReadLine()));
                    }
                }
            }

            return result;
        }
    }
}
