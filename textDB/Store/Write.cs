using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace textDB.Store
{
    public class Write
    {
        public void InsertValues(string tableName, string[] values)
        {
            FileStream fs = new FileStream(string.Concat(@"D:\db1\",tableName), FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(string.Join(",", values.Select(item => item.Replace(",","#comman#"))));
            sw.Write(sw.NewLine);            
            sw.Close();
            fs.Close();
        }
    }
}
