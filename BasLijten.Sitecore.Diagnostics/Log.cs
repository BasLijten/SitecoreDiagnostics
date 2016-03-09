using log4net;
using log4net.spi;
using Sitecore;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Diagnostics.PerformanceCounters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BasLijten.SC.Diagnostics
{
    public static class Log
    {
        private static Cache singles;
        public static bool Enabled
        {
            get
            {
                return Settings.ConfigurationIsSet;
            }
        }

        public static bool IsDebugEnabled
        {
            get
            {
                if (!Log.Enabled)
                {
                    return false;
                }
                ILog logger = LoggerFactory.GetLogger(typeof(Log));
                return logger != null && logger.IsDebugEnabled;
            }
        }

        public static Cache Singles
        {
            get
            {
                if (Log.singles == null && Log.Enabled)
                {
                    Log.singles = Cache.GetNamedInstance("Log singles", Settings.Caching.SmallCacheSize);
                }
                return Log.singles;
            }
        }

        public static void Audit(string message, object owner)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(owner, "owner");
            Log.Audit(message, owner.GetType());
        }

        /// <summary>
        /// Infoes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="loggerName">Name of the logger.</param>
        public static void Audit(string message, string loggerName)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(loggerName, "loggerName");
            object o = null;
            Log.Audit(message, o, loggerName);
        }

        /// <summary>
        /// Audits the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="ownerType">
        /// Type of the owner.
        /// </param>
        public static void Audit(string message, System.Type ownerType)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(ownerType, "ownerType");
            Log.Audit(message, ownerType, null);
        }

        /// <summary>
        /// Audits the specified owner type.
        /// </summary>
        /// <param name="ownerType">
        /// Type of the owner.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public static void Audit(System.Type ownerType, string format, params string[] parameters)
        {
            Assert.ArgumentNotNull(ownerType, "ownerType");
            Assert.ArgumentNotNull(format, "format");
            Assert.ArgumentNotNull(parameters, "parameters");
            Log.Audit(string.Format(format, parameters), ownerType);
        }

        /// <summary>
        /// Audits the specified owner type.
        /// </summary>
        /// <param name="owner">
        /// The owner.
        /// </param>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public static void Audit(object owner, string format, params string[] parameters)
        {
            Assert.ArgumentNotNull(owner, "owner");
            Assert.ArgumentNotNull(format, "format");
            Assert.ArgumentNotNull(parameters, "parameters");
            Log.Audit(string.Format(format, parameters), owner.GetType());
        }

        public static void Debug(string message, object owner)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(owner, "owner");
            Log.Debug(message, owner, null);
        }

        public static void Debug(string message)
        {
            Assert.ArgumentNotNull(message, "message");
            Log.Debug(message, typeof(Log));
        }

        public static void Debug(string message, string loggerName)
        {
            Assert.ArgumentNotNull(message, "message");
            Log.Debug(message, null, loggerName);
        }

        public static void Error(string message)
        {
            Assert.ArgumentNotNull(message, "message");            
            Log.Error(message, null, String.Empty);
        }

        public static void Error(string message, Exception exception)
        {
            Assert.ArgumentNotNull(message, "message");            
            Log.Error(message, exception, String.Empty);
        }

        public static void Error(string message, object owner)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(owner, "owner");
            Log.Error(message, null, owner.GetType());
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="ownerType">
        /// Type of the owner.
        /// </param>
        public static void Error(string message, System.Type ownerType)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(ownerType, "ownerType");
            Log.Error(message, null, ownerType);
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public static void Error(string message, System.Exception exception, object owner)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(owner, "owner");
            Log.Error(message, exception, owner.GetType());
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="ownerType">
        /// Type of the owner.
        /// </param>
        public static void Error(string message, System.Exception exception, System.Type ownerType)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(ownerType, "ownerType");
            Log.Error(message, exception, ownerType, null);
        }

        /// <summary>
        /// Logs the specified message with level <see cref="F:log4net.spi.Level.ERROR" />.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="loggerName">Name of the logger.</param>
        public static void Error(string message, System.Exception exception, string loggerName)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(loggerName, "loggerName");
            Log.Error(message, exception, null, loggerName);
        }

        public static void Fatal(string message)
        {
            Assert.ArgumentNotNull(message, "message");            
            Log.Fatal(message, String.Empty);
        }

        public static void Fatal(string message, Exception exception)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(exception, "exception");
            Log.Fatal(message, exception, String.Empty);
        }

        /// <summary>
		/// Fatals the specified message.
		/// </summary>
		/// <param name="message">
		/// The message.
		/// </param>
		/// <param name="owner">
		/// The owner.
		/// </param>
		public static void Fatal(string message, object owner)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(owner, "owner");
            Log.Fatal(message, owner.GetType());
        }

        /// <summary>
        /// Fatals the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="ownerType">
        /// Type of the owner.
        /// </param>
        public static void Fatal(string message, System.Type ownerType)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(ownerType, "ownerType");
            Log.Fatal(message, null, ownerType);
        }

        /// <summary>
        /// Fatals the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="loggerName">Name of the logger.</param>
        public static void Fatal(string message, string loggerName)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(loggerName, "loggerName");
            Log.Fatal(message, null, null, loggerName);
        }

        /// <summary>
        /// Fatals the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public static void Fatal(string message, System.Exception exception, object owner)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(owner, "owner");
            Log.Fatal(message, exception, owner.GetType());
        }

        /// <summary>
        /// Fatals the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="ownerType">
        /// Type of the owner.
        /// </param>
        public static void Fatal(string message, System.Exception exception, System.Type ownerType)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(ownerType, "ownerType");
            Log.Fatal(message, exception, ownerType, null);
        }

        public static void Info(string message)
        {
            Assert.ArgumentNotNull(message, "message");            
            Log.Info(message, String.Empty);
        }

        /// <summary>
        /// Infoes the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public static void Info(string message, object owner)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(owner, "owner");
            Log.Info(message, owner, null);
        }

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="loggerName">Name of the logger.</param>
        public static void Info(string message, string loggerName)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(loggerName, "loggerName");
            Log.Info(message, null, loggerName);
        }

        /// <summary>
        /// Logs a single error.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public static void SingleError(string message, object owner)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(owner, "owner");
            if (!Log.Enabled || Log.Singles == null || Log.Singles.ContainsKey(message))
            {
                return;
            }
            Log.Error(message, owner);
            Log.Singles.Add(message, string.Empty);
        }

        /// <summary>
        /// Logs a single fatal message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public static void SingleFatal(string message, System.Exception exception, object owner)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(owner, "owner");
            Log.SingleFatal(message, exception, owner.GetType());
        }

        /// <summary>
        /// Logs a single fatal message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="ownerType">
        /// Type of the owner.
        /// </param>
        public static void SingleFatal(string message, System.Exception exception, System.Type ownerType)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(ownerType, "ownerType");
            if (!Log.Enabled || Log.Singles == null || Log.Singles.ContainsKey(message))
            {
                return;
            }
            Log.Fatal(string.Format("SINGLE MSG: {0}", message), exception, ownerType);
            Log.Singles.Add(message, string.Empty);
            LogNotification.RaiseNotification(LogNotificationLevel.Fatal, message, exception);
        }

        /// <summary>
        /// Logs a single warning.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public static void SingleWarn(string message, object owner)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(owner, "owner");
            if (!Log.Enabled || Log.Singles == null || Log.Singles.ContainsKey(message))
            {
                return;
            }
            Log.Warn(message, owner);
            Log.Singles.Add(message, string.Empty);
        }

        public static void Warn(string message)
        {
            Assert.ArgumentNotNull(message, "message");            
            Log.Warn(message, null, String.Empty);
        }

        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public static void Warn(string message, object owner)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(owner, "owner");
            Log.Warn(message, null, owner);
        }

        public static void Warn(string message, Exception exception)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(exception, "exception");
            Log.Warn(message, exception, String.Empty);
        }

        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public static void Warn(string message, System.Exception exception, object owner)
        {
            Assert.ArgumentNotNull(message, "message");
            Assert.ArgumentNotNull(owner, "owner");
            Log.Warn(message, exception, owner, null);
        }

        /// <summary>
        /// Logs the specified message with level <see cref="F:log4net.spi.Level.WARN" />.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="loggerName">Name of the logger.</param>
        public static void Warn(string message, System.Exception exception, string loggerName)
        {
            Assert.ArgumentNotNull(message, "message");
            Log.Warn(message, exception, null, loggerName);
        }


        private static void Audit(string message, object owner, string loggerName)
        {
            ILogger logger = null;
            if (!Log.Enabled)
            {
                return;
            }
            if (owner != null)
                logger = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), owner.GetType());
            else
                logger = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), loggerName);
            if (logger == null)
            {
                return;
            }
            string msg = string.Format("AUDIT ({0}): {1}", Context.User.Name, message);
            logger.Log(typeof(Log).ToString(), Level.INFO, msg, null);
            if (SystemCount.LoggingAuditsLogged != null)
            {
                SystemCount.LoggingAuditsLogged.Increment(1L);
            }
        }

        private static void Debug(string message, object owner, string loggerName)
        {
            ILogger logger = null;
            if (!Log.Enabled)
            {
                return;
            }
            if (owner != null)
                logger = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), owner.GetType());
            else
                logger = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), loggerName);

            if (logger != null)
            {
                if (!logger.IsEnabledFor(Level.DEBUG))
                {
                    return;
                }
                logger.Log(typeof(Log).ToString(), Level.DEBUG, message, null);
                if (SystemCount.LoggingInformationsLogged != null)
                {
                    SystemCount.LoggingInformationsLogged.Increment(1L);
                }
            }
            LogNotification.RaiseNotification(LogNotificationLevel.Debug, message);
        }

        private static void Info(string message, object owner, string loggerName)
        {
            ILogger logger = null;
            if (!Log.Enabled)
            {
                return;
            }
            if (owner != null)
                logger = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), owner.GetType());
            else
                logger = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), loggerName);

            if (logger != null)
            {
                logger.Log(typeof(Log).ToString(), Level.INFO, message, null);
                if (SystemCount.LoggingInformationsLogged != null)
                {
                    SystemCount.LoggingInformationsLogged.Increment(1L);
                }
            }
            LogNotification.RaiseNotification(LogNotificationLevel.Info, message);
        }

        private static void Warn(string message, System.Exception exception, object owner, string loggerName)
        {
            ILogger logger = null;
            if (!Log.Enabled)
            {
                return;
            }
            if (owner != null)
                logger = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), owner.GetType());
            else
                logger = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), loggerName);

            if (logger != null)
            {
                if (exception != null)
                {
                    logger.Log(typeof(Log).ToString(), Level.WARN, message, exception);
                }
                else
                {
                    logger.Log(typeof(Log).ToString(), Level.WARN, message, null);
                }
                if (SystemCount.LoggingWarningsLogged != null)
                {
                    SystemCount.LoggingWarningsLogged.Increment(1L);
                }
            }
            LogNotification.RaiseNotification(LogNotificationLevel.Warning, message, exception);
        }

        private static void Error(string message, System.Exception exception, System.Type ownerType, string loggerName)
        {
            ILogger logger = null;
            if (!Log.Enabled)
            {
                return;
            }
            if (ownerType != null)
                logger = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), ownerType);
            else
                logger = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), loggerName);

            if (logger != null)
            {
                if (exception != null)
                {
                    logger.Log(typeof(Log).ToString(), Level.ERROR, message, exception);
                }
                else
                {
                    logger.Log(typeof(Log).ToString(), Level.ERROR, message, null);
                }
                if (SystemCount.LoggingErrorsLogged != null)
                {
                    SystemCount.LoggingErrorsLogged.Increment(1L);
                }
            }
            LogNotification.RaiseNotification(LogNotificationLevel.Error, message, exception);
        }

        private static void Fatal(string message, System.Exception exception, System.Type ownerType, string loggerName)
        {
            ILogger logger = null;
            if (!Log.Enabled)
            {
                return;
            }
            if (ownerType != null)
                logger = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), ownerType);
            else
                logger = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), loggerName);

            if (logger != null)
            {
                if (exception != null)
                {
                    logger.Log(typeof(Log).ToString(), Level.FATAL, message, exception);
                }
                else
                {
                    logger.Log(typeof(Log).ToString(), Level.FATAL, message, null);
                }
                if (SystemCount.LoggingFatalsLogged != null)
                {
                    SystemCount.LoggingFatalsLogged.Increment(1L);
                }
            }
            LogNotification.RaiseNotification(LogNotificationLevel.Fatal, message, exception);
        }

    }
}
