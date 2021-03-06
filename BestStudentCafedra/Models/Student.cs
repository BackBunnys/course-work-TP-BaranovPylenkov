﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BestStudentCafedra.Models
{
    public partial class Student: Person
    {
        public Student()
        {
            ActivityProtections = new HashSet<ActivityProtection>();
            GraduationWorks = new HashSet<GraduationWork>();
        }
        [Required(ErrorMessage ="Не указан номер зачетной книжки")]
        [Range(100000, 999999, ErrorMessage ="Номер зачетной книжки должен быть в диапозоне от 100000 до 999999")]
        [Display(Name = "Номер зач. книжки")]
        public int GradebookNumber { get; set; }
        [Required(ErrorMessage = "Не указана группа")]
        [Display(Name = "Группа")]
        public int GroupId { get; set; }
        [Phone(ErrorMessage ="Номер телефона указан в неверном формате")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Группа")]
        public virtual AcademicGroup Group { get; set; }
        public virtual ICollection<ActivityProtection> ActivityProtections { get; set; }
        public virtual ICollection<GraduationWork> GraduationWorks { get; set; }
        public virtual ICollection<StudentRating> StudentRatings { get; set; }
    }
}
