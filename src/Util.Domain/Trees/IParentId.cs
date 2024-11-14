namespace Util.Domain.Trees; 

/// <summary>
/// Interfaz que representa un identificador de padre genérico.
/// </summary>
/// <typeparam name="TParentId">El tipo del identificador del padre.</typeparam>
public interface IParentId<TParentId> {
    /// <summary>
    /// Obtiene o establece el identificador del padre.
    /// </summary>
    /// <value>
    /// El identificador del padre de tipo <typeparamref name="TParentId"/>.
    /// </value>
    /// <typeparam name="TParentId">
    /// El tipo del identificador del padre.
    /// </typeparam>
    TParentId ParentId { get; set; }
}