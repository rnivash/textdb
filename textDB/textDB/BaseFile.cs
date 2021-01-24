using System;
using System.Globalization;
using System.Linq;

namespace TextDB
{
    public static class BaseFile
    {
        public const string DbExtension = ".db1";

        public const string CommaSeparator = "#comma#";

        public const string Comma = ",";

        public static int GetId(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return (type.FullName + string.Join("|", type.GetProperties().Select(p =>
            string.Concat(p.Name, ":", p.PropertyType.Name)))).GetHashCode();
        }

        public static string GetFullName(Type entityType)
        {
            if (entityType is null)
            {
                throw new ArgumentNullException(nameof(entityType));
            }

            return string.Format(CultureInfo.InvariantCulture,
                "{0}{1}.{2}{3}",
                Engine.Instance.Config.FilePath,
                entityType.Name,
                GetId(entityType),
                DbExtension);
        }

        public static string Encode(string[] values)
        {
            return string.Join(Comma, values.Select(item => item?.Replace(Comma, CommaSeparator)));
        }

        public static string[] Decode(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return value.Split(new string[] { Comma }, StringSplitOptions.None)
                            .Select(item => item.Replace(CommaSeparator, Comma))
                            .ToArray();
        }
    }
}
