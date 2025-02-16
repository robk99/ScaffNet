namespace ScaffNet.Utils.ErrorHandling
{
    internal static class Errors
    {
        internal static string CommandError(string command, string error)
        {
            return $"Error executing command: {command} ; with error: {error}";
        }
    }
}
