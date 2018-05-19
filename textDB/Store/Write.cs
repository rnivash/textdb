using System.IO;
using System.Linq;
using TextDB.Bot;
using TextDB.Common;

namespace TextDB.Store
{
    internal class Write
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="values"></param>
        internal static void InsertValues(string tableName, string[] values)
        {
            using (FileStream fs = new FileStream(string.Concat(TextDbEngine.Instance.CurrentConfig.DbFilePath, tableName), FileMode.Append))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(string.Join(",", values.Select(item => item.Replace(DbConstants.Comma, DbConstants.CommaSeparator))));
                sw.Write(sw.NewLine);
            }

        }
    }
}
