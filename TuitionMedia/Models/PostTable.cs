using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuitionMedia.Models
{
    public class PostTable
    {
        public int Id { get; set; }
        public string PostSubject { get; set; }
        public string PostClass { get; set; }
        public string PostMedium { get; set; }
        public string PostTime { get; set; }
        public string PostSalary { get; set; }
        public string PostInstitution { get; set; }
        public string PostAddress { get; set; }
        public int PostUserId { get; set; }
        public int ActiveTaken { get; set; }
        public int AppliedUserId { get; set; }
        public string postuserimage { get; set; }
        public string postuserName { get; set; }
    }
}