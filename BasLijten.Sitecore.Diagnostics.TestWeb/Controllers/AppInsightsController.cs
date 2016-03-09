using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BasLijten.SC.Diagnostics.TestWeb.Controllers
{
    public class AppInsightsController : Controller
    {
        // GET: AppInsights
        public ActionResult Index()
        {
            try
            {


                var qs = Request.QueryString["logtype"];
                if (String.IsNullOrEmpty(qs))
                {
                    Log.Info("Application Insights Test has been started");
                }
                else
                {
                    if (qs == "ex")
                        throw new Exception("bla");
                    if (qs == "warn")
                        Log.Warn("Application Insights Warn message");
                }
            }
            catch(Exception ex)
            {
                Log.Error("query string was qs, throwing exception", ex);
            }
            

            return View();
        }
    }
}