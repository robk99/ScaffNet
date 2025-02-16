namespace ScaffNet.Utils.ErrorHandling
{
    public class ScaffNetException : Exception
    {
        public ScaffNetException(string message) : base(message) { }
    }

    public class ScaffNetCommandException : ScaffNetException
    {
        public ScaffNetCommandException(string command, string error)
            : base(Errors.CommandError(command, error)) { }
    }
}
