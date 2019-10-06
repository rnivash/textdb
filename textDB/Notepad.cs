using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using TextDB.Store;
using System.Linq;

namespace TextDB
{
    public class Notepad
    {
        public static IList<T> Select<T>() where T : new()
        {
            Type entityType = typeof(T);
            List<T> records = new List<T>();

            Create.InitFile(entityType.Name);

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

        public static IList<T> Migrate<T>(Func<PropertyInfo, string[], object> m2, Guid mid) where T : new()
        {
            //0. Check migration
            var li = Select<TextDbMigration>(itm => itm.MigrationId == mid.ToString());
            if (li.Count != 0)
            {
                return null;
            }
            var mi = new TextDbMigration { MigrationId = mid.ToString() };
            InsertValue<TextDbMigration>(mi);
            // 1. load raw data
            Type entityType = typeof(T); List<T> list = new List<T>();

            Create.InitFile(entityType.Name);

            List<string[]> mylist = Read.ReadData(entityType.Name);

            PropertyInfo[] pinfo = entityType.GetProperties();

            if (mylist.Count > 0)//&& pinfo.Length != mylist[0].Length)
            {
                //Data migration is required.
                // 2. loop through data and ask developer to decide how to migrate data
                foreach (string[] str in mylist)
                {
                    T obj = new T();
                    foreach (PropertyInfo pi in pinfo)
                    {
                        pi.SetValue(obj, Convert.ChangeType(m2(pi, str), pi.PropertyType, CultureInfo.InvariantCulture), null);
                    }
                    list.Add(obj);
                }

                // 3. wipe old raw data 
                Delete<T>();

                //4. save new raw data back
                InsertValue<T>(list);
            }

            return list;
        }
    }
}
