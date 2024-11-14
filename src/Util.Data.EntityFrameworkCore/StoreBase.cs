using Util.Data.Queries;
using Util.Data.Stores;
using Util.Exceptions;
using Util.Validation;

namespace Util.Data.EntityFrameworkCore;



/// Clase base abstracta para el almacenamiento de entidades.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que se almacenará, debe implementar la interfaz <see cref="IKey{Guid}"/>.</typeparam>
/// <remarks>
/// Esta clase proporciona una implementación básica para el almacenamiento de entidades en un contexto de trabajo.
/// </remarks>
/// <summary>
/// Clase base abstracta para la gestión de entidades en un almacén.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que se gestionará en el almacén.</typeparam>
/// <remarks>
/// Esta clase hereda de <see cref="StoreBase{TEntity, TKey}"/> utilizando <see cref="Guid"/> como tipo de clave.
/// Proporciona una implementación común para operaciones de almacenamiento de entidades.
/// </remarks>
public abstract class StoreBase<TEntity> : StoreBase<TEntity, Guid>, IStore<TEntity>
    where TEntity : class, IKey<Guid>
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="StoreBase"/>.
    /// </summary>
    /// <param name="unitOfWork">La unidad de trabajo que se utilizará para las operaciones de almacenamiento.</param>
    protected StoreBase(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}


/// <summary>
/// Clase base abstracta que define la estructura para un almacén de entidades.
/// Implementa las interfaces <see cref="IStore{TEntity, TKey}"/>, <see cref="IFilterSwitch"/> y <see cref="ITrack"/>.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que se almacenará. Debe ser una clase que implemente <see cref="IKey{TKey}"/>.</typeparam>
/// <typeparam name="TKey">El tipo de la clave única que identifica a cada entidad.</typeparam>
public abstract class StoreBase<TEntity, TKey> : IStore<TEntity, TKey>, IFilterSwitch, ITrack where TEntity : class, IKey<TKey>
{


    #region Campo

    private bool _disposed;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="StoreBase"/>.
    /// </summary>
    /// <param name="unitOfWork">La unidad de trabajo que se utilizará para las operaciones de almacenamiento.</param>
    protected StoreBase(IUnitOfWork unitOfWork)
    {
        UnitOfWork = (UnitOfWorkBase)unitOfWork;
    }

    #endregion

    #region atributo

    /// <summary>
    /// Representa la unidad de trabajo base utilizada para gestionar las operaciones de acceso a datos.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a la instancia de la unidad de trabajo, que permite realizar operaciones de 
    /// creación, lectura, actualización y eliminación (CRUD) en la base de datos de manera transaccional.
    /// </remarks>
    /// <value>
    /// Una instancia de <see cref="UnitOfWorkBase"/> que representa la unidad de trabajo actual.
    /// </value>
    protected UnitOfWorkBase UnitOfWork { get; }

    /// <summary>
    /// Obtiene el conjunto de entidades del tipo especificado.
    /// </summary>
    /// <value>
    /// Un <see cref="DbSet{TEntity}"/> que representa el conjunto de entidades.
    /// </value>
    /// <remarks>
    /// Este miembro permite acceder al conjunto de entidades de tipo <typeparamref name="TEntity"/> 
    /// a través de la unidad de trabajo actual.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se está gestionando.</typeparam>
    protected DbSet<TEntity> Set => UnitOfWork.Set<TEntity>();

    /// <summary>
    /// Indica si el seguimiento está habilitado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para determinar si el sistema está actualmente en modo de seguimiento.
    /// El valor predeterminado es <c>true</c>, lo que significa que el seguimiento está habilitado al iniciar.
    /// </remarks>
    /// <value>
    /// <c>true</c> si el seguimiento está habilitado; de lo contrario, <c>false</c>.
    /// </value>
    protected bool IsTracking { get; private set; } = true;

    #endregion

    #region NoTracking(Configurado para no rastrear entidades.)


    /// <summary>
    /// Desactiva el seguimiento de cambios en el contexto actual.
    /// </summary>
    /// <remarks>
    /// Cuando el seguimiento está desactivado, las entidades no se rastrean y los cambios en ellas no se guardarán en la base de datos.
    /// Esto puede mejorar el rendimiento en situaciones donde no se necesita realizar un seguimiento de los cambios.
    /// </remarks>
    public void NoTracking()
    {
        IsTracking = false;
    }

    #endregion

    #region EnableFilter(Habilitar filtro)

    /// <summary>
    /// Habilita un filtro específico en la unidad de trabajo.
    /// </summary>
    /// <typeparam name="TFilterType">El tipo del filtro que se desea habilitar. Debe ser una clase.</typeparam>
    /// <remarks>
    /// Este método permite activar un filtro definido en la unidad de trabajo, lo que puede ser útil para aplicar condiciones específicas 
    /// en las consultas realizadas a la base de datos.
    /// </remarks>
    /// <seealso cref="UnitOfWork"/>
    public void EnableFilter<TFilterType>() where TFilterType : class
    {
        UnitOfWork.EnableFilter<TFilterType>();
    }

    #endregion

    #region DisableFilter(Desactivar el filtro.)

