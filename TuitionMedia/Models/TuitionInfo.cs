using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuitionMedia.Models
{
    public class TuitionInfo
    {
        public int Id { get; set; }
        public string Medium { get; set; }
        public string Location { get; set; }
        public string Salary { get; set; }
        public string PreferSubject { get; set; }
        public int UserId { get; set; }
    }
}