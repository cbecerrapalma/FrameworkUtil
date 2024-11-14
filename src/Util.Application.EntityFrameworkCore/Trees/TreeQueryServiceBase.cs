using Util.Data;
using Util.Data.Trees;
using Util.Domain.Trees;

namespace Util.Applications.Trees; 

/// <summary>
/// Clase base abstracta para servicios de consulta de árboles.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que representa un nodo en el árbol.</typeparam>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos que se utilizará para la salida.</typeparam>
/// <typeparam name="TQuery">El tipo de objeto que representa la consulta.</typeparam>
public abstract class TreeQueryServiceBase<TEntity, TDto, TQuery>
    : TreeQueryServiceBase<TEntity, TDto, TQuery, Guid, Guid?>
    where TEntity : class, ITreeEntity<TEntity, Guid, Guid?>, new()
    where TDto : class, ITreeNode, new()
    where TQuery : class, ITreeQueryParameter {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TreeQueryServiceBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    /// <param name="repository">El repositorio que se utilizará para acceder a los datos de tipo <typeparamref name="TEntity"/>.</param>
    protected TreeQueryServiceBase(IServiceProvider serviceProvider, ITreeRepository<TEntity, Guid, Guid?> repository) : base(serviceProvider, repository) { }
}

/// <summary>
/// Clase base abstracta para servicios de consulta de árboles.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que representa un nodo en el árbol.</typeparam>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos que se utilizará para la salida.</typeparam>
/// <typeparam name="TQuery">El tipo que representa la consulta específica que se puede realizar.</typeparam>
/// <typeparam name="TKey">El tipo de la clave única que identifica cada entidad.</typeparam>
/// <typeparam name="TParentId">El tipo que representa la identificación del padre de la entidad.</typeparam>
public abstract class TreeQueryServiceBase<TEntity, TDto, TQuery, TKey, TParentId>
    : QueryServiceBase<TEntity, TDto, TQuery, TKey>, ITreeQueryService<TDto, TQuery>
    where TEntity : class, ITreeEntity<TEntity, TKey, TParentId>, new()
    where TDto : class, ITreeNode, new()
    where TQuery : class, ITreeQueryParameter {
    private readonly ITreeRepository<TEntity, TKey, TParentId> _repository;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TreeQueryServiceBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver dependencias.</param>
    /// <param name="repository">El repositorio que se utilizará para realizar operaciones sobre entidades de tipo <typeparamref name="TEntity"/>.</param>
    protected TreeQueryServiceBase(IServiceProvider serviceProvider, ITreeRepository<TEntity, TKey, TParentId> repository) : base(serviceProvider, repository) 
    {
        _repository = repository;
    }

    /// <summary>
    /// Obtiene una colección de condiciones basadas en el parámetro proporcionado.
    /// </summary>
    /// <param name="parameter">El parámetro de consulta que se utilizará para crear las condiciones.</param>
    /// <returns>
    /// Una colección de condiciones que se aplicarán a la entidad de tipo <typeparamref name="TEntity"/>.
    /// </returns>
    /// <typeparam name="TEntity">El tipo de entidad sobre la que se aplicarán las condiciones.</typeparam>
    /// <typeparam name="TQuery">El tipo del parámetro de consulta utilizado para generar las condiciones.</typeparam>
    /// <seealso cref="ICondition{TEntity}"/>
    protected override IEnumerable<ICondition<TEntity>> GetConditions( TQuery parameter ) {
        return new[] { new TreeCondition<TEntity, TParentId>( parameter ) };
    }

    /// <summary>
    /// Obtiene una lista de objetos DTO basados en los identificadores de padre proporcionados.
    /// </summary>
    /// <param name="parentIds">Una cadena que contiene los identificadores de padre, separados por comas.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El resultado de la tarea es una lista de objetos DTO que corresponden a los identificadores de padre especificados.
    /// </returns>
    /// <remarks>
    /// Este método convierte la cadena de identificadores de padre en una lista de tipo <typeparamref name="TParentId"/> 
    /// y luego busca en el repositorio todas las entidades que tienen un identificador de padre que coincide con los 
    /// identificadores proporcionados. Finalmente, convierte las entidades encontradas en objetos DTO.
    /// </remarks>
    /// <typeparam name="TDto">El tipo de objeto DTO que se devolverá.</typeparam>
    /// <typeparam name="TParentId">El tipo de identificador de padre que se utilizará para la búsqueda.</typeparam>
    /// <seealso cref="FindAllAsync"/>
    public virtual async Task<List<TDto>> GetByParentIds( string parentIds ) {
        var keys = Util.Helpers.Convert.ToList<TParentId>( parentIds );
        var entities = await _repository.FindAllAsync( t => keys.Contains( t.ParentId ) );
        return entities.Select( ToDto ).ToList();
    }
}