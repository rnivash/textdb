using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TextDB;

namespace textDB.Tests
{
    [TestClass]
    public class InsertTests
    {
        [TestInitialize]
        public void TestSetup()
        {
            Notepad.Delete<Student>();
        }

        [TestMethod]
        public void InsertEntityTest()
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
        public void UpdateEntityTest()
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
    }
}
