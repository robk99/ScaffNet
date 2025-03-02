using Microsoft.Extensions.Logging;
using ScaffNet.Utils.Logging;

namespace ScaffNet.Utils.EventHandling
{
    internal class ScaffEventHandler : IScaffEventHandler
    {
        internal static IScaffEventHandler Default { get; set; } = new ScaffEventHandler(LogLevel.Debug);

        private ScaffLogger _logger { get; set; }

        private ScaffEventHandler(LogLevel minimalLevel)
        {
            _logger = new ScaffLogger(minimalLevel);
        }

        public void OnDebug(string message) => _logger.LogDebug(message);
        public void OnInfo(string message) => _logger.LogInfo(message);
        public void OnWarning(string message) => _logger.LogWarning(message);
        public void OnError(string message) => _logger.LogError(message);
        public void OnCritical(string message) => _logger.LogCritical(message);
    }
}
