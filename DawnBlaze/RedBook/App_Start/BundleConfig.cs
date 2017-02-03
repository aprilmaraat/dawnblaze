using System;
using System.Web.Optimization;

namespace RedBook.App_Start
{
	public class BundleConfig
	{
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

			bundles.Add (new StyleBundle("~/Content/register").Include(
				"~/Content/register.css"));

			BundleTable.EnableOptimizations = true;

		}
	}
}

