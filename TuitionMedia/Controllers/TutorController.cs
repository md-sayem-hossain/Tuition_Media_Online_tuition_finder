using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using TuitionMedia.Context;
using TuitionMedia.Models;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;

namespace TuitionMedia.Controllers
{
    public class TutorController : Controller
    {
        //
        // GET: /Tutor/

        public ActionResult Index()
        {
            Notification();
            return View();

        }

       
        


        public ActionResult Teachers()
        {
            Notification();
            var s = db.Registrations.Where(c => c.RegisterAs == 2).ToList();
            if (s.Count == 0)
            {
                ViewBag.NoTeacher = "No Teacher Found";
            }
            else
            {
                ViewBag.NoTeacher = "found";
                ViewBag.teacherlist = s;
            }
            return View();
        }
        public ActionResult aboutus()
        {
            return View();
        }
        TuitionContext db =new TuitionContext();

        public ActionResult Newsfeed()
        {
            Notification();

            var newlist = db.Posttable.ToList();

            var newitem = db.NotificationTbs.ToList();

            foreach (var item in newlist)
            {
                var s = db.Registrations.Where(c => c.Id == item.PostUserId).FirstOrDefault();
                item.PostTime = item.PostTime +" | "+ s.AccountType;
            }

           
            ViewBag.getallpost = newlist;
            ViewBag.newitem = newitem;
            return View();
        }

        public ActionResult Registration()
        {
            return View();
        }



        public ActionResult test()
        {
            var newlist = db.Posttable.ToList();

            ViewBag.getallpost = newlist;
            return View();
        }

