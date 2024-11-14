using Util.Helpers;

namespace Util.FileStorage; 

/// <summary>
/// Clase que procesa nombres de archivos relacionados con el tiempo de usuario.
/// Implementa la interfaz <see cref="IFileNameProcessor"/>.
/// </summary>
public class UserTimeFileNameProcessor : IFileNameProcessor {
    public const string Policy = "USERTIME";
    private readonly ISession _session;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UserTimeFileNameProcessor"/>.
    /// </summary>
    /// <param name="session">La sesión que se utilizará para procesar los nombres de archivo de tiempo de usuario. Si es <c>null</c>, se utilizará una instancia de sesión nula.</param>
    public UserTimeFileNameProcessor( ISession session ) {
        _session = session ?? NullSession.Instance;
    }

    /// <inheritdoc />
    /// <summary>
    /// Procesa el nombre de un archivo dado y genera un nuevo nombre basado en el ID de sesión y la fecha actual.
    /// </summary>
    /// <param name="fileName">El nombre del archivo a procesar. Si está vacío, se retornará un objeto <see cref="ProcessedName"/> con valor nulo.</param>
    /// <returns>
    /// Un objeto <see cref="ProcessedName"/> que contiene el nuevo nombre del archivo procesado y el nombre original.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la extensión del archivo original y la fecha actual para crear un nuevo nombre de archivo único.
    /// Si el nombre del archivo proporcionado está vacío, se devuelve un <see cref="ProcessedName"/> con un valor nulo.
    /// </remarks>
    /// <seealso cref="ProcessedName"/>
    public ProcessedName Process( string fileName ) {
        if ( fileName.IsEmpty() )
            return new ProcessedName( null );
        var extension = Path.GetExtension( fileName );
        var name = $"{Id.Create()}{extension}";
        var result = Util.Helpers.Common.JoinPath( _session.UserId, $"{Time.Now:yyyy-MM-dd}", name );
        return new ProcessedName( result, fileName );
    }
}