namespace Util.Helpers;

/// <summary>
/// Clase estática que proporciona métodos relacionados con la gestión de identificadores.
/// </summary>
public static partial class Id {
    /// <summary>
    /// Crea un nuevo identificador YitId. 
    /// Si ya existe un valor de identificador, se convierte y se devuelve ese valor.
    /// En caso de que no exista un valor, se genera un nuevo identificador utilizando el generador YitId.
    /// Si ocurre un error durante la generación del identificador, se establece un nuevo generador con opciones predeterminadas y se intenta nuevamente.
    /// </summary>
    /// <returns>
    /// Un identificador YitId como un valor de tipo <see cref="long"/>.
    /// </returns>
    /// <remarks>
    /// Este método es estático y se puede llamar sin necesidad de instanciar la clase.
    /// Asegúrese de que el generador YitId esté configurado correctamente antes de llamar a este método para evitar excepciones.
    /// </remarks>
    public static long CreateYitId() {
        if( string.IsNullOrEmpty( _id.Value ) == false )
            return Convert.ToLong( _id.Value );
        try {
            return YitIdHelper.NextId();
        }
        catch {
            var options = new IdGeneratorOptions( 1 );
            YitIdHelper.SetIdGenerator( options );
            return YitIdHelper.NextId();
        }
    }
}