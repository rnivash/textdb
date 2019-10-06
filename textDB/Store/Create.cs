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
                DirectoryInfo dinfo = Directory.CreateDirectory(Engine.Instance.CurrentConfig.DbFilePath);
                if (dinfo.Exists)
                {
                    FileStream fs = File.Create(GetFullName(tableName));
                    fs.Close();
                }
            }
        }
    }
}
