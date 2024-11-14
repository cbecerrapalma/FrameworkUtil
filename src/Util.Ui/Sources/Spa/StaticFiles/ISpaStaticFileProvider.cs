// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.FileProviders;

namespace Util.Ui.Sources.Spa.StaticFiles;

/// <summary>
/// Define un contrato para un proveedor de archivos estáticos en una aplicación SPA (Single Page Application).
/// </summary>
public interface ISpaStaticFileProvider
{
    /// <summary>
    /// Obtiene una instancia del proveedor de archivos.
    /// </summary>
    /// <value>
    /// Un objeto que implementa <see cref="IFileProvider"/>.
    /// </value>
    IFileProvider FileProvider { get; }
}