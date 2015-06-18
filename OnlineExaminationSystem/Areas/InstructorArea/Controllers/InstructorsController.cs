using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OES.Data;
using OES.Model.Users;
using System.Net.Mail;

namespace OnlineExaminationSystem.Areas.InstructorArea.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class InstructorsController : Controller
    {
        private OESData db = new OESData();

       
      
        // GET: Instructors/Edit/5
        public ActionResult Edit()
        {
            var instname = HttpContext.User.Identity.Name;
            var instruct = (from inst in db.Instructors where inst.UserName == instname select inst).First();


            return View(instruct);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Password,Email")] Instructor instructor)
        {
            var instname = HttpContext.User.Identity.Name;
            var instruc = (from std in db.Instructors where std.UserName == instname select std).First();

            instruc.Email = instructor.Email;
            instruc.Password = instructor.Password;
            db.SaveChanges();
            return RedirectToAction("Index", "Course");
        }

     
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
