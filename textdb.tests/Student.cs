using System;
[assembly: Xunit.CollectionBehavior(DisableTestParallelization = true)]

namespace textDb.Tests
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
