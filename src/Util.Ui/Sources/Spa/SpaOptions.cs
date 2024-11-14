// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Util.Ui.Sources.Spa;

/// <summary>
/// Representa las opciones de configuración para una aplicación de una sola página (SPA).
/// </summary>
public class SpaOptions
{
    private PathString _defaultPage = "/index.html";
    private string _packageManagerCommand = "npm";

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SpaOptions"/>.
    /// </summary>
    /// <remarks>
    /// Esta clase se utiliza para configurar opciones específicas para el spa.
    /// </remarks>
    public SpaOptions()
    {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SpaOptions"/> 
    /// copiando las opciones de otra instancia proporcionada.
    /// </summary>
    /// <param name="copyFromOptions">La instancia de <see cref="SpaOptions"/> 
    /// desde la cual se copiarán las opciones.</param>
    internal SpaOptions(SpaOptions copyFromOptions)
    {
        _defaultPage = copyFromOptions.DefaultPage;
        _packageManagerCommand = copyFromOptions.PackageManagerCommand;
        DefaultPageStaticFileOptions = copyFromOptions.DefaultPageStaticFileOptions;
        SourcePath = copyFromOptions.SourcePath;
        DevServerPort = copyFromOptions.DevServerPort;
    }

    /// <summary>
    /// Obtiene o establece la página predeterminada.
    /// </summary>
    /// <value>
    /// Un objeto <see cref="PathString"/> que representa la página predeterminada.
    /// </value>
    /// <exception cref="ArgumentException">
    /// Se lanza cuando se intenta establecer un valor nulo o vacío para la propiedad <see cref="DefaultPage"/>.
    /// </exception>
    public PathString DefaultPage
    {
        get => _defaultPage;
        set
        {
            if (string.IsNullOrEmpty(value.Value))
            {
                throw new ArgumentException($"El valor para {nameof(DefaultPage)} no puede ser nulo o vacío.");
            }

            _defaultPage = value;
        }
    }

    /// <summary>
    /// Obtiene o establece las opciones de archivo estático para la página predeterminada.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite configurar las opciones relacionadas con los archivos estáticos
    /// que se sirven cuando se accede a la página predeterminada de la aplicación.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="StaticFileOptions"/> que contiene las configuraciones de archivos estáticos.
    /// </value>
    public StaticFileOptions DefaultPageStaticFileOptions { get; set; }

    /// <summary>
    /// Obtiene o establece la ruta de origen.
    /// </summary>
    /// <value>
    /// La ruta de origen como una cadena.
    /// </value>
    public string SourcePath { get; set; }

    /// <summary>
    /// Obtiene o establece el puerto del servidor de desarrollo.
    /// </summary>
    /// <value>
    /// Un entero que representa el puerto en el que se ejecuta el servidor de desarrollo.
    /// </value>
    public int DevServerPort { get; set; }

    /// <summary>
    /// Obtiene o establece el comando del gestor de paquetes.
    /// </summary>
    /// <value>
    /// El comando del gestor de paquetes.
    /// </value>
    /// <exception cref="ArgumentException">
    /// Se lanza cuando el valor establecido es nulo o vacío.
    /// </exception>
    public string PackageManagerCommand
    {
        get => _packageManagerCommand;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"El valor para {nameof(PackageManagerCommand)} no puede ser nulo o vacío.");
            }

            _packageManagerCommand = value;
        }
    }

    /// <summary>
    /// Obtiene o establece el tiempo de espera para el inicio.
    /// </summary>
    /// <remarks>
    /// Este valor determina cuánto tiempo se esperará antes de considerar que el inicio ha fallado.
    /// El valor predeterminado es de 120 segundos.
    /// </remarks>
    /// <returns>
    /// Un objeto <see cref="TimeSpan"/> que representa el tiempo de espera para el inicio.
    /// </returns>
    public TimeSpan StartupTimeout { get; set; } = TimeSpan.FromSeconds(120);
}