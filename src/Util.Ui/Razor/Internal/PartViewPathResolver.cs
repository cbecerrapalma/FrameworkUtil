using System.Text.RegularExpressions;
using Regex = Util.Helpers.Regex;

namespace Util.Ui.Razor.Internal;

/// <summary>
/// Clase que implementa la interfaz <see cref="IPartViewPathResolver"/>.
/// Proporciona métodos para resolver rutas de vistas de partes.
/// </summary>
public class PartViewPathResolver : IPartViewPathResolver {
    private readonly IPartViewPathFinder _pathFinder;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PartViewPathResolver"/>.
    /// </summary>
    /// <param name="pathFinder">Una instancia de <see cref="IPartViewPathFinder"/> que se utilizará para encontrar rutas de vistas.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="pathFinder"/> es <c>null</c>.</exception>
    public PartViewPathResolver( IPartViewPathFinder pathFinder ) {
        _pathFinder = pathFinder ?? throw new ArgumentNullException( nameof( pathFinder ) );
    }

    /// <inheritdoc />
    /// <summary>
    /// Resuelve las rutas de los componentes parciales especificados en el contenido dado.
    /// </summary>
    /// <param name="path">La ruta base desde la cual se resolverán los componentes parciales.</param>
    /// <param name="content">El contenido que contiene las definiciones de los componentes parciales.</param>
    /// <returns>
    /// Una lista de cadenas que representan las rutas resueltas de los componentes parciales.
    /// Si la ruta o el contenido están vacíos, se devuelve una lista vacía.
    /// </returns>
    /// <remarks>
    /// Este método utiliza expresiones regulares para buscar las definiciones de los componentes parciales
    /// en el contenido proporcionado. Se espera que las definiciones sigan el formato:
    /// <partial name="nombreDelComponente" />
    /// </remarks>
    /// <seealso cref="Regex"/>
    /// <seealso cref="_pathFinder"/>
    public List<string> Resolve( string path, string content ) {
        var result = new List<string>();
        if ( path.IsEmpty() )
            return result;
        if ( content.IsEmpty() )
            return result;
        var matches = Regex.Matches( content, "<[ ]*partial[ ]+name[ ]*=[ ]*\"(.+?)\"[ ]*/[ ]*>", RegexOptions.IgnoreCase );
        foreach ( Match match in matches ) {
            if ( match.Success == false )
                continue;
            var partPath = _pathFinder.Find( path, match.Groups[1].Value );
            result.Add( partPath );
        }
        return result;
    }
}