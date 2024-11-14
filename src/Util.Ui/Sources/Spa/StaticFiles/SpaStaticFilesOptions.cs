// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Util.Ui.Sources.Spa.StaticFiles;

/// <summary>
/// Representa las opciones de configuraci�n para los archivos est�ticos en una aplicaci�n SPA (Single Page Application).
/// </summary>
public class SpaStaticFilesOptions
{
    /// <summary>
    /// Obtiene o establece la ruta ra�z.
    /// </summary>
    /// <remarks>
    /// Esta propiedad almacena la ruta ra�z como una cadena. 
    /// Se inicializa con un valor predeterminado que no es nulo.
    /// </remarks>
    /// <value>
    /// Una cadena que representa la ruta ra�z.
    /// </value>
    public string RootPath { get; set; } = default!;
}