using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuitionMedia.Models
{
    public class NotificationTB
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
    }
}