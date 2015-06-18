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
using OES.Modules.Core;

namespace OnlineExaminationSystem.Areas.Administrator.Controllers
{
    //public class TEST
    //{
    //    public Semester mySemeseter = new Semester();
    //    public List<string> mySemeseterList = new List<string>();// { get; set; }

    //}
    [Authorize(Roles="Admin")]
    public class SemestersController : Controller
    {
        private OESData db = new OESData();

        // GET: Semesters
        public ActionResult Index(string value = null)
        {

            //ChapterViewModel model = new ChapterViewModel();
            //model.Registrations = Registrations;
            if (!string.IsNullOrWhiteSpace(value))
            {
                List<Semester> SelectedSemesters = new List<Semester>();
                if (value == "Fall")
                {
                    SelectedSemesters = db.Semesters.Where(r => r.SemesterName == SemesterName.Fall).ToList();
                    ViewBag.value = "Fall";
                }
                else if (value == "Spring")
                {
                    SelectedSemesters = db.Semesters.Where(r => r.SemesterName == SemesterName.Spring).ToList();
                    ViewBag.value = "Spring";
                }
                else if (value == "Summer")
                {
                    SelectedSemesters = db.Semesters.Where(r => r.SemesterName == SemesterName.Summer).ToList();
                    ViewBag.value = "Summer";
                }
                else
                {
                    SelectedSemesters = db.Semesters.ToList();
                    ViewBag.value = "All";
                }
            
                return View(SelectedSemesters);
            }
            else
                ViewBag.value = "All";
            return View(db.Semesters.ToList());
           

        }


       


        public ActionResult SelectSemestersName(string SelectedSemesterName)
        {


            return RedirectToAction("Index", new { value = SelectedSemesterName });
            //return RedirectToAction("Index", new { id = id });
        }

        //public ActionResult SelectSemestersYear(string SelectedSemesterYear)
        //{
        //    int SemesYear = int.Parse(SelectedSemesterYear);
        //    List<Semester> SelectedSemesters = db.Semesters.Where(r => r.SemesterYear == SemesYear).ToList();
        //    return Index(SelectedSemesters.AsEnumerable());
        //    //return RedirectToAction("Index", new { id = id });
        //}


        // GET: Semesters/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester semester = db.Semesters.Find(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            return View(semester);
        }

        // GET: Semesters/Create
        public ActionResult Create()
        {
            //Semester semester = new Semester();
            //List<string> SemesterList = new List<string>();
            //SemesterList.Add("test");
            //TEST mytest = new TEST();
            //mytest.mySemeseter = semester;
            //mytest.mySemeseterList = SemesterList;
           
            return View();
        }

        // POST: Semesters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StartDate , EndDate ,SemesterName , SemesterYear")] Semester semester)
        {
            var found = db.Semesters.Where(a => a.SemesterName == semester.SemesterName).Where(a => a.SemesterYear == semester.SemesterYear).FirstOrDefault();
            if (found == null)
            {
                if (semester.SemesterYear >= DateTime.Now.Year)
                {
                    if (ModelState.IsValid)
                    {
                        semester.SemesterTitle = (semester.SemesterName + " " + semester.SemesterYear).ToString();
                        db.Semesters.Add(semester);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(semester);
                }
                else
                {
                    var err = ResultError.AddPropertyError(semester, e => e.SemesterYear, "Semester year must be in the future ");

                    ModelState.AddModelError(err.Key, err.Message);

                    return View(semester);
                }

            }
            else
            {
                var err = ResultError.AddPropertyError(semester, e => e.SemesterName, "This Semester  Already Exists.");

                ModelState.AddModelError(err.Key, err.Message);

                return View(semester);
            }

            
        }

        // GET: Semesters/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester semester = db.Semesters.Find(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            return View(semester);
        }

        // POST: Semesters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SemesterId,SemesterTitle,StartDate,EndDate")] Semester semester)
        {
            if (ModelState.IsValid)
            {
                db.Entry(semester).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(semester);
        }

        // GET: Semesters/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester semester = db.Semesters.Find(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            return View(semester);
        }

        // POST: Semesters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                Semester semester = db.Semesters.Find(id);
                db.Semesters.Remove(semester);
                db.SaveChanges();
            }
            catch (Exception) { }

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
