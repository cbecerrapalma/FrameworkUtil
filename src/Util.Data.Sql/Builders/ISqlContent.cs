namespace Util.Data.Sql.Builders; 

/// <summary>
/// Define una interfaz para el contenido SQL.
/// </summary>
public interface ISqlContent {
    /// <summary>
    /// Agrega contenido a un objeto <see cref="StringBuilder"/> existente.
    /// </summary>
    /// <param name="builder">El objeto <see cref="StringBuilder"/> al que se le agregará el contenido.</param>
    /// <remarks>
    /// Este método permite modificar el contenido del <see cref="StringBuilder"/> 
    /// proporcionado, añadiendo datos al final del mismo.
    /// </remarks>
    /// <seealso cref="StringBuilder"/>
    void AppendTo( StringBuilder builder );
}