    /// <summary>
    /// Desactiva un filtro específico en el contexto de trabajo actual.
    /// </summary>
    /// <typeparam name="TFilterType">El tipo del filtro que se desea desactivar. Debe ser una clase.</typeparam>
    /// <returns>
    /// Un objeto <see cref="IDisposable"/> que, al ser dispuesto, reactivará el filtro desactivado.
    /// </returns>
    /// <remarks>
    /// Este método permite desactivar temporalmente un filtro para realizar operaciones específicas
    /// sin la influencia de dicho filtro. Es importante asegurarse de que el objeto devuelto sea
    /// dispuesto adecuadamente para restaurar el estado original.
    /// </remarks>
    /// <seealso cref="IDisposable"/>
    public IDisposable DisableFilter<TFilterType>() where TFilterType : class
    {
        return UnitOfWork.DisableFilter<TFilterType>();
    }

    #endregion

    #region Find(Buscar entidad)

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una consulta de entidades del conjunto actual.
    /// </summary>
    /// <returns>
    /// Una consulta que representa el conjunto de entidades, 
    /// que puede ser rastreado o no, dependiendo del estado de 
    /// la propiedad <c>IsTracking</c>.
    /// </returns>
    /// <remarks>
    /// Si el contexto ha sido dispuesto, se lanzará una excepción. 
    /// Si <c>IsTracking</c> es verdadero, se devolverá el conjunto 
    /// de entidades rastreadas. De lo contrario, se devolverá el 
    /// conjunto de entidades sin rastrear y se establecerá 
    /// <c>IsTracking</c> en verdadero.
    /// </remarks>
    /// <exception cref="ObjectDisposedException">
    /// Se lanza cuando el contexto ha sido dispuesto.
    /// </exception>
    public IQueryable<TEntity> Find()
    {
        ThrowIfDisposed();
        if (IsTracking)
            return Set;
        var result = Set.AsNoTracking();
        IsTracking = true;
        return result;
    }

    /// <inheritdoc />
    /// <summary>
    /// Busca entidades que cumplen con una condición específica.
    /// </summary>
    /// <param name="condition">La condición que deben cumplir las entidades a buscar.</param>
    /// <returns>
    /// Una consulta que representa las entidades que cumplen con la condición especificada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la interfaz <see cref="ICondition{TEntity}"/> para filtrar las entidades.
    /// </remarks>
    public IQueryable<TEntity> Find(ICondition<TEntity> condition)
    {
        return Find().Where(condition);
    }

