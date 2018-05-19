using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextDB;

namespace textdb.sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var dd = new DataBase
            {
                Name = "fdsfsdfs"
            };

            Notepad.InsertValue<DataBase>(dd);
        }
    }

    public class DataBase
    {
        public string Name { get; set; }
    }
}
