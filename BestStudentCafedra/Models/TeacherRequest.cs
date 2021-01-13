﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestStudentCafedra.Models
{
    public class TeacherRequest: Request
    {
        public int Id { get; set; }
        public int GraduationWorkId { get; set; }
        public int TeacherId { get; set; }
        public string Motivation { get; set; }
        public RequestType RequestType { get; set; }

        public override void Approve(Person approvingPerson)
        {
            if (approvingPerson is Teacher teacher)
            {
                if (teacher.Id != TeacherId)
                    throw new ArgumentException("Teacher request can be approved only by teacher to which this request is directed");
                
                base.Approve(teacher);
                if (RequestType == RequestType.ADVISER)
                    GraduationWork.ScientificAdviserId = TeacherId;
                else if (RequestType == RequestType.REVIEWER)
                    GraduationWork.ReviewerId = TeacherId;
            }
            else throw new ArgumentException("Teacher request can be approved only by teacher");
        }

        public virtual Teacher Teacher { get; set; }
        public virtual GraduationWork GraduationWork { get; set; }
    }
    public enum RequestType { ADVISER, REVIEWER };
}