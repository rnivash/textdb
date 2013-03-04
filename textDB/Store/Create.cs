using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace textDB.Store
{
    public class Create
    {
        internal bool CreateDb(string tableName)
        {
            DirectoryInfo dinfo = Directory.CreateDirectory(@"d:\db1");
            if(dinfo.Exists){
                FileStream fs = File.Create(string.Concat(@"d:\db1\",tableName));
                fs.Close();
            }
            return true;
        }
    }
}
