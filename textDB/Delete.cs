using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TextDB
{
    public static class Delete
    {
        public static void DeleteFile(Type entityType)
        {
            string tableFullName = BaseFile.GetFullName(entityType);

            if (File.Exists(tableFullName))
            {
                File.Delete(tableFullName);
            }
        }
    }
}
