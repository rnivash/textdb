using System;
using System.IO;

namespace textDb.Base
{
    internal static class Create
    {
        internal static void InitFile(Type entityType)
        {
            string tableFullName = BaseFile.GetFullName(entityType);

            if (!File.Exists(tableFullName))
            {
                try
                {
                    using (FileStream fs = File.Create(tableFullName))
                    {
                        // Optionally, you can write some initial content to the file here
                    }
                }
                catch (IOException)
                {
                    // Handle the case where the file was created by another process
                }
            }
        }
    }
}
