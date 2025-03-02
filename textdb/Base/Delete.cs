using System;
using System.IO;

namespace textDb.Base
{
    internal static class Delete
    {
        internal static void DeleteFile(Type entityType)
        {
            string tableFullName = BaseFile.GetFullName(entityType);

            if (File.Exists(tableFullName))
            {
                File.Delete(tableFullName);
            }
        }
    }
}
