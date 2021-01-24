using System;
using Xunit;
using textDb;

namespace textDb.Tests
{
    public class UnitTest1
    {
        [Fact]
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

            Assert.Equal(1, list.Count);
            Assert.Equal("Darshan", list[0].Name);

        }
    }
}
