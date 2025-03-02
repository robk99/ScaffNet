using ScaffNet.Utils.EventHandling;
using EventHandler = ScaffNet.Utils.EventHandling.ScaffEventHandler;

namespace ScaffNet.Utils
{
    /// <summary>
    /// General purpose utility class for clients.
    /// </summary>
    public class ScaffUtils
    {
        /// <summary>
        /// Sets the default Event Handler that inherited IScaffEventHandler
        /// </summary>
        /// <param name="handler"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SetEventHandler(IScaffEventHandler handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            EventHandler.Default = handler;
        }
    }
}
