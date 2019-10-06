using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using TextDB.Store;

namespace TextDB
{
    public partial class Notepad
    {
        public static IList<T> Select<T>() where T : new()
        {
            Type entityType = typeof(T);
            Create.InitFile(entityType.Name);

            List<T> records = new List<T>();

            List<string[]> rawRecords = Read.ReadData(entityType.Name);

            PropertyInfo[] propertyInfos = entityType.GetProperties();

            if (rawRecords.Count <= 0 || propertyInfos.Length != rawRecords[0].Length)
            {
                return records;
            }

            foreach (string[] rawData in rawRecords)
            {
                int index = 0;
                T data = new T();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    var cValue = Convert.ChangeType(rawData[index++], propertyInfo.PropertyType,
                        CultureInfo.InvariantCulture);
                    propertyInfo.SetValue(data, cValue, null);
                }
                records.Add(data);
            }

            return records;
        }

        public static IList<T> Select<T>(Predicate<T> filter) where T : new()
        {
            var records = Select<T>();

            return records
                .Where(item => filter.Invoke(item))
                .ToList();
        }

        public static void InsertValue<T>(T entity)
        {
            InsertValue<T>(new List<T>() { entity });
        }

        public static void InsertValue<T>(IList<T> entities)
        {
            Type entityType = typeof(T);
            Create.InitFile(entityType.Name);

            PropertyInfo[] propertyInfos = entityType.GetProperties();

            string[][] records = new string[entities.Count][];
            int i = -1;
            foreach (T entity in entities)
            {
                int j = 0;
                records[++i] = new string[propertyInfos.Length];
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    records[i][j++] = propertyInfo.GetValue(entity, null)?.ToString();
                }
            }

            Write.WriteData(entityType.Name, records);
        }

        public static void Delete<T>()
        {
            Type tt = typeof(T);
            TextDB.Store.Delete.DeleteFile(tt.Name);
        }

        public static void Delete<T>(T object1) where T : new()
        {
            Type tt = typeof(T);
            IList<T> mylist = Select<T>();
            TextDB.Store.Delete.DeleteFile(tt.Name);
            PropertyInfo[] pinfo = tt.GetProperties();
            
            foreach (T newT in mylist)
            {
                bool add = false;
                foreach (PropertyInfo pi in pinfo)
                {
                    if (!pi.GetValue(newT, null).Equals(pi.GetValue(object1, null)))
                    {
                        add = true;
                        break;
                    }
                }
                if (add)
                {
                    InsertValue<T>(newT);
                }
            }
        }

        public static void Delete<T>(Predicate<T> filter) where T : new()
        {
            Type tt = typeof(T);
            IList<T> mylist = Select<T>();
            TextDB.Store.Delete.DeleteFile(tt.Name);
            if (filter != null)
            {
                foreach (T newT in mylist)
                {
                    if (!filter.Invoke(newT))
                    {
                        InsertValue<T>(newT);
                    }
                }
            }
        }

        public static void Update<T>(T object1, Predicate<T> filter) where T : new()
        {
            Type tt = typeof(T);
            IList<T> mylist = Select<T>(filter);
            Delete<T>(filter);
            PropertyInfo[] pinfo = tt.GetProperties();
            foreach (T newT in mylist)
            {
                foreach (PropertyInfo pi in pinfo)
                {
                    pi.SetValue(newT, pi.GetValue(object1, null), null);
                }
                InsertValue<T>(newT);
            }
        }
    }
}
