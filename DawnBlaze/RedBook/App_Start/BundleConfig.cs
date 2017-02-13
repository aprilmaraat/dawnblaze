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

			bundles.Add(new ScriptBundle("~/Scripts/Login/Register").Include(
				"~/Scripts/Login/register.js"));

			bundles.Add(new ScriptBundle("~/Scripts/Login/Forgot-Password").Include(
				"~/Scripts/Login/forgot-password.js"));

			// CSS

			bundles.Add(new StyleBundle("~/Content/Layout").Include(
				"~/Content/layout.css"));

			bundles.Add(new StyleBundle("~/Content/Header").Include(
				"~/Content/header.css"));

			bundles.Add (new StyleBundle("~/Content/Home").Include(
				"~/Content/home.css"));

			bundles.Add (new StyleBundle("~/Content/Login").Include(
				"~/Content/login.css"));

			bundles.Add (new StyleBundle("~/Content/Register").Include(
				"~/Content/register.css"));

			bundles.Add (new StyleBundle("~/Content/Forgot-Password").Include(
				"~/Content/forgot-password.css"));

			BundleTable.EnableOptimizations = true;

		}
	}
}

