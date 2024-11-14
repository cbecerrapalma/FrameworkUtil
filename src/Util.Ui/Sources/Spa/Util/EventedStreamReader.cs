// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;

namespace Util.Ui.Sources.Spa.Util;

/// <summary>
/// Clase que representa un lector de flujo que genera eventos.
/// </summary>
/// <remarks>
/// Esta clase permite leer datos de un flujo y notificar a los suscriptores cuando se producen cambios en los datos.
/// </remarks>
internal sealed class EventedStreamReader
{
    /// <summary>
    /// Delegado que representa un m�todo que manejar� la recepci�n de un fragmento de datos.
    /// </summary>
    /// <param name="chunk">Un segmento de caracteres que representa el fragmento de datos recibido.</param>
    public delegate void OnReceivedChunkHandler(ArraySegment<char> chunk);
    /// <summary>
    /// Delegado que representa el m�todo que manejar� la recepci�n de una l�nea de texto.
    /// </summary>
    /// <param name="line">La l�nea de texto recibida.</param>
    public delegate void OnReceivedLineHandler(string line);
    /// <summary>
    /// Delegado que representa el m�todo que se invoca cuando un flujo se cierra.
    /// </summary>
    /// <remarks>
    /// Este delegado se utiliza para notificar a los suscriptores que un flujo ha sido cerrado,
    /// permitiendo que se realicen acciones espec�ficas en respuesta a este evento.
    /// </remarks>
    /// <example>
    /// Ejemplo de uso:
    /// <code>
    /// OnStreamClosedHandler handler = () => { Console.WriteLine("El flujo ha sido cerrado."); };
    /// </code>
    /// </example>
    public delegate void OnStreamClosedHandler();

    public event OnReceivedChunkHandler OnReceivedChunk;
    public event OnReceivedLineHandler OnReceivedLine;
    public event OnStreamClosedHandler OnStreamClosed;

