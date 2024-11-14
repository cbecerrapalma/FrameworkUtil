// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Hosting;

namespace Util.Ui.Sources.Spa.Proxying;

/// <summary>
/// Proporciona métodos de extensión para facilitar el proxying de aplicaciones SPA (Single Page Application).
/// </summary>
public static class SpaProxyingExtensions {
    /// <summary>
    /// Configura un proxy para el servidor de desarrollo de una aplicación de una sola página (SPA).
    /// </summary>
    /// <param name="spaBuilder">La instancia de <see cref="ISpaBuilder"/> que se está configurando.</param>
    /// <param name="baseUri">La URI base del servidor de desarrollo.</param>
    /// <remarks>
    /// Este método permite redirigir las solicitudes a un servidor de desarrollo especificado por la URI base.
    /// Es útil para el desarrollo de aplicaciones SPA que requieren un backend separado.
    /// </remarks>
    /// <seealso cref="ISpaBuilder"/>
    public static void UseProxyToSpaDevelopmentServer(
        this ISpaBuilder spaBuilder,
        string baseUri ) {
        UseProxyToSpaDevelopmentServer(
            spaBuilder,
            new Uri( baseUri ) );
    }

    /// <summary>
    /// Configura un proxy para el servidor de desarrollo de una aplicación de una sola página (SPA).
    /// </summary>
    /// <param name="spaBuilder">La instancia de <see cref="ISpaBuilder"/> que se está configurando.</param>
    /// <param name="baseUri">La URI base que se utilizará para el proxy.</param>
    /// <remarks>
    /// Este método permite redirigir las solicitudes al servidor de desarrollo de la SPA
    /// utilizando la URI proporcionada. Es útil durante el desarrollo para facilitar la
    /// integración entre el servidor y la aplicación cliente.
    /// </remarks>
    /// <seealso cref="ISpaBuilder"/>
    public static void UseProxyToSpaDevelopmentServer(
        this ISpaBuilder spaBuilder,
        Uri baseUri ) {
        UseProxyToSpaDevelopmentServer(
            spaBuilder,
            () => Task.FromResult( baseUri ) );
    }

    /// <summary>
    /// Configura el uso de un proxy para el servidor de desarrollo de una aplicación de una sola página (SPA).
    /// </summary>
    /// <param name="spaBuilder">El objeto <see cref="ISpaBuilder"/> que se utiliza para configurar la aplicación SPA.</param>
    /// <param name="baseUriTaskFactory">Una función que devuelve una tarea que produce una <see cref="Uri"/> base para el proxy.</param>
    /// <remarks>
    /// Este método habilita el uso de WebSockets y asegura que las solicitudes no se agoten, lo que es crucial
    /// para ciertos tipos de respuestas como eventos enviados por el servidor. 
    /// Se omiten las solicitudes que comienzan con "/view/" y "/api/", permitiendo que sean manejadas 
    /// por otros middleware.
    /// </remarks>
    /// <seealso cref="ISpaBuilder"/>
    /// <seealso cref="SpaProxy"/>
    public static void UseProxyToSpaDevelopmentServer(
        this ISpaBuilder spaBuilder,
        Func<Task<Uri>> baseUriTaskFactory ) {
        var applicationBuilder = spaBuilder.ApplicationBuilder;
        var applicationStoppingToken = GetStoppingToken( applicationBuilder );

        // Since we might want to proxy WebSockets requests (e.g., by default, AngularCliMiddleware
        // requires it), enable it for the app
        applicationBuilder.UseWebSockets();

        // It's important not to time out the requests, as some of them might be to
        // server-sent event endpoints or similar, where it's expected that the response
        // takes an unlimited time and never actually completes
        var neverTimeOutHttpClient =
            SpaProxy.CreateHttpClientForProxy( Timeout.InfiniteTimeSpan );

        applicationBuilder.Use(async(context, next) => {
            if ( context.Request.Path.Value != null && context.Request.Path.Value.StartsWith( "/view/" ) ) {
                await next();
                return;
            }
            if ( context.Request.Path.Value != null && context.Request.Path.Value.StartsWith( "/api/" ) ) {
                await next();
                return;
            }
            await SpaProxy.PerformProxyRequest(
                context, neverTimeOutHttpClient, baseUriTaskFactory(), applicationStoppingToken,
                proxy404s: true );
        } );
    }

    /// <summary>
    /// Obtiene un token de cancelación que se activa cuando la aplicación está en proceso de detenerse.
    /// </summary>
    /// <param name="appBuilder">El constructor de la aplicación que proporciona acceso a los servicios de la aplicación.</param>
    /// <returns>
    /// Un <see cref="CancellationToken"/> que se puede utilizar para detectar cuándo la aplicación está deteniéndose.
    /// </returns>
    /// <remarks>
    /// Este método es útil para realizar tareas de limpieza o para cancelar operaciones en curso cuando la aplicación recibe una señal de detención.
    /// </remarks>
    private static CancellationToken GetStoppingToken( IApplicationBuilder appBuilder ) {
        var applicationLifetime = appBuilder
            .ApplicationServices
            .GetRequiredService<IHostApplicationLifetime>();
        return applicationLifetime.ApplicationStopping;
    }
}