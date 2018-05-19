using System.IO;
using TextDB.Bot;

namespace TextDB.Store
{
    internal static class Create
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        internal static bool CreateDb(string tableName)
        {
            DirectoryInfo dinfo = Directory.CreateDirectory(TextDbEngine.Instance.CurrentConfig.DbFilePath);
            if(dinfo.Exists){
                FileStream fs = File.Create(string.Concat(TextDbEngine.Instance.CurrentConfig.DbFilePath, tableName));
                fs.Close();
            }
            return true;
        }
    }
}