    private readonly StreamReader _streamReader;
    private readonly StringBuilder _linesBuffer;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EventedStreamReader"/>.
    /// </summary>
    /// <param name="streamReader">El <see cref="StreamReader"/> que se utilizar� para leer datos.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="streamReader"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este constructor inicia una tarea que ejecuta el m�todo <c>Run</c> para leer datos de manera as�ncrona.
    /// </remarks>
    public EventedStreamReader(StreamReader streamReader)
    {
        _streamReader = streamReader ?? throw new ArgumentNullException(nameof(streamReader));
        _linesBuffer = new StringBuilder();
        Task.Factory.StartNew(Run, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
    }

    /// <summary>
    /// Espera a que se reciba una l�nea que contenga alguno de los contenidos especificados.
    /// </summary>
    /// <param name="contents">Una matriz de cadenas que representan los contenidos a buscar en las l�neas recibidas.</param>
    /// <returns>Una tarea que representa la operaci�n asincr�nica. El resultado de la tarea es verdadero si se encontr� un contenido, de lo contrario, se lanzar� una excepci�n al cerrarse el flujo.</returns>
    /// <remarks>
    /// Este m�todo utiliza un <see cref="TaskCompletionSource{T}"/> para manejar la finalizaci�n de la tarea. 
    /// Se registran manejadores de eventos para recibir l�neas y para el cierre del flujo. 
    /// Si se recibe una l�nea que contiene alguno de los contenidos especificados, se completa la tarea con �xito.
    /// Si el flujo se cierra antes de que se reciba una l�nea v�lida, se lanza una excepci�n <see cref="EndOfStreamException"/>.
    /// </remarks>
    /// <exception cref="EndOfStreamException">Se lanza cuando el flujo se cierra antes de que se reciba una l�nea v�lida.</exception>
    public Task<bool> WaitForMatch(string[] contents)
    {
        var tcs = new TaskCompletionSource<bool>();
        var completionLock = new object();

        OnReceivedLineHandler onReceivedLineHandler = null;
        OnStreamClosedHandler onStreamClosedHandler = null;

        void ResolveIfStillPending(Action applyResolution)
        {
            lock (completionLock)
            {
                if (!tcs.Task.IsCompleted)
                {
                    OnReceivedLine -= onReceivedLineHandler;
                    OnStreamClosed -= onStreamClosedHandler;
                    applyResolution();
                }
            }
        }

        onReceivedLineHandler = line => {
            
            if ( contents.Any( content => line.Contains( content,StringComparison.OrdinalIgnoreCase ) ) ) {
                Console.WriteLine( $"dbug: {line}" );
                ResolveIfStillPending( () => tcs.SetResult( true ) );
            }
        };

        onStreamClosedHandler = () =>
        {
            ResolveIfStillPending(() => tcs.SetException(new EndOfStreamException()));
        };

        OnReceivedLine += onReceivedLineHandler;
        OnStreamClosed += onStreamClosedHandler;

        return tcs.Task;
    }

    /// <summary>
    /// Ejecuta un bucle as�ncrono que lee datos de un <see cref="_streamReader"/> y procesa l�neas completas.
    /// </summary>
    /// <remarks>
    /// Este m�todo lee datos en bloques de caracteres y los procesa para detectar l�neas completas.
    /// Cuando se encuentra una l�nea completa, se invoca el evento <see cref="OnCompleteLine"/>.
    /// Si se alcanza el final del flujo, se invoca el evento <see cref="OnClosed"/>.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operaci�n as�ncrona.
    /// </returns>
    private async Task Run()
    {
        var buf = new char[8 * 1024];
        while (true)
        {
            var chunkLength = await _streamReader.ReadAsync(buf, 0, buf.Length);
            if (chunkLength == 0)
            {
                if (_linesBuffer.Length > 0)
                {
                    OnCompleteLine(_linesBuffer.ToString());
                    _linesBuffer.Clear();
                }

                OnClosed();
                break;
            }

            OnChunk(new ArraySegment<char>(buf, 0, chunkLength));

            int lineBreakPos;
            var startPos = 0;

            // get all the newlines
            while ((lineBreakPos = Array.IndexOf(buf, '\n', startPos, chunkLength - startPos)) >= 0 && startPos < chunkLength)
            {
                var length = (lineBreakPos + 1) - startPos;
                _linesBuffer.Append(buf, startPos, length);
                OnCompleteLine(_linesBuffer.ToString());
                _linesBuffer.Clear();
                startPos = lineBreakPos + 1;
            }

            // get the rest
            if (lineBreakPos < 0 && startPos < chunkLength)
            {
                _linesBuffer.Append(buf, startPos, chunkLength - startPos);
            }
        }
    }

    /// <summary>
    /// Maneja la recepci�n de un fragmento de datos.
    /// </summary>
    /// <param name="chunk">El fragmento de datos recibido como un segmento de caracteres.</param>
    /// <remarks>
    /// Este m�todo se invoca cuando se recibe un nuevo fragmento de datos.
    /// Si hay suscriptores al evento <see cref="OnReceivedChunk"/>, se invocar� el evento con el fragmento recibido.
    /// </remarks>
    /// <seealso cref="OnReceivedChunk"/>
    private void OnChunk(ArraySegment<char> chunk)
    {
        var dlg = OnReceivedChunk;
        dlg?.Invoke(chunk);
    }

    /// <summary>
    /// Maneja la finalizaci�n de una l�nea recibida.
    /// </summary>
    /// <param name="line">La l�nea que se ha completado y se va a procesar.</param>
    private void OnCompleteLine(string line)
    {
        var dlg = OnReceivedLine;
        dlg?.Invoke(line);
    }

    /// <summary>
    /// Se invoca cuando se cierra el flujo.
    /// </summary>
    /// <remarks>
    /// Este m�todo llama a todos los suscriptores del evento <see cref="OnStreamClosed"/> 
    /// para notificar que el flujo ha sido cerrado.
    /// </remarks>
    private void OnClosed()
    {
        var dlg = OnStreamClosed;
        dlg?.Invoke();
    }
}