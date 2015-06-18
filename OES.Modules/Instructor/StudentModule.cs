using OES.Data;
using OES.Model.Users;
using OES.Modules.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Modules.Instructor
{
    public class StudentModule
    {
        #region Student
        public Result<Student> AddStudent(Student student)
        {
            var result = new Result<Student>();
            OESData db = new OESData();
            try
            {
                var errors = ValidateUser(student);
                if (errors.Count > 0)
                {
                    result.Errors = new List<ResultError>(errors);
                    result.Success = false;
                }
                else
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                    db.Dispose();
                    result.Success = true;
                    result.ReturnObject = student;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ReturnObject = student;
                result.AttachedException = ex;
                result.Errors = new List<ResultError>() { 
                    new ResultError(ex)
                };
            }
            return result;

        }

        public static List<ResultError> ValidateUser(User student)
        {
            List<ResultError> errors = new List<ResultError>();
            OESData db = new OESData();
            if (db.Users.Where(s => s.UserName == student.UserName).FirstOrDefault() != null)
            {
                errors.Add(ResultError.AddPropertyError(student, e => e.UserName, "This UserName already exists"));
            }
            else if (db.Users.Where(s => s.Name == student.Name).FirstOrDefault() != null)
            {
                errors.Add(ResultError.AddPropertyError(student, e => e.Name, "This Name already exists"));
            }

            return errors;
        }
        #endregion
    }
}
