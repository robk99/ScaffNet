using Microsoft.Extensions.Logging;

namespace ScaffNet.Utils.Logging
{
    internal class ScaffLogger : IScaffLogger
    {
        public static ScaffLogger Default { get; internal set; } = new ScaffLogger(LogLevel.Debug);
        private LogLevel _minimalLevel { get; set; }

        public ScaffLogger(LogLevel minimalLevel)
        {
            _minimalLevel = minimalLevel;
        }

        public void LogDebug(string message) => Log(message, LogLevel.Debug);
        public void LogInfo(string message) => Log(message, LogLevel.Information);
        public void LogWarning(string message) => Log(message, LogLevel.Warning);
        public void LogError(string message) => Log(message, LogLevel.Error);
        public void LogCritical(string message) => Log(message, LogLevel.Critical);


        /// <summary>
        /// Calls one of the Log methods based on the log level.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logLevel"></param>
        private void Log(string message, LogLevel level)
        {
            if (_minimalLevel == LogLevel.None || level < _minimalLevel) return;

            switch (level)
            {
                case LogLevel.Debug:
                    Console.WriteLine(message);
                    break;

                case LogLevel.Information:
                    Console.WriteLine(message);
                    break;

                case LogLevel.Warning:
                    Console.WriteLine(message);
                    break;

                case LogLevel.Error:
                    Console.WriteLine(message);
                    break;

                case LogLevel.Critical:
                    Console.WriteLine(message);
                    break;
            }
        }
    }
}
