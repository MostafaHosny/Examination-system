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
using OES.Model;

namespace OnlineExaminationSystem.Areas.Administrator.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StudentsController : Controller
    {
        private OESData db = new OESData();

        // GET: Students
        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(string id)
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

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserName,Name,Password,Email,Role,BirthDate , StudentStatus")] Student student)
        {
            if (ModelState.IsValid)
            {

                StudentModule Student = new StudentModule();
                var result = Student.AddStudent(student);
                if (result.Success)
                {
                    sendEmail(student);
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError(err.Key, err.Message);
                    }
                }


                db.Students.Add(student);
                db.SaveChanges();
                try { sendEmail(student); }
                catch (Exception) { }
                return RedirectToAction("Index");

            }

            return View(student);
        }

        private void sendEmail(Student student)
        {
            try
            {
                string body = "your username is :" + student.UserName + "\n" + "Your password is :" + student.Password;
                MailMessage message = new MailMessage("MTI.oes@hotmail.com", student.Email, "Your Password and username is", body);
                NetworkCredential netCred = new NetworkCredential("MTI.oes@hotmail.com", "MTIOESoes");
                SmtpClient smtpobj = new SmtpClient("smtp.live.com", 587);
                smtpobj.EnableSsl = true;
                smtpobj.Credentials = netCred;
                smtpobj.Send(message);
            }
            catch(Exception e)
            { }
        }



        // GET: Students/Edit/5
        public ActionResult Edit(string id)
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

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserName,Name,Email,Role,Code,BirthDate,StudentStatus")] Student student)
        {

            if (ModelState.IsValid)
            {
                var stud = db.Students.Where(r => r.UserName == student.UserName).FirstOrDefault();
                stud.Name = student.Name;
                stud.Email = student.Email;
                stud.StudentStatus = student.StudentStatus;
                stud.UserName = student.UserName;
                stud.BirthDate = student.BirthDate;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        public ActionResult ResetPasswrod(string id)
        {
            BaseEntity ent = new BaseEntity();
            var std = db.Students.Where(r => r.UserId == id).FirstOrDefault();
            std.Password = ent.GeneratePassword();
            //db.Entry(instructor).State = EntityState.Modified;
            db.SaveChanges();
            sendEmail(std);
            return RedirectToAction("Index");


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
