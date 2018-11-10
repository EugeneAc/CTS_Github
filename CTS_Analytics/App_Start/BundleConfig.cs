using System.Web;
using System.Web.Optimization;

namespace CTS_Analytics
{
  public class BundleConfig
  {
    // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
    public static void RegisterBundles(BundleCollection bundles)
    {
      bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                  "~/Scripts/jquery-{version}.js"));

      bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                  "~/Scripts/jquery.validate*"));

			bundles.Add(new ScriptBundle("~/bundles/analscripts").Include(
				 "~/Scripts/jquery.cookie-1.4.1.min.js",
				 "~/Scripts/daterangepicker.js",
				  "~/Scripts/AnalScripts.js"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                  "~/Scripts/modernizr-*"));

      bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                //"~/Scripts/bootstrap.js",
                "~/Scripts/respond.js",
                "~/Scripts/moment.min.js",
                 "~/Scripts/moment-with-locales.min.js",
                "~/Scripts/bootstrap-sortable.js",
				"~/Scripts/bootstrap-datetimepicker.js"
				));

	   bundles.Add(new StyleBundle("~/Content/css").Include(
                //"~/Content/United.bootstrap.css",
                "~/Content/bootstrap-datetimepicker.css",
                "~/Content/bootstrap-sortable.css",
                "~/Content/daterangepicker.css",
                "~/Content/normalize.css",
				"~/Content/anal_theme.css"
				));

			bundles.Add(new StyleBundle("~/Content/sitecss").Include(
			   "~/Content/site.css"));
		}
  }
}
