namespace TaskManagementSystem.HelperExtensionMethods
{
    public static class Threading
    {
        public static Task ForEachAsync<T>(this IEnumerable<T> list, Func<T, Task> action)
        {
            return Task.WhenAll(list.Select(action));
        }
    }
}
