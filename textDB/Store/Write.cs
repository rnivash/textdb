using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TestDB.Common;
using TestDB.Bot;

namespace textDB.Store
{
    internal class Write
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="values"></param>
        internal void InsertValues(string tableName, string[] values)
        {
            FileStream fs = new FileStream(string.Concat(textDbEngine.Instance.CurrentConfig.DbFilePath, tableName), FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(string.Join(",", values.Select(item => item.Replace(DbConstants.Comma, DbConstants.CommaSeparator))));
            sw.Write(sw.NewLine);
            sw.Close();
            fs.Close();
        }
    }
}
