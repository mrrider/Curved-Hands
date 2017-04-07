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

            BundleTable.EnableOptimizations = true;
        }
    }
}
