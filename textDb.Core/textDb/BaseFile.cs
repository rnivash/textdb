using System;
using System.Globalization;
using System.Linq;

namespace textDb
{
    public static class BaseFile
    {
        private const string DbExtension = ".db1";

        private const string CommaSeparator = "#comma#";

        private const string Comma = ",";
        static int GetDeterministicHashCode(this string str)
        {
            unchecked
            {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }
        private static int GetId(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return (type.FullName + string.Join("|", type.GetProperties().Select(p =>
            string.Concat(p.Name, ":", p.PropertyType.Name)))).GetDeterministicHashCode();
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

            return value.Split(new[] { Comma }, StringSplitOptions.None)
                            .Select(item => item.Replace(CommaSeparator, Comma))
                            .ToArray();
        }
    }
}
