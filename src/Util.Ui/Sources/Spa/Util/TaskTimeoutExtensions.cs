// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Util.Ui.Sources.Spa.Util;

/// <summary>
/// Proporciona métodos de extensión para manejar timeouts en tareas.
/// </summary>
internal static class TaskTimeoutExtensions
{
    /// <summary>
    /// Extiende la funcionalidad de la clase <see cref="Task"/> para permitir un tiempo de espera.
    /// </summary>
    /// <param name="task">La tarea que se desea ejecutar con un tiempo de espera.</param>
    /// <param name="timeoutDelay">El tiempo máximo que se permitirá para la ejecución de la tarea.</param>
    /// <param name="message">El mensaje que se utilizará en caso de que se produzca un tiempo de espera.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica.
    /// </returns>
    /// <exception cref="TimeoutException">
    /// Se lanza cuando la tarea no se completa dentro del tiempo especificado por <paramref name="timeoutDelay"/>.
    /// </exception>
    /// <remarks>
    /// Este método permite ejecutar una tarea con un límite de tiempo. Si la tarea no se completa dentro del tiempo especificado,
    /// se lanza una excepción <see cref="TimeoutException"/> con el mensaje proporcionado.
    /// </remarks>
    public static async Task WithTimeout(this Task task, TimeSpan timeoutDelay, string message)
    {
        if (task == await Task.WhenAny(task, Task.Delay(timeoutDelay)))
        {
            task.Wait(); // Allow any errors to propagate
        }
        else
        {
            throw new TimeoutException(message);
        }
    }

    /// <summary>
    /// Extiende la funcionalidad de la clase <see cref="Task{T}"/> para permitir la ejecución de una tarea con un tiempo de espera especificado.
    /// </summary>
    /// <typeparam name="T">El tipo de resultado que devuelve la tarea.</typeparam>
    /// <param name="task">La tarea que se desea ejecutar con un tiempo de espera.</param>
    /// <param name="timeoutDelay">El período de tiempo que se espera antes de que se produzca un tiempo de espera.</param>
    /// <param name="message">El mensaje que se utilizará en caso de que se produzca un tiempo de espera.</param>
    /// <returns>
    /// Un objeto de tipo <typeparamref name="T"/> que representa el resultado de la tarea si se completa dentro del tiempo especificado.
    /// </returns>
    /// <exception cref="TimeoutException">Se lanza si la tarea no se completa dentro del tiempo especificado.</exception>
    /// <remarks>
    /// Este método es útil para evitar que una tarea se ejecute indefinidamente, proporcionando un mecanismo para manejar situaciones en las que una operación puede tardar más de lo esperado.
    /// </remarks>
    /// <seealso cref="Task{T}"/>
    public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeoutDelay, string message)
    {
        if (task == await Task.WhenAny(task, Task.Delay(timeoutDelay)))
        {
            return task.Result;
        }
        else
        {
            throw new TimeoutException(message);
        }
    }
}