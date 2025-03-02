using System;
using System.Collections.Generic;
using Xunit;
using textDb;

namespace textDb.Tests
{
    public class ReadTests
    {
        private INotepad _note;
        public ReadTests()
        {
            _note = new Notepad();
        }
        private void CleanDb(){
            _note.Delete<Student>();
        }

        [Fact]
        public void SelectEntityTest()
        {
            CleanDb();
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
            _note.Insert(studs);

            var list = _note.Select<Student>();
            var list2 = _note.Select<Student>(stud => stud.Name == "Nivash");

            Assert.Equal(2, list.Count);
            Assert.Equal("B", list2[0].Section);
            Assert.Single(list2);

        }
    }
}
