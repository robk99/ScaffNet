namespace ScaffNet.Utils.MessageHandling
{
    internal static class ErrorMessages
    {
        internal static string CommandError(string command, string error) =>
            $"Error executing command: {command} ; with error: {error}";

    }
}
