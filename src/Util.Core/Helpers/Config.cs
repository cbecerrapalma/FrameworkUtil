namespace Util.Helpers; 

/// <summary>
/// Clase estática que contiene configuraciones globales para la aplicación.
/// </summary>
public static class Config {
    private static IConfiguration _configuration;

    /// <summary>
    /// Establece la configuración proporcionada en el objeto estático.
    /// </summary>
    /// <param name="configuration">La instancia de <see cref="IConfiguration"/> que se va a establecer.</param>
    public static void SetConfiguration( IConfiguration configuration ) {
        _configuration = configuration;
    }

    /// <summary>
    /// Obtiene el valor asociado a una clave especificada.
    /// </summary>
    /// <param name="key">La clave para la cual se desea obtener el valor.</param>
    /// <returns>El valor asociado a la clave especificada como una cadena.</returns>
    public static string GetValue( string key ) {
        return GetValue<string>( key );
    }

    /// <summary>
    /// Obtiene un valor de configuración asociado a una clave específica.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se desea obtener.</typeparam>
    /// <param name="key">La clave de configuración para la que se desea obtener el valor.</param>
    /// <returns>El valor de configuración asociado a la clave especificada, convertido al tipo T.</returns>
    /// <remarks>
    /// Este método utiliza la configuración actual para recuperar el valor correspondiente a la clave proporcionada.
    /// Asegúrese de que la clave existe en la configuración, de lo contrario, se devolverá el valor predeterminado del tipo T.
    /// </remarks>
    /// <seealso cref="GetConfiguration"/>
    public static T GetValue<T>( string key ) {
        return GetConfiguration().GetValue<T>( key );
    }

    /// <summary>
    /// Obtiene una sección de configuración y la convierte en un objeto del tipo especificado.
    /// </summary>
    /// <typeparam name="TOptions">El tipo del objeto que se desea obtener a partir de la sección de configuración.</typeparam>
    /// <param name="section">El nombre de la sección de configuración que se desea obtener.</param>
    /// <returns>Un objeto del tipo <typeparamref name="TOptions"/> que representa la sección de configuración.</returns>
    /// <remarks>
    /// Este método busca la sección de configuración especificada y utiliza el método <c>Get</c> para convertirla en un objeto del tipo deseado.
    /// Asegúrese de que la sección de configuración esté correctamente definida en el archivo de configuración.
    /// </remarks>
    /// <seealso cref="GetSection(string)"/>
    public static TOptions Get<TOptions>( string section ) {
        return GetSection( section ).Get<TOptions>();
    }

    /// <summary>
    /// Obtiene una sección de configuración específica.
    /// </summary>
    /// <param name="section">El nombre de la sección de configuración que se desea obtener.</param>
    /// <returns>La sección de configuración solicitada.</returns>
    /// <remarks>
    /// Este método utiliza la configuración global para recuperar la sección especificada.
    /// Asegúrese de que la sección exista en la configuración antes de llamarlo.
    /// </remarks>
    /// <seealso cref="GetConfiguration"/>
    public static IConfigurationSection GetSection( string section ) {
        return GetConfiguration().GetSection( section );
    }

    /// <summary>
    /// Obtiene la configuración de la aplicación.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="IConfiguration"/> que representa la configuración actual.
    /// </returns>
    /// <remarks>
    /// Si la configuración ya ha sido inicializada, se devuelve la instancia existente.
    /// De lo contrario, se crea una nueva instancia de configuración.
    /// </remarks>
    private static IConfiguration GetConfiguration() {
        return _configuration ??= CreateConfiguration();
    }

    /// <summary>
    /// Crea una configuración de aplicación a partir de archivos JSON y variables de entorno.
    /// </summary>
    /// <param name="basePath">La ruta base donde se encuentran los archivos de configuración. Si es nula, se utilizará el directorio base de la aplicación.</param>
    /// <param name="jsonFiles">Una lista opcional de archivos JSON adicionales que se agregarán a la configuración.</param>
    /// <returns>Un objeto <see cref="IConfiguration"/> que contiene la configuración combinada.</returns>
    /// <remarks>
    /// Este método primero carga el archivo "appsettings.json" y, si existe, también carga un archivo específico del entorno, 
    /// seguido de las variables de entorno. Si se proporcionan archivos JSON adicionales, se agregarán a la configuración.
    /// </remarks>
    /// <seealso cref="IConfiguration"/>
    public static IConfiguration CreateConfiguration( string basePath = null,params string[] jsonFiles ) {
        basePath ??= Common.ApplicationBaseDirectory;
        var builder = new ConfigurationBuilder()
            .SetBasePath( basePath )
            .AddJsonFile( "appsettings.json", true, false );
        var environment = Environment.GetEnvironmentName();
        if ( environment.IsEmpty() == false )
            builder.AddJsonFile( $"appsettings.{environment}.json", true, false );
        builder.AddEnvironmentVariables();
        if ( jsonFiles == null )
            return builder.Build();
        foreach ( var file in jsonFiles ) 
            builder.AddJsonFile( file, true, false );
        return builder.Build();
    }

    /// <summary>
    /// Obtiene la cadena de conexión para el nombre especificado.
    /// </summary>
    /// <param name="name">El nombre de la cadena de conexión que se desea obtener.</param>
    /// <returns>La cadena de conexión correspondiente al nombre especificado.</returns>
    /// <remarks>
    /// Este método utiliza la configuración actual para recuperar la cadena de conexión.
    /// Asegúrese de que el nombre proporcionado exista en la configuración.
    /// </remarks>
    public static string GetConnectionString( string name ) {
        return GetConfiguration().GetConnectionString( name );
    }
}