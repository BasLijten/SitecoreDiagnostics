using log4net.Appender;
using log4net.spi;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BasLijten.SC.Diagnostics.ApplicationInsights
{
    public sealed class ApplicationInsightsAppender : AppenderSkeleton
    {
        private TelemetryClient telemetryClient;

        /// <summary>
        /// Get/set The Application Insights instrumentationKey for your application. 
        /// </summary>
        /// <remarks>
        /// This is normally pushed from when Appender is being initialized.
        /// </remarks>
        public string InstrumentationKey
        {
            get;
            set;
        }

        internal TelemetryClient TelemetryClient
        {
            get
            {
                return this.telemetryClient;
            }
        }

        /// <summary>
        /// The <see cref="T:Microsoft.ApplicationInsights.Log4NetAppender.ApplicationInsightsAppender" /> requires a layout.
        /// This Appender converts the LoggingEvent it receives into a text string and requires the layout format string to do so.
        /// </summary>
        protected override bool RequiresLayout
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Initializes the Appender and perform instrumentationKey validation.
        /// </summary>
        public override void ActivateOptions()
        {
            base.ActivateOptions();

            this.telemetryClient = new TelemetryClient();
            //TODO: in config
            this.telemetryClient.Context.InstrumentationKey = "75c82881-b330-4c8a-8f2b-eb0ddaec75c3";
            //if (!string.IsNullOrEmpty(config.InstrumentationKey))
            //{
            //    this.telemetryClient.Context.InstrumentationKey = config.InstrumentationKey;
            //}
            this.telemetryClient.Context.GetInternalContext().SdkVersion = "Log4Net: " + ApplicationInsightsAppender.GetAssemblyVersion();
        }

        /// <summary>
        /// Append LoggingEvent Application Insights logging framework.
        /// </summary>
        /// <param name="loggingEvent">Events to be logged.</param>
        protected override void Append(LoggingEvent loggingEvent)
        {
            if (!String.IsNullOrEmpty(loggingEvent.GetExceptionStrRep()))
            {
                this.SendException(loggingEvent);
                return;
            }
            this.SendTrace(loggingEvent);
        }

        private static string GetAssemblyVersion()
        {
            return typeof(ApplicationInsightsAppender).Assembly.GetCustomAttributes(false).OfType<AssemblyFileVersionAttribute>().First<AssemblyFileVersionAttribute>().Version;
        }

        private static void AddLoggingEventProperty(string key, string value, IDictionary<string, string> metaData)
        {
            if (value != null)
            {
                metaData.Add(key, value);
            }
        }

        private void SendException(LoggingEvent loggingEvent)
        {
            try
            {
                ExceptionTelemetry exceptionTelemetry = new ExceptionTelemetry()
                {
                    SeverityLevel = this.GetSeverityLevel(loggingEvent.Level)
                };
                this.BuildCustomProperties(loggingEvent, exceptionTelemetry);
                this.telemetryClient.Track(exceptionTelemetry);
            }
            catch (ArgumentNullException ex)
            {
                throw new LogException(ex.Message, ex);
            }
        }

        private void SendTrace(LoggingEvent loggingEvent)
        {
            try
            {
                //loggingEvent.GetProperties();
                string message = (loggingEvent.RenderedMessage != null) ? base.RenderLoggingEvent(loggingEvent) : "Log4Net Trace";
                TraceTelemetry traceTelemetry = new TraceTelemetry(message)
                {
                    SeverityLevel = this.GetSeverityLevel(loggingEvent.Level)
                };
                this.BuildCustomProperties(loggingEvent, traceTelemetry);
                this.telemetryClient.Track(traceTelemetry);
            }
            catch (ArgumentNullException ex)
            {
                throw new LogException(ex.Message, ex);
            }
        }

        private void BuildCustomProperties(LoggingEvent loggingEvent, ITelemetry trace)
        {
            trace.Timestamp = loggingEvent.TimeStamp;
            trace.Context.User.Id = loggingEvent.UserName;
            IDictionary<string, string> properties;
            if (trace is ExceptionTelemetry)
            {
                properties = ((ExceptionTelemetry)trace).Properties;
            }
            else
            {
                properties = ((TraceTelemetry)trace).Properties;
            }
            ApplicationInsightsAppender.AddLoggingEventProperty("LoggerName", loggingEvent.LoggerName, properties);
            ApplicationInsightsAppender.AddLoggingEventProperty("ThreadName", loggingEvent.ThreadName, properties);
            LocationInfo locationInformation = loggingEvent.LocationInformation;
            if (locationInformation != null)
            {
                ApplicationInsightsAppender.AddLoggingEventProperty("ClassName", locationInformation.ClassName, properties);
                ApplicationInsightsAppender.AddLoggingEventProperty("FileName", locationInformation.FileName, properties);
                ApplicationInsightsAppender.AddLoggingEventProperty("MethodName", locationInformation.MethodName, properties);
                ApplicationInsightsAppender.AddLoggingEventProperty("LineNumber", locationInformation.LineNumber, properties);
            }
            ApplicationInsightsAppender.AddLoggingEventProperty("Domain", loggingEvent.Domain, properties);
            ApplicationInsightsAppender.AddLoggingEventProperty("Identity", loggingEvent.Identity, properties);
            var properties2 = loggingEvent.Properties;
            //PropertiesDictionary properties2 = loggingEvent.GetProperties();
            if (properties2 != null)
            {
                string[] keys = properties2.GetKeys();
                for (int i = 0; i < keys.Length; i++)
                {
                    string text = keys[i];
                    if (!string.IsNullOrEmpty(text) && !text.StartsWith("log4net", StringComparison.OrdinalIgnoreCase))
                    {
                        object obj = properties2[text];
                        if (obj != null)
                        {
                            ApplicationInsightsAppender.AddLoggingEventProperty(text, obj.ToString(), properties);
                        }
                    }
                }
            }
        }

        private SeverityLevel? GetSeverityLevel(Level logginEventLevel)
        {

            if (logginEventLevel == null)
            {
                return null;
            }
            if (logginEventLevel < Level.INFO)
            {
                return new SeverityLevel?(SeverityLevel.Verbose);
            }
            if (logginEventLevel < Level.WARN)
            {
                return new SeverityLevel?(SeverityLevel.Information);
            }
            if (logginEventLevel < Level.ERROR)
            {
                return new SeverityLevel?(SeverityLevel.Warning);
            }
            if (logginEventLevel < Level.SEVERE)
            {
                return new SeverityLevel?(SeverityLevel.Error);
            }
            return new SeverityLevel?(SeverityLevel.Critical);
        }
    }
}
