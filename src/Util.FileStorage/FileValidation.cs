namespace Util.FileStorage;

/// <summary>
/// Proporciona métodos para la validación de archivos.
/// </summary>
public static class FileValidation {
    /// <summary>
    /// Verifica si la extensión de un archivo es válida según una lista de extensiones aceptadas.
    /// </summary>
    /// <param name="fileName">El nombre del archivo que se va a verificar.</param>
    /// <param name="accepts">Una cadena que contiene las extensiones aceptadas, separadas por comas.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la extensión del archivo es válida o si no se proporcionaron extensiones aceptadas; 
    /// de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Si el nombre del archivo está vacío, la función devolverá <c>false</c>. 
    /// Si la lista de extensiones aceptadas está vacía, la función devolverá <c>true</c>.
    /// </remarks>
    public static bool IsValidExtension( string fileName, string accepts ) {
        if( fileName.IsEmpty() )
            return false;
        if( accepts.IsEmpty() )
            return true;
        var extension = Path.GetExtension( fileName );
        var list = accepts.Split( ',' ).Where( t => t.IsEmpty() == false ).ToList();
        if ( list.Count == 0 )
            return true;
        return list.Any( type => type.TrimStart( '.' ) == extension.TrimStart( '.' ) );
    }
}