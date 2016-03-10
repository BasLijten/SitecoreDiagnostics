using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Stopwatch sw = Stopwatch.StartNew();
            TelemetryClient telemetry = new TelemetryClient();
            telemetry.TrackEvent("AppInsights Event");
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
                    if(qs=="dependency")
                    {
                        var a = DateTime.UtcNow;
                        new Dependency().DoRun();
                        telemetry.TrackDependency("Dependency 1", "Do Run", a, sw.Elapsed, true);
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Error("query string was qs, throwing exception", ex);
            }

            sw.Stop();
            
            telemetry.TrackMetric("time of Index", sw.Elapsed.Milliseconds);
            return View();
        }
    }
}