using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OES.Data;
using OES.Model.Examination;
using OnlineExaminationSystem.Areas.InstructorArea.Models;
using OES.Modules.Instructor;
using OES.Modules.Common;
using OnlineExaminationSystem.Models;


namespace OnlineExaminationSystem.Areas.InstructorArea.Controllers
{
    [Authorize(Roles="Instructor")]
    public class StudentsController :  InstructorBaseController
    {
        private OESData db = new OESData();

        public StudentsController()
        {
            ViewBag.Page = "Students";
        }
        // GET: Students
        public ActionResult Index(string id = null)
        {
            var model = new StudentExamViewModel();
            if (!string.IsNullOrWhiteSpace(id))
            {
               
              model.SelectedRegistration = ExamModule.GetRegistrationForStudentExams(id);
           //      //model.StudentList = (from student in db.Students join studentregs in db.StudentRegistration on student.UserId equals studentregs.UserId join regs in db.Registrations on studentregs.RegistrationId equals regs.RegistrationId where regs.RegistrationId == model.SelectedRegistration.RegistrationId select student).ToList();//on studentregs in db.StudentRegistration   //db.Students.Where(q => q.UserId.Equals(db.StudentRegistration.Where (x => x.RegistrationId == model.SelectedRegistration.RegistrationId).Select(a => a.UserId))).ToList();
           //        for (int i = 0; i < model.StudentList.Count; i++)
           //      {

           //             foreach (var STU in db.Students)
           //             if (STU.UserId == model.StudentList[i].UserId)
           //             {
           //                 model.StudentList[i].StudentRegistrations = new List<StudentRegistration>();
           //                 model.StudentList[i].StudentRegistrations = STU.StudentRegistrations;

           //             }
           //     }
            }
            model.StudentList = db.Students.ToList();
            model.Registrations = db.Registrations.Include(q => q.Exams).Include(a => a.ExamVersions).Include(c => c.Course)
            .Include(e =>e.Students).Include(r=> r.Semester).Include(p =>p.Chapters).ToList();
            return View(model);
        }

        public ActionResult Select(string id)
        {
            return RedirectToAction("Index", new { id = id });
        }

        // GET: Students/Details/5
//        public ActionResult Details(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Student student = db.Students.Find(id);
//            if (student == null)
//            {
//                return HttpNotFound();
//            }
//            return View(student);
//        }

//        // GET: Students/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: Students/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "UserName,Name,Password,Email,Role,BirthDate , StudentStatus")] Student student)
//        {
//            if (ModelState.IsValid)
//            {
              
//                db.Students.Add(student);
//                db.SaveChanges();
//                //try { sendEmail(student); }
//                //catch (Exception) { }
//                return RedirectToAction("Index");
                
//            }

//            return View(student);
//        }

//private void sendEmail(Student student)
//{
//    string body = "your username is :"+ student.UserName +"\n" + "Your password is :" + student.Password ;
//    MailMessage message = new MailMessage("mostafa.hosny2015@hotmail.com", student.Email, "Your Password and username" , body);
//    NetworkCredential netCred = new NetworkCredential("mostafa.hosny2015@hotmail.com", "pass");
//    SmtpClient smtpobj= new SmtpClient("smtp.live.com", 587); 
//    smtpobj.EnableSsl = true;
//    smtpobj.Credentials = netCred;
//    smtpobj.Send(message);
//}

       

//        // GET: Students/Edit/5
//        public ActionResult Edit(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Student student = db.Students.Find(id);
//            if (student == null)
//            {
//                return HttpNotFound();
//            }
//            return View(student);
//        }

//        // POST: Students/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "UserId,UserName,Name,Password,Email,Role,Code,BirthDate")] Student student)
//        {
          
//            if (ModelState.IsValid)
//            {
               
//                db.Entry(student).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(student);
//        }

//        // GET: Students/Delete/5
//        public ActionResult Delete(string id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Student student = db.Students.Find(id);
//            if (student == null)
//            {
//                return HttpNotFound();
//            }
//            return View(student);
//        }

//        // POST: Students/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(string id)
//        {
//            Student student = db.Students.Find(id);
//            db.Students.Remove(student);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
    }
}
