using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasLijten.SC.Diagnostics.ApplicationInsights.Pipelines
{
    public class ApplicationInsightsPipelineArgs: PipelineArgs
    {
        private string instrumentationKey;
        public string InstrumentationKey
        {
            get { return instrumentationKey; }
            set { instrumentationKey = value; }
        }
    }
}