    /// <inheritdoc />
    /// <summary>
    /// Busca entidades que cumplen con una condición específica.
    /// </summary>
    /// <param name="condition">Una expresión que define la condición que deben cumplir las entidades.</param>
    /// <returns>
    /// Un <see cref="IQueryable{TEntity}"/> que contiene las entidades que cumplen con la condición especificada.
    /// </returns>
    /// <remarks>
    /// Este método permite filtrar las entidades de acuerdo a una expresión booleana, 
    /// facilitando la consulta de datos en la base de datos.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se está buscando.</typeparam>
    public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> condition)
    {
        return Find().Where(condition);
    }

    #endregion

    #region FindById(Buscar entidades a través de identificadores.)

    /// <inheritdoc />
    /// <summary>
    /// Busca una entidad por su identificador.
    /// </summary>
    /// <param name="id">El identificador de la entidad que se desea buscar.</param>
    /// <returns>La entidad correspondiente al identificador especificado, o null si no se encuentra.</returns>
    /// <remarks>
    /// Este método lanza una excepción si el contexto ha sido eliminado. 
    /// Si el identificador está vacío, se devuelve null. 
    /// Si el seguimiento de cambios está habilitado, se utiliza el conjunto de entidades para buscar la entidad.
    /// De lo contrario, se busca la entidad mediante una consulta que compara el identificador.
    /// </remarks>
    /// <exception cref="ObjectDisposedException">Se lanza si el contexto ha sido eliminado.</exception>
    /// <seealso cref="Set"/>
    /// <seealso cref="Single"/>
    public TEntity FindById(object id)
    {
        ThrowIfDisposed();
        if (id.SafeString().IsEmpty())
            return null;
        var key = GetKey(id);
        if (IsTracking)
            return Set.Find(key);
        return Single(t => t.Id.Equals(key));
    }

    /// <summary>
    /// Obtiene la clave a partir del identificador proporcionado.
    /// </summary>
    /// <param name="id">El identificador del cual se desea obtener la clave.</param>
    /// <returns>
    /// La clave correspondiente al identificador, que puede ser del tipo TKey.
    /// </returns>
    /// <remarks>
    /// Si el identificador ya es del tipo TKey, se devuelve directamente. 
    /// De lo contrario, se intenta convertir el identificador al tipo TKey utilizando un método de conversión.
    /// </remarks>
    /// <typeparam name="TKey">El tipo de la clave que se está manejando.</typeparam>
    protected object GetKey(object id)
    {
        if (id is TKey)
            return id;
        return Util.Helpers.Convert.To<TKey>(id);
    }

    #endregion

    #region FindByIdAsync(Buscar entidades a través de identificadores.)

    /// <inheritdoc />
    /// <summary>
    /// Busca una entidad por su identificador de manera asíncrona.
    /// </summary>
    /// <param name="id">El identificador de la entidad que se desea buscar.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El valor de la tarea es la entidad encontrada, o <c>null</c> si no se encuentra ninguna entidad con el identificador especificado.
    /// </returns>
    /// <remarks>
    /// Este método lanza una excepción si se solicita la cancelación antes de completar la operación.
    /// Si el objeto está siendo rastreado, se utiliza <c>Set.FindAsync</c> para buscar la entidad.
    /// De lo contrario, se utiliza <c>SingleAsync</c> para buscar la entidad que coincida con el identificador.
    /// </remarks>
    /// <exception cref="OperationCanceledException">
    /// Se lanza si se cancela la operación antes de que se complete.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// Se lanza si el contexto ha sido eliminado antes de llamar a este método.
    /// </exception>
    public async Task<TEntity> FindByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (id.SafeString().IsEmpty())
            return null;
        var key = GetKey(id);
        if (IsTracking)
            return await Set.FindAsync(new[] { key }, cancellationToken);
        return await SingleAsync(t => t.Id.Equals(key), cancellationToken);
    }

    #endregion

    #region FindByIds(Encuentre una lista de entidades por una lista de identificadores)

    /// <inheritdoc />
    /// <summary>
    /// Busca entidades por sus identificadores.
    /// </summary>
    /// <param name="ids">Un arreglo de identificadores de tipo <typeparamref name="TKey"/> que se utilizarán para buscar las entidades.</param>
    /// <returns>
    /// Una lista de entidades de tipo <typeparamref name="TEntity"/> que coinciden con los identificadores proporcionados.
    /// </returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite pasar los identificadores como un arreglo.
    /// </remarks>
    /// <seealso cref="FindByIds(IEnumerable{TKey})"/>
    public virtual List<TEntity> FindByIds(params TKey[] ids)
    {
        return FindByIds((IEnumerable<TKey>)ids);
    }

    /// <inheritdoc />
    /// <summary>
    /// Busca entidades por una colección de identificadores.
    /// </summary>
    /// <param name="ids">Una colección de identificadores de las entidades a buscar.</param>
    /// <returns>
    /// Una lista de entidades que coinciden con los identificadores proporcionados, 
    /// o null si la colección de identificadores es null.
    /// </returns>
    /// <remarks>
    /// Este método utiliza una expresión lambda para filtrar las entidades que 
    /// tienen un identificador que se encuentra en la colección proporcionada.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se está buscando.</typeparam>
    /// <typeparam name="TKey">El tipo de identificador de las entidades.</typeparam>
    /// <seealso cref="Find(System.Linq.Expressions.Expression{Func{TEntity, bool}})"/>
    public virtual List<TEntity> FindByIds(IEnumerable<TKey> ids)
    {
        return ids == null ? null : Find(t => ids.Contains(t.Id)).ToList();
    }

    /// <inheritdoc />
    /// <summary>
    /// Busca entidades por una lista de identificadores en formato de cadena.
    /// </summary>
    /// <param name="ids">Una cadena que contiene los identificadores separados por comas.</param>
    /// <returns>
    /// Una lista de entidades del tipo <typeparamref name="TEntity"/> que coinciden con los identificadores proporcionados.
    /// </returns>
    /// <remarks>
    /// Este método convierte la cadena de identificadores en una lista de tipo <typeparamref name="TKey"/> 
    /// antes de llamar a otro método que realiza la búsqueda.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se busca.</typeparam>
    /// <typeparam name="TKey">El tipo de clave que se utiliza para identificar las entidades.</typeparam>
    /// <seealso cref="FindByIds(List{TKey})"/>
    public virtual List<TEntity> FindByIds(string ids)
    {
        return FindByIds(Util.Helpers.Convert.ToList<TKey>(ids));
    }

    #endregion

    #region FindByIdsAsync(Encuentre una lista de entidades por una lista de identificadores)

    /// <inheritdoc />
    /// <summary>
    /// Busca entidades por sus identificadores.
    /// </summary>
    /// <param name="ids">Un arreglo de identificadores de las entidades a buscar.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El valor de retorno contiene una lista de entidades encontradas.
    /// </returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite pasar un arreglo de identificadores.
    /// </remarks>
    /// <seealso cref="FindByIdsAsync(IEnumerable{TKey})"/>
    public virtual async Task<List<TEntity>> FindByIdsAsync(params TKey[] ids)
    {
        return await FindByIdsAsync((IEnumerable<TKey>)ids);
    }

    /// <inheritdoc />
    /// <summary>
    /// Busca entidades por una colección de identificadores.
    /// </summary>
    /// <param name="ids">Una colección de identificadores de las entidades a buscar.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene una lista de entidades encontradas.
    /// Si <paramref name="ids"/> es nulo, se devuelve null.
    /// </returns>
    /// <remarks>
    /// Este método lanza una excepción si se solicita la cancelación a través del <paramref name="cancellationToken"/>.
    /// </remarks>
    /// <exception cref="OperationCanceledException">
    /// Se lanza si la operación es cancelada mediante el <paramref name="cancellationToken"/>.
    /// </exception>
    public virtual async Task<List<TEntity>> FindByIdsAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return ids == null ? null : await Find(t => ids.Contains(t.Id)).ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Busca entidades por una lista de identificadores.
    /// </summary>
    /// <param name="ids">Una cadena que contiene los identificadores de las entidades a buscar, separados por comas.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene una lista de entidades encontradas.
    /// </returns>
    /// <remarks>
    /// Este método convierte la cadena de identificadores en una lista de claves y luego llama a otro método para realizar la búsqueda.
    /// </remarks>
    /// <seealso cref="FindByIdsAsync(List{TKey}, CancellationToken)"/>
    public virtual async Task<List<TEntity>> FindByIdsAsync(string ids, CancellationToken cancellationToken = default)
    {
        return await FindByIdsAsync(Util.Helpers.Convert.ToList<TKey>(ids), cancellationToken);
    }

    #endregion

    #region Single(Buscar una sola entidad.)

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un único elemento que cumple con la condición especificada.
    /// </summary>
    /// <param name="condition">Una expresión que define la condición que debe cumplir el elemento.</param>
    /// <returns>
    /// El elemento que cumple con la condición especificada, o <c>null</c> si no se encuentra ningún elemento.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el método <see cref="Find"/> para obtener la colección de elementos y luego aplica la condición.
    /// Si hay más de un elemento que cumple con la condición, se devolverá el primero encontrado.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se está buscando.</typeparam>
    public virtual TEntity Single(Expression<Func<TEntity, bool>> condition)
    {
        return Find().FirstOrDefault(condition);
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un único elemento de la colección que cumple con la condición especificada.
    /// </summary>
    /// <param name="condition">Una expresión que define la condición que el elemento debe cumplir.</param>
    /// <param name="action">Una función que permite modificar la consulta antes de aplicar la condición.</param>
    /// <returns>
    /// El único elemento que cumple con la condición especificada, o <c>null</c> si no se encuentra ningún elemento.
    /// </returns>
    /// <remarks>
    /// Si el parámetro <paramref name="action"/> es <c>null</c>, se utilizará el método <see cref="Single(Expression{Func{TEntity, bool}})"/> 
    /// para obtener el elemento que cumple con la condición.
    /// </remarks>
    /// <seealso cref="Single(Expression{Func{TEntity, bool}})"/>
    public virtual TEntity Single(Expression<Func<TEntity, bool>> condition, Func<IQueryable<TEntity>, IQueryable<TEntity>> action)
    {
        if (action == null)
            return Single(condition);
        return action(Find()).FirstOrDefault(condition);
    }

    #endregion

    #region SingleAsync(Buscar una sola entidad.)

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una entidad que cumple con una condición específica de manera asíncrona.
    /// </summary>
    /// <param name="condition">Una expresión que define la condición que debe cumplir la entidad.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene la entidad que cumple con la condición, o <c>null</c> si no se encuentra ninguna.
    /// </returns>
    /// <remarks>
    /// Este método lanza una excepción si se solicita la cancelación antes de que se complete la operación.
    /// </remarks>
    /// <exception cref="OperationCanceledException">
    /// Se lanza si se cancela la operación a través del <paramref name="cancellationToken"/>.
    /// </exception>
    public virtual async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await Find().FirstOrDefaultAsync(condition, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un único elemento de tipo <typeparamref name="TEntity"/> que cumple con la condición especificada.
    /// </summary>
    /// <param name="condition">Una expresión que define la condición que debe cumplir el elemento.</param>
    /// <param name="action">Una función que permite modificar la consulta antes de ejecutar la búsqueda.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un resultado que es el elemento encontrado o <c>null</c> si no se encuentra ningún elemento que cumpla con la condición.
    /// </returns>
    /// <remarks>
    /// Este método permite aplicar una acción adicional a la consulta antes de buscar el elemento. Si no se proporciona una acción, se utiliza la búsqueda por defecto.
    /// </remarks>
    /// <seealso cref="SingleAsync(Expression{Func{TEntity, bool}}, CancellationToken)"/>
    /// <typeparam name="TEntity">El tipo de entidad que se está consultando.</typeparam>
    public virtual async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> condition, Func<IQueryable<TEntity>, IQueryable<TEntity>> action, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (action == null)
            return await SingleAsync(condition, cancellationToken);
        return await action(Find()).FirstOrDefaultAsync(condition, cancellationToken);
    }

    #endregion

    #region FindAll(Buscar lista de entidades.)

    /// <inheritdoc />
    /// <summary>
    /// Busca y devuelve una lista de entidades del tipo especificado.
    /// </summary>
    /// <param name="condition">Una expresión que define la condición que deben cumplir las entidades a buscar. Si es <c>null</c>, se devolverán todas las entidades.</param>
    /// <returns>Una lista de entidades que cumplen con la condición especificada.</returns>
    /// <remarks>
    /// Este método utiliza la función <c>Find</c> para obtener las entidades. Si no se proporciona ninguna condición, se devolverán todas las entidades disponibles.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se está buscando.</typeparam>
    /// <seealso cref="Find"/>
    public virtual List<TEntity> FindAll(Expression<Func<TEntity, bool>> condition = null)
    {
        return condition == null ? Find().ToList() : Find(condition).ToList();
    }

    #endregion

    #region FindAllAsync(Buscar lista de entidades)

    /// <inheritdoc />
    /// <summary>
    /// Busca y devuelve una lista de entidades de tipo <typeparamref name="TEntity"/> que cumplen con la condición especificada.
    /// </summary>
    /// <param name="condition">
    /// Una expresión que define la condición que deben cumplir las entidades. Si es <c>null</c>, se devolverán todas las entidades.
    /// </param>
    /// <param name="cancellationToken">
    /// Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.
    /// </param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene una lista de entidades que cumplen con la condición especificada.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobrescrito en clases derivadas.
    /// </remarks>
    /// <seealso cref="Find"/>
    /// <typeparam name="TEntity">
    /// El tipo de entidad que se está buscando.
    /// </typeparam>
    public virtual async Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> condition = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (condition == null)
            return await Find().ToListAsync(cancellationToken);
        return await Find(condition).ToListAsync(cancellationToken);
    }

    #endregion

    #region Exists(Determinar si existe.)

    /// <inheritdoc />
    /// <summary>
    /// Verifica si existen elementos en la colección con los identificadores especificados.
    /// </summary>
    /// <param name="ids">Un arreglo de identificadores de tipo <typeparamref name="TKey"/> que se van a verificar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si al menos uno de los identificadores existe en la colección; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza una expresión lambda para comprobar si alguno de los identificadores proporcionados se encuentra en la colección.
    /// </remarks>
    /// <typeparam name="TKey">El tipo de los identificadores que se están utilizando para buscar en la colección.</typeparam>
    /// <seealso cref="Exists(Func{T, bool})"/>
    public virtual bool Exists(params TKey[] ids)
    {
        return ids != null && Exists(t => ids.Contains(t.Id));
    }

    /// <inheritdoc />
    /// <summary>
    /// Verifica si existe al menos un elemento en la colección que cumple con la condición especificada.
    /// </summary>
    /// <param name="condition">Una expresión que representa la condición que deben cumplir los elementos.</param>
    /// <returns>
    /// Devuelve <c>true</c> si existe al menos un elemento que cumple con la condición; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la expresión proporcionada para filtrar los elementos de la colección.
    /// Es importante asegurarse de que la condición no sea nula antes de invocar este método.
    /// </remarks>
    public virtual bool Exists(Expression<Func<TEntity, bool>> condition)
    {
        return condition != null && Find().Any(condition);
    }

    #endregion

    #region ExistsAsync(Determinar si existe.)

    /// <inheritdoc />
    /// <summary>
    /// Verifica si existen entidades en la base de datos con los identificadores especificados.
    /// </summary>
    /// <param name="ids">Un arreglo de identificadores de tipo <typeparamref name="TKey"/> que se utilizarán para buscar las entidades.</param>
    /// <returns>
    /// Devuelve una tarea que representa la operación asíncrona. El valor de la tarea es <c>true</c> si al menos una de las entidades existe; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza una expresión lambda para comprobar si los identificadores proporcionados están presentes en la colección de entidades.
    /// </remarks>
    /// <seealso cref="ExistsAsync(System.Linq.Expressions.Expression{Func{TEntity,bool}})"/>
    public virtual async Task<bool> ExistsAsync(params TKey[] ids)
    {
        return ids != null && await ExistsAsync(t => ids.Contains(t.Id));
    }

    /// <inheritdoc />
    /// <summary>
    /// Verifica de manera asíncrona si existe al menos un elemento que cumpla con la condición especificada.
    /// </summary>
    /// <param name="condition">Una expresión que representa la condición que deben cumplir los elementos.</param>
    /// <param name="cancellationToken">Token que permite cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un valor booleano que indica si existe al menos un elemento que cumple con la condición.</returns>
    /// <remarks>
    /// Este método lanza una excepción si se solicita la cancelación a través del <paramref name="cancellationToken"/>.
    /// </remarks>
    /// <exception cref="OperationCanceledException">
    /// Se lanza si se cancela la operación a través del <paramref name="cancellationToken"/>.
    /// </exception>
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return condition != null && await Find().AnyAsync(condition, cancellationToken);
    }

    #endregion

    #region Count(Buscar cantidad)

    /// <inheritdoc />
    /// <summary>
    /// Cuenta el número de entidades que cumplen con una condición especificada.
    /// </summary>
    /// <param name="condition">Una expresión que representa la condición que deben cumplir las entidades. Si es <c>null</c>, se contará el total de entidades.</param>
    /// <returns>
    /// El número de entidades que cumplen con la condición especificada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el método <c>Find</c> para obtener las entidades y luego aplica la condición si se proporciona.
    /// </remarks>
    /// <seealso cref="Find"/>
    public virtual int Count(Expression<Func<TEntity, bool>> condition = null)
    {
        return condition == null ? Find().Count() : Find().Count(condition);
    }

    #endregion

    #region CountAsync(Buscar cantidad)

    /// <inheritdoc />
    /// <summary>
    /// Cuenta el número de entidades que cumplen con una condición específica de manera asíncrona.
    /// </summary>
    /// <param name="condition">Una expresión que define la condición que deben cumplir las entidades. Si es <c>null</c>, se contarán todas las entidades.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Un <see cref="Task{int}"/> que representa el número de entidades que cumplen con la condición especificada.</returns>
    /// <remarks>
    /// Este método es útil para obtener el conteo de entidades en una base de datos sin necesidad de cargar todas las entidades en memoria.
    /// Si se proporciona una condición, solo se contarán las entidades que la cumplan.
    /// </remarks>
    /// <exception cref="OperationCanceledException">Se lanza si la operación es cancelada a través del <paramref name="cancellationToken"/>.</exception>
    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> condition = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return condition == null
            ? await Find().CountAsync(cancellationToken)
            : await Find().CountAsync(condition, cancellationToken);
    }

    #endregion

    #region AddAsync(Agregar entidad)

    /// <summary>
    /// Agrega una entidad de forma asíncrona a la fuente de datos.
    /// </summary>
    /// <param name="entity">La entidad que se va a agregar.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona de agregar la entidad.</returns>
    /// <remarks>
    /// Este método valida la entidad antes de intentar agregarla a la fuente de datos.
    /// Si se solicita la cancelación a través del <paramref name="cancellationToken"/>, se lanzará una excepción.
    /// </remarks>
    /// <exception cref="OperationCanceledException">Se lanza si se solicita la cancelación de la operación.</exception>
    /// <exception cref="ObjectDisposedException">Se lanza si el objeto ha sido liberado.</exception>
    /// <exception cref="ArgumentNullException">Se lanza si la entidad es nula.</exception>
    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        Validate(entity);
        await Set.AddAsync(entity, cancellationToken);
    }

    /// <summary>
    /// Valida una colección de entidades.
    /// </summary>
    /// <param name="entities">La colección de entidades a validar.</param>
    /// <remarks>
    /// Este método verifica que la colección de entidades no sea nula y luego valida cada entidad individualmente.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si la colección de entidades es nula.</exception>
    protected virtual void Validate(IEnumerable<TEntity> entities)
    {
        entities.CheckNull(nameof(entities));
        foreach (var entity in entities)
            Validate(entity);
    }

    /// <summary>
    /// Valida la entidad proporcionada.
    /// </summary>
    /// <param name="entity">La entidad que se va a validar.</param>
    /// <remarks>
    /// Este método verifica si la entidad es nula y, si implementa la interfaz <see cref="IValidation"/>,
    /// llama al método <see cref="IValidation.Validate"/> para realizar la validación específica.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de la entidad que se está validando.</typeparam>
    protected virtual void Validate(TEntity entity)
    {
        entity.CheckNull(nameof(entity));
        if (entity is IValidation validation)
            validation.Validate();
    }

    /// <summary>
    /// Agrega una colección de entidades de forma asíncrona.
    /// </summary>
    /// <param name="entities">Una colección de entidades a agregar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <remarks>
    /// Este método valida las entidades antes de agregarlas y lanza una excepción si la operación es cancelada.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de agregar las entidades.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// Se lanza si la operación es cancelada mediante el <paramref name="cancellationToken"/>.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// Se lanza si el objeto ha sido descartado antes de llamar a este método.
    /// </exception>
    /// <exception cref="ValidationException">
    /// Se lanza si las entidades no son válidas según las reglas de validación definidas.
    /// </exception>
    public virtual async Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        var list = entities.ToList();
        Validate(list);
        await Set.AddRangeAsync(list, cancellationToken);
    }

    #endregion

    #region UpdateAsync(Modificar entidad)

    /// <inheritdoc />
    /// <summary>
    /// Actualiza una entidad de tipo <typeparamref name="TEntity"/> de manera asíncrona.
    /// </summary>
    /// <param name="entity">La entidad que se va a actualizar.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación de actualización asíncrona.</returns>
    /// <exception cref="OperationCanceledException">Se lanza si la operación es cancelada a través del <paramref name="cancellationToken"/>.</exception>
    /// <remarks>
    /// Este método llama a la función <see cref="Update(TEntity)"/> para realizar la actualización.
    /// </remarks>
    /// <seealso cref="Update(TEntity)"/>
    /// <typeparam name="TEntity">El tipo de la entidad que se va a actualizar.</typeparam>
    public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Update(entity);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Actualiza una entidad en el contexto de trabajo actual.
    /// </summary>
    /// <param name="entity">La entidad que se va a actualizar.</param>
    /// <remarks>
    /// Este método verifica si el contexto ha sido dispuesto, valida la entidad proporcionada,
    /// obtiene la entrada de la entidad en el contexto de trabajo y valida la versión de la entidad
    /// antes de realizar la actualización.
    /// </remarks>
    /// <exception cref="ObjectDisposedException">Se lanza si el contexto ha sido dispuesto.</exception>
    /// <exception cref="ArgumentNullException">Se lanza si la entidad proporcionada es nula.</exception>
    /// <seealso cref="Validate(TEntity)"/>
    /// <seealso cref="ValidateVersion(DbEntityEntry, TEntity)"/>
    /// <seealso cref="UpdateEntity(DbEntityEntry, TEntity)"/>
    protected void Update(TEntity entity)
    {
        ThrowIfDisposed();
        Validate(entity);
        var entry = UnitOfWork.Entry(entity);
        ValidateVersion(entry, entity);
        UpdateEntity(entry, entity);
    }

    /// <summary>
    /// Valida la versión de una entidad para asegurar la coherencia de los datos durante las operaciones de actualización.
    /// </summary>
    /// <param name="entry">La entrada de la entidad que contiene el estado actual y los valores originales de la entidad.</param>
    /// <param name="entity">La instancia de la entidad que se está validando.</param>
    /// <remarks>
    /// Este método verifica si la entidad implementa la interfaz <see cref="IVersion"/>. 
    /// Si la entidad está en estado agregado, no se realiza ninguna validación. 
    /// Si la versión de la entidad es nula o está vacía, se lanza una excepción de concurrencia.
    /// Si la versión es válida, se compara con la versión original almacenada en la entrada.
    /// Si las versiones no coinciden, se lanza una excepción de concurrencia.
    /// </remarks>
    /// <exception cref="ConcurrencyException">
    /// Se lanza cuando hay un conflicto de concurrencia debido a una versión no coincidente.
    /// </exception>
    protected virtual void ValidateVersion(EntityEntry<TEntity> entry, TEntity entity)
    {
        if (entity is not IVersion current)
            return;
        if (entry.State == EntityState.Added)
            return;
        if (current.Version == null || current.Version.Length == 0)
        {
            ThrowConcurrencyException(entity);
            return;
        }
        var oldVersion = entry.OriginalValues.GetValue<byte[]>("Version");
        for (int i = 0; i < oldVersion.Length; i++)
        {
            if (current.Version[i] != oldVersion[i])
                ThrowConcurrencyException(entity);
        }
    }

    /// <summary>
    /// Lanza una excepción de concurrencia para la entidad especificada.
    /// </summary>
    /// <param name="entity">La entidad que ha causado la excepción de concurrencia.</param>
    /// <exception cref="ConcurrencyException">Se lanza cuando se detecta un conflicto de concurrencia.</exception>
    private void ThrowConcurrencyException(TEntity entity)
    {
        throw new ConcurrencyException(new Exception($"Type:{typeof(TEntity)},Id:{entity.Id}"));
    }

    /// <summary>
    /// Actualiza una entidad en el contexto de seguimiento de cambios.
    /// </summary>
    /// <param name="entry">La entrada de la entidad que se va a actualizar.</param>
    /// <param name="entity">La entidad con los nuevos valores que se aplicarán.</param>
    /// <remarks>
    /// Este método busca una entrada existente en el rastreador de cambios que coincida con la entidad proporcionada.
    /// Si se encuentra, se actualizan los valores actuales de la entrada con los valores de la entidad.
    /// Si la entrada está en estado 'Desconectado', se actualiza la entidad en el contexto.
    /// </remarks>
    protected void UpdateEntity(EntityEntry<TEntity> entry, TEntity entity)
    {
        var oldEntry = UnitOfWork.ChangeTracker.Entries<TEntity>().FirstOrDefault(t => t.Entity.Equals(entity));
        if (oldEntry != null)
        {
            oldEntry.CurrentValues.SetValues(entity);
            return;
        }
        if (entry.State == EntityState.Detached)
            UnitOfWork.Update(entity);
    }

    /// <inheritdoc />
    /// <summary>
    /// Actualiza una colección de entidades de forma asíncrona.
    /// </summary>
    /// <param name="entities">La colección de entidades que se van a actualizar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación de actualización asíncrona.</returns>
    /// <exception cref="OperationCanceledException">Se lanza si se solicita la cancelación a través del <paramref name="cancellationToken"/>.</exception>
    /// <remarks>
    /// Este método llama a un método de actualización sincrónico y devuelve una tarea completada.
    /// </remarks>
    public virtual Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Update(entities);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Actualiza un conjunto de entidades en el contexto actual.
    /// </summary>
    /// <param name="entities">Una colección de entidades que se van a actualizar.</param>
    /// <remarks>
    /// Este método verifica si el contexto ha sido dispuesto antes de intentar actualizar las entidades.
    /// Si la colección de entidades es nula, se lanzará una excepción.
    /// Cada entidad en la colección se actualiza individualmente mediante el método <see cref="Update(TEntity)"/>.
    /// </remarks>
    /// <exception cref="ObjectDisposedException">Se lanza si el contexto ha sido dispuesto.</exception>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="entities"/> es nulo.</exception>
    protected void Update(IEnumerable<TEntity> entities)
    {
        ThrowIfDisposed();
        entities.CheckNull(nameof(entities));
        foreach (var entity in entities)
            Update(entity);
    }

    #endregion

    #region RemoveAsync(Eliminar entidad)

    /// <summary>
    /// Elimina de forma asíncrona una entidad identificada por su ID.
    /// </summary>
    /// <param name="id">El identificador de la entidad que se desea eliminar.</param>
    /// <param name="cancellationToken">Token de cancelación para poder cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación de eliminación asíncrona.</returns>
    /// <remarks>
    /// Este método lanza una excepción si la operación es cancelada o si el objeto ha sido dispuesto.
    /// Si la entidad no se encuentra, el comportamiento de este método dependerá de la implementación de la 
    /// función <c>Delete</c>.
    /// </remarks>
    /// <exception cref="OperationCanceledException">Se lanza si la operación es cancelada.</exception>
    /// <exception cref="ObjectDisposedException">Se lanza si el objeto ha sido dispuesto.</exception>
    public virtual async Task RemoveAsync(object id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        var entity = await FindByIdAsync(id, cancellationToken);
        Delete(entity);
    }

    /// <summary>
    /// Elimina una entidad del contexto. Si la entidad implementa la interfaz <see cref="IDelete"/>,
    /// se marca como eliminada en lugar de ser eliminada físicamente.
    /// </summary>
    /// <param name="entity">La entidad que se desea eliminar.</param>
    /// <remarks>
    /// Si la entidad es nula, no se realiza ninguna acción. Si la entidad implementa la interfaz <see cref="IDelete"/>,
    /// se establece su propiedad <c>IsDeleted</c> en <c>true</c>. De lo contrario, la entidad se elimina del conjunto.
    /// </remarks>
    private void Delete(TEntity entity)
    {
        if (entity == null)
            return;
        if (entity is IDelete model)
        {
            model.IsDeleted = true;
            return;
        }
        Set.Remove(entity);
    }

    /// <summary>
    /// Elimina una entidad de forma asíncrona.
    /// </summary>
    /// <param name="entity">La entidad que se desea eliminar.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación de eliminación asíncrona.</returns>
    /// <remarks>
    /// Este método verifica si la entidad es nula antes de intentar eliminarla.
    /// Si la entidad es nula, no se realiza ninguna acción.
    /// </remarks>
    public virtual async Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
            return;
        await RemoveAsync(entity.Id, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina de forma asíncrona un conjunto de entidades identificadas por sus claves.
    /// </summary>
    /// <param name="ids">Una colección de claves que identifican las entidades a eliminar.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <remarks>
    /// Este método lanza una excepción si la operación es cancelada a través del <paramref name="cancellationToken"/>.
    /// También lanza una excepción si se intenta utilizar después de que el objeto ha sido dispuesto.
    /// Si la colección <paramref name="ids"/> es nula, no se realiza ninguna acción.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de eliminación.
    /// </returns>
    /// <exception cref="OperationCanceledException">Se lanza si la operación es cancelada.</exception>
    /// <exception cref="ObjectDisposedException">Se lanza si el objeto ha sido dispuesto.</exception>
    public virtual async Task RemoveAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (ids == null)
            return;
        var entities = await FindByIdsAsync(ids, cancellationToken);
        Delete(entities);
    }

    /// <summary>
    /// Elimina una lista de entidades.
    /// </summary>
    /// <param name="list">La lista de entidades a eliminar.</param>
    /// <remarks>
    /// Este método verifica si la lista es nula o está vacía antes de proceder a eliminar cada entidad.
    /// Si la lista es nula, el método no realiza ninguna acción. Si la lista está vacía, también se omite la eliminación.
    /// </remarks>
    private void Delete(List<TEntity> list)
    {
        if (list == null)
            return;
        if (!list.Any())
            return;
        foreach (var entity in list)
            Delete(entity);
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina de forma asíncrona una colección de entidades especificadas.
    /// </summary>
    /// <param name="entities">Una colección de entidades a eliminar. Si es <c>null</c>, no se realiza ninguna acción.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de eliminación.
    /// </returns>
    /// <remarks>
    /// Este método llama a otro método de eliminación que toma una colección de identificadores de entidad.
    /// Asegúrese de que las entidades proporcionadas no sean <c>null</c> antes de invocar este método.
    /// </remarks>
    /// <seealso cref="RemoveAsync(IEnumerable{TEntity}, CancellationToken)"/>
    public virtual async Task RemoveAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        if (entities == null)
            return;
        await RemoveAsync(entities.Select(t => t.Id), cancellationToken);
    }

    #endregion

    #region ThrowIfDisposed(Si se ha liberado, se lanzará una excepción.)

    /// <summary>
    /// Verifica si el objeto ha sido dispuesto y lanza una excepción si es así.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// Se lanza cuando el objeto ha sido dispuesto y se intenta acceder a él.
    /// </exception>
    protected void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }

    #endregion

    #region Dispose(liberar)

    /// <summary>
    /// Libera los recursos utilizados por la instancia actual de la clase.
    /// </summary>
    /// <remarks>
    /// Este método establece el estado de la instancia como desechada, lo que indica que
    /// los recursos ya no están disponibles para su uso. Es importante llamar a este
    /// método cuando ya no se necesite la instancia para evitar fugas de memoria.
    /// </remarks>
    public void Dispose()
    {
        _disposed = true;
    }

    #endregion
}