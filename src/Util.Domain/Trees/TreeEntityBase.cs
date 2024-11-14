using Util.Domain.Entities;

namespace Util.Domain.Trees; 

/// <summary>
/// Clase base abstracta que representa una entidad de árbol.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad que hereda de <see cref="ITreeEntity{TEntity, TKey, TParentKey}"/>.</typeparam>
/// <remarks>
/// Esta clase proporciona la funcionalidad básica para manejar entidades en una estructura de árbol,
/// incluyendo la gestión de identificadores y relaciones jerárquicas.
/// </remarks>
public abstract class TreeEntityBase<TEntity> : TreeEntityBase<TEntity, Guid, Guid?> where TEntity : ITreeEntity<TEntity, Guid, Guid?> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TreeEntityBase"/>.
    /// </summary>
    /// <param name="id">El identificador único de la entidad.</param>
    /// <param name="path">La ruta asociada a la entidad.</param>
    /// <param name="level">El nivel de la entidad en la jerarquía.</param>
    protected TreeEntityBase( Guid id, string path, int level )
        : base( id, path, level ) {
    }
}

/// <summary>
/// Clase base abstracta que representa una entidad de árbol.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad que hereda de <see cref="ITreeEntity{TEntity, TKey, TParentId}"/>.</typeparam>
/// <typeparam name="TKey">El tipo de la clave única de la entidad.</typeparam>
/// <typeparam name="TParentId">El tipo del identificador del padre de la entidad.</typeparam>
/// <remarks>
/// Esta clase proporciona la funcionalidad básica para manejar entidades que tienen una estructura jerárquica.
/// Debe ser heredada por clases que representen entidades específicas en un árbol.
/// </remarks>
public abstract class TreeEntityBase<TEntity, TKey, TParentId> : AggregateRoot<TEntity, TKey>, ITreeEntity<TEntity, TKey, TParentId> where TEntity : ITreeEntity<TEntity, TKey, TParentId> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TreeEntityBase"/>.
    /// </summary>
    /// <param name="id">El identificador único de la entidad.</param>
    /// <param name="path">La ruta que representa la ubicación de la entidad en una estructura jerárquica.</param>
    /// <param name="level">El nivel de profundidad de la entidad en la jerarquía.</param>
    protected TreeEntityBase(TKey id, string path, int level)
        : base(id) 
    {
        Path = path;
        Level = level;
    }

    /// <summary>
    /// Obtiene o establece el identificador del padre.
    /// </summary>
    /// <typeparam name="TParentId">El tipo del identificador del padre.</typeparam>
    /// <remarks>
    /// Esta propiedad permite acceder al identificador del objeto padre asociado,
    /// facilitando la relación jerárquica entre objetos.
    /// </remarks>
    public TParentId ParentId { get; set; }

    /// <summary>
    /// Obtiene la ruta asociada a este objeto.
    /// </summary>
    /// <value>
    /// Una cadena que representa la ruta.
    /// </value>
    /// <remarks>
    /// Esta propiedad es de solo lectura desde fuera de la clase, pero puede ser establecida internamente.
    /// </remarks>
    public virtual string Path { get;private set; }

    /// <summary>
    /// Obtiene el nivel actual.
    /// </summary>
    /// <remarks>
    /// Esta propiedad representa el nivel de un objeto en un contexto específico. 
    /// El valor de esta propiedad es de solo lectura desde fuera de la clase, 
    /// ya que el setter es privado.
    /// </remarks>
    /// <value>
    /// Un entero que indica el nivel actual.
    /// </value>
    public int Level { get; private set; }

    /// <summary>
    /// Obtiene o establece un valor que indica si la funcionalidad está habilitada.
    /// </summary>
    /// <value>
    /// <c>true</c> si la funcionalidad está habilitada; de lo contrario, <c>false</c>.
    /// </value>
    public bool Enabled { get; set; }

    /// <summary>
    /// Obtiene o establece el identificador de orden.
    /// </summary>
    /// <remarks>
    /// Este identificador puede ser nulo, lo que indica que no se ha asignado un valor de orden.
    /// </remarks>
    /// <value>
    /// Un entero que representa el identificador de orden, o <c>null</c> si no se ha establecido.
    /// </value>
    public int? SortId { get; set; }

    /// <summary>
    /// Inicializa la ruta utilizando los valores predeterminados.
    /// </summary>
    /// <remarks>
    /// Este método es una sobrecarga que llama a la versión de <see cref="InitPath(object)"/> 
    /// con el valor predeterminado.
    /// </remarks>
    public virtual void InitPath() {
        InitPath( default );
    }

    /// <summary>
    /// Inicializa la ruta del objeto actual en función del objeto padre proporcionado.
    /// </summary>
    /// <param name="parent">El objeto padre del tipo <typeparamref name="TEntity"/> que se utiliza para establecer el nivel y la ruta del objeto actual. Si es <c>null</c>, se establece el nivel en 1 y la ruta solo contendrá el Id del objeto actual.</param>
    /// <remarks>
    /// Este método es virtual, lo que permite que las clases derivadas lo sobreescriban si es necesario.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo del objeto padre que se está utilizando para inicializar la ruta.</typeparam>
    public virtual void InitPath( TEntity parent ) {
        if( Equals( parent, null ) ) {
            Level = 1;
            Path = $"{Id},";
            return;
        }
        Level = parent.Level + 1;
        Path = $"{parent.Path}{Id},";
    }

    /// <summary>
    /// Obtiene una lista de identificadores de los padres a partir de una ruta dada.
    /// </summary>
    /// <param name="excludeSelf">Indica si se debe excluir el identificador del objeto actual de la lista de resultados.</param>
    /// <returns>Una lista de identificadores de tipo <typeparamref name="TKey"/> que representan los padres en la ruta.</returns>
    /// <remarks>
    /// Si la propiedad <c>Path</c> está vacía, se devuelve una lista vacía.
    /// La ruta se divide por comas y se filtran los identificadores vacíos o que son solo comas.
    /// Si <c>excludeSelf</c> es verdadero, se excluye el identificador del objeto actual de la lista.
    /// </remarks>
    /// <typeparam name="TKey">El tipo de los identificadores que se devolverán en la lista.</typeparam>
    public List<TKey> GetParentIdsFromPath( bool excludeSelf = true ) {
        if( Path.IsEmpty() )
            return new List<TKey>();
        var result = Path.Split( ',' ).Where( id => !id.IsEmpty() && id != "," ).ToList();
        if( excludeSelf )
            result = result.Where( id => id.SafeString().ToUpperInvariant() != Id.SafeString().ToUpperInvariant() ).ToList();
        return result.Select( Util.Helpers.Convert.To<TKey> ).ToList();
    }
}