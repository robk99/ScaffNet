namespace ScaffNet.Utils.Logging
{
    public interface IScaffLogger
    {
        void LogDebug(string message);
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogCritical(string message);
    }
}
