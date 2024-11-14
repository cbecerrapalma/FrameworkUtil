namespace Util.Helpers;

/// <summary>
/// Clase estática que proporciona métodos y propiedades relacionados con la gestión de identificadores.
/// </summary>
public static partial class Id {
    private static readonly AsyncLocal<string> _id = new();

    /// <summary>
    /// Establece el valor del identificador.
    /// </summary>
    /// <param name="id">El identificador que se va a establecer.</param>
    public static void SetId( string id ) {
        _id.Value = id;
    }

    /// <summary>
    /// Restablece el valor del identificador.
    /// </summary>
    /// <remarks>
    /// Este método establece el valor de la variable estática <c>_id</c> a <c>null</c>,
    /// lo que puede ser útil para reiniciar el estado de la aplicación o del objeto.
    /// </remarks>
    public static void Reset() {
        _id.Value = null;
    }

    /// <summary>
    /// Crea un nuevo identificador único en formato de cadena.
    /// </summary>
    /// <returns>
    /// Un identificador único en formato de cadena. Si el valor de <c>_id</c> es nulo o vacío, se genera un nuevo GUID; 
    /// de lo contrario, se devuelve el valor de <c>_id</c>.
    /// </returns>
    public static string Create() {
        return string.IsNullOrEmpty( _id.Value ) ? System.Guid.NewGuid().ToString( "N" ) : _id.Value;
    }

    /// <summary>
    /// Crea un nuevo identificador único (GUID).
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="Guid"/> que representa un identificador único. 
    /// Si el valor de <c>_id</c> es nulo o vacío, se genera un nuevo GUID; 
    /// de lo contrario, se convierte el valor existente de <c>_id</c> a un GUID.
    /// </returns>
    public static Guid CreateGuid() {
        return string.IsNullOrEmpty( _id.Value ) ? System.Guid.NewGuid() : _id.Value.ToGuid();
    }
}