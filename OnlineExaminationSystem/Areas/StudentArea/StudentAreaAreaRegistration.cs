using System.Web.Mvc;

namespace OnlineExaminationSystem.Areas.StudentArea
{
    public class StudentAreaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "StudentArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "StudentArea_default",
                "Student/{controller}/{action}/{id}",
                new { controller = "Course", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}