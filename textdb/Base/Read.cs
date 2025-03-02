using System;
using System.Collections.ObjectModel;
using System.IO;

namespace textDb.Base
{
    internal static class Read
    {
        internal static Collection<string[]> ReadData(Type entityType)
        {
            Collection<string[]> result = new Collection<string[]>();
            int retryCount = 3;
            int delay = 1000; // 1 second delay

            while (retryCount > 0)
            {
                try
                {
                    using (FileStream fs = new FileStream(BaseFile.GetFullName(entityType), FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while (!sr.EndOfStream)
                        {
                            result.Add(BaseFile.Decode(sr.ReadLine()));
                        }
                    }
                    break; // Exit the loop if successful
                }
                catch (IOException ex) when (ex is IOException && retryCount > 1)
                {
                    // If the file is being used by another process, wait and retry
                    retryCount--;
                    System.Threading.Thread.Sleep(delay);
                }
                catch (Exception ex)
                {
                    // Handle or log the exception as needed
                    throw new IOException("An error occurred while reading the data.", ex);
                }
            }

            if (retryCount == 0)
            {
                throw new IOException("Failed to read the data after multiple attempts.");
            }

            return result;
        }
    }
}
