using OES.Data;
using OES.Model.Examination;
using OES.Modules.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace OES.Modules.Instructor
{
    public class ChapterModule
    {
        public ChapterModule() { }

        public Result<Chapter> Add(Chapter chapter)
        {
            var result = new Result<Chapter>();
            try
            {
                OESData db = new OESData();
                if (db.Registrations.Include(r => r.Chapters)
                    .FirstOrDefault(reg => reg.RegistrationId.Equals(chapter.RegistrationId, StringComparison.OrdinalIgnoreCase))
                    .Chapters.Any(c => c.Number.Equals(chapter.Number)))
                {
                    result.Success = false;
                    result.Errors = new List<ResultError>() { 
                        new ResultError(){
                            Message = "This chapter number already exist in this course registeration.",
                            Key = "Number"
                        }
                    };
                    result.ReturnObject = chapter;
                    return result;
                }
                db.Chapters.Add(chapter);
                db.SaveChanges();
                db.Dispose();
                result.Success = true;
                result.ReturnObject = chapter;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors = new List<ResultError>(){
                    new ResultError(){Message = ex.Message}
                };
                result.AttachedException = ex;
            }
            return result;
        }

    }
}
