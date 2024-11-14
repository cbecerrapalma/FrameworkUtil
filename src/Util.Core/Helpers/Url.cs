namespace Util.Helpers;

/// <summary>
/// Proporciona métodos estáticos para manipular y trabajar con URLs.
/// </summary>
public static class Url {
    /// <summary>
    /// Combina múltiples rutas en una sola ruta, manejando correctamente los esquemas HTTP y HTTPS,
    /// así como la normalización de las barras diagonales.
    /// </summary>
    /// <param name="paths">Un arreglo de cadenas que representan las rutas a combinar.</param>
    /// <returns>Una cadena que representa la ruta combinada. Si no se proporcionan rutas válidas, devuelve una cadena vacía.</returns>
    /// <remarks>
    /// Este método elimina las rutas vacías o que solo contienen espacios en blanco,
    /// y reemplaza las barras invertidas por barras diagonales. También maneja rutas relativas
    /// y absolutas, asegurando que el resultado sea una ruta válida.
    /// </remarks>
    /// <example>
    /// <code>
    /// string combinedPath = JoinPath("http://example.com", "folder", "file.txt");
    /// // Resultado: "http://example.com/folder/file.txt"
    /// </code>
    /// </example>
    /// <seealso cref="System.IO.Path"/>
    public static string JoinPath( params string[] paths ) {
        if ( paths == null )
            return string.Empty;
        paths = paths.Where( path => string.IsNullOrWhiteSpace( path ) == false ).Select( t => t.Replace( @"\", "/" ) ).ToArray();
        if ( paths.Length == 0 )
            return string.Empty;
        var firstPath = paths.First();
        var lastPath = paths.Last();
        string schema = string.Empty;
        if ( firstPath.StartsWith( "http:", StringComparison.OrdinalIgnoreCase ) )
            schema = "http://";
        if ( firstPath.StartsWith( "https:", StringComparison.OrdinalIgnoreCase ) )
            schema = "https://";
        paths = paths.Select( t => t.Trim( '/' ) ).ToArray();
        var result = Path.Combine( paths ).Replace( @"\", "/" );
        if ( paths.Any( path => path.StartsWith( "." ) ) ) {
            result = Path.GetFullPath( Path.Combine( paths ) );
            result = result.RemoveStart( AppContext.BaseDirectory ).Replace( @"\", "/" );
        }
        if ( firstPath.StartsWith( '/' ) )
            result = $"/{result}";
        if ( lastPath.EndsWith( '/' ) )
            result = $"{result}/";
        result = result.RemoveStart( "http:/" ).RemoveStart( "https:/" );
        if (schema.IsEmpty())
            return result;
        return schema + result.RemoveStart( "/" );
    }
}