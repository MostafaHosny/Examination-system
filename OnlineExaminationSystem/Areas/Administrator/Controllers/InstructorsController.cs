using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OES.Data;
using OES.Model;
using OES.Model.Users;
using System.Net.Mail;
using OES.Modules.Core;
using OES.Modules.Instructor;

namespace OnlineExaminationSystem.Areas.Administrator.Controllers
{
    [Authorize(Roles="Admin")]
    public class InstructorsController : Controller
    {
        private OESData db = new OESData();

        // GET: Instructors
        public ActionResult Index()
        {
            return View(db.Instructors.ToList());
            
        }

        // GET: Instructors/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Users.Find(id) as Instructor;
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // GET: Instructors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,UserName,Name,Password,Email,Role,Code,InstructorStatus")] Instructor instructor)
        {

            if (ModelState.IsValid)
            {
                 var result = AddInstructor(instructor);
                if (result.Success)
                {
                    sendEmail(instructor);
                    return RedirectToAction("Index");
                }
                 else
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError(err.Key, err.Message);
                    }
                }
                db.Instructors.Add(instructor);
                db.SaveChanges();
                try { sendEmail(instructor); }
                catch (Exception) { }
                return RedirectToAction("Index");
            }

            return View(instructor);
          
        }

        private void sendEmail(Instructor instructor)
        {
            try
            {
                string body = "your username is :" + instructor.UserName + "\n" + "Your password is :" + instructor.Password;
                MailMessage message = new MailMessage("MTI.oes@hotmail.com", instructor.Email, "Your Password and username Is ", body);
                NetworkCredential netCred = new NetworkCredential("MTI.oes@hotmail.com", "MTIOESoes");
                SmtpClient smtpobj = new SmtpClient("smtp.live.com", 587);
                smtpobj.EnableSsl = true;
                smtpobj.Credentials = netCred;
                smtpobj.Send(message);
            }
            catch (Exception ex ){ }
        }

        // GET: Instructors/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Users.Find(id) as Instructor;
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserName,Name,Password,Email,InstructorStatus")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                var ins = db.Instructors.Where(r => r.UserName == instructor.UserName).FirstOrDefault();
                ins.Name = instructor.Name;
                ins.Email = instructor.Email;
                ins.InstructorStatus = instructor.InstructorStatus;
                ins.UserName = instructor.UserName;

                //db.Entry(instructor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(instructor);
        }

        public ActionResult ResetPasswrod(string id)
        {
            BaseEntity ent = new BaseEntity();
                var ins = db.Instructors.Where(r => r.UserId == id).FirstOrDefault();
                ins.Password = ent.GeneratePassword();
                //db.Entry(instructor).State = EntityState.Modified;
                db.SaveChanges();
                sendEmail(ins);
                return RedirectToAction("Index");
            
           
        }

        // GET: Instructors/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Users.Find(id) as Instructor;
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Instructor instructor = db.Users.Find(id) as Instructor;
            db.Users.Remove(instructor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public Result<Instructor> AddInstructor(Instructor instructor)
        {
            var result = new Result<Instructor>();
            OESData db = new OESData();
            try
            {
                var errors =  StudentModule.ValidateUser(instructor);
                if (errors.Count > 0)
                {
                    result.Errors = new List<ResultError>(errors);
                    result.Success = false;
                }
                else
                {
                    db.Instructors.Add(instructor);
                    db.SaveChanges();
                    db.Dispose();
                    result.Success = true;
                    result.ReturnObject = instructor;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ReturnObject = instructor;
                result.AttachedException = ex;
                result.Errors = new List<ResultError>() { 
                    new ResultError(ex)
                };
            }
            return result;

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
