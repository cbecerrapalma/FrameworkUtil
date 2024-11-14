// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Util.Ui.Sources.Spa.Util;

/// <summary>
/// Representa un lector de cadenas que puede generar eventos al leer.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IDisposable"/> para liberar recursos no administrados.
/// </remarks>
internal sealed class EventedStreamStringReader : IDisposable
{
    private readonly EventedStreamReader _eventedStreamReader;
    private bool _isDisposed;
    private readonly StringBuilder _stringBuilder = new StringBuilder();

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EventedStreamStringReader"/>.
    /// </summary>
    /// <param name="eventedStreamReader">Una instancia de <see cref="EventedStreamReader"/> que se utilizará para leer las líneas de un flujo de eventos.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="eventedStreamReader"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este constructor suscribe el método <see cref="OnReceivedLine"/> al evento <c>OnReceivedLine</c> del <paramref name="eventedStreamReader"/>.
    /// </remarks>
    public EventedStreamStringReader(EventedStreamReader eventedStreamReader)
    {
        _eventedStreamReader = eventedStreamReader
            ?? throw new ArgumentNullException(nameof(eventedStreamReader));
        _eventedStreamReader.OnReceivedLine += OnReceivedLine;
    }

    /// <summary>
    /// Obtiene el contenido del <see cref="_stringBuilder"/> como una cadena.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el contenido actual del <see cref="_stringBuilder"/>.
    /// </returns>
    /// <remarks>
    /// Este método es útil para recuperar el texto acumulado en el objeto <see cref="_stringBuilder"/> 
    /// de manera eficiente, permitiendo su uso en operaciones que requieren una representación en forma de cadena.
    /// </remarks>
    public string ReadAsString() => _stringBuilder.ToString();

    /// <summary>
    /// Maneja la recepción de una línea de texto y la agrega al StringBuilder.
    /// </summary>
    /// <param name="line">La línea de texto que se ha recibido.</param>
    private void OnReceivedLine(string line) => _stringBuilder.AppendLine(line);

    /// <summary>
    /// Libera los recursos utilizados por la instancia actual de la clase.
    /// </summary>
    /// <remarks>
    /// Este método desuscribe el evento <c>OnReceivedLine</c> del <c>_eventedStreamReader</c>
    /// y marca la instancia como desechada para evitar la liberación de recursos múltiples.
    /// </remarks>
    public void Dispose()
    {
        if (!_isDisposed)
        {
            _eventedStreamReader.OnReceivedLine -= OnReceivedLine;
            _isDisposed = true;
        }
    }
}