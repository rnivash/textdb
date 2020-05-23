using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TextDB;

namespace textDB.Tests
{
    [TestClass]
    public class ReadTests
    {
        [TestInitialize]
        public void TestSetup()
        {
            Notepad.Delete<Student>();
        }

        [TestMethod]
        public void SelectEntityTest()
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
