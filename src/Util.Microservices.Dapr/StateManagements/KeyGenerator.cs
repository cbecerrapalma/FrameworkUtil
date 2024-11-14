namespace Util.Microservices.Dapr.StateManagements;

/// <summary>
/// Clase que implementa la interfaz <see cref="IKeyGenerator"/> para generar claves únicas.
/// </summary>
public class KeyGenerator : IKeyGenerator {
    /// <inheritdoc />
    /// <summary>
    /// Crea una clave única basada en el tipo de dato y un identificador proporcionado.
    /// </summary>
    /// <typeparam name="TValue">El tipo de dato que implementa la interfaz <see cref="IDataKey"/>.</typeparam>
    /// <param name="id">El identificador que se usará para generar la clave.</param>
    /// <returns>Una cadena que representa la clave única, o null si el identificador está vacío.</returns>
    /// <remarks>
    /// La clave se genera combinando el nombre del tipo de dato (reemplazando los puntos por guiones bajos) 
    /// y el identificador proporcionado, separados por un guion bajo.
    /// </remarks>
    /// <seealso cref="IDataKey"/>
    public string CreateKey<TValue>( string id ) where TValue : IDataKey {
        return id.IsEmpty() ? null : $"{typeof( TValue ).Name!.Replace( ".", "_" )}_{id}";
    }
}