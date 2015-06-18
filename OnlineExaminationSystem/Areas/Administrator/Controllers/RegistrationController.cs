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
using OnlineExaminationSystem.Areas.Administrator.Models;

namespace OnlineExaminationSystem.Areas.Administrator.Controllers
{
    public class RegistrationController : Controller
    {
        public enum Status
        {
            Undergraduate,
            Graduate,
            Isolated
        }
        private OESData db = new OESData();

        // GET: Administrator/Registration
        public ActionResult Index()
        {

            var stat = Status.Undergraduate;
            var registrations = db.Registrations.Include(r => r.Course).Include(r => r.Instructor).Include(r => r.Semester).Include(r => r.Students);
            return View(registrations.ToList());
        }

        public ActionResult Students(string id)
        {
            StudentsRegistration studentsRegistration = new StudentsRegistration();
            studentsRegistration.Registration = db.Registrations.Include(r => r.Course)
                .Include(r => r.Instructor)
                .Include(r => r.Semester)
                .Include(r => r.Students.Select(s => s.Student))
                .FirstOrDefault(r => r.RegistrationId.Equals(id, StringComparison.OrdinalIgnoreCase));
            studentsRegistration.Students = db.Students.Where(e => e.StudentStatus == OES.Status.Undergraduate).ToList();
            if (studentsRegistration.Registration == null)
            {
                return HttpNotFound();
            }
            return View(studentsRegistration);
        }

        

        // GET: Administrator/Registration/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Title");
            ViewBag.InstructorId = new SelectList(db.Instructors.Where(r => r.InstructorStatus == OES.inststatus.Active), "UserId", "Name");
            ViewBag.SemesterId = new SelectList(db.Semesters, "SemesterId", "SemesterTitle");
            return View();
        }

        // POST: Administrator/Registration/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SemesterId,InstructorId,CourseId")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                db.Registrations.Add(registration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Title", registration.CourseId);
            ViewBag.InstructorId = new SelectList(db.Instructors, "UserId", "Name", registration.InstructorId);
            ViewBag.SemesterId = new SelectList(db.Semesters, "SemesterId", "SemesterTitle", registration.SemesterId);
            return View(registration);
        }

        // GET: Administrator/Registration/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Code", registration.CourseId);
            ViewBag.InstructorId = new SelectList(db.Instructors, "UserId", "Name", registration.InstructorId);
            ViewBag.SemesterId = new SelectList(db.Semesters, "SemesterId", "SemesterTitle", registration.SemesterId);
            return View(registration);
        }

        // POST: Administrator/Registration/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RegistrationId,SemesterId,InstructorId,CourseId")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                var reg = db.Registrations.Where(r => r.RegistrationId == registration.RegistrationId).FirstOrDefault();
                reg.SemesterId = registration.SemesterId;
                reg.CourseId = registration.CourseId;
                reg.InstructorId = registration.InstructorId;
                //db.Entry(registration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "Title", registration.CourseId);
            ViewBag.InstructorId = new SelectList(db.Instructors, "UserId", "Name", registration.InstructorId);
            ViewBag.SemesterId = new SelectList(db.Semesters, "SemesterId", "SemesterTitle", registration.SemesterId);
            return View(registration);
        }

        // GET: Administrator/Registration/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // POST: Administrator/Registration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Registration registration = db.Registrations.Find(id);
           
            try
            {
                db.Registrations.Remove(registration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            catch (Exception ex)
            {
               
                ViewBag.error = "Delete regester students first ";
            }

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

        public ActionResult AddStudent(string id, string studentId)
        {
            Registration reg = db.Registrations.Include(r=> r.Students).FirstOrDefault(r => r.RegistrationId.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (reg != null)
            {
                StudentRegistration student = null;
                if (reg.Students != null)
                {
                    student = reg.Students.FirstOrDefault(s => s.UserId.Equals(studentId, StringComparison.OrdinalIgnoreCase));
                }
                if (student == null)
                {

                    StudentRegistration studentReg = new StudentRegistration()
                    {
                        RegistrationId = id,
                        UserId = studentId
                    };
                    reg.Students = reg.Students ?? new List<StudentRegistration>();
                    reg.Students.Add(studentReg);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Students", new { id = id });
        }

        public ActionResult RemoveStudent(string id, string studentId)
        {
            Registration reg = db.Registrations.Include(r => r.Students).FirstOrDefault(r => r.RegistrationId.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (reg != null)
            {
                StudentRegistration student = null;
                if (reg.Students != null)
                {
                    student = reg.Students.FirstOrDefault(s => s.UserId.Equals(studentId, StringComparison.OrdinalIgnoreCase));
                }
                if (student != null)
                {
                    reg.Students.Remove(student);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Students", new { id = id });
        }
    }
}
