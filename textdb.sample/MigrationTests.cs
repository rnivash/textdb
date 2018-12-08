using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextDB;

namespace textDB.Tests
{
    [TestClass]
    public class MigrationTests
    {
        [TestInitialize]
        public void TestSetup()
        {
            Notepad.Delete<Student>();
        }

        [TestMethod]
        public void Migration_Normal_Case()
        {
            var student = new Student { Name = "Nivash", Age = 33, IsActive= true, Section = "A", CreatedOn = DateTime.Now };

            Notepad.InsertValue(student);

            Notepad.Migrate<Student>(delegate(System.Reflection.PropertyInfo pi, string[] s2) 
            {
                if(pi.Name == "Name")
                {
                    return "Naresh";
                }
                if (pi.Name == "Age")
                {
                    return s2[1];
                }
                if (pi.Name == "Section")
                {
                    return s2[2];
                }
                if (pi.Name == "CreatedOn")
                {
                    return s2[3];
                }
                if (pi.Name == "IsActive")
                {
                    return s2[4];
                }
                return "";
            }, Guid.NewGuid());

            var slist = Notepad.Select<Student>();

            Assert.AreEqual(slist[0].Name, "Naresh");
        }

        [TestMethod]
        public void Migration_Avoid_Duplicate()
        {
            var student = new Student { Name = "Nivash", Age = 33, IsActive = true, Section = "A", CreatedOn = DateTime.Now };

            Notepad.InsertValue(student);

            var mid = Guid.NewGuid();

            var list1 = Notepad.Migrate<Student>(delegate (System.Reflection.PropertyInfo pi, string[] s2)
            {
                if (pi.Name == "Name")
                {
                    return "Naresh";
                }
                if (pi.Name == "Age")
                {
                    return s2[1];
                }
                if (pi.Name == "Section")
                {
                    return s2[2];
                }
                if (pi.Name == "CreatedOn")
                {
                    return s2[3];
                }
                if (pi.Name == "IsActive")
                {
                    return s2[4];
                }
                return "";
            }, mid);

            var list2 = Notepad.Migrate<Student>(delegate (System.Reflection.PropertyInfo pi, string[] s2)
            {
                return "";
            }, mid);


            Assert.AreEqual(list1.Count, 1);
            Assert.IsNull(list2);
        }
    }
}
