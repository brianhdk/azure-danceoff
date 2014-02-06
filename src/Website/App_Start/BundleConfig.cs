using System.Web;
using System.Web.Optimization;

namespace Website
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));
			bundles.Add(new ScriptBundle("~/bundles/signalr").Include("~/Scripts/jquery.signalR-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));
			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js"));

	        bundles.Add(new ScriptBundle("~/bundles/angularjs").Include(
				"~/Scripts/angular.js",
		        "~/Scripts/angular-animate.js",
 				"~/Scripts/lodash-2.4.1.js",
				"~/Scripts/application/app.js"));

	        bundles.Add(new StyleBundle("~/Content/css").Include(
		        "~/Content/bootstrap.css",
		        "~/Content/angular.css",
		        "~/Content/site.css",
		        "~/Content/danceoff.css"));
        }
    }
}
