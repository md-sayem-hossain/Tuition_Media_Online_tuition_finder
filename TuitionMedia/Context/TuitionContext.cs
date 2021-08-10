using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TuitionMedia.Models;

namespace TuitionMedia.Context
{
    public class TuitionContext:DbContext
    {
        public DbSet<Login> Logins { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<TuitionInfo> TuitionInfos { get; set; }
        public DbSet<PostTable> Posttable { get; set; }
        public DbSet<NotificationTB> NotificationTbs { get; set; }
    }
}