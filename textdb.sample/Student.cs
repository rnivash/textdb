using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace textdb.sample
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
