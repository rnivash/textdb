using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TestDB.Common;
using TestDB.Bot;

namespace textDB.Store
{
    internal class Read
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        internal List<string[]> Select(string tableName)
        {
            List<string[]> result = new List<string[]>();
            FileStream fs = new FileStream(string.Concat(textDbEngine.Instance.CurrentConfig.DbFilePath, tableName), FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            while (!sr.EndOfStream)
            {
                result.Add(sr.ReadLine().Split(new string[] { DbConstants.Comma }, StringSplitOptions.None)
                    .Select(item => item.Replace(DbConstants.CommaSeparator, DbConstants.Comma)).ToArray());
            }

            sr.Close();
            fs.Close();
            return result;
        }
    }
}
