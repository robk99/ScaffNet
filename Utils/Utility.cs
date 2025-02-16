namespace ScaffNet.Utils
{
    /// <summary>
    /// General purpose utility class for clients.
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// Sets the default logger that inherited ScaffLogger
        /// </summary>
        /// <param name="handler"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SetLogger(ScaffLogger handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            ScaffLogger.Default = handler;
        }
    }
}
