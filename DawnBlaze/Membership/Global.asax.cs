using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;

namespace Membership
{
	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes (RouteCollection routes)
		{
			routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");

			routes.MapRoute (
				"Default",
				"{controller}/{action}/{id}",
				new { controller = "Login", action = "Index", id = "" }
//				new { controller = "Home", action = "Index", id = "" }
			);

		}

		public static void RegisterBundles(BundleCollection bundles)
		{

			// JS

			bundles.Add(new ScriptBundle("~/Scripts/jquery").Include(
				"~/Scripts/jquery-1.12.0.min.js"));

			bundles.Add(new ScriptBundle("~/Scripts/Shared/Layout").Include(
				"~/Scripts/Shared/layout.js"));

			bundles.Add(new ScriptBundle("~/Scripts/Login/Index").Include(
				"~/Scripts/Login/index.js"));

			// CSS

			bundles.Add(new StyleBundle("~/Content/layout").Include(
				"~/Content/layout.css"));

			bundles.Add(new StyleBundle("~/Content/header").Include(
				"~/Content/header.css"));

			bundles.Add (new StyleBundle("~/Content/home").Include(
				"~/Content/home.css"));

			bundles.Add (new StyleBundle("~/Content/login").Include(
				"~/Content/login.css"));

			BundleTable.EnableOptimizations = true;

		}

		public static void RegisterGlobalFilters (GlobalFilterCollection filters)
		{
			filters.Add (new HandleErrorAttribute ());
		}

		protected void Application_Start ()
		{
			AreaRegistration.RegisterAllAreas ();
			RegisterGlobalFilters (GlobalFilters.Filters);
			RegisterRoutes (RouteTable.Routes);
			RegisterBundles(BundleTable.Bundles);
		}
	}
}
