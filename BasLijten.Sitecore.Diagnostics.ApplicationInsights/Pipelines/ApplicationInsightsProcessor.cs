using Microsoft.ApplicationInsights.Extensibility;
using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasLijten.SC.Diagnostics.ApplicationInsights.Pipelines
{
    public class ApplicationInsightsProcessor/*: IApplicationInsightsProcessor*/
    {
        public void Process(PipelineArgs args)
        {
            SetupApplicationInsightsContext("aa");
        }        

        private void SetupApplicationInsightsContext(string key)
        {
            TelemetryConfiguration.Active.InstrumentationKey = Sitecore.Configuration.Settings.GetSetting("InstrumentationKey");
        }
    }
}
