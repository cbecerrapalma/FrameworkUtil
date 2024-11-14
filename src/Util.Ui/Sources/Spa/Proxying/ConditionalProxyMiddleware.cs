// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Hosting;

namespace Util.Ui.Sources.Spa.Proxying;

// This duplicates and updates the proxying logic in SpaServices so that we can update
// the project templates without waiting for 2.1 to ship. When 2.1 is ready to ship,
// merge the additional proxying features (e.g., proxying websocket connections) back
// into the SpaServices proxying code. It's all internal.
/// <summary>
/// Clase que representa un middleware que aplica condiciones espec�ficas para el procesamiento de solicitudes.
/// </summary>
/// <remarks>
/// Este middleware permite la ejecuci�n condicional de ciertos procesos en funci�n de criterios definidos.
/// </remarks>
internal sealed class ConditionalProxyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Task<Uri> _baseUriTask;
    private readonly string _pathPrefix;
    private readonly bool _pathPrefixIsRoot;
    private readonly HttpClient _httpClient;
    private readonly CancellationToken _applicationStoppingToken;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ConditionalProxyMiddleware"/>.
    /// </summary>
    /// <param name="next">El siguiente delegado de solicitud que se ejecutar� en la cadena de middleware.</param>
    /// <param name="pathPrefix">El prefijo de ruta que se utilizar� para las solicitudes proxy.</param>
    /// <param name="requestTimeout">El tiempo de espera para las solicitudes.</param>
    /// <param name="baseUriTask">Una tarea que representa la URI base para el proxy.</param>
    /// <param name="applicationLifetime">Proporciona informaci�n sobre el ciclo de vida de la aplicaci�n.</param>
    /// <remarks>
    /// Este constructor asegura que el <paramref name="pathPrefix"/> comience con una barra diagonal.
    /// Si no es as�, se le antepone una barra diagonal. Adem�s, se inicializan los componentes necesarios
    /// para el funcionamiento del middleware de proxy condicional.
    /// </remarks>
    public ConditionalProxyMiddleware(
        RequestDelegate next,
        string pathPrefix,
        TimeSpan requestTimeout,
        Task<Uri> baseUriTask,
        IHostApplicationLifetime applicationLifetime)
    {
        if (!pathPrefix.StartsWith('/'))
        {
            pathPrefix = "/" + pathPrefix;
        }

        _next = next;
        _pathPrefix = pathPrefix;
        _pathPrefixIsRoot = string.Equals(_pathPrefix, "/", StringComparison.Ordinal);
        _baseUriTask = baseUriTask;
        _httpClient = SpaProxy.CreateHttpClientForProxy(requestTimeout);
        _applicationStoppingToken = applicationLifetime.ApplicationStopping;
    }

    /// <summary>
    /// Invoca el middleware para procesar la solicitud HTTP si la ruta de la solicitud coincide con el prefijo especificado.
    /// </summary>
    /// <param name="context">El contexto HTTP que contiene informaci�n sobre la solicitud y la respuesta.</param>
    /// <returns>
    /// Una tarea que representa la operaci�n asincr�nica de invocar el middleware.
    /// </returns>
    /// <remarks>
    /// Si la ruta de la solicitud comienza con el prefijo especificado o si el prefijo es la ra�z,
    /// se llama al m�todo <see cref="InvokeCore(HttpContext)"/> para manejar la solicitud.
    /// De lo contrario, se pasa el contexto al siguiente middleware en la cadena de ejecuci�n.
    /// </remarks>
    public Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments(_pathPrefix) || _pathPrefixIsRoot)
        {
            return InvokeCore(context);
        }
        return _next.Invoke(context);
    }

    /// <summary>
    /// Invoca el procesamiento del contexto HTTP, intentando realizar una solicitud de proxy.
    /// </summary>
    /// <param name="context">El contexto HTTP que se va a procesar.</param>
    /// <returns>Una tarea que representa la operaci�n asincr�nica.</returns>
    /// <remarks>
    /// Este m�todo intenta realizar una solicitud de proxy utilizando el m�todo <see cref="SpaProxy.PerformProxyRequest"/>.
    /// Si la solicitud se puede procesar como un proxy, se retorna inmediatamente. 
    /// De lo contrario, se contin�a con el siguiente middleware en la cadena de ejecuci�n.
    /// </remarks>
    private async Task InvokeCore(HttpContext context)
    {
        var didProxyRequest = await SpaProxy.PerformProxyRequest(
            context, _httpClient, _baseUriTask, _applicationStoppingToken, proxy404s: false);
        if (didProxyRequest)
        {
            return;
        }

        // Not a request we can proxy
        await _next.Invoke(context);
    }
}