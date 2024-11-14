// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.FileProviders;

namespace Util.Ui.Sources.Spa.StaticFiles;

/// <summary>
/// Proporciona archivos est�ticos para aplicaciones de una sola p�gina (SPA).
/// Esta clase es sellada y no puede ser heredada.
/// </summary>
internal sealed class DefaultSpaStaticFileProvider : ISpaStaticFileProvider
{
    private readonly IFileProvider _fileProvider;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DefaultSpaStaticFileProvider"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizar� para obtener el entorno de hospedaje.</param>
    /// <param name="options">Las opciones de configuraci�n para los archivos est�ticos de la SPA.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="options"/> es null.</exception>
    /// <exception cref="ArgumentException">Se lanza si la propiedad <paramref name="options.RootPath"/> es null o est� vac�a.</exception>
    /// <remarks>
    /// Este constructor verifica que las opciones no sean nulas y que la ruta ra�z no est� vac�a. 
    /// Si la ruta ra�z es v�lida y existe, se inicializa un <see cref="PhysicalFileProvider"/> 
    /// para servir archivos est�ticos desde esa ubicaci�n.
    /// </remarks>
    public DefaultSpaStaticFileProvider(
        IServiceProvider serviceProvider,
        SpaStaticFilesOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (string.IsNullOrEmpty(options.RootPath))
        {
            throw new ArgumentException($"The {nameof(options.RootPath)} property " +
                $"of {nameof(options)} cannot be null or empty.");
        }

        var env = serviceProvider.GetRequiredService<IWebHostEnvironment>();
        var absoluteRootPath = Path.Combine(
            env.ContentRootPath,
            options.RootPath);

        // PhysicalFileProvider will throw if you pass a non-existent path,
        // but we don't want that scenario to be an error because for SPA
        // scenarios, it's better if non-existing directory just means we
        // don't serve any static files.
        if (Directory.Exists(absoluteRootPath))
        {
            _fileProvider = new PhysicalFileProvider(absoluteRootPath);
        }
    }

    /// <summary>
    /// Obtiene el proveedor de archivos utilizado por la clase.
    /// </summary>
    /// <value>
    /// Un objeto que implementa <see cref="IFileProvider"/> que proporciona acceso a los archivos.
    /// </value>
    public IFileProvider FileProvider => _fileProvider;
}