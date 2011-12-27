using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carpass
{
    public static class LogManager 
    {
        static ILog _log;
        public static ILog CurrentLog { get { return _log ?? (_log = new DefaultLog()); } private set { _log = value; } }

        class DefaultLog:ILog
        {
            ILog _log;

            public DefaultLog()
            {
                var log = typeof(ILog);
                var impLog = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                    .FirstOrDefault(x => x.GetInterfaces().Contains(log));
                _log = ServiceLocator.GetInstance(impLog) as ILog;
            }

            public void Log(Exception e, LogLevel level = LogLevel.Error)
            {
                _log.Log(e, level);
            }

            public void Log(string message, LogLevel level = LogLevel.Warning)
            {
                _log.Log(message, level);
            }
        }

        static LogManager()
        {
            //var log = typeof(ILog);
            //var impLog = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            //    .FirstOrDefault(x => x.GetInterfaces().Contains(log));
            try
            {
                CurrentLog = ServiceLocator.GetInstance<ILog>();
            }
            catch
            {
 
            }
        }

        public static void SetLog(ILog logger)
        {
            CurrentLog = logger;
        }

        public static void Info(this ILog log, string message, params object[] parameters)
        {
            log.Log(string.Format(message, parameters), LogLevel.Info);
        }

        public static void Error(this ILog log, string message, params object[] parameters)
        {
            log.Log(string.Format(message, parameters), LogLevel.Error);
        }
    }

    public enum LogLevel
    {
        Info, Warning, Error
    }

    public interface ILog
    {
        void Log(Exception e, LogLevel level = LogLevel.Error);
        void Log(string message, LogLevel level = LogLevel.Warning);
    }
}
