using UnityEngine;
// ReSharper disable CheckNamespace


public static class GameObjectExtensions
{
    /// <summary>
    /// Returns the objects itself if it exists, null otherwise.
    /// </summary>
    /// <remarks>
    /// This method helps differentiate between a null reference and a destroyed Unity object. Unity's "== null" check
    /// can incorrectly return true for destroyed objects, leading to misleading behaviour. The OrNull method use
    /// Unity's "null check", and if the object has been marked for destruction, it ensures an actual null reference is returned,
    /// aiding in correctly chaining operations and preventing NullReferenceExceptions.
    /// </remarks>>
    /// <param name="obj">The object being checked</param>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <returns>The objects itself if it exists and not destroyed, null otherwise.</returns>
    public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;
}