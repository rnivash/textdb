
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using textDB.Store;

namespace textDB
{
    public class Notepad
    {
        public List<T> Select<T>() where T : new()
        {
            return this.Select<T>(null);
        }

        public List<T> Select<T>(Predicate<T> filter) where T : new()
        {
            Type tt = typeof(T);
            if (!File.Exists(string.Concat(@"D:\db1\", tt.Name, ".db1")))
            {
                Create createdb = new Create();
                createdb.CreateDb(string.Concat(tt.Name, ".db1"));
            }

            List<T> list = new List<T>();
            Read readdb = new Read();
            List<string[]> mylist = readdb.Select(string.Concat(tt.Name, ".db1"));

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

        public void InsertValue<T>(T object1){
            Type tt = typeof(T);
            if (!File.Exists(string.Concat(@"D:\db1\", tt.Name, ".db1")))
            {
                Create createdb = new Create();
                createdb.CreateDb(string.Concat(tt.Name, ".db1"));
            }
            Write writedb = new Write();
            PropertyInfo[] pinfo = tt.GetProperties();
            List<string> result = new List<string>();
            foreach (PropertyInfo pi in pinfo)
            {
                result.Add(pi.GetValue(object1, null).ToString());
            }
            writedb.InsertValues(string.Concat(tt.Name, ".db1"), result.ToArray());
        }

        public void Delete<T>()
        {
            Type tt = typeof(T);
            if (File.Exists(string.Concat(@"D:\db1\", tt.Name, ".db1")))
            {
                File.Delete(string.Concat(@"D:\db1\", tt.Name, ".db1"));
            }
        }

        public void Delete<T>(T object1) where T : new()
        {
            Type tt = typeof(T);
            if (File.Exists(string.Concat(@"D:\db1\", tt.Name, ".db1")))
            {
                List<T> mylist = this.Select<T>();
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

        public void Delete<T>(Predicate<T> filter) where T : new()
        {
            Type tt = typeof(T);
            if (File.Exists(string.Concat(@"D:\db1\", tt.Name, ".db1")))
            {
                List<T> mylist = this.Select<T>();
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

        public void Update<T>(T object1, Predicate<T> filter) where T : new()
        {
            Type tt = typeof(T);
            List<T> mylist = this.Select<T>(filter);
            this.Delete<T>(filter);
            PropertyInfo[] pinfo = tt.GetProperties();
            foreach (T newT in mylist)
            {
                foreach (PropertyInfo pi in pinfo)
                {
                    pi.SetValue(newT,pi.GetValue(object1, null),null);
                }
                this.InsertValue<T>(newT);
            }
        }
    }
}