        [HttpPost]
        public ActionResult Registration(Registration register, HttpPostedFileBase UserImage)
        {

            var v= db.Registrations.OrderByDescending(u => u.Id).FirstOrDefault();


            TuitionInfo tuitionInfo  =  new TuitionInfo();
            tuitionInfo.Location = "empty";
            tuitionInfo.PreferSubject = "empty";
            tuitionInfo.Salary = "empty";
            tuitionInfo.Medium = "empty";
            tuitionInfo.UserId = v.Id+1;

            if (register.RegisterAs == 2)
            {
                register.AccountType = "Teacher";
            }
            else if (register.RegisterAs == 1)
            {
                register.AccountType = "Student";
            }
            using (var ctx = new TuitionContext())
            {
                var varification = ctx.Registrations.Where(c => c.Email == register.Email).ToList().Count;
                if (varification ==0)
                {
                    if (ModelState.IsValid)
                    {
                        if (UserImage != null && UserImage.ContentLength > 0)
                        {
                            try
                            {
                                var s = UserImage.FileName;
                                var fileName = Guid.NewGuid() + Path.GetExtension(UserImage.FileName);
                                var uploadUrl = Server.MapPath("~/Images");
                                UserImage.SaveAs(Path.Combine(uploadUrl, fileName));
                                register.UserImage = "Images/" + fileName;
                            }
                            catch (Exception ex)
                            {
                                ViewBag.Error = "ERROR:" + ex.Message;
                            }
                            ctx.Registrations.Add(register);

                            if (register.RegisterAs == 2)
                            {
                                ctx.TuitionInfos.Add(tuitionInfo);
                            }
                            else
                            {
                                
                            }


                            
                            ctx.SaveChanges();
                        }
                    }
                    ViewBag.message = "";
                    Response.Redirect("Login");
                }
                else
                {
                    ViewBag.message = "email already exists";
                }
               
            }



            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            using (var ctx = new TuitionContext())
            {
                Registration user = ctx.Registrations.FirstOrDefault(c => c.Email == email && c.Password == password);




                if (user == null)
                {
                    ViewBag.userfoundmsz = "No User Found.";
                }
                else
                {
                    Session["user"] = user;
                    Session["userId"] = user.Id;
                    if (user.RegisterAs == 1)
                    {
                        Session["studentId"] = user.Id;
                        Session["studentName"] = user.Name;
                    }
                    else if (user.RegisterAs == 2)
                    {
                        Session["teacherId"] = user.Id;
                        Session["teacherName"] = user.Name;
                

                    }
                    return RedirectToAction("Index", "Tutor");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Tutor");
        }




        public ActionResult MyProfile()
        {
            Notification();
            int n = Convert.ToInt32(Session["userId"]);
            if (n <= 0)
            {
                return RedirectToAction("Login", "Tutor");
            }
            else
            {
                using (var ctx = new TuitionContext())
                {
                    Registration s = ctx.Registrations.Find(n);
                    var s1 = ctx.TuitionInfos.FirstOrDefault(c => c.UserId == s.Id);

                    ViewBag.profile = s;
                    if (s1 != null)
                    {
                        ViewBag.profile = s;
                        ViewBag.hastution = "Yes";
                        ViewBag.tuitioninfo = s1;
                    }
                    else
                    {
                        ViewBag.profile = s;
                        ViewBag.hastution = "no";
                    }


                }
            }
            return View();
        }














       

        //public ActionResult Profile()
        //{
        //    Notification();
        //    ViewBag.profile = "";
        //    ViewBag.tuitioninfo = "";
        //    int n = Convert.ToInt32(Session["userId"]);
        //    if (n<=0)
        //    {
        //        return RedirectToAction("Login", "Tutor");
        //    }
        //    else
        //    {
        //        using( var ctx = new TuitionContext())
        //        {
        //            Registration s = new Registration();
        //            s = ctx.Registrations.Find(n);

        //            var s1 = ctx.TuitionInfos.FirstOrDefault(c => c.UserId == s.Id);
        //            ViewBag.profile = s;
        //            ViewBag.tuitioninfo = s1;
        //        }
                   
                
               
        //        //var s2 = db.Registrations.Where(c => c.Id == n);
                
        //        GetMyPost();
        //    }
        //    return View();
        //}

        public ActionResult UpdateProfile([Bind(Include = "Id,Name,Address,Gender,Institution,Phone")] Registration registration, HttpPostedFileBase UserImage)
        {
            Notification();
            var c = db.Registrations.Find(registration.Id);
            if (UserImage != null && UserImage.ContentLength > 0)
            {
                try
                {
                    var s = UserImage.FileName;
                    var fileName = Guid.NewGuid() + Path.GetExtension(UserImage.FileName);
                    var uploadUrl = Server.MapPath("~/Images");
                    UserImage.SaveAs(Path.Combine(uploadUrl, fileName));
                    registration.UserImage = "Images/" + fileName;
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "ERROR:" + ex.Message;
                }
               
                c.UserImage = registration.UserImage;
            }
            else
            {
                c.UserImage = c.UserImage;  
            }
            c.Gender = registration.Gender;
            c.Phone = registration.Phone;
            c.Name = registration.Name;
            c.Address = registration.Address;
            c.Institution = registration.Institution;
            db.SaveChanges();
            return RedirectToAction("MyProfile","Tutor");
        }


        public ActionResult UpdateTuition(TuitionInfo tuitionInfo)
        {
            Notification();
            var c = db.TuitionInfos.Find(tuitionInfo.Id);
            c.Location = tuitionInfo.Location;
            c.Medium = tuitionInfo.Medium;
            c.Salary = tuitionInfo.Salary;
            c.PreferSubject = tuitionInfo.PreferSubject;
            db.SaveChanges();
            return RedirectToAction("MyProfile", "Tutor");

        }


        public ActionResult GetMyPost()
        {
            int n = Convert.ToInt32(Session["userId"]);
            var s = db.Posttable.Where(c=> c.PostUserId == n).ToList().Count;
            if (s <= 0)
            {
                ViewBag.NoPost = "No Post Found";
            }
            else
            {
                ViewBag.NoPost = "Found";
                var s1 = db.Posttable.Where(c => c.PostUserId == n).ToList();
                var s2 = db.Registrations.Where(c => c.Id == n).FirstOrDefault();
             
                ViewBag.Mypost = s1;
                ViewBag.MypostImage = s2;


                List<Registration> reg = new List<Registration>();
                
                foreach(var item in s1)
                {
                    Registration ss = db.Registrations.FirstOrDefault(v => v.Id == item.AppliedUserId);
                   reg.Add(ss); 
                    
                }
                ViewBag.responsedpost = reg;
            }



            


            return RedirectToAction("MyPost", "Tutor");

        }

        public ActionResult MyPost()
        {

            Notification();
            int n = Convert.ToInt32(Session["userId"]);
            var s = db.Posttable.Where(c => c.PostUserId == n).ToList().Count;
            if (s <= 0)
            {
                ViewBag.NoPost = "No Post Found";
            }
            else
            {
                ViewBag.NoPost = "Found";
                var s1 = db.Posttable.Where(c => c.PostUserId == n).ToList();
                var s2 = db.Registrations.Where(c => c.Id == n).FirstOrDefault();

                ViewBag.Mypost = s1;
                ViewBag.MypostImage = s2;


                List<Registration> reg = new List<Registration>();

                foreach (var item in s1)
                {
                    Registration ss = db.Registrations.FirstOrDefault(v => v.Id == item.AppliedUserId);
                    reg.Add(ss);

                }
                ViewBag.responsedpost = reg;
            }

            return View();
        }



        public ActionResult CreatePost()
        {
            Notification();
            return View();
        }


        [HttpPost]

        public ActionResult CreatePost(PostTable postTable)
        {
            Notification();
            var now = DateTime.Now;
            postTable.PostTime = now.ToString();
            postTable.PostUserId = Convert.ToInt32(Session["userId"]);
            postTable.ActiveTaken = 0;
            postTable.AppliedUserId = 0;



            var s = db.Registrations.Find(postTable.PostUserId);

            postTable.postuserName = s.Name;
            postTable.postuserimage = s.UserImage;


            using (var ctx = new TuitionContext())
            {
                if(ModelState.IsValid)
                {
                    ctx.Posttable.Add(postTable);
                    ctx.SaveChanges();
                    ViewBag.PostMessage = "SuccessFully Posted";
                }
            }
            

            return View();
        }


        public ActionResult ViewRequest()
        {
            Notification();
            int n = Convert.ToInt32(Session["userId"]);
            var s = db.Posttable.Where(c => c.PostUserId == n).ToList().Count;
            if (s <= 0)
            {
                ViewBag.NoPost = "No Post Found";
            }
            else
            {
                ViewBag.NoPost = "Found";
            }


            var s1 = db.Posttable.Where(c => c.PostUserId == n).ToList();
            var s2 = db.Registrations.Where(c => c.Id == n).FirstOrDefault();

            ViewBag.Mypost = s1;
            ViewBag.MypostImage = s2;


            List<Registration> reg = new List<Registration>();

            foreach (var item in s1)
            {
                Registration ss = db.Registrations.FirstOrDefault(v => v.Id == item.AppliedUserId);
                if (ss != null)
                {
                    reg.Add(ss);
                }
            }
            if (reg.Count != 0)
            {
                ViewBag.responsedpost = reg;
            }
            


            return View(reg);
        }



        public ActionResult EditPost(int id)
        {
            Notification();
            var s = db.Posttable.Find(id);
            ViewBag.Posts = s;

            return View();
        }

      
        [HttpPost]
        public ActionResult UpdatePost(PostTable posttable)
        {
            var s = db.Posttable.Find(posttable.Id);

            s.PostAddress = posttable.PostAddress;
            s.PostClass = posttable.PostClass;
            s.PostInstitution = posttable.PostInstitution;
            s.PostMedium = posttable.PostMedium;
            s.PostSalary = posttable.PostSalary;
            s.PostSubject = posttable.PostSubject;


            db.SaveChanges();
            
            ViewBag.Posts = s;

            return RedirectToAction("MyPost", "Tutor");
        }

        public ActionResult GetAllPost()
        {
            Notification();
            var s = db.Posttable.OrderBy(a => a.Id).ToList(); 
            ViewBag.GetAllPost = s;
            return RedirectToAction("Newsfeed", "Tutor");
        }


        public ActionResult ApplyButtonClick(int id)
        {
            Notification();
            int userid = Convert.ToInt32(Session["userId"]);

            if (userid == 0)
            {
                return RedirectToAction("Login", "Tutor");

            }

            NotificationTB notification = new NotificationTB();


            notification.PostId = id;
            notification.UserId = userid;

            var d = db.Posttable.Find(id);
            d.AppliedUserId = notification.UserId;

            db.NotificationTbs.Add(notification);




            db.SaveChanges();

            return RedirectToAction("Newsfeed", "Tutor");
        }

        public ActionResult Notification()
        {

            int n = Convert.ToInt32(Session["userId"]);

            if (n <= 0)
            {
                return RedirectToAction("Login", "Tutor");
            }

            var s = db.NotificationTbs.Where(c => c.UserId == n).ToList();

            List<PostTable> posts =  new List<PostTable>();
            foreach(var item in s)
            {
                PostTable d = db.Posttable.Where(c => c.AppliedUserId ==  item.UserId && c.Id == item.PostId).FirstOrDefault();
                posts.Add(d);
            }



            var nn = s.Count();

            ViewBag.noticount = nn;
            ViewBag.allnoti = posts;

            return View();
        }

        public ActionResult viewProfile(int id)
        {
            Notification();
            var s = db.Registrations.Where(c=> c.Id == id).ToList();

            ViewBag.profile = s;

            return View();
        }



        public ActionResult contact()
        {

            Notification();
            return View();
        }


        public ActionResult DeletePost(int id)
        {
 
            var s = db.Posttable.Find(id);
            var s1 = db.NotificationTbs.Where(c => c.PostId == id).FirstOrDefault();

            db.Posttable.Remove(s);
            db.NotificationTbs.Remove(s1);
            db.SaveChanges();
            return RedirectToAction("MyPost", "Tutor");
        }


        public ActionResult Settings()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Settings(string password, string newpass, string repeatpass)
        {
            Notification();
             int n = Convert.ToInt32(Session["userId"]);

            var d = db.Registrations.FirstOrDefault(c=> c.Id == n);

            if (d.Password == password)
            {
                d.Password = newpass;
            }
            db.SaveChanges();
            return View();
        }
    }


 
}