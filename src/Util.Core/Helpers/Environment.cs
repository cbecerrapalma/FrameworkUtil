namespace Util.Helpers; 

/// <summary>
/// Proporciona información sobre el entorno de ejecución de la aplicación.
/// </summary>
public static class Environment {
    private const string DOTNET_ENVIRONMENT = "DOTNET_ENVIRONMENT";
    private const string ASPNETCORE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";
    private const string Development = "Development";
    /// <summary>
    /// Obtiene una cadena que representa el salto de línea del entorno actual.
    /// </summary>
    /// <remarks>
    /// Esta propiedad estática proporciona una forma de acceder al carácter de salto de línea específico del sistema operativo,
    /// lo que permite que las aplicaciones manejen correctamente los saltos de línea en diferentes plataformas.
    /// </remarks>
    /// <returns>
    /// Una cadena que contiene el carácter de salto de línea del entorno actual.
    /// </returns>
    public static string NewLine => System.Environment.NewLine;
    /// <summary>
    /// Obtiene o establece un valor que indica si se está en modo de prueba.
    /// </summary>
    /// <value>
    /// <c>true</c> si está en modo de prueba; de lo contrario, <c>false</c>.
    /// </value>
    public static bool IsTest { get; set; }

    /// <summary>
    /// Establece una variable de entorno con el nombre y valor especificados.
    /// </summary>
    /// <param name="name">El nombre de la variable de entorno que se va a establecer.</param>
    /// <param name="value">El valor que se asignará a la variable de entorno. Se convertirá a una cadena segura.</param>
    /// <remarks>
    /// Este método utiliza el método <see cref="System.Environment.SetEnvironmentVariable"/> 
    /// para establecer la variable de entorno. El valor se convierte a una cadena utilizando 
    /// el método <see cref="SafeString"/> para asegurar que no se produzcan errores de conversión.
    /// </remarks>
    public static void SetEnvironmentVariable( string name, object value ) {
        System.Environment.SetEnvironmentVariable( name, value.SafeString() );
    }

    /// <summary>
    /// Obtiene el valor de una variable de entorno especificada.
    /// </summary>
    /// <param name="name">El nombre de la variable de entorno que se desea obtener.</param>
    /// <returns>
    /// El valor de la variable de entorno especificada, o null si la variable no existe.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="System.Environment"/> para acceder a las variables de entorno del sistema.
    /// </remarks>
    public static string GetEnvironmentVariable( string name ) {
        return System.Environment.GetEnvironmentVariable( name );
    }

    /// <summary>
    /// Obtiene el valor de una variable de entorno y lo convierte al tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo al que se desea convertir el valor de la variable de entorno.</typeparam>
    /// <param name="name">El nombre de la variable de entorno que se desea obtener.</param>
    /// <returns>El valor de la variable de entorno convertido al tipo especificado.</returns>
    /// <remarks>
    /// Este método utiliza la función <see cref="GetEnvironmentVariable(string)"/> para obtener el valor de la variable de entorno
    /// y luego lo convierte utilizando <see cref="Convert.To{T}(object)"/>.
    /// </remarks>
    /// <seealso cref="GetEnvironmentVariable(string)"/>
    /// <seealso cref="Convert"/>
    public static T GetEnvironmentVariable<T>( string name ) {
        return Convert.To<T>( GetEnvironmentVariable( name ) );
    }

    /// <summary>
    /// Obtiene el nombre del entorno de la aplicación.
    /// </summary>
    /// <returns>
    /// Devuelve el nombre del entorno configurado, que puede ser el valor de la variable de entorno 
    /// <c>ASPNETCORE_ENVIRONMENT</c> o, si no está configurado, el valor de 
    /// <c>DOTNET_ENVIRONMENT</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica primero si la variable de entorno <c>ASPNETCORE_ENVIRONMENT</c> 
    /// está configurada. Si no lo está, intenta obtener el valor de <c>DOTNET_ENVIRONMENT</c>.
    /// </remarks>
    public static string GetEnvironmentName() {
        var environment = GetEnvironmentVariable( ASPNETCORE_ENVIRONMENT );
        if ( environment.IsEmpty() == false )
            return environment;
        return GetEnvironmentVariable( DOTNET_ENVIRONMENT );
    }

    /// <summary>
    /// Establece el entorno de desarrollo para la aplicación si no se ha definido previamente.
    /// </summary>
    /// <remarks>
    /// Este método verifica las variables de entorno <c>DOTNET_ENVIRONMENT</c> y <c>ASPNETCORE_ENVIRONMENT</c>.
    /// Si ambas están vacías, se configuran a "Development".
    /// </remarks>
    public static void SetDevelopment() {
        var environment = GetEnvironmentVariable( DOTNET_ENVIRONMENT );
        if ( environment.IsEmpty() == false )
            return;
        environment = GetEnvironmentVariable( ASPNETCORE_ENVIRONMENT );
        if ( environment.IsEmpty() == false )
            return;
        SetEnvironmentVariable( DOTNET_ENVIRONMENT, Development );
        SetEnvironmentVariable( ASPNETCORE_ENVIRONMENT, Development );
    }

    /// <summary>
    /// Determina si la aplicación se está ejecutando en un entorno de desarrollo.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si la aplicación está en un entorno de desarrollo; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Esta función verifica las variables de entorno <c>DOTNET_ENVIRONMENT</c> y <c>ASPNETCORE_ENVIRONMENT</c>
    /// para determinar si alguna de ellas está configurada como "Development".
    /// </remarks>
    public static bool IsDevelopment() {
	    var environment = GetEnvironmentVariable( DOTNET_ENVIRONMENT );
	    if ( environment == Development )
		    return true;
	    environment = GetEnvironmentVariable( ASPNETCORE_ENVIRONMENT );
	    if ( environment == Development )
		    return true;
	    return false;
    }
}