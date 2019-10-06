using System;
using System.Linq;

namespace TextDB.Store
{
    public class BaseFile
    {
        public static string GetFullName(string typeName)
        {
            return string.Concat(Engine.Instance.Config.FilePath, 
                string.Concat(typeName, Constants.DbExtension));
        }

        public static string Encode(string[] values)
        {
            return string.Join(Constants.Comma, values.Select(item => item
            .Replace(Constants.Comma, Constants.CommaSeparator)));
        }

        public static string[] Decode(string value)
        {
            return value.Split(new string[] { Constants.Comma }, StringSplitOptions.None)
                            .Select(item => item.Replace(Constants.CommaSeparator, Constants.Comma))
                            .ToArray();
        }
    }
}
