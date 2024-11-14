namespace Util.Data.Sql.Builders; 

/// <summary>
/// Define un contrato para los dialectos que implementan diferentes comportamientos 
/// o características en el sistema.
/// </summary>
public interface IDialect {
    /// <summary>
    /// Obtiene el identificador de apertura.
    /// </summary>
    /// <returns>
    /// Un <see cref="string"/> que representa el identificador de apertura.
    /// </returns>
    /// <remarks>
    /// Este método es útil para obtener un identificador que se utiliza al iniciar un proceso o una operación.
    /// </remarks>
    string GetOpeningIdentifier();
    /// <summary>
    /// Obtiene el identificador de cierre.
    /// </summary>
    /// <returns>
    /// Un <see cref="string"/> que representa el identificador de cierre.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para recuperar el identificador que se utiliza al final de una estructura o bloque.
    /// </remarks>
    /// <seealso cref="GetOpeningIdentifier"/>
    string GetClosingIdentifier();
    /// <summary>
    /// Obtiene un nombre seguro a partir de un nombre dado.
    /// </summary>
    /// <param name="name">El nombre original que se desea convertir en un nombre seguro.</param>
    /// <returns>Un string que representa el nombre seguro generado.</returns>
    /// <remarks>
    /// Este método puede realizar operaciones como eliminar caracteres no permitidos,
    /// reemplazar espacios por guiones bajos, o cualquier otra transformación necesaria
    /// para asegurar que el nombre sea válido en contextos específicos, como nombres de archivos
    /// o identificadores en bases de datos.
    /// </remarks>
    string GetSafeName( string name );
    /// <summary>
    /// Obtiene el prefijo asociado.
    /// </summary>
    /// <returns>
    /// Un <see cref="string"/> que representa el prefijo.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para recuperar el prefijo que puede ser utilizado en otras operaciones
    /// dentro de la aplicación. Asegúrese de que el prefijo esté correctamente configurado antes de
    /// llamar a este método.
    /// </remarks>
    string GetPrefix();
    /// <summary>
    /// Reemplaza partes específicas de una cadena SQL para asegurar su validez y seguridad.
    /// </summary>
    /// <param name="sql">La cadena SQL que se desea modificar.</param>
    /// <returns>Una nueva cadena SQL con las modificaciones aplicadas.</returns>
    /// <remarks>
    /// Este método puede ser utilizado para prevenir inyecciones SQL al sanitizar la entrada.
    /// Asegúrese de que la cadena SQL original no contenga elementos que puedan comprometer la seguridad.
    /// </remarks>
    /// <seealso cref="System.String"/>
    string ReplaceSql( string sql );
}