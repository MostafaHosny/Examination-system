using System.Web.Mvc;

namespace OnlineExaminationSystem.Areas.InstructorArea
{
    public class InstructorAreaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "InstructorArea";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "InstructorArea_default",
                "Instructor/{controller}/{action}/{id}",
                new { controller="Course", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}