using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuitionMedia.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        public int RegisterAs { get; set; }
        public string AccountType { get; set; }
        public string UserImage { get; set; }
        public string Address { get; set; }
        public string Institution { get; set; }
      
    }
}