using System.Web;
using System.Web.Mvc;

namespace RedBook
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters (GlobalFilterCollection filters)
		{
			filters.Add (new HandleErrorAttribute ());
//			filters.Add (new SessionExpireAttribute());
		}
	}

//	public class SessionExpireAttribute : ActionFilterAttribute
//	{
//		public override void OnActionExecuting(ActionExecutingContext filterContext)
//		{
//			if( HttpContext.Current.Session["username"] == null ) 
//			{
//				filterContext.Result = new RedirectResult("Login");
//				return;
//			}
//
//			base.OnActionExecuting(filterContext);
//		}
//	}

}

