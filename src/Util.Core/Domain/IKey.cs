namespace Util.Domain; 

/// <summary>
/// Define una interfaz para representar una clave de tipo genérico.
/// </summary>
/// <typeparam name="TKey">El tipo de la clave que implementa esta interfaz.</typeparam>
public interface IKey<out TKey> {
    /// <summary>
    /// Representa la clave única del objeto.
    /// </summary>
    /// <typeparam name="TKey">El tipo de la clave única.</typeparam>
    /// <value>La clave única del objeto.</value>
    TKey Id { get; }
}