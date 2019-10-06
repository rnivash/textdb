using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextDB;

namespace textDB.Tests
{
    [TestClass]
    public class NotepadTests
    {
        [TestInitialize]
        public void TestSetup()
        {
            Notepad.Delete<Student>();
        }

        [TestMethod]
        public void Insert_Entity_Test()
        {
            var entity1 = new Student
            {
                Name = "Darshan",
                Age = 13,
                Section = "A",
                CreatedOn = DateTime.Now,
                IsActive = true
            };

            Notepad.InsertValue(entity1);

            var list = Notepad.Select<Student>();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("Darshan", list[0].Name);
        }

        [TestMethod]
        public void Delete_Entity_Test()
        {
            var entity1 = new Student
            {
                Name = "Darshan",
                Age = 13,
                Section = "A",
                CreatedOn = DateTime.Now,
                IsActive = true
            };

            Notepad.InsertValue(entity1);

            Notepad.Delete<Student>();

            var list = Notepad.Select<Student>();

            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void Delete_Specific_Entity_Test()
        {
            var entity1 = new Student
            {
                Name = "Darshan",
                Age = 13,
                Section = "A",
                IsActive = true
            };
            var entity2 = new Student
            {
                Name = "Nivash",
                Age = 34,
                Section = "B",
                IsActive = true
            };

            Notepad.InsertValue<Student>(new List<Student> { entity1, entity2 });

            Notepad.Delete<Student>(entity1);

            var list = Notepad.Select<Student>();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("Nivash", list[0].Name);
        }

        [TestMethod]
        public void Delete_Entity_Filter_Test()
        {
            var entity1 = new Student
            {
                Name = "Darshan",
                Age = 13,
                Section = "A",
                IsActive = true
            };
            var entity2 = new Student
            {
                Name = "Nivash",
                Age = 34,
                Section = "B",
                IsActive = true
            };

            Notepad.InsertValue<Student>(new List<Student> { entity1, entity2 });

            Notepad.Delete<Student>(std => std.Name == "Nivash" && std.Age == 34);

            var list = Notepad.Select<Student>();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("Darshan", list[0].Name);
        }

        [TestMethod]
        public void Update_Entity_Test()
        {
            var entity1 = new Student
            {
                Name = "Darshan",
                Age = 13,
                Section = "A",
                CreatedOn = DateTime.Now,
                IsActive = true
            };

            Notepad.InsertValue(entity1);

            var list = Notepad.Select<Student>();

            var itm = list[0];
            itm.Section = "B";

            Notepad.Update<Student>(itm, key => key.Name == itm.Name);

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("B", list[0].Section);
        }

        [TestMethod]
        public void Select_Entity_Test()
        {
            var entity1 = new Student
            {
                Name = "Darshan",
                Age = 13,
                Section = "A",
                CreatedOn = DateTime.Now,
                IsActive = true
            };
            var entity2 = new Student
            {
                Name = "Nivash",
                Age = 13,
                Section = "B",
                CreatedOn = DateTime.Now,
                IsActive = true
            };
            IList<Student> studs = new List<Student>() { entity1, entity2 };
            Notepad.InsertValue(studs);

            var list = Notepad.Select<Student>();
            var list2 = Notepad.Select<Student>(stud => stud.Name == "Nivash");

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("B", list2[0].Section);
            Assert.AreEqual(1, list2.Count);

        }
    }
}
