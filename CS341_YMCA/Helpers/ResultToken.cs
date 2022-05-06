/**
 * Wrapper class for a result and the status of an operation.
 * @author Zach Goethel
 */

namespace CS341_YMCA.Helpers;

/// <summary>
/// Contains basic data about the results and return value of an API call.
/// </summary>
/// <typeparam name="T">Type of the return value.</typeparam>
public class ResultToken<T>
{
    /// <summary>
    /// Whether the call succeeded or had an error.
    /// </summary>
    public bool Success { get; set; } = true;

    /// <summary>
    /// A provided error message, present after an error.
    /// </summary>
    public string? Error { get; set; } = null;

    /// <summary>
    /// Action's returned value of the proper type.
    /// </summary>
    public T? Value { get; set; } = default;

    /// <summary>
    /// Captured timestamp to track when the action completed.
    /// </summary>
    public DateTime TimeStamp { get; set; } = DateTime.Now;

    /// <summary>
    /// Propagates values and errors of the result.
    /// </summary>
    /// <returns>The returned result of the action.</returns>
    /// <exception cref="Exception">If an error occurred.</exception>
    public T? Get()
    {
        // Throw captured error if it exists
        if (!Success)
            throw new Exception(Error);
        // Finally, return value if it exists
        return Value;
    }
}
