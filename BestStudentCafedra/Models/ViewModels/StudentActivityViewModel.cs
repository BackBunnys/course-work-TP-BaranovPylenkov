using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Models.ViewModels
{
    public class StudentActivityViewModel
    {
        public Activity Activity { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
