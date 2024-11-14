namespace Util.Ui.Razor.Internal;

/// <summary>
/// Define un contrato para resolver contenido de vista.
/// </summary>
public interface IViewContentResolver {
    /// <summary>
    /// Resuelve la ruta especificada y devuelve una cadena que representa la ruta completa.
    /// </summary>
    /// <param name="path">La ruta a resolver, que puede ser relativa o absoluta.</param>
    /// <returns>Una cadena que representa la ruta completa resuelta.</returns>
    /// <remarks>
    /// Este método toma una ruta como entrada y determina su representación completa,
    /// teniendo en cuenta el contexto actual del sistema de archivos.
    /// </remarks>
    /// <seealso cref="System.IO.Path"/>
    string Resolve( string path );
}