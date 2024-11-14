using Util.Applications.Dtos;
using Util.Data;
using Util.Data.Trees;
using Util.Domain;
using Util.Domain.Compare;
using Util.Domain.Trees;
using Util.Properties;

namespace Util.Applications.Trees;

/// <summary>
/// Clase base abstracta para servicios de árbol que manejan entidades, DTOs y consultas.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que representa el nodo del árbol.</typeparam>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos asociado a la entidad.</typeparam>
/// <typeparam name="TQuery">El tipo de consulta utilizada para filtrar o buscar entidades.</typeparam>
public abstract class TreeServiceBase<TEntity, TDto, TQuery>
    : TreeServiceBase<TEntity, TDto, TDto, TDto, TQuery, Guid, Guid?>
    where TEntity : class, ITreeEntity<TEntity, Guid, Guid?>, new()
    where TDto : class, ITreeNode, new()
    where TQuery : class, ITreeQueryParameter
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TreeServiceBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    /// <param name="unitOfWork">La unidad de trabajo que gestiona las transacciones de la base de datos.</param>
    /// <param name="repository">El repositorio que proporciona acceso a los datos de tipo <typeparamref name="TEntity"/>.</param>
    protected TreeServiceBase(IServiceProvider serviceProvider, IUnitOfWork unitOfWork, ITreeRepository<TEntity, Guid, Guid?> repository)
        : base(serviceProvider, unitOfWork, repository)
    {
    }
}

/// <summary>
/// Clase base abstracta para servicios de árbol que proporciona operaciones comunes
/// para entidades, DTOs y solicitudes de creación y actualización.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que representa el nodo del árbol.</typeparam>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos para la entidad.</typeparam>
/// <typeparam name="TCreateRequest">El tipo de objeto que representa la solicitud de creación de una nueva entidad.</typeparam>
/// <typeparam name="TUpdateRequest">El tipo de objeto que representa la solicitud de actualización de una entidad existente.</typeparam>
/// <typeparam name="TQuery">El tipo de objeto que representa la consulta para obtener entidades.</typeparam>
public abstract class TreeServiceBase<TEntity, TDto, TCreateRequest, TUpdateRequest, TQuery>
    : TreeServiceBase<TEntity, TDto, TCreateRequest, TUpdateRequest, TQuery, Guid, Guid?>
    where TEntity : class, ITreeEntity<TEntity, Guid, Guid?>, new()
    where TDto : class, ITreeNode, new()
    where TCreateRequest : class, IRequest, new()
    where TUpdateRequest : class, IDto, new()
    where TQuery : class, ITreeQueryParameter
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TreeServiceBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    /// <param name="unitOfWork">La unidad de trabajo que gestiona las transacciones de la base de datos.</param>
    /// <param name="repository">El repositorio que se utilizará para acceder a los datos de tipo <typeparamref name="TEntity"/>.</param>
    protected TreeServiceBase(IServiceProvider serviceProvider, IUnitOfWork unitOfWork, ITreeRepository<TEntity, Guid, Guid?> repository) : base(serviceProvider, unitOfWork, repository) { }
}

