using System;
using System.Collections.Generic;
using Xunit;
using textDb;

namespace textDb.Tests
{
    public class DeleteTests
    {
        public DeleteTests()
        {
            Notepad.Delete<Student>();
        }

        [Fact]
        public void DeleteEntityTest()
        {
            var entity1 = new Student
            {
                Name = "Darshan",
                Age = 13,
                Section = "A",
                CreatedOn = DateTime.Now,
                IsActive = true
            };

            Notepad.Insert(entity1);

            Notepad.Delete<Student>();

            var list = Notepad.Select<Student>();

            Assert.Equal(0, list.Count);
        }

        [Fact]
        public void DeleteSpecificEntityTest()
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

            Notepad.Insert<Student>(new List<Student> { entity1, entity2 });

            Notepad.Delete<Student>(entity1);

            var list = Notepad.Select<Student>();

            Assert.Equal(1, list.Count);
            Assert.Equal("Nivash", list[0].Name);
        }

        [Fact]
        public void DeleteEntityFilterTest()
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

            Notepad.Insert<Student>(new List<Student> { entity1, entity2 });

            Notepad.Delete<Student>(std => std.Name == "Nivash" && std.Age == 34);

            var list = Notepad.Select<Student>();

            Assert.Equal(1, list.Count);
            Assert.Equal("Darshan", list[0].Name);
        }
    }
}
