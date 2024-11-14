using Util.Applications.Dtos;
using Util.Data;
using Util.Data.Queries;
using Util.Domain.Compare;
using Util.Domain.Entities;
using Util.Domain.Repositories;
using Util.Helpers;
using Util.Properties;

namespace Util.Applications;

/// <summary>
/// Clase base abstracta para servicios CRUD que maneja entidades de tipo <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad que se manejará en el servicio.</typeparam>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos que se utilizará.</typeparam>
/// <typeparam name="TQuery">El tipo de consulta que se utilizará para filtrar o buscar entidades.</typeparam>
/// <remarks>
/// Esta clase proporciona una implementación base para las operaciones de creación, lectura, actualización y eliminación
/// de entidades, utilizando un identificador de tipo <see cref="Guid"/> como clave primaria.
/// </remarks>
public abstract class CrudServiceBase<TEntity, TDto, TQuery> : CrudServiceBase<TEntity, TDto, TQuery, Guid>
    where TEntity : class, IAggregateRoot<TEntity, Guid>, new()
    where TDto : class, IDto, new()
    where TQuery : IPage
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CrudServiceBase{TEntity}"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    /// <param name="unitOfWork">La unidad de trabajo que se utilizará para gestionar las transacciones.</param>
    /// <param name="repository">El repositorio que se utilizará para acceder a los datos de la entidad.</param>
    protected CrudServiceBase(IServiceProvider serviceProvider, IUnitOfWork unitOfWork, IRepository<TEntity> repository) : base(serviceProvider, unitOfWork, repository)
    {
    }
}

/// <summary>
/// Clase base abstracta para servicios CRUD que proporciona operaciones comunes 
/// para manejar entidades de tipo <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad que se manejará.</typeparam>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos asociado a la entidad.</typeparam>
/// <typeparam name="TQuery">El tipo de consulta que se utilizará para filtrar los datos.</typeparam>
/// <typeparam name="TKey">El tipo de la clave primaria de la entidad.</typeparam>
public abstract class CrudServiceBase<TEntity, TDto, TQuery, TKey>
    : CrudServiceBase<TEntity, TDto, TDto, TDto, TQuery, TKey>, ICrudService<TDto, TQuery>
    where TEntity : class, IAggregateRoot<TEntity, TKey>, new()
    where TDto : class, IDto, new()
    where TQuery : IPage
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CrudServiceBase{TEntity, TKey}"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    /// <param name="unitOfWork">La unidad de trabajo que se utilizará para gestionar las transacciones.</param>
    /// <param name="repository">El repositorio que se utilizará para realizar operaciones CRUD sobre la entidad.</param>
    protected CrudServiceBase(IServiceProvider serviceProvider, IUnitOfWork unitOfWork, IRepository<TEntity, TKey> repository) : base(serviceProvider, unitOfWork, repository)
    {
    }
}

/// <summary>
/// Clase base abstracta para servicios CRUD (Crear, Leer, Actualizar, Eliminar).
/// Proporciona una interfaz común para la manipulación de entidades y sus representaciones DTO.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad que se va a manejar.</typeparam>
/// <typeparam name="TDto">El tipo de DTO (Data Transfer Object) que representa la entidad.</typeparam>
/// <typeparam name="TCreateRequest">El tipo de objeto que representa la solicitud de creación de una nueva entidad.</typeparam>
/// <typeparam name="TUpdateRequest">El tipo de objeto que representa la solicitud de actualización de una entidad existente.</typeparam>
/// <typeparam name="TQuery">El tipo de objeto que representa los parámetros de consulta para filtrar las entidades.</typeparam>
public abstract class CrudServiceBase<TEntity, TDto, TCreateRequest, TUpdateRequest, TQuery>
    : CrudServiceBase<TEntity, TDto, TCreateRequest, TUpdateRequest, TQuery, Guid>
    where TEntity : class, IAggregateRoot<TEntity, Guid>, new()
    where TDto : class, IDto, new()
    where TCreateRequest : class, IRequest, new()
    where TUpdateRequest : class, IDto, new()
    where TQuery : IPage
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CrudServiceBase{TEntity}"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    /// <param name="unitOfWork">La unidad de trabajo que gestiona las transacciones de la base de datos.</param>
    /// <param name="repository">El repositorio que proporciona acceso a los datos de la entidad.</param>
    protected CrudServiceBase(IServiceProvider serviceProvider, IUnitOfWork unitOfWork, IRepository<TEntity, Guid> repository) : base(serviceProvider, unitOfWork, repository)
    {
    }
}

