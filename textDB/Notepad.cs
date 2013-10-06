using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TestDB.Bot;
using TestDB.Common;
using textDB.Store;

namespace textDB
{
    /// <summary>
    /// Notepad
    /// </summary>
    public class Notepad
    {
        /// <summary>
        /// To select/get all records in the table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<T> Select<T>() where T : new()
        {
            return this.Select<T>(null);
        }

        /// <summary>
        /// To select/get records based on the filter condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IList<T> Select<T>(Predicate<T> filter) where T : new()
        {
            Type tt = typeof(T);

            CreateDb(tt);

            List<T> list = new List<T>();
            Read readdb = new Read();
            List<string[]> mylist = readdb.Select(string.Concat(tt.Name, DbConstants.DbExtension));

            PropertyInfo[] pinfo = tt.GetProperties();
            int i = 0;
            foreach (string[] str in mylist)
            {
                i = 0;
                T obj = new T();
                foreach (PropertyInfo pi in pinfo)
                {
                    pi.SetValue(obj, Convert.ChangeType(str[i++], pi.PropertyType), null);
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
            if (!File.Exists(string.Concat(textDbEngine.Instance.CurrentConfig.DbFilePath, tt.Name, DbConstants.DbExtension)))
            {
                Create createdb = new Create();
                createdb.CreateDb(string.Concat(tt.Name, DbConstants.DbExtension));
            }
        }

        /// <summary>
        /// To insert a new record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object1"></param>
        public void InsertValue<T>(T object1)
        {
            Type tt = typeof(T);
            if (!File.Exists(string.Concat(textDbEngine.Instance.CurrentConfig.DbFilePath, tt.Name, DbConstants.DbExtension)))
            {
                Create createdb = new Create();
                createdb.CreateDb(string.Concat(tt.Name, DbConstants.DbExtension));
            }
            Write writedb = new Write();
            PropertyInfo[] pinfo = tt.GetProperties();
            List<string> result = new List<string>();
            foreach (PropertyInfo pi in pinfo)
            {
                result.Add(pi.GetValue(object1, null).ToString());
            }
            writedb.InsertValues(string.Concat(tt.Name, DbConstants.DbExtension), result.ToArray());
        }

        /// <summary>
        /// It will drop the entire table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Delete<T>()
        {
            Type tt = typeof(T);
            if (File.Exists(string.Concat(textDbEngine.Instance.CurrentConfig.DbFilePath, tt.Name, DbConstants.DbExtension)))
            {
                File.Delete(string.Concat(@"D:\db1\", tt.Name, DbConstants.DbExtension));
            }
        }

        /// <summary>
        /// To delete a record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object1"></param>
        public void Delete<T>(T object1) where T : new()
        {
            Type tt = typeof(T);
            if (File.Exists(string.Concat(textDbEngine.Instance.CurrentConfig.DbFilePath, tt.Name, DbConstants.DbExtension)))
            {
                IList<T> mylist = this.Select<T>();
                this.Delete<T>();
                PropertyInfo[] pinfo = tt.GetProperties();
                bool add = false;
                foreach (T newT in mylist)
                {
                    add = false;
                    foreach (PropertyInfo pi in pinfo)
                    {
                        if (pi.GetValue(newT, null) != pi.GetValue(object1, null))
                        {
                            add = true;
                            break;
                        }
                    }
                    if (add)
                    {
                        this.InsertValue<T>(newT);
                    }
                }
            }
        }

        /// <summary>
        /// To delete records based on filter condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        public void Delete<T>(Predicate<T> filter) where T : new()
        {
            Type tt = typeof(T);
            if (File.Exists(string.Concat(textDbEngine.Instance.CurrentConfig.DbFilePath, tt.Name, DbConstants.DbExtension)))
            {
                IList<T> mylist = this.Select<T>();
                this.Delete<T>();
                foreach (T newT in mylist)
                {
                    if (!filter.Invoke(newT))
                    {
                        this.InsertValue<T>(newT);
                    }
                }
            }
        }

        /// <summary>
        /// To update the records based on the filter condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object1"></param>
        /// <param name="filter"></param>
        public void Update<T>(T object1, Predicate<T> filter) where T : new()
        {
            Type tt = typeof(T);
            IList<T> mylist = this.Select<T>(filter);
            this.Delete<T>(filter);
            PropertyInfo[] pinfo = tt.GetProperties();
            foreach (T newT in mylist)
            {
                foreach (PropertyInfo pi in pinfo)
                {
                    pi.SetValue(newT, pi.GetValue(object1, null), null);
                }
                this.InsertValue<T>(newT);
            }
        }
    }
}
