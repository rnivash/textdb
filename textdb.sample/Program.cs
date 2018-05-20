using System;
using System.Collections.Generic;
using System.Dynamic;
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

            Notepad.Delete<Student>();

            var entity1 = new Student
            {
                Name = "Darshan",
                Age = 13,
                Section = "A",
                CreatedOn = DateTime.Now,
                IsActive = true
            };

            Notepad.InsertValue(entity1);

            var entity2 = new Student
            {
                Name = "Shruthi",
                Age = 16,
                Section = "A",
                CreatedOn = DateTime.Now,
                IsActive = true
            };

            Notepad.InsertValue(entity2);

            var list = Notepad.Select<Student>();
            Display(list, "Fetch All");

            var list2 = Notepad.Select<Student>(stud => stud.Name == "Darshan");
            Display(list2, "Get matching record");

            var itm = list[1];
            itm.Section = "B";
            Notepad.Update<Student>(itm, key => key.Name == itm.Name);
            var list3 = Notepad.Select<Student>(stud => stud.Name == itm.Name);
            Display(list3, "After update");

            Notepad.Delete<Student>(itm);

            var lis4 = Notepad.Select<Student>();
            Display(lis4, "After Delete");


        }

        private static void Display(IList<Student> list, string action)
        {
            Console.WriteLine("----------{0}-------------", action);
            foreach (var item in list)
            {
                Console.WriteLine("Name : {0}", item.Name);
                Console.WriteLine("Section : {0}", item.Section);
                Console.WriteLine("Age : {0}", item.Age);
                Console.WriteLine("Created On : {0}", item.CreatedOn);
                Console.WriteLine("Active : {0}", item.IsActive);
            }
        }
    }

}
