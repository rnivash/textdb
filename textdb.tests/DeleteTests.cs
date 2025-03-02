using System;
using System.Collections.Generic;
using Xunit;
using textDb;

namespace textDb.Tests
{
    public class DeleteTests
    {
        private INotepad _note;
        public DeleteTests()
        {
            _note = new Notepad();

        }
        private void CleanDb(){
            _note.Delete<Student>();
        }

        [Fact]
        public void DeleteEntityTest()
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

            _note.Delete<Student>();

            var list = _note.Select<Student>();

            Assert.Equal(0, list.Count);
        }

        [Fact]
        public void DeleteSpecificEntityTest()
        {
            CleanDb();
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

            _note.Insert<Student>(new List<Student> { entity1, entity2 });

            _note.Delete<Student>(entity1);

            var list = _note.Select<Student>();

            Assert.Equal(1, list.Count);
            Assert.Equal("Nivash", list[0].Name);
        }

        [Fact]
        public void DeleteEntityFilterTest()
        {
            CleanDb();
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

            _note.Insert<Student>(new List<Student> { entity1, entity2 });

            _note.Delete<Student>(std => std.Name == "Nivash" && std.Age == 34);

            var list = _note.Select<Student>();

            Assert.Equal(1, list.Count);
            Assert.Equal("Darshan", list[0].Name);
        }
    }
}
