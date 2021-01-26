using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Models
{
    public class GroupDiscipline
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int DisciplineId { get; set; }

        public virtual AcademicGroup AcademicGroup { get; set; }
        public virtual Discipline Discipline { get; set; }
    }
}
