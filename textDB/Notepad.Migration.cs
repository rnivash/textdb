using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using TextDB.Store;

namespace TextDB
{
    public partial class Notepad
    {
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
