// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Util.Ui.Sources.Spa;

/// <summary>
/// Representa un constructor de aplicaciones de una sola página (SPA) por defecto.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ISpaBuilder"/> y proporciona 
/// la funcionalidad necesaria para configurar y construir una aplicación de 
/// una sola página.
/// </remarks>
internal sealed class DefaultSpaBuilder : ISpaBuilder
{
    /// <summary>
    /// Obtiene el objeto <see cref="IApplicationBuilder"/> asociado.
    /// </summary>
    /// <remarks>
    /// Este objeto se utiliza para configurar la tubería de solicitudes de la aplicación.
    /// </remarks>
    /// <returns>
    /// Un objeto <see cref="IApplicationBuilder"/> que permite la configuración de la aplicación.
    /// </returns>
    public IApplicationBuilder ApplicationBuilder { get; }

    /// <summary>
    /// Obtiene las opciones de configuración para el spa.
    /// </summary>
    /// <value>
    /// Un objeto <see cref="SpaOptions"/> que contiene las opciones de configuración.
    /// </value>
    public SpaOptions Options { get; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DefaultSpaBuilder"/>.
    /// </summary>
    /// <param name="applicationBuilder">El objeto <see cref="IApplicationBuilder"/> que se utilizará para construir la aplicación.</param>
    /// <param name="options">Las opciones de configuración para el SPA, representadas por un objeto <see cref="SpaOptions"/>.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="applicationBuilder"/> o <paramref name="options"/> son nulos.</exception>
    public DefaultSpaBuilder(IApplicationBuilder applicationBuilder, SpaOptions options)
    {
        ApplicationBuilder = applicationBuilder
            ?? throw new ArgumentNullException(nameof(applicationBuilder));

        Options = options
            ?? throw new ArgumentNullException(nameof(options));
    }
}