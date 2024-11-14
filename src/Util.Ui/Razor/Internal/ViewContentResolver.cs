using Util.Helpers;
using File = Util.Helpers.File;

namespace Util.Ui.Razor.Internal;

/// <summary>
/// Clase que implementa la interfaz <see cref="IViewContentResolver"/>.
/// Proporciona métodos para resolver contenido de vista en la aplicación.
/// </summary>
public class ViewContentResolver : IViewContentResolver {
    /// <inheritdoc />
    /// <summary>
    /// Resuelve la ruta especificada y devuelve el contenido del archivo como una cadena.
    /// </summary>
    /// <param name="path">La ruta del archivo que se desea resolver.</param>
    /// <returns>
    /// El contenido del archivo como una cadena si se encuentra y se puede leer; de lo contrario, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Si la ruta está vacía, se devuelve <c>null</c>. Si el archivo no existe en la ruta proporcionada,
    /// se intenta construir la ruta utilizando el directorio actual.
    /// </remarks>
    /// <seealso cref="File.FileExists(string)"/>
    /// <seealso cref="File.ReadToString(string)"/>
    public string Resolve( string path ) {
        if ( path.IsEmpty() )
            return null;
        if ( File.FileExists( path ) == false )
            path = Url.JoinPath( Util.Helpers.Common.GetCurrentDirectory(), path );
        return File.ReadToString( path );
    }
}