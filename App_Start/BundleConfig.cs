using System.Web;
using System.Web.Optimization;

namespace TTP_Project
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/css/css").Include(
                      "~/css/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-{version}.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
