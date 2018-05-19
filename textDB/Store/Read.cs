using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TextDB.Bot;
using TextDB.Common;

namespace TextDB.Store
{
    internal class Read
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        internal static List<string[]> Select(string tableName)
        {
            List<string[]> result = new List<string[]>();

            using (FileStream fs = new FileStream(string.Concat(TextDbEngine.Instance.CurrentConfig.DbFilePath, tableName), FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);
                {
                    while (!sr.EndOfStream)
                    {
                        result.Add(sr.ReadLine().Split(new string[] { DbConstants.Comma }, StringSplitOptions.None)
                            .Select(item => item.Replace(DbConstants.CommaSeparator, DbConstants.Comma)).ToArray());
                    }
                }
            }
            return result;
        }
    }
}
