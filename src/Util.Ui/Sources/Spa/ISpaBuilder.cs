// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Util.Ui.Sources.Spa;

/// <summary>
/// Define un contrato para construir aplicaciones de una sola página (SPA).
/// </summary>
public interface ISpaBuilder
{
    /// <summary>
    /// Obtiene una instancia de <see cref="IApplicationBuilder"/>.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite acceder al constructor de la aplicación, 
    /// que se utiliza para configurar la canalización de solicitudes HTTP.
    /// </remarks>
    /// <returns>
    /// Una instancia de <see cref="IApplicationBuilder"/> que se puede utilizar 
    /// para agregar middleware a la aplicación.
    /// </returns>
    IApplicationBuilder ApplicationBuilder { get; }

    /// <summary>
    /// Obtiene las opciones de configuración para el middleware de SPA (Single Page Application).
    /// </summary>
    /// <value>
    /// Un objeto <see cref="SpaOptions"/> que contiene las configuraciones específicas para la aplicación de una sola página.
    /// </value>
    SpaOptions Options { get; }
}