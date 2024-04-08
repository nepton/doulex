namespace Doulex;

/// <summary>
/// The extension methods for enumerable
/// </summary>
public static class ArrayExtensions
{
    /// <summary>
    /// Return null if the enumerable is empty
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T[]? ToNullIfEmpty<T>(this T[] source)
    {
        return source.Any() ? source : null;
    }

    /// <summary>
    /// For each item in the enumerable
    /// </summary>
    /// <param name="source"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    public static void ForEach<T>(this T[] source, Action<T> action)
    {
        foreach (var item in source)
        {
            action(item);
        }
    }

    /// <summary>
    /// For each item in the enumerable
    /// </summary>
    /// <param name="source"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    public static void ForEach<T>(this T[] source, Action<T, int> action)
    {
        for (var i = 0; i < source.Length; i++)
        {
            action(source[i], i);
        }
    }

    /// <summary>
    /// For each item in the enumerable
    /// </summary>
    /// <param name="source"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Task ForEachAsync<T>(this T[] source, Func<T, Task> action)
    {
        return Task.WhenAll(source.Select(action));
    }

    /// <summary>
    /// For each item in the enumerable
    /// </summary>
    /// <param name="source"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Task ForEachAsync<T>(this T[] source, Func<T, int, Task> action)
    {
        return Task.WhenAll(source.Select(action));
    }
}
