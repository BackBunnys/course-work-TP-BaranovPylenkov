using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Models
{
    public class SchedulePlan
    {
        public SchedulePlan()
        {
            SchedulePlanEvents = new HashSet<SchedulePlanEvent>();
        }
        public int Id { get; set; }
        public int GroupId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? LastChangedDate { get; set; }
        public string ApprovingOfficerName { get; set; }

        public virtual AcademicGroup Group { get; set; }
        public virtual ICollection<SchedulePlanEvent> SchedulePlanEvents { get; set; }
    }
}
