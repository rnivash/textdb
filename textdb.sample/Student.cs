using System;
using System.Reflection;

[assembly: AssemblyVersion("3.1.0")]

namespace textDB.Tests
{

    public class Student
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public string Section { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsActive { get; set; }
    }
}
