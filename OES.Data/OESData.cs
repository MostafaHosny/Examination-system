using OES.Model.Examination;
using OES.Model.Users;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Data
{
    public class OESData : DbContext
    {
        public OESData()
            : base("DefaultConnection")
        {
            Database.SetInitializer<OESData>(new EOSDBInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }


        public DbSet<Semester> Semesters { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public DbSet<Registration> Registrations { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Exam> Exams { get; set; }

        public DbSet<Chapter> Chapters { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<QuestionVersion> QuestionsVersions { get; set; }
       
        public DbSet<Answer> Answers { get; set; }

        public DbSet<ExamVersion> ExamVersions { get; set; }

        public DbSet<StudentRegistration> StudentRegistration { get; set; }
        public DbSet<ExamChapter> ExamChapters { get; set; }

    }

    //public class EOSDBInitializer : CreateDatabaseIfNotExists<EOSData>
    public class EOSDBInitializer : DropCreateDatabaseIfModelChanges<OESData>
    {
        protected override void Seed(OESData context)
        {
            base.Seed(context);
        }
    }
}
