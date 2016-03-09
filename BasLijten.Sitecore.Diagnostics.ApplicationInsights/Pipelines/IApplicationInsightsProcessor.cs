using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasLijten.SC.Diagnostics.ApplicationInsights.Pipelines
{
    public interface IApplicationInsightsProcessor
    {
        void Process(ApplicationInsightsPipelineArgs args);
    }
}
