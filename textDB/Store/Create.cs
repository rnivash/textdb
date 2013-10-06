using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TestDB.Bot;

namespace textDB.Store
{
    internal class Create
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        internal bool CreateDb(string tableName)
        {
            DirectoryInfo dinfo = Directory.CreateDirectory(textDbEngine.Instance.CurrentConfig.DbFilePath);
            if(dinfo.Exists){
                FileStream fs = File.Create(string.Concat(textDbEngine.Instance.CurrentConfig.DbFilePath, tableName));
                fs.Close();
            }
            return true;
        }
    }
}
