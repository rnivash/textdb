using System;
using System.IO;

namespace textDb.Core
{
    public static class Create
    {
        public static void InitFile(Type entityType)
        {
            string tableFullName = BaseFile.GetFullName(entityType);

            if (!File.Exists(tableFullName))
            {
                FileStream fs = File.Create(tableFullName);
                fs.Close();
            }
        }
    }
}
