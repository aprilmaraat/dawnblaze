﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace RedBook.Controllers
{
	//[RequireHttps]
	public class HomeController : Controller
	{
		public ActionResult Index ()
		{
			if (HttpContext.User.Identity.IsAuthenticated) {
				
				return View ();

			} 
			else {
				
				return RedirectToAction("Index", "Account");

			}
		}
	}
}