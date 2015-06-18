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
using OES.Modules.Instructor;

namespace OnlineExaminationSystem.Areas.StudentArea.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentsController : Controller
    {
        private OESData db = new OESData();

      


        // GET: Students/Edit/5
        public ActionResult Edit()
        {
            var stdname = HttpContext.User.Identity.Name;
            var student = (from std in db.Students where std.UserName == stdname select std).First();

            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include ="Password,Email")] Student student)
        {
            var stdname = HttpContext.User.Identity.Name;
            var stud = (from std in db.Students where std.UserName == stdname select std).First();

            stud.Email = student.Email;
            stud.Password = student.Password;
            //db.Entry(student).State = EntityState.Modified;
             db.SaveChanges();
             return RedirectToAction("Index", "Course");
            
           
        }

        // GET: Students/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
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
