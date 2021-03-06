﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Models.ViewModels
{
    public class GroupRatingViewModel
    {
        public AcademicGroup Group;
        public SemesterDiscipline SemesterDiscipline;

        [Display(Name = "Рейтинг контроль")]
        public ICollection<RatingControl> RatingControls { get; set; }

        public GroupRatingViewModel()
        {
            RatingControls = new HashSet<RatingControl>();
        }
    }
}
