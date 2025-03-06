using ScaffNet.Utils.MessageHandling;

namespace ScaffNet.Utils.ExceptionHandling
{
    public class ScaffNetException : Exception
    {
        public ScaffNetException(string message) : base(message) { }
    }

    public class ScaffNetCommandException : ScaffNetException
    {
        public ScaffNetCommandException(string command, string error)
            : base(ErrorMessages.CommandError(command, error)) { }
    }
}
