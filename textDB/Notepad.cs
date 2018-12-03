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
    /// <summary>
    /// Notepad
    /// </summary>
    public static class Notepad
    {
        /// <summary>
        /// To select/get all records in the table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IList<T> Select<T>() where T : new()
        {
            return Select<T>(null);
        }

        /// <summary>
        /// To select/get records based on the filter condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IList<T> Select<T>(Predicate<T> filter) where T : new()
        {
            Type tt = typeof(T);

            CreateDb(tt);

            List<T> list = new List<T>();
            List<string[]> mylist = Read.Select(string.Concat(tt.Name, DbConstants.DbExtension));

            PropertyInfo[] pinfo = tt.GetProperties();
            int i = 0;
            foreach (string[] str in mylist)
            {
                i = 0;
                T obj = new T();
                foreach (PropertyInfo pi in pinfo)
                {
                    pi.SetValue(obj, Convert.ChangeType(str[i++], pi.PropertyType, CultureInfo.InvariantCulture), null);
                }
                if (filter == null)
                {
                    list.Add(obj);
                }
                else if (filter.Invoke(obj))
                {
                    list.Add(obj);
                }
            }
            return list;
        }

        /// <summary>
        /// To create a db
        /// </summary>
        /// <param name="tt"></param>
        private static void CreateDb(Type tt)
        {
            if (!File.Exists(string.Concat(TextDbEngine.Instance.CurrentConfig.DbFilePath, tt.Name, DbConstants.DbExtension)))
            {
                Create.CreateDb(string.Concat(tt.Name, DbConstants.DbExtension));
            }
        }

        /// <summary>
        /// To insert a new record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object1"></param>
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

        /// <summary>
        /// It will drop the entire table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private static void Delete(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        public static void Delete<T>()
        {
            Type tt = typeof(T);
            Delete(FileName<T>());
        }

        /// <summary>
        /// To delete a record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object1"></param>
        public static void Delete<T>(T object1) where T : new()
        {
            Type tt = typeof(T);
            if (File.Exists(FileName<T>()))
            {
                IList<T> mylist = Select<T>();
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

        /// <summary>
        /// To delete records based on filter condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        public static void Delete<T>(Predicate<T> filter) where T : new()
        {
            if (File.Exists(FileName<T>()))
            {
                IList<T> mylist = Select<T>();
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

        private static string FileName<T>()
        {
            Type tt = typeof(T);
            return string.Concat(TextDbEngine.Instance.CurrentConfig.DbFilePath, tt.Name, DbConstants.DbExtension);
        }

        /// <summary>
        /// To update the records based on the filter condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object1"></param>
        /// <param name="filter"></param>
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
