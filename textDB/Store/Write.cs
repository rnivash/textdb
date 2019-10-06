using System.IO;
using System.Linq;

namespace TextDB.Store
{
    public class Write : BaseFile
    {
        public static void WriteData(string tableName, string[] values)
        {
            using (FileStream fs = new FileStream(GetFullName(tableName), FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(Encode(values));
                }
            }
        }

        public static void WriteData(string tableName, string[][] values)
        {
            using (FileStream fs = new FileStream(GetFullName(tableName), FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    foreach(string[] lin in values)
                    {
                        sw.WriteLine(Encode(lin));
                    }
                }
            }
        }
    }
}
