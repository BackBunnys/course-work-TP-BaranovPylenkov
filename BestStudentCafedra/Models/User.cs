﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Models
{
    public class User: IdentityUser
    {
        public bool IsConfirmed { get; set; }
        public int? SubjectAreaId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string MiddleName { get; set; }
    }
}
