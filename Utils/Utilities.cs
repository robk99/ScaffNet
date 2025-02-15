namespace ScaffNet.Utils
{
    public class Utilities
    {
        public static void SetLogger(ScaffLogger handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            ScaffLogger.Default = handler;
        }
    }
}
