using Util.Domain.Trees;

namespace Util.Data.Trees; 

/// <summary>
/// Clase que representa una condición de árbol genérica para entidades que implementan 
/// las interfaces <see cref="IPath"/>, <see cref="IEnabled"/> y <see cref="IParentId{T}"/>.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que debe cumplir con las interfaces requeridas.</typeparam>
/// <remarks>
/// Esta clase hereda de <see cref="TreeCondition{TEntity, TKey}"/> donde el tipo de clave es 
/// <see cref="Guid?"/>. Se utiliza para definir condiciones específicas en una estructura de árbol 
/// que involucra entidades con propiedades de ruta, estado habilitado y un identificador de padre.
/// </remarks>
public class TreeCondition<TEntity> : TreeCondition<TEntity, Guid?> where TEntity : IPath, IEnabled, IParentId<Guid?> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TreeCondition"/>.
    /// </summary>
    /// <param name="parameter">El parámetro de consulta del árbol que se utilizará para inicializar la condición.</param>
    public TreeCondition( ITreeQueryParameter parameter ) : base( parameter ) {
    }
}

/// <summary>
/// Representa una condición que se aplica a entidades que implementan las interfaces 
/// <see cref="IPath"/>, <see cref="IEnabled"/> y <see cref="IParentId{TParentId}"/>.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que debe cumplir con las condiciones especificadas.</typeparam>
/// <typeparam name="TParentId">El tipo del identificador del padre de la entidad.</typeparam>
/// <seealso cref="ICondition{TEntity}"/>
public class TreeCondition<TEntity, TParentId> : ICondition<TEntity> where TEntity : IPath, IEnabled, IParentId<TParentId> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TreeCondition"/> 
    /// utilizando los parámetros proporcionados.
    /// </summary>
    /// <param name="parameter">El objeto que contiene los parámetros de consulta para el árbol.</param>
    /// <remarks>
    /// Si el parámetro proporcionado es nulo, el constructor no realizará ninguna acción.
    /// Se aplican condiciones basadas en las propiedades del parámetro, 
    /// como ParentId, Path, Level y Enabled, para construir la condición del árbol.
    /// </remarks>
    public TreeCondition( ITreeQueryParameter parameter ) {
        if ( parameter == null )
            return;
        var parentId = Util.Helpers.Convert.To<TParentId>( parameter.ParentId );
        if ( parameter.ParentId.IsEmpty() == false )
            Condition = Condition.And( t => t.ParentId.Equals( parentId ) );
        if ( parameter.Path.IsEmpty() == false )
            Condition = Condition.And( t => t.Path.StartsWith( parameter.Path ) );
        if ( parameter.Level != null )
            Condition = Condition.And( t => t.Level == parameter.Level );
        if ( parameter.Enabled != null )
            Condition = Condition.And( t => t.Enabled == parameter.Enabled );
    }

    /// <summary>
    /// Representa una condición que se aplica a una entidad de tipo <typeparamref name="TEntity"/>.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite definir una expresión que puede ser utilizada para filtrar entidades 
    /// en consultas, facilitando la creación de condiciones dinámicas en tiempo de ejecución.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de la entidad sobre la que se aplica la condición.</typeparam>
    /// <value>
    /// Una expresión que representa una función que toma una entidad de tipo <typeparamref name="TEntity"/> 
    /// y devuelve un valor booleano indicando si cumple con la condición especificada.
    /// </value>
    protected Expression<Func<TEntity, bool>> Condition { get; set; }

    /// <summary>
    /// Obtiene la condición de filtrado para la entidad especificada.
    /// </summary>
    /// <returns>
    /// Una expresión que representa la condición de filtrado, 
    /// que se evalúa como un predicado para la entidad de tipo <typeparamref name="TEntity"/>.
    /// </returns>
    /// <typeparam name="TEntity">
    /// El tipo de entidad para el cual se aplica la condición.
    /// </typeparam>
    public Expression<Func<TEntity, bool>> GetCondition() {
        return Condition;
    }
}