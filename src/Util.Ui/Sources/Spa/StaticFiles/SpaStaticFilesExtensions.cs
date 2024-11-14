// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.FileProviders;

namespace Util.Ui.Sources.Spa.StaticFiles;

/// <summary>
/// Proporciona métodos de extensión para la configuración de archivos estáticos en aplicaciones SPA (Single Page Application).
/// </summary>
public static class SpaStaticFilesExtensions
{
    /// <summary>
    /// Agrega un proveedor de archivos estáticos para aplicaciones de una sola página (SPA) al contenedor de servicios.
    /// </summary>
    /// <param name="services">La colección de servicios donde se registrará el proveedor de archivos estáticos.</param>
    /// <param name="configuration">Una acción opcional para configurar las opciones de archivos estáticos de la SPA.</param>
    /// <remarks>
    /// Este método permite a los desarrolladores configurar opciones adicionales para el proveedor de archivos estáticos
    /// mediante el parámetro de configuración. Si no se establece la ruta raíz en las opciones, se lanzará una excepción.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Se lanza si la propiedad <see cref="SpaStaticFilesOptions.RootPath"/> no está configurada.
    /// </exception>
    /// <seealso cref="SpaStaticFilesOptions"/>
    /// <seealso cref="ISpaStaticFileProvider"/>
    public static void AddSpaStaticFiles(
        this IServiceCollection services,
        Action<SpaStaticFilesOptions> configuration = null)
    {
        services.AddSingleton<ISpaStaticFileProvider>(serviceProvider =>
        {
            // Use the options configured in DI (or blank if none was configured)
            var optionsProvider = serviceProvider.GetService<IOptions<SpaStaticFilesOptions>>()!;
            var options = optionsProvider.Value;

            // Allow the developer to perform further configuration
            configuration?.Invoke(options);

            if (string.IsNullOrEmpty(options.RootPath))
            {
                throw new InvalidOperationException($"No {nameof(SpaStaticFilesOptions.RootPath)} " +
                    $"was set on the {nameof(SpaStaticFilesOptions)}.");
            }

            return new DefaultSpaStaticFileProvider(serviceProvider, options);
        });
    }

    /// <summary>
    /// Extiende la funcionalidad de <see cref="IApplicationBuilder"/> para habilitar el uso de archivos estáticos de una aplicación SPA (Single Page Application).
    /// </summary>
    /// <param name="applicationBuilder">El <see cref="IApplicationBuilder"/> que se está configurando.</param>
    /// <remarks>
    /// Este método proporciona una forma conveniente de configurar los archivos estáticos para una aplicación de una sola página, utilizando las opciones predeterminadas.
    /// Para configuraciones personalizadas, se puede utilizar el método sobrecargado que acepta <see cref="StaticFileOptions"/>.
    /// </remarks>
    public static void UseSpaStaticFiles(this IApplicationBuilder applicationBuilder)
    {
        UseSpaStaticFiles(applicationBuilder, new StaticFileOptions());
    }

    /// <summary>
    /// Extiende la funcionalidad de <see cref="IApplicationBuilder"/> para utilizar archivos estáticos de una aplicación SPA (Single Page Application).
    /// </summary>
    /// <param name="applicationBuilder">El <see cref="IApplicationBuilder"/> que se está configurando.</param>
    /// <param name="options">Las opciones de configuración para los archivos estáticos.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="applicationBuilder"/> o <paramref name="options"/> son nulos.</exception>
    /// <remarks>
    /// Este método permite servir archivos estáticos desde el directorio raíz de la aplicación, 
    /// configurando las opciones necesarias para su correcto funcionamiento.
    /// </remarks>
    /// <seealso cref="StaticFileOptions"/>
    public static void UseSpaStaticFiles(this IApplicationBuilder applicationBuilder, StaticFileOptions options)
    {
        ArgumentNullException.ThrowIfNull(applicationBuilder);
        ArgumentNullException.ThrowIfNull(options);

        UseSpaStaticFilesInternal(applicationBuilder,
            staticFileOptions: options,
            allowFallbackOnServingWebRootFiles: false);
    }