/// <summary>
/// Clase base abstracta para servicios CRUD que define las operaciones básicas 
/// para manejar entidades de tipo <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad que se manejará.</typeparam>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos (DTO) asociado a la entidad.</typeparam>
/// <typeparam name="TCreateRequest">El tipo de objeto que representa la solicitud para crear una nueva entidad.</typeparam>
/// <typeparam name="TUpdateRequest">El tipo de objeto que representa la solicitud para actualizar una entidad existente.</typeparam>
/// <typeparam name="TQuery">El tipo de objeto que representa una consulta para filtrar o buscar entidades.</typeparam>
/// <typeparam name="TKey">El tipo de la clave única que identifica la entidad.</typeparam>
/// <remarks>
/// Esta clase proporciona métodos abstractos que deben ser implementados por las 
/// clases derivadas para realizar operaciones de creación, lectura, actualización 
/// y eliminación (CRUD) sobre las entidades.
/// </remarks>
public abstract class CrudServiceBase<TEntity, TDto, TCreateRequest, TUpdateRequest, TQuery, TKey>
    : QueryServiceBase<TEntity, TDto, TQuery, TKey>, ICrudService<TDto, TCreateRequest, TUpdateRequest, TQuery>
    where TEntity : class, IAggregateRoot<TEntity, TKey>, new()
    where TDto : class, IDto, new()
    where TCreateRequest : class, IRequest, new()
    where TUpdateRequest : class, IDto, new()
    where TQuery : IPage
{

    #region Campo

    private readonly IRepository<TEntity, TKey> _repository;

    #endregion

    #region Método constructor

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CrudServiceBase{TEntity, TKey}"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver dependencias.</param>
    /// <param name="unitOfWork">La unidad de trabajo que gestionará las transacciones.</param>
    /// <param name="repository">El repositorio que se utilizará para acceder a los datos de la entidad.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="unitOfWork"/> o <paramref name="repository"/> son nulos.</exception>
    /// <remarks>
    /// Este constructor establece las propiedades necesarias para el funcionamiento del servicio CRUD,
    /// asegurando que se proporcionen instancias válidas de <paramref name="unitOfWork"/> y <paramref name="repository"/>.
    /// También se obtiene la descripción de la entidad utilizando reflexión.
    /// </remarks>
    protected CrudServiceBase(IServiceProvider serviceProvider, IUnitOfWork unitOfWork, IRepository<TEntity, TKey> repository) : base(serviceProvider, repository)
    {
        UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        EntityDescription = Reflection.GetDisplayNameOrDescription<TEntity>();
    }

    #endregion

    #region atributo

    /// <summary>
    /// Representa una unidad de trabajo que se utiliza para gestionar las transacciones de datos.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y proporciona acceso a la instancia de la unidad de trabajo
    /// que se utiliza para realizar operaciones de persistencia en la base de datos.
    /// </remarks>
    protected IUnitOfWork UnitOfWork { get; }

    /// <summary>
    /// Obtiene la descripción de la entidad.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y proporciona información descriptiva sobre la entidad
    /// a la que pertenece. La descripción puede ser utilizada para mostrar detalles adicionales
    /// en la interfaz de usuario o para fines de registro.
    /// </remarks>
    /// <returns>
    /// Una cadena que representa la descripción de la entidad.
    /// </returns>
    protected string EntityDescription { get; }

    #endregion

    #region CreateAsync(Crear)

    /// <inheritdoc />
    /// <summary>
    /// Crea de manera asíncrona una nueva entidad a partir de la solicitud de creación proporcionada.
    /// </summary>
    /// <param name="request">La solicitud de creación que contiene los datos necesarios para crear la entidad.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que es el identificador de la entidad creada como una cadena.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="request"/> es nulo.</exception>
    /// <remarks>
    /// Este método valida la solicitud, convierte la solicitud en una entidad, 
    /// y luego realiza la creación de la entidad en la base de datos. 
    /// Después de crear la entidad, se confirma la transacción y se ejecutan 
    /// operaciones adicionales necesarias después de la creación.
    /// </remarks>
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
    /// Convierte un objeto de tipo <typeparamref name="TCreateRequest"/> a un objeto de tipo <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="request">El objeto de solicitud que se va a convertir.</param>
    /// <returns>Un objeto de tipo <typeparamref name="TEntity"/> que representa la conversión del objeto de solicitud.</returns>
    /// <typeparam name="TCreateRequest">El tipo del objeto de solicitud que se va a convertir.</typeparam>
    /// <typeparam name="TEntity">El tipo del objeto de entidad resultante de la conversión.</typeparam>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una lógica de conversión personalizada.
    /// </remarks>
    protected virtual TEntity ToEntity(TCreateRequest request)
    {
        return request.MapTo<TEntity>();
    }

    /// <summary>
    /// Crea de manera asíncrona una entidad en el repositorio.
    /// </summary>
    /// <param name="entity">La entidad a crear.</param>
    /// <returns>Una tarea que representa la operación asíncrona de creación.</returns>
    /// <remarks>
    /// Este método realiza las siguientes acciones:
    /// 1. Llama a <see cref="CreateBeforeAsync(TEntity)"/> antes de la creación.
    /// 2. Inicializa la entidad mediante el método <see cref="Init()"/>.
    /// 3. Agrega la entidad al repositorio mediante <see cref="_repository.AddAsync(TEntity)"/>.
    /// 4. Llama a <see cref="CreateAfterAsync(TEntity)"/> después de la creación.
    /// </remarks>
    private async Task CreateAsync(TEntity entity)
    {
        await CreateBeforeAsync(entity);
        entity.Init();
        await _repository.AddAsync(entity);
        await CreateAfterAsync(entity);
    }

    /// <summary>
    /// Método virtual que se ejecuta antes de crear una entidad.
    /// </summary>
    /// <param name="entity">La entidad que se va a crear.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método se puede sobrescribir en una clase derivada para agregar lógica adicional 
    /// antes de la creación de la entidad.
    /// </remarks>
    protected virtual Task CreateBeforeAsync(TEntity entity)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Método que se ejecuta de manera asíncrona después de la creación de una entidad.
    /// </summary>
    /// <param name="entity">La entidad que se ha creado.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar
    /// una funcionalidad adicional después de la creación de la entidad.
    /// </remarks>
    protected virtual Task CreateAfterAsync(TEntity entity)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Realiza la confirmación de los cambios en la unidad de trabajo de manera asíncrona.
    /// </summary>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una implementación personalizada.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de confirmación. 
    /// La tarea se completará cuando se haya confirmado la unidad de trabajo.
    /// </returns>
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
    /// Actualiza de manera asíncrona una entidad utilizando la información proporcionada en el objeto de solicitud.
    /// </summary>
    /// <param name="request">El objeto de solicitud que contiene los datos necesarios para actualizar la entidad.</param>
    /// <exception cref="InvalidOperationException">Se lanza si el identificador de la solicitud está vacío.</exception>
    /// <remarks>
    /// Este método primero valida que el objeto de solicitud no sea nulo y que contenga un identificador válido.
    /// Luego, busca la entidad existente, clona su estado actual y aplica los cambios a la nueva entidad.
    /// Finalmente, se guardan los cambios en la base de datos y se realizan operaciones adicionales después de la actualización.
    /// </remarks>
    /// <returns>Una tarea que representa la operación asíncrona de actualización.</returns>
    /// <seealso cref="FindOldEntityAsync"/>
    /// <seealso cref="ToEntity"/>
    /// <seealso cref="CommitAsync"/>
    /// <seealso cref="UpdateCommitAfterAsync"/>
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
    /// Busca una entidad antigua de tipo <typeparamref name="TEntity"/> de forma asíncrona utilizando su identificador.
    /// </summary>
    /// <param name="id">El identificador de la entidad que se desea buscar.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El valor de la tarea contiene la entidad encontrada de tipo <typeparamref name="TEntity"/> o null si no se encuentra ninguna entidad con el identificador especificado.
    /// </returns>
    /// <typeparam name="TEntity">El tipo de la entidad que se está buscando.</typeparam>
    /// <remarks>
    /// Este método utiliza un repositorio para realizar la búsqueda de la entidad.
    /// </remarks>
    /// <seealso cref="IRepository{TEntity}.FindByIdAsync(object)"/>
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
    /// La entidad actualizada con los valores del objeto de solicitud.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una lógica de mapeo personalizada.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de la entidad que se está actualizando.</typeparam>
    /// <typeparam name="TUpdateRequest">El tipo del objeto de solicitud de actualización.</typeparam>
    protected virtual TEntity ToEntity(TEntity oldEntity, TUpdateRequest request)
    {
        return request.MapTo(oldEntity);
    }

    /// <summary>
    /// Actualiza de forma asíncrona una entidad en el repositorio.
    /// </summary>
    /// <param name="entity">La entidad que se va a actualizar.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de actualización.
    /// </returns>
    /// <remarks>
    /// Este método realiza la actualización en tres etapas: 
    /// primero ejecuta la lógica previa a la actualización, 
    /// luego actualiza la entidad en el repositorio y, 
    /// finalmente, ejecuta la lógica posterior a la actualización.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de la entidad que se va a actualizar.</typeparam>
    private async Task UpdateAsync(TEntity entity)
    {
        await UpdateBeforeAsync(entity);
        await _repository.UpdateAsync(entity);
        await UpdateAfterAsync(entity);
    }

    /// <summary>
    /// Realiza operaciones de actualización antes de que se complete la tarea asíncrona.
    /// </summary>
    /// <param name="entity">La entidad que se va a actualizar.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar 
    /// lógica específica antes de la actualización.
    /// </remarks>
    protected virtual Task UpdateBeforeAsync(TEntity entity)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Actualiza el estado del entidad después de realizar una operación asíncrona.
    /// </summary>
    /// <param name="entity">La entidad que se va a actualizar.</param>
    /// <returns>Una tarea que representa la operación asíncrona de actualización.</returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una lógica de actualización específica.
    /// </remarks>
    protected virtual Task UpdateAfterAsync(TEntity entity)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Actualiza el compromiso después de realizar cambios en la entidad especificada.
    /// </summary>
    /// <param name="entity">La entidad que se va a actualizar.</param>
    /// <param name="changeValues">Colección de valores que han cambiado en la entidad.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica de actualización.
    /// </returns>
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
    /// <returns>Una tarea que representa la operación de eliminación asíncrona.</returns>
    /// <remarks>
    /// Este método primero verifica si la cadena de identificadores está vacía. Si es así, no realiza ninguna acción.
    /// Luego, busca las entidades correspondientes a los identificadores proporcionados. Si no se encuentran entidades,
    /// el método termina sin realizar ninguna acción adicional. Si se encuentran entidades, se ejecutan los métodos
    /// <c>DeleteBeforeAsync</c>, <c>RemoveAsync</c>, <c>DeleteAfterAsync</c>, <c>CommitAsync</c> y <c>DeleteCommitAfterAsync</c>
    /// en ese orden, para asegurar que se manejen adecuadamente las operaciones previas y posteriores a la eliminación.
    /// </remarks>
    /// <seealso cref="DeleteBeforeAsync(IEnumerable{EntityType})"/>
    /// <seealso cref="RemoveAsync(IEnumerable{EntityType})"/>
    /// <seealso cref="DeleteAfterAsync(IEnumerable{EntityType})"/>
    /// <seealso cref="CommitAsync()"/>
    /// <seealso cref="DeleteCommitAfterAsync(IEnumerable{EntityType})"/>
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
    /// Elimina de manera asíncrona las entidades especificadas antes de realizar otra operación.
    /// </summary>
    /// <param name="entities">Una lista de entidades que se van a eliminar.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación.</returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación específica
    /// de la lógica de eliminación antes de realizar otras operaciones.
    /// </remarks>
    protected virtual Task DeleteBeforeAsync(List<TEntity> entities)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Elimina de manera asíncrona una lista de entidades después de realizar ciertas operaciones.
    /// </summary>
    /// <param name="entities">La lista de entidades que se deben eliminar.</param>
    /// <returns>Una tarea que representa la operación de eliminación asíncrona.</returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación específica
    /// de la lógica de eliminación. La implementación predeterminada no realiza ninguna acción.
    /// </remarks>
    protected virtual Task DeleteAfterAsync(List<TEntity> entities)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Elimina de forma asíncrona los compromisos asociados a una lista de entidades.
    /// </summary>
    /// <param name="entities">Una lista de entidades de tipo <typeparamref name="TEntity"/> que se utilizarán para la eliminación de compromisos.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de eliminación de compromisos.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación específica.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se está procesando.</typeparam>
    protected virtual Task DeleteCommitAfterAsync(List<TEntity> entities)
    {
        return Task.CompletedTask;
    }

    #endregion

    #region SaveAsync(Guardar en lote)

    /// <summary>
    /// Guarda una lista de entidades, permitiendo la creación, actualización y eliminación de registros.
    /// </summary>
    /// <param name="creationList">Lista de entidades a crear.</param>
    /// <param name="updateList">Lista de entidades a actualizar.</param>
    /// <param name="deleteList">Lista de entidades a eliminar.</param>
    /// <returns>Una lista de entidades que han sido procesadas.</returns>
    /// <remarks>
    /// Este método maneja las operaciones de guardado de manera asíncrona, asegurando que las entidades se
    /// procesen en el orden correcto. Si todas las listas son nulas, se devuelve una lista vacía.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si alguna de las listas es nula y no se maneja adecuadamente.</exception>
    /// <seealso cref="FilterList(List{TDto}, List{TDto}, List{TDto})"/>
    /// <seealso cref="ToEntities(List{TDto})"/>
    /// <seealso cref="SaveBeforeAsync(List{TEntity}, List{TEntity}, List{TEntity})"/>
    /// <seealso cref="AddListAsync(List{TEntity})"/>
    /// <seealso cref="UpdateListAsync(List{TEntity})"/>
    /// <seealso cref="DeleteListAsync(List{TEntity})"/>
    /// <seealso cref="SaveAfterAsync(List{TEntity}, List{TEntity}, List{TEntity})"/>
    /// <seealso cref="CommitAsync()"/>
    /// <seealso cref="SaveCommitAfterAsync(List{TEntity}, List{TEntity}, List{TEntity}, List{TEntity})"/>
    /// <seealso cref="GetResult(List{TEntity}, List{TEntity})"/>
    public virtual async Task<List<TDto>> SaveAsync(List<TDto> creationList, List<TDto> updateList, List<TDto> deleteList)
    {
        if (creationList == null && updateList == null && deleteList == null)
            return new List<TDto>();
        creationList ??= new List<TDto>();
        updateList ??= new List<TDto>();
        deleteList ??= new List<TDto>();
        FilterList(creationList, updateList, deleteList);
        var addEntities = ToEntities(creationList);
        var updateEntities = ToEntities(updateList);
        var deleteEntities = ToEntities(deleteList);
        await SaveBeforeAsync(addEntities, updateEntities, deleteEntities);
        await AddListAsync(addEntities);
        var changeValues = await UpdateListAsync(updateEntities);
        await DeleteListAsync(deleteEntities);
        await SaveAfterAsync(addEntities, updateEntities, deleteEntities);
        await CommitAsync();
        await SaveCommitAfterAsync(addEntities, updateEntities, deleteEntities, changeValues);
        return GetResult(addEntities, updateEntities);
    }

    /// <summary>
    /// Filtra las listas de creación y actualización eliminando los elementos que están en la lista de eliminación.
    /// </summary>
    /// <param name="creationList">Lista de elementos que se están creando.</param>
    /// <param name="updateList">Lista de elementos que se están actualizando.</param>
    /// <param name="deleteList">Lista de elementos que se deben eliminar.</param>
    /// <remarks>
    /// Este método modifica las listas de creación y actualización en función de los elementos presentes en la lista de eliminación.
    /// Se asume que el método <c>FilterByDeleteList</c> se encarga de realizar el filtrado correspondiente.
    /// </remarks>
    private void FilterList(List<TDto> creationList, List<TDto> updateList, List<TDto> deleteList)
    {
        FilterByDeleteList(creationList, deleteList);
        FilterByDeleteList(updateList, deleteList);
    }

    /// <summary>
    /// Filtra una lista de elementos eliminando aquellos que están presentes en una lista de eliminación.
    /// </summary>
    /// <param name="list">La lista de elementos que se va a filtrar.</param>
    /// <param name="deleteList">La lista de elementos que se deben eliminar de la lista original.</param>
    /// <remarks>
    /// Este método modifica la lista original eliminando los elementos que coinciden en la lista de eliminación.
    /// Se utiliza un bucle para recorrer la lista y se verifica si cada elemento está presente en la lista de eliminación.
    /// Si se encuentra una coincidencia, el elemento se elimina de la lista.
    /// </remarks>
    private void FilterByDeleteList(List<TDto> list, List<TDto> deleteList)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var item = list[i];
            if (deleteList.Any(d => d.Id == item.Id))
                list.Remove(item);
        }
    }

    /// <summary>
    /// Convierte una lista de objetos DTO en una lista de entidades.
    /// </summary>
    /// <param name="dtos">Lista de objetos DTO a convertir.</param>
    /// <returns>Una lista de entidades correspondientes a los objetos DTO proporcionados.</returns>
    /// <remarks>
    /// Este método utiliza la función <see cref="ToEntity"/> para realizar la conversión de cada objeto DTO a su entidad correspondiente.
    /// Además, se asegura de que la lista resultante no contenga duplicados mediante el uso de <see cref="Distinct"/>.
    /// </remarks>
    private List<TEntity> ToEntities(List<TDto> dtos)
    {
        return dtos.Select(ToEntity).Distinct().ToList();
    }

    /// <summary>
    /// Convierte un objeto de tipo TDto a un objeto de tipo TEntity.
    /// </summary>
    /// <param name="request">El objeto de tipo TDto que se va a convertir.</param>
    /// <returns>Un objeto de tipo TEntity que representa la conversión del objeto TDto proporcionado.</returns>
    /// <typeparam name="TEntity">El tipo de entidad a la que se convertirá el objeto.</typeparam>
    /// <typeparam name="TDto">El tipo de objeto de transferencia de datos que se convertirá.</typeparam>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una implementación personalizada.
    /// </remarks>
    protected virtual TEntity ToEntity(TDto request)
    {
        return request.MapTo<TEntity>();
    }

    /// <summary>
    /// Guarda los cambios antes de realizar operaciones de creación, actualización o eliminación.
    /// </summary>
    /// <param name="creationList">Lista de entidades a crear.</param>
    /// <param name="updateList">Lista de entidades a actualizar.</param>
    /// <param name="deleteList">Lista de entidades a eliminar.</param>
    /// <returns>Una tarea que representa la operación asincrónica de guardado.</returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para implementar lógica específica
    /// de guardado antes de realizar las operaciones en la base de datos.
    /// </remarks>
    protected virtual Task SaveBeforeAsync(List<TEntity> creationList, List<TEntity> updateList, List<TEntity> deleteList)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Agrega una lista de entidades de forma asíncrona.
    /// </summary>
    /// <param name="list">La lista de entidades a agregar.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método verifica si la lista está vacía antes de intentar agregar las entidades.
    /// Si la lista no contiene elementos, el método simplemente retorna sin realizar ninguna acción.
    /// Cada entidad en la lista se agrega llamando al método <see cref="CreateAsync(TEntity)"/>.
    /// </remarks>
    private async Task AddListAsync(List<TEntity> list)
    {
        if (list.Count == 0)
            return;
        foreach (var entity in list)
            await CreateAsync(entity);
    }

    /// <summary>
    /// Actualiza una lista de entidades de tipo <typeparamref name="TEntity"/> de manera asíncrona.
    /// </summary>
    /// <param name="list">Lista de entidades a actualizar.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El resultado contiene una lista de tuplas, donde cada tupla consiste en una entidad actualizada y una colección de cambios.</returns>
    /// <remarks>
    /// Si la lista proporcionada está vacía, se devuelve una lista vacía sin realizar ninguna operación.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de las entidades que se están actualizando.</typeparam>
    /// <seealso cref="FindOldEntities(List{TEntity})"/>
    /// <seealso cref="UpdateAsync(TEntity)"/>
    private async Task<List<Tuple<TEntity, ChangeValueCollection>>> UpdateListAsync(List<TEntity> list)
    {
        var result = new List<Tuple<TEntity, ChangeValueCollection>>();
        if (list.Count == 0)
            return result;
        var oldEntities = await FindOldEntities(list);
        foreach (var entity in list)
        {
            var oldEntity = oldEntities.Find(t => t.Id.Equals(entity.Id));
            if (oldEntity != null)
                result.Add(new Tuple<TEntity, ChangeValueCollection>(entity, oldEntity.GetChanges(entity)));
            await UpdateAsync(entity);
        }
        return result;
    }

    /// <summary>
    /// Busca entidades antiguas en el repositorio que coincidan con los identificadores
    /// de la lista proporcionada.
    /// </summary>
    /// <param name="list">Una lista de entidades que se utilizará para filtrar las entidades en el repositorio.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El resultado contiene una lista de
    /// entidades que se encontraron en el repositorio y que tienen identificadores que
    /// coinciden con los de la lista proporcionada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza una expresión lambda para filtrar las entidades en el repositorio
    /// basándose en los identificadores de la lista de entrada.
    /// </remarks>
    private async Task<List<TEntity>> FindOldEntities(List<TEntity> list)
    {
        return await _repository.FindAllAsync(item => list.Select(t => t.Id).Contains(item.Id));
    }

    /// <summary>
    /// Elimina de manera asíncrona una lista de entidades, procesando primero sus hijos.
    /// </summary>
    /// <param name="list">La lista de entidades a eliminar.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de eliminación.
    /// </returns>
    /// <remarks>
    /// Este método verifica si la lista está vacía antes de proceder con la eliminación.
    /// Si la lista contiene entidades, se invoca el método <see cref="DeleteChildrenAsync"/> 
    /// para cada entidad en la lista, asegurando que se eliminen primero los hijos 
    /// antes de eliminar la entidad principal.
    /// </remarks>
    private async Task DeleteListAsync(List<TEntity> list)
    {
        if (list.Count == 0)
            return;
        foreach (var entity in list)
            await DeleteChildrenAsync(entity);
    }

    /// <summary>
    /// Elimina de manera asíncrona los hijos de la entidad padre especificada.
    /// </summary>
    /// <param name="parent">La entidad padre cuyos hijos se van a eliminar.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de eliminación.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar
    /// una implementación personalizada de la eliminación de hijos.
    /// </remarks>
    /// <seealso cref="TEntity"/>
    protected virtual async Task DeleteChildrenAsync(TEntity parent)
    {
        await _repository.RemoveAsync(parent.Id);
    }

    /// <summary>
    /// Guarda los cambios realizados en las listas de entidades después de una operación asíncrona.
    /// </summary>
    /// <param name="creationList">Lista de entidades que se han creado.</param>
    /// <param name="updateList">Lista de entidades que se han actualizado.</param>
    /// <param name="deleteList">Lista de entidades que se han eliminado.</param>
    /// <returns>Una tarea que representa la operación asíncrona de guardado.</returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación específica
    /// de cómo se deben guardar las entidades en una base de datos o en otro almacenamiento.
    /// </remarks>
    protected virtual Task SaveAfterAsync(List<TEntity> creationList, List<TEntity> updateList, List<TEntity> deleteList)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Guarda los cambios realizados en las entidades después de una operación asíncrona.
    /// </summary>
    /// <param name="creationList">Lista de entidades que han sido creadas.</param>
    /// <param name="updateList">Lista de entidades que han sido actualizadas.</param>
    /// <param name="deleteList">Lista de entidades que han sido eliminadas.</param>
    /// <param name="changeValues">Colección de tuplas que contienen entidades y sus valores de cambio asociados.</param>
    /// <returns>Una tarea que representa la operación asíncrona de guardar cambios.</returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una implementación específica.
    /// </remarks>
    protected virtual Task SaveCommitAfterAsync(List<TEntity> creationList, List<TEntity> updateList, List<TEntity> deleteList, List<Tuple<TEntity, ChangeValueCollection>> changeValues)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Combina dos listas de entidades y las convierte en una lista de objetos DTO.
    /// </summary>
    /// <param name="creationList">Lista de entidades que representan las creaciones.</param>
    /// <param name="updateList">Lista de entidades que representan las actualizaciones.</param>
    /// <returns>Una lista de objetos DTO que resultan de la combinación de las listas de creación y actualización.</returns>
    /// <remarks>
    /// Este método utiliza la concatenación de las dos listas y aplica la función de conversión <c>ToDto</c> a cada elemento.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se está procesando.</typeparam>
    /// <typeparam name="TDto">El tipo de objeto DTO al que se convertirá cada entidad.</typeparam>
    protected virtual List<TDto> GetResult(List<TEntity> creationList, List<TEntity> updateList)
    {
        return creationList.Concat(updateList).Select(ToDto).ToList();
    }

    #endregion
}