using System;
using System.Collections.Generic;
using System.IO;

namespace textDb.Base
{
    internal static class Write
    {
        internal static void WriteData(Type entityType, string[] values)
        {
            int retryCount = 3;
            int delay = 1000; // 1 second delay

            for (int i = 0; i < retryCount; i++)
            {
            try
            {
                using (var fs = new FileStream(BaseFile.GetFullName(entityType), FileMode.Append, FileAccess.Write, FileShare.Read))
                using (var sw = new StreamWriter(fs))
                {
                sw.WriteLine(BaseFile.Encode(values));
                }
                break; // Exit the loop if successful
            }
            catch (IOException) when (i < retryCount - 1)
            {
                // Log the exception or handle it as needed
                System.Threading.Thread.Sleep(delay); // Wait before retrying
            }
            }
        }

        internal static void WriteData(Type entityType, IList<string[]> values)
        {
            if (values is null)
            {
            throw new ArgumentNullException(nameof(values));
            }

            int retryCount = 3;
            int delay = 1000; // 1 second delay

            for (int i = 0; i < retryCount; i++)
            {
            try
            {
                using (var fs = new FileStream(BaseFile.GetFullName(entityType), FileMode.Append, FileAccess.Write, FileShare.Read))
                using (var sw = new StreamWriter(fs))
                {
                foreach (var line in values)
                {
                    sw.WriteLine(BaseFile.Encode(line));
                }
                }
                break; // Exit the loop if successful
            }
            catch (IOException) when (i < retryCount - 1)
            {
                // Log the exception or handle it as needed
                System.Threading.Thread.Sleep(delay); // Wait before retrying
            }
            }
        }
    }
}
