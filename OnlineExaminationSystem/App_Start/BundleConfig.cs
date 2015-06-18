using System.Web;
using System.Web.Optimization;

namespace OnlineExaminationSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            #region Scripts
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                   "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/controls").Include(
                       "~/Scripts/controls.js"));


            bundles.Add(new ScriptBundle("~/bundles/component").IncludeDirectory(
                       "~/Scripts/component/", "*.js", true));


            bundles.Add(new ScriptBundle("~/bundles/LearnCenter").Include(
                       "~/Scripts/easing.js",
                       "~/Scripts/move-top.js",
                       "~/Scripts/responsive-nav.js",
                       "~/Scripts/jquery.wmuSlider.js"));


            bundles.Add(new ScriptBundle("~/bundles/metisMenu").Include(
                       "~/Scripts/jquery.metisMenu.js"));


            bundles.Add(new ScriptBundle("~/bundles/jquery/controls").Include(
                       "~/Scripts/jquery.metisMenu.js",
                       "~/Scripts/jquery.mousewheel.js",
                       "~/Scripts/jquery.mixitup.min.js",
                       "~/Scripts/jquery.contentcarousel.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                       "~/Scripts/custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include(
                "~/Scripts/dataTables/jquery.dataTables.js").Include(
                "~/Scripts/dataTables/dataTables.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/raphael").Include(
                "~/Scripts/morris/raphael-2.1.0.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/morris").Include(
                "~/Scripts/morris/morris.js"));

            #endregion

            #region CSS
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
              "~/Content/themes/base/core.css",
              "~/Content/themes/base/resizable.css",
              "~/Content/themes/base/selectable.css",
              "~/Content/themes/base/accordion.css",
              "~/Content/themes/base/autocomplete.css",
              "~/Content/themes/base/button.css",
              "~/Content/themes/base/dialog.css",
              "~/Content/themes/base/slider.css",
              "~/Content/themes/base/tabs.css",
              "~/Content/themes/base/datepicker.css",
              "~/Content/themes/base/progressbar.css",
              "~/Content/themes/base/theme.css"));


            bundles.Add(new StyleBundle("~/Content/custom").Include(
                     "~/Content/custom.css"));


            bundles.Add(new StyleBundle("~/Content/LearnCenter").Include(
                     "~/Content/LearnCenter.css"));

            bundles.Add(new StyleBundle("~/Content/font-awesome").Include(
                     "~/Content/font-awesome.css"));


            bundles.Add(new StyleBundle("~/Content/dataTables").Include(
                     "~/Content/dataTables/dataTables.bootstrap.css"));


            bundles.Add(new StyleBundle("~/Content/morris").Include(
                     "~/Content/morris/morris-0.4.3.min.css"));


            bundles.Add(new StyleBundle("~/Content/component").IncludeDirectory(
                     "~/Content/component", "*.css"));

            #endregion

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
