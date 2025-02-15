using Microsoft.Extensions.Logging;

namespace ScaffNet.Utils
{
    public class ScaffLogger
    {
        public static ScaffLogger Default { get; internal set; } = new ScaffLogger(LogLevel.Debug);

        private LogLevel _minimalLevel { get; set; }

        protected ScaffLogger(LogLevel minimalLevel)
        {
            _minimalLevel = minimalLevel;
        }

        /// <summary>
        /// Calls one of the Log methods based on the log level.
        /// If no log level is provided, the minimal log level is used.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logLevel"></param>
        private void Log(string message, LogLevel? level = null)
        {
            if (level == null) level = _minimalLevel;
            if (_minimalLevel == LogLevel.None || level < _minimalLevel) return;

            switch (_minimalLevel)
            {
                case LogLevel.Debug:
                    LogDebugBehaviour(message);
                    break;

                case LogLevel.Information:
                    LogInfoBehaviour(message);
                    break;

                case LogLevel.Warning:
                    LogWarningBehaviour(message);
                    break;

                case LogLevel.Error:
                    LogErrorBehaviour(message);
                    break;

                case LogLevel.Critical:
                    LogCriticalBehaviour(message);
                    break;
            }
        }

        public void LogDebug(string message) => Log(message, LogLevel.Debug);
        public void LogInfo(string message) => Log(message, LogLevel.Information);
        public void LogWarning(string message) => Log(message, LogLevel.Warning);
        public void LogError(string message) => Log(message, LogLevel.Error);
        public void LogCritical(string message) => Log(message, LogLevel.Critical);

        protected virtual void LogDebugBehaviour(string message) => Console.WriteLine(message);

        protected virtual void LogInfoBehaviour(string message) => Console.WriteLine(message);

        protected virtual void LogWarningBehaviour(string message) => Console.WriteLine(message);

        protected virtual void LogErrorBehaviour(string message) => Console.WriteLine(message);

        protected virtual void LogCriticalBehaviour(string message) => Console.WriteLine(message);
    }
}
