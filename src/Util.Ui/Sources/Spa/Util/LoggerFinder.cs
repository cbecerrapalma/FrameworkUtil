// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Util.Ui.Sources.Spa.Util;

/// <summary>
/// Clase estática que proporciona métodos para encontrar y gestionar instancias de registradores (loggers).
/// </summary>
internal static class LoggerFinder
{
    /// <summary>
    /// Obtiene o crea un registrador de logs (logger) basado en el constructor de la aplicación.
    /// </summary>
    /// <param name="appBuilder">El constructor de la aplicación que proporciona acceso a los servicios de la aplicación.</param>
    /// <param name="logCategoryName">El nombre de la categoría de logs que se utilizará para el registrador.</param>
    /// <returns>Un objeto <see cref="ILogger"/> que se puede utilizar para registrar información de logs.</returns>
    /// <remarks>
    /// Si el sistema de inyección de dependencias proporciona un registrador, se utilizará. 
    /// De lo contrario, se configurará un registrador por defecto que no registra nada.
    /// </remarks>
    public static ILogger GetOrCreateLogger(
        IApplicationBuilder appBuilder,
        string logCategoryName)
    {
        // If the DI system gives us a logger, use it. Otherwise, set up a default one
        var loggerFactory = appBuilder.ApplicationServices.GetService<ILoggerFactory>();
        var logger = loggerFactory != null
            ? loggerFactory.CreateLogger(logCategoryName)
            : NullLogger.Instance;
        return logger;
    }
}