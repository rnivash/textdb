using System;
using Xunit;
using textDb;

namespace textDb.Tests
{
    public class InsertTests
    {
        public InsertTests(){
            Notepad.Delete<Student>();
        }

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
            Assert.Equal(13, list[0].Age);
            Assert.Equal("A", list[0].Section);
            Assert.Equal(entity1.CreatedOn, list[0].CreatedOn);
            Assert.Equal(true, list[0].IsActive);

        }
    }
}
