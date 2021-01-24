using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace textDb
{
    public static class Notepad
    {
        public static IList<T> Select<T>() where T : new()
        {
            Type entityType = typeof(T);

            Create.InitFile(entityType);

            List<T> records = new List<T>();

            Collection<string[]> rawRecords = Read.ReadData(entityType);

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
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Type entityType = typeof(T);
            Create.InitFile(entityType);

            PropertyInfo[] propertyInfos = entityType.GetProperties();

            List<string[]> records = new List<string[]>();
            foreach (T entity in entities)
            {
                int j = 0;
                string[] row = new string[propertyInfos.Length];
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    if(propertyInfo.PropertyType == typeof(DateTime) ){
                        row[j++] =  ((DateTime)propertyInfo.GetValue(entity, null)).ToString("MM/dd/yyyy HH:mm:ss.fffffffzzz");
                    }
                    else{
                        row[j++] = propertyInfo.GetValue(entity, null)?.ToString();
                    }
                }
                records.Add(row);
            }

            Write.WriteData(entityType, records);
        }

        public static void Delete<T>()
        {
            Type entityType = typeof(T);
            textDb.Delete.DeleteFile(entityType);
        }

        public static void Delete<T>(T object1) where T : new()
        {
            Type entityType = typeof(T);
            IList<T> deleteList = Select<T>();
            textDb.Delete.DeleteFile(entityType);
            PropertyInfo[] pinfo = entityType.GetProperties();
            
            foreach (T newT in deleteList)
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
                    InsertValue(newT);
                }
            }
        }

        public static void Delete<T>(Predicate<T> filter) where T : new()
        {
            Type entityType = typeof(T);
            IList<T> deleteList = Select<T>();
            textDb.Delete.DeleteFile(entityType);
            if (filter != null)
            {
                foreach (T newT in deleteList)
                {
                    if (!filter.Invoke(newT))
                    {
                        InsertValue(newT);
                    }
                }
            }
        }

        public static void Update<T>(T object1, Predicate<T> filter) where T : new()
        {
            Type entityType = typeof(T);
            IList<T> updateList = Select(filter);
            Delete(filter);
            PropertyInfo[] pinfo = entityType.GetProperties();
            foreach (T newT in updateList)
            {
                foreach (PropertyInfo pi in pinfo)
                {
                    pi.SetValue(newT, pi.GetValue(object1, null), null);
                }
                InsertValue(newT);
            }
        }
    }
}
