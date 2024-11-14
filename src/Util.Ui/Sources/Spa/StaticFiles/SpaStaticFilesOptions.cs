// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Util.Ui.Sources.Spa.StaticFiles;

/// <summary>
/// Representa las opciones de configuración para los archivos estáticos en una aplicación SPA (Single Page Application).
/// </summary>
public class SpaStaticFilesOptions
{
    /// <summary>
    /// Obtiene o establece la ruta raíz.
    /// </summary>
    /// <remarks>
    /// Esta propiedad almacena la ruta raíz como una cadena. 
    /// Se inicializa con un valor predeterminado que no es nulo.
    /// </remarks>
    /// <value>
    /// Una cadena que representa la ruta raíz.
    /// </value>
    public string RootPath { get; set; } = default!;
}