namespace ScaffNet.Utils.EventHandling
{
    public interface IScaffEventHandler
    {
        public void OnDebug(string message);
        public void OnInfo(string message);
        public void OnWarning(string message);
        public void OnError(string message);
        public void OnCritical(string message);
    }
}
