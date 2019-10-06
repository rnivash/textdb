using System;
using System.IO;

namespace TextDB.Store
{
    public class Create : BaseFile
    {
        public static void InitFile(string tableName)
        {
            if (!File.Exists(GetFullName(tableName)))
            {
                FileStream fs = File.Create(GetFullName(tableName));
                fs.Close();
            }
        }
    }
}
