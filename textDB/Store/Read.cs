using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace textDB.Store
{
    public class Read
    {
        public List<string[]> Select(string tableName)
        {
            List<string[]> result = new List<string[]>();
            FileStream fs = new FileStream(string.Concat(@"D:\db1\", tableName), FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            while(!sr.EndOfStream){
                result.Add(sr.ReadLine().Split(new string []{","},StringSplitOptions.None)
                    .Select(item => item.Replace("#comman#",",")).ToArray());
            }
            
            sr.Close();
            fs.Close();
            return result;
        }
    }
}
