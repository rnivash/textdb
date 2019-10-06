using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TextDB.Store
{
    public class Delete : BaseFile
    {
        public static void DeleteFile(string typeName)
        {
            if (File.Exists(GetFullName(typeName)))
            {
                File.Delete(GetFullName(typeName));
            }
        }
    }
}
