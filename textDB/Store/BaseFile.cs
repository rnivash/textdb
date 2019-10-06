using System;
using System.Linq;

namespace TextDB.Store
{
    public class BaseFile
    {
        public static string GetFullName(string typeName)
        {
            return string.Concat(Engine.Instance.CurrentConfig.DbFilePath, 
                string.Concat(typeName, DbConstants.DbExtension));
        }

        public static string Encode(string[] values)
        {
            return string.Join(DbConstants.Comma, values.Select(item => item
            .Replace(DbConstants.Comma, DbConstants.CommaSeparator)));
        }

        public static string[] Decode(string value)
        {
            return value.Split(new string[] { DbConstants.Comma }, StringSplitOptions.None)
                            .Select(item => item.Replace(DbConstants.CommaSeparator, DbConstants.Comma))
                            .ToArray();
        }
    }
}
