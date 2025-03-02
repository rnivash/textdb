using System;
using Xunit;
using textDb;

namespace textDb.Tests
{
    public class InsertTests
    {
        private INotepad _note;
        public InsertTests()
        {
            _note = new Notepad();

        }

        private void CleanDb(){
            _note.Delete<Student>();
        }

        [Fact]
        public void InsertEntityTest()
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

            _note.Insert(entity1);

            var list = _note.Select<Student>();

            Assert.Equal(1, list.Count);
            Assert.Equal("Darshan", list[0].Name);
            Assert.Equal(13, list[0].Age);
            Assert.Equal("A", list[0].Section);
            Assert.Equal(entity1.CreatedOn, list[0].CreatedOn);
            Assert.True(list[0].IsActive);

        }

        [Fact]
        public void InsertEntityWithNullValueTest()
        {
            CleanDb();
            var entity1 = new Student
            {
                Name = "Darshan",
                Age = 13,
                Section = null,
                CreatedOn = DateTime.Now,
                IsActive = true
            };

            _note.Insert(entity1);

            var list = _note.Select<Student>();

            Assert.Equal(1, list.Count);
            Assert.Equal("Darshan", list[0].Name);
            Assert.Equal("", list[0].Section);
        }

        [Fact]
        public void UpdateEntityTest()
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

            _note.Insert(entity1);

            var list = _note.Select<Student>();

            var itm = list[0];
            itm.Section = "B";

            _note.Update<Student>(itm, key => key.Name == itm.Name);

            Assert.Equal(1, list.Count);
            Assert.Equal("B", list[0].Section);
        }
    }
}
