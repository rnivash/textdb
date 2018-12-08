using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using TextDB.Bot;
using TextDB.Common;
using TextDB.Store;

namespace TextDB
{
    public static class Notepad
    {
        public static IList<T> Select<T>() where T : new()
        {
            return Select2<T>();
        }

        public static IList<T> Select<T>(Predicate<T> filter) where T : new()
        {
            return filter == null ? Select2<T>() : Select2<T>(filter);
        }

        public static void Save<T>(T object1)
        {
            InsertValue(object1);
        }

        public static void InsertValue<T>(T object1)
        {
            Type tt = typeof(T);
            if (!File.Exists(FileName<T>()))
            {
                Create.CreateDb(string.Concat(tt.Name, DbConstants.DbExtension));
            }
            PropertyInfo[] pinfo = tt.GetProperties();
            List<string> result = new List<string>();
            foreach (PropertyInfo pi in pinfo)
            {
                result.Add(pi.GetValue(object1, null)?.ToString());
            }
            Write.InsertValues(string.Concat(tt.Name, DbConstants.DbExtension), result.ToArray());
        }

        public static void Save<T>(IList<T> object1)
        {
            InsertValue(object1);
        }

        public static void InsertValue<T>(IList<T> object1)
        {
            Type tt = typeof(T);
            if (!File.Exists(FileName<T>()))
            {
                Create.CreateDb(string.Concat(tt.Name, DbConstants.DbExtension));
            }
            PropertyInfo[] pinfo = tt.GetProperties();
            string[][] result = new string[object1.Count][];
            int i = -1;
            foreach (T entity in object1)
            {
                int j = 0;
                result[++i] = new string[pinfo.Length];
                foreach (PropertyInfo pi in pinfo)
                {
                    result[i][j++] = pi.GetValue(entity, null)?.ToString();
                }
            }

            Write.InsertValues(string.Concat(tt.Name, DbConstants.DbExtension), result);
        }

        public static void Delete<T>()
        {
            Delete(FileName<T>());
        }

        public static void Delete<T>(T object1) where T : new()
        {
            Type tt = typeof(T);
            if (File.Exists(FileName<T>()))
            {
                IList<T> mylist = Select2<T>();
                Delete(FileName<T>());
                PropertyInfo[] pinfo = tt.GetProperties();
                bool add = false;
                foreach (T newT in mylist)
                {
                    add = false;
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
        }

        public static void Delete<T>(Predicate<T> filter) where T : new()
        {
            if (File.Exists(FileName<T>()))
            {
                IList<T> mylist = Select2<T>();
                Delete(FileName<T>());
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
        }

        public static void Update<T>(T object1, Predicate<T> filter) where T : new()
        {
            Type tt = typeof(T);
            IList<T> mylist = Select2<T>(filter);
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
            var li = Select2<TextDbMigration>(itm => itm.MigrationId == mid.ToString());
            if (li.Count != 0)
            {
                return null;
            }
            var mi = new TextDbMigration { MigrationId = mid.ToString() };
            InsertValue<TextDbMigration>(mi);
            // 1. load raw data
            Type entityType = typeof(T); List<T> list = new List<T>();

            CreateDb(entityType);

            List<string[]> mylist = Read.Select(string.Concat(entityType.Name, DbConstants.DbExtension));

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

        private static IList<T> Select2<T>() where T : new()
        {
            Type entityType = typeof(T); int i; List<T> list = new List<T>();

            CreateDb(entityType);

            List<string[]> mylist = Read.Select(string.Concat(entityType.Name, DbConstants.DbExtension));

            PropertyInfo[] pinfo = entityType.GetProperties();

            if (mylist.Count <= 0 || pinfo.Length != mylist[0].Length)
            {
                //Data migration is required.
                return list;
            }

            foreach (string[] str in mylist)
            {
                i = 0;
                T obj = new T();
                foreach (PropertyInfo pi in pinfo)
                {
                    pi.SetValue(obj, Convert.ChangeType(str[i++], pi.PropertyType, CultureInfo.InvariantCulture), null);
                }
                list.Add(obj);
            }

            return list;
        }

        private static IList<T> Select2<T>(Predicate<T> filter) where T : new()
        {
            Type entityType = typeof(T); int i; List<T> list = new List<T>();

            CreateDb(entityType);

            List<string[]> mylist = Read.Select(string.Concat(entityType.Name, DbConstants.DbExtension));

            PropertyInfo[] pinfo = entityType.GetProperties();

            if (mylist.Count <= 0 || pinfo.Length != mylist[0].Length)
            {
                //Data migration is required.
                return list;
            }

            foreach (string[] str in mylist)
            {
                i = 0;
                T obj = new T();
                foreach (PropertyInfo pi in pinfo)
                {
                    pi.SetValue(obj, Convert.ChangeType(str[i++], pi.PropertyType, CultureInfo.InvariantCulture), null);
                }
                if (filter.Invoke(obj))
                {
                    list.Add(obj);
                }
            }

            return list;
        }

        private static void Delete(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        private static string FileName<T>()
        {
            Type tt = typeof(T);
            return string.Concat(TextDbEngine.Instance.CurrentConfig.DbFilePath, tt.Name, DbConstants.DbExtension);
        }

        private static void CreateDb(Type tt)
        {
            if (!File.Exists(string.Concat(TextDbEngine.Instance.CurrentConfig.DbFilePath, tt.Name, DbConstants.DbExtension)))
            {
                Create.CreateDb(string.Concat(tt.Name, DbConstants.DbExtension));
            }
        }
    }
}
