using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Models.ViewModels
{
    public class GroupRatingViewModel
    {
        public AcademicGroup Group;
        public IEnumerable<RatingControl> RatingControls;
    }
}
