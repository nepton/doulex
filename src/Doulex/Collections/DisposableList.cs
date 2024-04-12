namespace Doulex.Collections;

/// <summary>
/// Used to hold disposable objects. In create scenario, you can use the <see cref="DisposableList"/> to hold the disposable objects.
/// Any action let the <see cref="DisposableList"/> leave the scope, the <see cref="DisposableList"/> will dispose all the disposable objects.
/// Until <see cref="DisposableList.Clear()"/> invoked, the <see cref="DisposableList"/> will not dispose the disposable objects.
/// </summary>
/// <example>
/// ```csharp
/// public Handler CreateHandler(string param1, string param2)
/// {
///     using var disposableList = new DisposableList();
///
///     // Create the resource1
///     var resource1 = disposableList.Add(new Resource1(param1));
///     if(resource1.IsError)
///     {
///         throw new Exception(resource1.Error);
///     }
/// 
///     // Create the resource2
///     var resource2 = disposableList.Add(new Resource2(param2));
///     resource2.Initialize();
///
///     // Create the resource3
///     var resource3 = disposableList.Add(new Resource3(resource1, resource2));
///
///     // Create success. Return the resource3
///     disposableList.Clear();
///     return resource3;
/// }
/// ```
/// </example>
public class DisposableList : IDisposable
{
    private readonly List<IDisposable> _disposables = [];

    /// <summary>
    /// Add the disposable object to the list
    /// </summary>
    /// <param name="disposable"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Add<T>(T disposable) where T : IDisposable
    {
        _disposables.Add(disposable);
        return disposable;
    }

    /// <summary>
    /// Add all the disposable objects to the list
    /// </summary>
    /// <param name="disposables"></param>
    /// <typeparam name="T"></typeparam>
    public void AddRange<T>(IEnumerable<T> disposables) where T : IDisposable
    {
        _disposables.AddRange(disposables.OfType<IDisposable>());
    }

    /// <summary>
    /// Remove the disposable object from the list
    /// </summary>
    /// <param name="disposable"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Remove<T>(T disposable) where T : IDisposable
    {
        _disposables.Remove(disposable);
        return disposable;
    }

    /// <summary>
    /// Clear the list
    /// </summary>
    public void Clear()
    {
        _disposables.Clear();
    }

    /// <summary>
    /// Dispose all the disposable objects in the list
    /// </summary>
    public void Dispose()
    {
        foreach (var disposable in _disposables)
        {
            try
            {
                disposable.Dispose();
            }
            catch
            {
                // ignored
            }
        }
    }
}
