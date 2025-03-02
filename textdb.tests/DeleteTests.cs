using System;
using System.Collections.Generic;
using Xunit;

namespace textDb.Tests
{
    public class DeleteTests
    {
        private INotepad _note;

        public DeleteTests()
        {
            _note = new Notepad();
        }

        private void CleanDb()
        {
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

            Assert.Empty(list);
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
            _note.Delete(entity1);

            var list = _note.Select<Student>();

            Assert.Single(list);
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

            Assert.Single(list);
            Assert.Equal("Darshan", list[0].Name);
        }

        public class Student
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string Section { get; set; }
            public DateTime CreatedOn { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
