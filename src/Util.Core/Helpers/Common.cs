namespace Util.Helpers; 

/// <summary>
/// Proporciona métodos y propiedades comunes que pueden ser utilizados en toda la aplicación.
/// </summary>
public static class Common {
    /// <summary>
    /// Obtiene el directorio base de la aplicación.
    /// </summary>
    /// <value>
    /// Un <see cref="string"/> que representa el directorio base donde se está ejecutando la aplicación.
    /// </value>
    /// <remarks>
    /// Este valor se obtiene a través de <see cref="AppContext.BaseDirectory"/> y puede ser útil para localizar recursos
    /// o archivos de configuración que se encuentran en el mismo directorio que la aplicación.
    /// </remarks>
    public static string ApplicationBaseDirectory => AppContext.BaseDirectory;
    /// <summary>
    /// Obtiene una cadena que representa el salto de línea del entorno actual.
    /// </summary>
    /// <remarks>
    /// Esta propiedad estática devuelve el valor de <c>System.Environment.NewLine</c>,
    /// que es el carácter de nueva línea utilizado por el sistema operativo en el que se está ejecutando la aplicación.
    /// </remarks>
    /// <returns>
    /// Una cadena que representa el salto de línea.
    /// </returns>
    public static string Line => System.Environment.NewLine;
    /// <summary>
    /// Obtiene un valor que indica si el sistema operativo actual es Linux.
    /// </summary>
    /// <value>
    /// <c>true</c> si el sistema operativo es Linux; de lo contrario, <c>false</c>.
    /// </value>
    /// <remarks>
    /// Esta propiedad utiliza la clase <see cref="RuntimeInformation"/> para determinar la plataforma actual.
    /// </remarks>
    public static bool IsLinux => RuntimeInformation.IsOSPlatform( OSPlatform.Linux );
    /// <summary>
    /// Obtiene un valor que indica si el sistema operativo actual es Windows.
    /// </summary>
    /// <remarks>
    /// Esta propiedad utiliza la clase <see cref="RuntimeInformation"/> para determinar si la plataforma actual es Windows.
    /// </remarks>
    /// <returns>
    /// Devuelve <c>true</c> si el sistema operativo es Windows; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    public static bool IsWindows => RuntimeInformation.IsOSPlatform( OSPlatform.Windows );

    /// <summary>
    /// Obtiene el tipo de un parámetro genérico especificado.
    /// </summary>
    /// <typeparam name="T">El tipo del parámetro genérico.</typeparam>
    /// <returns>El tipo correspondiente al parámetro genérico <typeparamref name="T"/>.</returns>
    public static Type GetType<T>() {
        return GetType( typeof( T ) );
    }

    /// <summary>
    /// Obtiene el tipo subyacente de un tipo nullable o el tipo mismo si no es nullable.
    /// </summary>
    /// <param name="type">El tipo del cual se desea obtener el tipo subyacente.</param>
    /// <returns>El tipo subyacente si el tipo es nullable; de lo contrario, el tipo original.</returns>
    public static Type GetType( Type type ) {
        return Nullable.GetUnderlyingType( type ) ?? type;
    }

    /// <summary>
    /// Obtiene la ruta física correspondiente a una ruta relativa, 
    /// combinándola con una ruta base opcional.
    /// </summary>
    /// <param name="relativePath">La ruta relativa que se desea convertir a una ruta física.</param>
    /// <param name="basePath">La ruta base con la que se combinará la ruta relativa. 
    /// Si no se proporciona, se utilizará el directorio base de la aplicación.</param>
    /// <returns>La ruta física resultante después de combinar la ruta base y la ruta relativa.</returns>
    /// <remarks>
    /// Este método elimina cualquier prefijo de tilde (~), barra diagonal (/) 
    /// o barra invertida (\) de la ruta relativa antes de combinarla con la 
    /// ruta base. Esto es útil para normalizar rutas que pueden provenir 
    /// de diferentes fuentes.
    /// </remarks>
    /// <seealso cref="System.IO.Path"/>
    public static string GetPhysicalPath( string relativePath, string basePath = null ) {
        if ( relativePath.StartsWith( "~" ) )
            relativePath = relativePath.TrimStart( '~' );
        if ( relativePath.StartsWith( "/" ) )
            relativePath = relativePath.TrimStart( '/' );
        if ( relativePath.StartsWith( "\\" ) )
            relativePath = relativePath.TrimStart( '\\' );
        basePath ??= ApplicationBaseDirectory;
        return Path.Combine( basePath, relativePath );
    }

    /// <summary>
    /// Combina múltiples rutas en una sola ruta.
    /// </summary>
    /// <param name="paths">Un arreglo de cadenas que representan las rutas a combinar.</param>
    /// <returns>
    /// Una cadena que representa la ruta combinada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la función <see cref="Url.JoinPath"/> para realizar la combinación de las rutas.
    /// </remarks>
    public static string JoinPath( params string[] paths ) {
        return Url.JoinPath( paths );
    }

    /// <summary>
    /// Obtiene el directorio de trabajo actual del proceso.
    /// </summary>
    /// <returns>
    /// Una cadena que representa la ruta del directorio de trabajo actual.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="System.IO.Directory"/> para acceder al directorio actual.
    /// </remarks>
    public static string GetCurrentDirectory() {
        return Directory.GetCurrentDirectory();
    }

    /// <summary>
    /// Obtiene el directorio padre de la ruta actual, subiendo un número específico de niveles.
    /// </summary>
    /// <param name="depth">El número de niveles a subir en la jerarquía de directorios. Por defecto es 1.</param>
    /// <returns>La ruta completa del directorio padre correspondiente al nivel especificado.</returns>
    /// <remarks>
    /// Si el directorio padre no existe en el nivel especificado, se devolverá la ruta del directorio existente más cercano.
    /// </remarks>
    /// <example>
    /// <code>
    /// string parentDirectory = GetParentDirectory(2);
    /// </code>
    /// </example>
    public static string GetParentDirectory( int depth = 1 ) {
        var path = Directory.GetCurrentDirectory();
        for ( int i = 0; i < depth; i++ ) {
            var parent = Directory.GetParent( path );
            if ( parent is { Exists: true } )
                path = parent.FullName;
        }
        return path;
    }
}