/// <summary>
/// Clase base abstracta para servicios de árbol que proporciona operaciones comunes 
/// para gestionar entidades de tipo árbol.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que representa un nodo en el árbol.</typeparam>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos para la entidad.</typeparam>
/// <typeparam name="TCreateRequest">El tipo de objeto que representa la solicitud para crear una nueva entidad.</typeparam>
/// <typeparam name="TUpdateRequest">El tipo de objeto que representa la solicitud para actualizar una entidad existente.</typeparam>
/// <typeparam name="TQuery">El tipo de objeto que representa una consulta para filtrar entidades.</typeparam>
/// <typeparam name="TKey">El tipo de la clave única que identifica una entidad.</typeparam>
/// <typeparam name="TParentId">El tipo de la identificación del padre de la entidad en el árbol.</typeparam>
public abstract class TreeServiceBase<TEntity, TDto, TCreateRequest, TUpdateRequest, TQuery, TKey, TParentId>
    : TreeQueryServiceBase<TEntity, TDto, TQuery, TKey, TParentId>, ITreeService<TDto, TCreateRequest, TUpdateRequest, TQuery>
    where TEntity : class, ITreeEntity<TEntity, TKey, TParentId>, new()
    where TDto : class, ITreeNode, new()
    where TCreateRequest : class, IRequest, new()
    where TUpdateRequest : class, IDto, new()
    where TQuery : class, ITreeQueryParameter
{

    #region Campo

    private readonly ITreeRepository<TEntity, TKey, TParentId> _repository;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TreeServiceBase{TEntity, TKey, TParentId}"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver dependencias.</param>
    /// <param name="unitOfWork">La unidad de trabajo que gestiona las transacciones de la base de datos.</param>
    /// <param name="repository">El repositorio que proporciona acceso a los datos de tipo <typeparamref name="TEntity"/>.</param>
    protected TreeServiceBase(IServiceProvider serviceProvider, IUnitOfWork unitOfWork, ITreeRepository<TEntity, TKey, TParentId> repository)
        : base(serviceProvider, repository)
    {
        UnitOfWork = unitOfWork;
        _repository = repository;
    }

    #endregion

    #region atributo

    /// <summary>
    /// Representa una unidad de trabajo que permite gestionar las operaciones de acceso a datos.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y se utiliza para acceder a la instancia de la unidad de trabajo 
    /// que se está utilizando en el contexto actual.
    /// </remarks>
    protected IUnitOfWork UnitOfWork { get; }

    #endregion

    #region CreateAsync(Crear)

    /// <summary>
    /// Crea una nueva entidad de forma asíncrona a partir de la solicitud proporcionada.
    /// </summary>
    /// <param name="request">La solicitud de creación que contiene los datos necesarios para crear la entidad.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que es el identificador de la entidad creada.</returns>
    /// <remarks>
    /// Este método valida la solicitud, convierte los datos en una entidad y la persiste en la base de datos.
    /// Después de la creación, se realiza un commit de la transacción y se ejecutan operaciones adicionales si es necesario.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si la solicitud es nula.</exception>
    /// <exception cref="ValidationException">Se lanza si la validación de la solicitud falla.</exception>
    /// <seealso cref="ToEntity(TCreateRequest)"/>
    /// <seealso cref="CreateAsync(TEntity)"/>
    /// <seealso cref="CommitAsync()"/>
    /// <seealso cref="CreateCommitAfterAsync(TEntity)"/>
    public virtual async Task<string> CreateAsync(TCreateRequest request)
    {
        request.CheckNull(nameof(request));
        request.Validate();
        var entity = ToEntity(request);
        entity.CheckNull(nameof(entity));
        await CreateAsync(entity);
        await CommitAsync();
        await CreateCommitAfterAsync(entity);
        return entity.Id.ToString();
    }

    /// <summary>
    /// Convierte un objeto de tipo <typeparamref name="TCreateRequest"/> en un objeto de tipo <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="request">El objeto de solicitud que se va a convertir.</param>
    /// <returns>Un objeto de tipo <typeparamref name="TEntity"/> que representa la conversión del objeto de solicitud.</returns>
    /// <typeparam name="TCreateRequest">El tipo del objeto de solicitud que se va a convertir.</typeparam>
    /// <typeparam name="TEntity">El tipo del objeto de entidad resultante de la conversión.</typeparam>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una lógica de conversión personalizada.
    /// </remarks>
    protected virtual TEntity ToEntity(TCreateRequest request)
    {
        return request.MapTo<TEntity>();
    }

    /// <summary>
    /// Crea de manera asíncrona una nueva entidad en el repositorio.
    /// </summary>
    /// <param name="entity">La entidad que se va a crear.</param>
    /// <returns>Una tarea que representa la operación asíncrona de creación.</returns>
    /// <remarks>
    /// Este método realiza varias operaciones antes y después de la creación de la entidad.
    /// Primero, se ejecuta <see cref="CreateBeforeAsync(TEntity)"/> para realizar cualquier 
    /// configuración previa necesaria. Luego, se inicializa la entidad y se establece su 
    /// ruta en función de su entidad padre. Finalmente, se agrega la entidad al repositorio 
    /// y se ejecuta <see cref="CreateAfterAsync(TEntity)"/> para cualquier acción posterior.
    /// </remarks>
    private async Task CreateAsync(TEntity entity)
    {
        await CreateBeforeAsync(entity);
        entity.Init();
        var parent = await _repository.FindByIdAsync(entity.ParentId);
        entity.InitPath(parent);
        await _repository.AddAsync(entity);
        await CreateAfterAsync(entity);
    }

    /// <summary>
    /// Método virtual que se ejecuta antes de crear una entidad.
    /// </summary>
    /// <param name="entity">La entidad que se va a crear.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método puede ser sobreescrito en una clase derivada para proporcionar
    /// lógica adicional antes de la creación de la entidad.
    /// </remarks>
    protected virtual Task CreateBeforeAsync(TEntity entity)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Crea una tarea que se ejecuta después de que se haya procesado la entidad.
    /// </summary>
    /// <param name="entity">La entidad que se ha procesado.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método es virtual y puede ser anulado en una clase derivada para proporcionar una implementación específica.
    /// </remarks>
    protected virtual Task CreateAfterAsync(TEntity entity)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Realiza la confirmación de los cambios en la unidad de trabajo de forma asíncrona.
    /// </summary>
    /// <remarks>
    /// Este método es virtual, lo que permite que las clases derivadas lo sobreescriban para proporcionar
    /// una implementación personalizada si es necesario.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de confirmación.
    /// </returns>
    /// <seealso cref="UnitOfWork.CommitAsync"/>
    protected virtual async Task CommitAsync()
    {
        await UnitOfWork.CommitAsync();
    }

    /// <summary>
    /// Crea un compromiso de forma asíncrona después de realizar una operación con la entidad especificada.
    /// </summary>
    /// <param name="entity">La entidad sobre la cual se realizará el compromiso.</param>
    /// <returns>Una tarea que representa la operación asíncrona de creación del compromiso.</returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una implementación específica.
    /// </remarks>
    protected virtual Task CreateCommitAfterAsync(TEntity entity)
    {
        return Task.CompletedTask;
    }

    #endregion

    #region UpdateAsync(Modificar)

    /// <inheritdoc />
    /// <summary>
    /// Actualiza una entidad de acuerdo a la solicitud proporcionada.
    /// </summary>
    /// <param name="request">La solicitud de actualización que contiene los datos necesarios para actualizar la entidad.</param>
    /// <exception cref="InvalidOperationException">Se lanza si el identificador de la solicitud está vacío.</exception>
    /// <remarks>
    /// Este método realiza las siguientes acciones:
    /// <list type="bullet">
    /// <item>Verifica que la solicitud no sea nula.</item>
    /// <item>Valida la solicitud para asegurarse de que contiene datos correctos.</item>
    /// <item>Busca la entidad antigua utilizando el identificador proporcionado en la solicitud.</item>
    /// <item>Clona la entidad antigua y la actualiza con los datos de la solicitud.</item>
    /// <item>Calcula los cambios entre la entidad antigua y la nueva.</item>
    /// <item>Actualiza la entidad en la base de datos.</item>
    /// <item>Confirma los cambios realizados.</item>
    /// <item>Ejecuta cualquier lógica adicional después de la confirmación de la actualización.</item>
    /// </list>
    /// </remarks>
    /// <returns>Una tarea que representa la operación asincrónica de actualización.</returns>
    public virtual async Task UpdateAsync(TUpdateRequest request)
    {
        request.CheckNull(nameof(request));
        request.Validate();
        if (request.Id.IsEmpty())
            throw new InvalidOperationException(R.IdIsEmpty);
        var oldEntity = await FindOldEntityAsync(request.Id);
        oldEntity.CheckNull(nameof(oldEntity));
        var entity = ToEntity(oldEntity.Clone(), request);
        entity.CheckNull(nameof(entity));
        var changes = oldEntity.GetChanges(entity);
        await UpdateAsync(entity);
        await CommitAsync();
        await UpdateCommitAfterAsync(entity, changes);
    }

    /// <summary>
    /// Busca una entidad antigua de tipo TEntity de manera asíncrona utilizando su identificador.
    /// </summary>
    /// <param name="id">El identificador de la entidad que se desea buscar.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El valor de la tarea contiene la entidad encontrada de tipo TEntity, o null si no se encontró ninguna entidad con el identificador especificado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un repositorio para realizar la búsqueda de la entidad. Asegúrese de que el repositorio esté correctamente inicializado antes de llamar a este método.
    /// </remarks>
    private async Task<TEntity> FindOldEntityAsync(object id)
    {
        return await _repository.FindByIdAsync(id);
    }

    /// <summary>
    /// Convierte un objeto de solicitud de actualización en una entidad existente.
    /// </summary>
    /// <param name="oldEntity">La entidad existente que se va a actualizar.</param>
    /// <param name="request">El objeto de solicitud que contiene los datos de actualización.</param>
    /// <returns>
    /// La entidad actualizada con los datos del objeto de solicitud.
    /// </returns>
    /// <typeparam name="TEntity">El tipo de la entidad que se está actualizando.</typeparam>
    /// <typeparam name="TUpdateRequest">El tipo del objeto de solicitud que contiene los datos de actualización.</typeparam>
    /// <seealso cref="MapTo(TEntity)"/>
    protected virtual TEntity ToEntity(TEntity oldEntity, TUpdateRequest request)
    {
        return request.MapTo(oldEntity);
    }

    /// <summary>
    /// Actualiza de manera asíncrona una entidad en el repositorio.
    /// </summary>
    /// <param name="entity">La entidad que se va a actualizar.</param>
    /// <returns>Una tarea que representa la operación asíncrona de actualización.</returns>
    /// <remarks>
    /// Este método realiza una serie de pasos antes y después de la actualización de la entidad.
    /// Primero, se ejecuta <see cref="UpdateBeforeAsync(TEntity)"/> para realizar cualquier operación previa necesaria.
    /// Luego, se actualiza la ruta de la entidad mediante <see cref="_repository.UpdatePathAsync(TEntity)"/>.
    /// A continuación, se realiza la actualización de la entidad en sí con <see cref="_repository.UpdateAsync(TEntity)"/>.
    /// Finalmente, se ejecuta <see cref="UpdateAfterAsync(TEntity)"/> para llevar a cabo cualquier operación posterior.
    /// </remarks>
    /// <exception cref="Exception">Se lanza si ocurre un error durante el proceso de actualización.</exception>
    private async Task UpdateAsync(TEntity entity)
    {
        await UpdateBeforeAsync(entity);
        await _repository.UpdatePathAsync(entity);
        await _repository.UpdateAsync(entity);
        await UpdateAfterAsync(entity);
    }

    /// <summary>
    /// Actualiza el estado del objeto antes de realizar una operación asíncrona.
    /// </summary>
    /// <param name="entity">La entidad que se va a actualizar.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar
    /// una lógica de actualización específica antes de la operación asíncrona.
    /// </remarks>
    protected virtual Task UpdateBeforeAsync(TEntity entity)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Actualiza el estado de la entidad después de realizar una operación asíncrona.
    /// </summary>
    /// <param name="entity">La entidad que se va a actualizar.</param>
    /// <returns>Una tarea que representa la operación asíncrona de actualización.</returns>
    protected virtual Task UpdateAfterAsync(TEntity entity)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Actualiza el compromiso después de realizar cambios en la entidad especificada.
    /// </summary>
    /// <param name="entity">La entidad que se va a actualizar.</param>
    /// <param name="changeValues">Una colección de valores que han cambiado en la entidad.</param>
    /// <returns>Una tarea que representa la operación asincrónica de actualización.</returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación específica.
    /// </remarks>
    protected virtual Task UpdateCommitAfterAsync(TEntity entity, ChangeValueCollection changeValues)
    {
        return Task.CompletedTask;
    }

    #endregion

    #region DeleteAsync(Eliminar)

    /// <inheritdoc />
    /// <summary>
    /// Elimina de manera asíncrona las entidades especificadas por sus identificadores.
    /// </summary>
    /// <param name="ids">Una cadena que contiene los identificadores de las entidades a eliminar, separados por comas.</param>
    /// <remarks>
    /// Este método verifica si la cadena de identificadores está vacía y, en caso afirmativo, no realiza ninguna acción.
    /// Luego, busca las entidades correspondientes a los identificadores proporcionados. Si no se encuentran entidades, el método termina sin realizar ninguna acción.
    /// Antes de eliminar las entidades, se pueden realizar operaciones adicionales mediante el método <see cref="DeleteBeforeAsync"/>.
    /// Después de eliminar las entidades, se pueden realizar operaciones adicionales mediante el método <see cref="DeleteAfterAsync"/>.
    /// Finalmente, se confirma la transacción mediante el método <see cref="CommitAsync"/> y se pueden realizar operaciones finales mediante el método <see cref="DeleteCommitAfterAsync"/>.
    /// </remarks>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación.</returns>
    public virtual async Task DeleteAsync(string ids)
    {
        if (ids.IsEmpty())
            return;
        var entities = await _repository.FindByIdsAsync(ids);
        if (entities?.Count == 0)
            return;
        await DeleteBeforeAsync(entities);
        await _repository.RemoveAsync(entities);
        await DeleteAfterAsync(entities);
        await CommitAsync();
        await DeleteCommitAfterAsync(entities);
    }

    /// <summary>
    /// Elimina entidades antes de realizar una operación asíncrona.
    /// </summary>
    /// <param name="entities">Una lista de entidades que se van a eliminar.</param>
    /// <returns>Una tarea que representa la operación de eliminación asíncrona.</returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación específica.
    /// </remarks>
    protected virtual Task DeleteBeforeAsync(List<TEntity> entities)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Elimina de manera asíncrona una lista de entidades después de realizar alguna operación.
    /// </summary>
    /// <param name="entities">La lista de entidades que se deben eliminar.</param>
    /// <returns>Una tarea que representa la operación de eliminación asíncrona.</returns>
    protected virtual Task DeleteAfterAsync(List<TEntity> entities)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Elimina los compromisos asociados a una lista de entidades de forma asíncrona.
    /// </summary>
    /// <param name="entities">Una lista de entidades de tipo <typeparamref name="TEntity"/> que se utilizarán para eliminar los compromisos.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación de compromisos.</returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación específica.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se está manejando.</typeparam>
    protected virtual Task DeleteCommitAfterAsync(List<TEntity> entities)
    {
        return Task.CompletedTask;
    }

    #endregion

    #region EnableAsync(Habilitar)

    /// <summary>
    /// Habilita de manera asíncrona los elementos especificados por sus identificadores.
    /// </summary>
    /// <param name="ids">Una cadena que contiene los identificadores de los elementos a habilitar, separados por comas.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de habilitar los elementos.
    /// </returns>
    /// <remarks>
    /// Este método llama a otro método llamado <see cref="Enable(string, bool)"/> 
    /// con el segundo parámetro establecido en verdadero.
    /// </remarks>
    public virtual async Task EnableAsync(string ids)
    {
        await Enable(ids, true);
    }

    /// <summary>
    /// Habilita o deshabilita entidades basadas en una lista de identificadores.
    /// </summary>
    /// <param name="ids">Una cadena que contiene los identificadores de las entidades a habilitar o deshabilitar.</param>
    /// <param name="enabled">Un valor booleano que indica si las entidades deben ser habilitadas (true) o deshabilitadas (false).</param>
    /// <remarks>
    /// Este método verifica si la lista de identificadores está vacía y si las entidades existen en el repositorio.
    /// Si las entidades están habilitadas, se verifica si se permite habilitarlas mediante el método <see cref="AllowEnable"/>.
    /// Si están deshabilitadas, se verifica si se permite deshabilitarlas mediante el método <see cref="AllowDisable"/>.
    /// Luego, actualiza el estado de cada entidad y realiza un commit de los cambios.
    /// Dependiendo del estado final, se ejecutan métodos adicionales para manejar el commit después de habilitar o deshabilitar.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona.
    /// </returns>
    private async Task Enable(string ids, bool enabled)
    {
        if (ids.IsEmpty())
            return;
        var entities = await _repository.FindByIdsAsync(ids);
        if (entities == null || entities.Count == 0)
            return;
        foreach (var entity in entities)
        {
            if (enabled && await AllowEnable(entity) == false)
                return;
            if (enabled == false && await AllowDisable(entity) == false)
                return;
            entity.Enabled = enabled;
            await _repository.UpdateAsync(entity);
        }
        await CommitAsync();
        if (enabled)
        {
            await EnableCommitAfterAsync(entities);
            return;
        }
        await DisableCommitAfterAsync(entities);
    }

    /// <summary>
    /// Permite determinar si se puede habilitar la entidad especificada.
    /// </summary>
    /// <param name="entity">La entidad que se va a evaluar para habilitar.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica, con un valor booleano que indica si se permite habilitar la entidad.
    /// </returns>
    protected virtual Task<bool> AllowEnable(TEntity entity)
    {
        return Task.FromResult(true);
    }

    /// <summary>
    /// Permite determinar si se puede deshabilitar la entidad especificada.
    /// </summary>
    /// <param name="entity">La entidad que se va a evaluar para permitir o no su deshabilitación.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica, con un valor booleano que indica si se permite deshabilitar la entidad.
    /// </returns>
    protected virtual Task<bool> AllowDisable(TEntity entity)
    {
        return Task.FromResult(true);
    }

    /// <summary>
    /// Habilita la confirmación después de realizar operaciones asincrónicas sobre una lista de entidades.
    /// </summary>
    /// <param name="entities">Una lista de entidades sobre las que se realizará la operación.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    protected virtual Task EnableCommitAfterAsync(List<TEntity> entities)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Desactiva el compromiso de las entidades después de realizar una operación asíncrona.
    /// </summary>
    /// <param name="entities">Una lista de entidades de tipo <typeparamref name="TEntity"/> que se procesarán.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de desactivación del compromiso.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación específica.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de las entidades que se están procesando.</typeparam>
    protected virtual Task DisableCommitAfterAsync(List<TEntity> entities)
    {
        return Task.CompletedTask;
    }

    #endregion

    #region DisableAsync(Deshabilitar)

    /// <summary>
    /// Desactiva los elementos especificados por sus identificadores.
    /// </summary>
    /// <param name="ids">Una cadena que contiene los identificadores de los elementos a desactivar, separados por comas.</param>
    /// <returns>Una tarea que representa la operación asincrónica de desactivación.</returns>
    public virtual Task DisableAsync(string ids)
    {
        return Enable(ids, false);
    }

    #endregion
}