    /// <summary>
    /// Configura el uso de archivos estáticos para aplicaciones de una sola página (SPA).
    /// </summary>
    /// <param name="app">La instancia de <see cref="IApplicationBuilder"/> que se está configurando.</param>
    /// <param name="staticFileOptions">Opciones de configuración para los archivos estáticos.</param>
    /// <param name="allowFallbackOnServingWebRootFiles">Indica si se permite la recuperación de archivos del directorio raíz web.</param>
    /// <remarks>
    /// Este método permite que las aplicaciones que utilizan múltiples SPAs sirvan sus archivos estáticos de manera adecuada.
    /// Si se proporciona un proveedor de archivos, este tendrá prioridad sobre cualquier otro configurado.
    /// Si no se proporciona un proveedor, se intentará obtener uno de la configuración de inyección de dependencias.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="staticFileOptions"/> es nulo.</exception>
    /// <seealso cref="StaticFileOptions"/>
    /// <seealso cref="IApplicationBuilder"/>
    internal static void UseSpaStaticFilesInternal(
        this IApplicationBuilder app,
        StaticFileOptions staticFileOptions,
        bool allowFallbackOnServingWebRootFiles)
    {
        ArgumentNullException.ThrowIfNull(staticFileOptions);

        // If the file provider was explicitly supplied, that takes precedence over any other
        // configured file provider. This is most useful if the application hosts multiple SPAs
        // (via multiple calls to UseSpa()), so each needs to serve its own separate static files
        // instead of using AddSpaStaticFiles/UseSpaStaticFiles.
        // But if no file provider was specified, try to get one from the DI config.
        if (staticFileOptions.FileProvider == null)
        {
            var shouldServeStaticFiles = ShouldServeStaticFiles(
                app,
                allowFallbackOnServingWebRootFiles,
                out var fileProviderOrDefault);
            if (shouldServeStaticFiles)
            {
                staticFileOptions.FileProvider = fileProviderOrDefault;
            }
            else
            {
                // The registered ISpaStaticFileProvider says we shouldn't
                // serve static files
                return;
            }
        }

        app.UseStaticFiles(staticFileOptions);
    }

    /// <summary>
    /// Determina si se deben servir archivos estáticos en la aplicación.
    /// </summary>
    /// <param name="app">La instancia de <see cref="IApplicationBuilder"/> que se está configurando.</param>
    /// <param name="allowFallbackOnServingWebRootFiles">Indica si se permite la opción de servir archivos estáticos desde el directorio raíz de la aplicación.</param>
    /// <param name="fileProviderOrDefault">Proporciona el <see cref="IFileProvider"/> que se utilizará para servir los archivos estáticos, o null si no se debe servir ninguno.</param>
    /// <returns>Devuelve true si se deben servir archivos estáticos; de lo contrario, false.</returns>
    /// <remarks>
    /// Si se ha configurado un <see cref="ISpaStaticFileProvider"/> y este no proporciona un <see cref="IFileProvider"/>, 
    /// se asume que no se deben servir archivos estáticos. Esto es común en entornos de desarrollo 
    /// donde los archivos estáticos de una SPA se sirven desde un servidor de desarrollo.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Se lanza si se intenta usar archivos estáticos de SPA sin haber registrado un <see cref="ISpaStaticFileProvider"/> 
    /// en el proveedor de servicios y no se permite la opción de servir archivos desde el directorio raíz.
    /// </exception>
    private static bool ShouldServeStaticFiles(
        IApplicationBuilder app,
        bool allowFallbackOnServingWebRootFiles,
        out IFileProvider fileProviderOrDefault)
    {
        var spaStaticFilesService = app.ApplicationServices.GetService<ISpaStaticFileProvider>();
        if (spaStaticFilesService != null)
        {
            // If an ISpaStaticFileProvider was configured but it says no IFileProvider is available
            // (i.e., it supplies 'null'), this implies we should not serve any static files. This
            // is typically the case in development when SPA static files are being served from a
            // SPA development server (e.g., Angular CLI or create-react-app), in which case no
            // directory of prebuilt files will exist on disk.
            fileProviderOrDefault = spaStaticFilesService.FileProvider;
            return fileProviderOrDefault != null;
        }
        else if (!allowFallbackOnServingWebRootFiles)
        {
            throw new InvalidOperationException($"To use {nameof(UseSpaStaticFiles)}, you must " +
                $"first register an {nameof(ISpaStaticFileProvider)} in the service provider, typically " +
                $"by calling services.{nameof(AddSpaStaticFiles)}.");
        }
        else
        {
            // Fall back on serving wwwroot
            fileProviderOrDefault = null;
            return true;
        }
    }
}