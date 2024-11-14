using Util.Data.EntityFrameworkCore.ValueComparers;
using Util.Data.EntityFrameworkCore.ValueConverters;
using Util.Dates;
using Util.Domain.Auditing;
using Util.Domain.Compare;
using Util.Domain.Events;
using Util.Domain.Extending;
using Util.Events;
using Util.Exceptions;
using Util.Helpers;
using Util.Properties;
using Util.Sessions;

namespace Util.Data.EntityFrameworkCore;

/// <summary>
/// Clase base abstracta que representa una unidad de trabajo.
/// Esta clase hereda de <see cref="DbContext"/> y proporciona una implementación
/// común para manejar transacciones y operaciones de base de datos.
/// </summary>
/// <remarks>
/// La clase implementa las interfaces <see cref="IUnitOfWork"/> y <see cref="IFilterSwitch"/>.
/// </remarks>
public abstract class UnitOfWorkBase : DbContext, IUnitOfWork, IFilterSwitch
{

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UnitOfWorkBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    /// <param name="options">Las opciones de contexto de base de datos que se utilizarán.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="serviceProvider"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este constructor configura varios servicios necesarios para la unidad de trabajo, incluyendo el entorno de host, 
    /// el administrador de filtros, el administrador de inquilinos, la sesión, el bus de eventos local y el 
    /// administrador de acciones de unidad de trabajo. Si alguno de estos servicios no está disponible, se 
    /// utilizarán instancias nulas predeterminadas.
    /// </remarks>
    protected UnitOfWorkBase(IServiceProvider serviceProvider, DbContextOptions options)
        : base(options)
    {
        ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        Environment = serviceProvider.GetService<IHostEnvironment>();
        FilterManager = ServiceProvider.GetService<IFilterManager>();
        TenantManager = ServiceProvider.GetService<ITenantManager>() ?? NullTenantManager.Instance;
        Session = serviceProvider.GetService<ISession>() ?? NullSession.Instance;
        EventBus = serviceProvider.GetService<ILocalEventBus>() ?? NullEventBus.Instance;
        ActionManager = serviceProvider.GetService<IUnitOfWorkActionManager>() ?? NullUnitOfWorkActionManager.Instance;
        SaveBeforeEvents = new List<IEvent>();
        SaveAfterEvents = new List<IEvent>();
    }

    #endregion

    #region atributo

    /// <summary>
    /// Obtiene el proveedor de servicios que se utiliza para la inyección de dependencias.
    /// </summary>
    /// <remarks>
    /// Este proveedor de servicios permite resolver instancias de servicios registrados
    /// en el contenedor de inyección de dependencias.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="IServiceProvider"/> que representa el proveedor de servicios.
    /// </value>
    protected IServiceProvider ServiceProvider { get; }
    /// <summary>
    /// Obtiene el entorno de host actual.
    /// </summary>
    /// <value>
    /// El entorno de host que proporciona información sobre el entorno de ejecución, 
    /// como el entorno de desarrollo, prueba o producción.
    /// </value>
    protected IHostEnvironment Environment { get; }
    /// <summary>
    /// Obtiene o establece la sesión actual.
    /// </summary>
    /// <value>
    /// La sesión actual de tipo <see cref="ISession"/>.
    /// </value>
    protected ISession Session { get; set; }
    /// <summary>
    /// Obtiene el administrador de filtros.
    /// </summary>
    /// <value>
    /// Un objeto que implementa la interfaz <see cref="IFilterManager"/>.
    /// </value>
    protected IFilterManager FilterManager { get; }
    /// <summary>
    /// Obtiene la instancia del administrador de inquilinos.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso al administrador de inquilinos, que se encarga de gestionar la información y las operaciones relacionadas con los inquilinos en el sistema.
    /// </remarks>
    /// <value>
    /// Una instancia de <see cref="ITenantManager"/> que permite interactuar con los inquilinos.
    /// </value>
    protected ITenantManager TenantManager { get; }
    /// <summary>
    /// Obtiene la instancia del bus de eventos.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso al bus de eventos utilizado para la comunicación entre componentes.
    /// </remarks>
    /// <value>
    /// Una instancia de <see cref="IEventBus"/> que permite la 
    protected IEventBus EventBus { get; }
    /// <summary>
    /// Obtiene la instancia del administrador de acciones de unidad de trabajo.
    /// </summary>
    /// <value>
    /// Una instancia de <see cref="IUnitOfWorkActionManager"/> que gestiona las acciones relacionadas con la unidad de trabajo.
    /// </value>
    protected IUnitOfWorkActionManager ActionManager { get; }
    /// <summary>
    /// Obtiene la lista de eventos que se deben guardar antes de realizar otras operaciones.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y proporciona acceso a los eventos que deben ser 
    /// procesados antes de llevar a cabo cualquier acción que dependa de su estado.
    /// </remarks>
    /// <returns>
    /// Una lista de objetos que implementan la interfaz <see cref="IEvent"/>.
    /// </returns>
    protected List<IEvent> SaveBeforeEvents { get; }
    /// <summary>
    /// Obtiene la lista de eventos que se guardan después de la ejecución de ciertas operaciones.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y proporciona acceso a una colección de eventos que se 
    /// deben procesar o almacenar después de que se complete una acción específica en el sistema.
    /// </remarks>
    /// <value>
    /// Una lista de objetos que implementan la interfaz <see cref="IEvent"/>.
    /// </value>
    protected List<IEvent> SaveAfterEvents { get; }
    /// <summary>
    /// Obtiene un valor que indica si el filtro de eliminación está habilitado.
    /// </summary>
    /// <remarks>
    /// Este valor se determina consultando el <see cref="FilterManager"/> para verificar si el filtro de eliminación
    /// (IDelete) está habilitado. Si <see cref="FilterManager"/> es nulo, se devuelve false.
    /// </remarks>
    /// <returns>
    /// <c>true</c> si el filtro de eliminación está habilitado; de lo contrario, <c>false</c>.
    /// </returns>
    public virtual bool IsDeleteFilterEnabled => FilterManager?.IsEnabled<IDelete>() ?? false;
    /// <summary>
    /// Obtiene un valor que indica si el filtro de inquilinos está habilitado.
    /// </summary>
    /// <value>
    /// <c>true</c> si el filtro de inquilinos está habilitado; de lo contrario, <c>false</c>.
    /// </value>
    /// <remarks>
    /// Este valor se determina a través del administrador de filtros. Si el administrador de filtros es nulo,
    /// se devolverá <c>false</c>.
    /// </remarks>
    public virtual bool IsTenantFilterEnabled => FilterManager?.IsEnabled<ITenant>() ?? false;
    /// <summary>
    /// Obtiene el identificador del inquilino actual.
    /// </summary>
    /// <value>
    /// El identificador del inquilino actual como una cadena.
    /// </value>
    /// <remarks>
    /// Este miembro es virtual y puede ser sobreescrito en una clase derivada.
    /// </remarks>
    /// <seealso cref="TenantManager"/>
    public virtual string CurrentTenantId => TenantManager.GetTenantId();
    /// <summary>
    /// Indica si se debe recortar las cadenas de texto.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es virtual, lo que permite que las clases derivadas puedan 
    /// sobrescribir su comportamiento si es necesario.
    /// </remarks>
    /// <returns>
    /// Devuelve <c>true</c> si las cadenas deben ser recortadas; de lo contrario, <c>false</c>.
    /// </returns>
    protected virtual bool IsTrimString => true;

    #endregion

    #region Operaciones auxiliares

    /// <summary>
    /// Obtiene el identificador del usuario actual desde la sesión.
    /// </summary>
    /// <returns>
    /// Un string que representa el identificador del usuario.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación diferente.
    /// </remarks>
    protected virtual string GetUserId()
    {
        return Session.UserId;
    }

    #endregion

    #region EnableFilter(Habilitar filtros)

    /// <summary>
    /// Habilita un filtro del tipo especificado.
    /// </summary>
    /// <typeparam name="TFilterType">El tipo del filtro que se desea habilitar. Debe ser una clase.</typeparam>
    /// <remarks>
    /// Este método verifica si el administrador de filtros (FilterManager) está disponible antes de intentar habilitar el filtro.
    /// </remarks>
    /// <seealso cref="FilterManager"/>
    public void EnableFilter<TFilterType>() where TFilterType : class
    {
        FilterManager?.EnableFilter<TFilterType>();
    }

    #endregion

    #region DisableFilter(Desactivar el filtro)

    /// <summary>
    /// Desactiva un filtro del tipo especificado.
    /// </summary>
    /// <typeparam name="TFilterType">El tipo del filtro que se desea desactivar. Debe ser una clase.</typeparam>
    /// <returns>
    /// Un objeto que implementa <see cref="IDisposable"/> que se puede utilizar para reactivar el filtro o liberar recursos.
    /// </returns>
    /// <remarks>
    /// Si <see cref="FilterManager"/> es nulo, se devuelve una acción de disposición nula.
    /// </remarks>
    /// <seealso cref="FilterManager"/>
    public IDisposable DisableFilter<TFilterType>() where TFilterType : class
    {
        if (FilterManager == null)
            return DisposeAction.Null;
        return FilterManager.DisableFilter<TFilterType>();
    }

    #endregion

    #region OnConfiguring(Configuración)

    /// <summary>
    /// Configura las opciones del contexto de base de datos.
    /// </summary>
    /// <param name="optionsBuilder">El constructor de opciones para el contexto de base de datos.</param>
    /// <remarks>
    /// Este método se llama cuando se configura el contexto de base de datos. 
    /// Se pueden agregar configuraciones adicionales como logging y tenant.
    /// </remarks>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ConfigLog(optionsBuilder);
        ConfigTenant(optionsBuilder);
    }

    #endregion

    #region ConfigLog(Configuración de registro)

    /// <summary>
    /// Configura el registro de errores para el contexto de la base de datos.
    /// </summary>
    /// <remarks>
    /// Este método habilita los errores detallados y el registro de datos sensibles
    /// solo si el entorno no es de producción. Si el entorno es nulo o es producción,
    /// no se realizan cambios en la configuración del registro.
    /// </remarks>
    /// <param name="optionsBuilder">El objeto <see cref="DbContextOptionsBuilder"/> 
    /// que se utiliza para configurar el contexto de la base de datos.</param>
    protected virtual void ConfigLog(DbContextOptionsBuilder optionsBuilder)
    {
        if (Environment == null)
            return;
        if (Environment.IsProduction())
            return;
        optionsBuilder.EnableDetailedErrors().EnableSensitiveDataLogging();
    }

    #endregion

    #region ConfigTenant(Configurar inquilinos)

    /// <summary>
    /// Configura la cadena de conexión del inquilino (tenant) en el constructor de contexto de base de datos.
    /// </summary>
    /// <param name="optionsBuilder">El constructor de opciones para el contexto de base de datos.</param>
    /// <remarks>
    /// Este método verifica si el gestor de inquilinos está habilitado y si se permiten múltiples bases de datos.
    /// Si el inquilino actual es nulo o la cadena de conexión está vacía, no se realiza ninguna configuración.
    /// </remarks>
    /// <seealso cref="TenantManager"/>
    protected virtual void ConfigTenant(DbContextOptionsBuilder optionsBuilder)
    {
        if (TenantManager.Enabled() == false)
            return;
        if (TenantManager.AllowMultipleDatabase() == false)
            return;
        var tenant = TenantManager.GetTenant();
        if (tenant == null)
            return;
        var name = ConnectionStringNameAttribute.GetName(GetType());
        var connectionString = tenant.ConnectionStrings.GetConnectionString(name);
        if (connectionString.IsEmpty())
            return;
        ConfigTenantConnectionString(optionsBuilder, connectionString);
    }

    /// <summary>
    /// Configura la cadena de conexión para el inquilino en el contexto de la base de datos.
    /// </summary>
    /// <param name="optionsBuilder">El objeto que se utiliza para construir las opciones del contexto de la base de datos.</param>
    /// <param name="connectionString">La cadena de conexión que se utilizará para conectarse a la base de datos del inquilino.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una lógica de configuración específica.
    /// </remarks>
    protected virtual void ConfigTenantConnectionString(DbContextOptionsBuilder optionsBuilder, string connectionString)
    {
    }

    #endregion

    #region OnModelCreating(Configurar el modelo)

    /// <summary>
    /// Configura el modelo de datos cuando se está creando el modelo.
    /// </summary>
    /// <param name="modelBuilder">El objeto <see cref="ModelBuilder"/> que se utiliza para construir el modelo.</param>
    /// <remarks>
    /// Este método se sobrescribe para aplicar configuraciones personalizadas a las entidades del modelo,
    /// incluyendo filtros, propiedades adicionales, control de versiones, eliminación lógica,
    /// identificación de inquilinos, manejo de fechas en formato UTC y recorte de cadenas.
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ApplyConfigurations(modelBuilder);
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            ApplyFilters(modelBuilder, entityType);
            ApplyExtraProperties(modelBuilder, entityType);
            ApplyVersion(modelBuilder, entityType);
            ApplyIsDeleted(modelBuilder, entityType);
            ApplyTenantId(modelBuilder, entityType);
            ApplyUtc(modelBuilder, entityType);
            ApplyTrimString(modelBuilder, entityType);
        }
    }

    #endregion

    #region ApplyConfigurations(Configurar tipo de entidad)

    /// <summary>
    /// Aplica las configuraciones de entidad desde el ensamblado actual.
    /// </summary>
    /// <param name="modelBuilder">El objeto <see cref="ModelBuilder"/> que se utiliza para configurar el modelo.</param>
    /// <remarks>
    /// Este método se utiliza para cargar automáticamente todas las configuraciones de entidad 
    /// que se encuentran en el ensamblado donde se define la clase que contiene este método.
    /// </remarks>
    protected virtual void ApplyConfigurations(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    #endregion

    #region ApplyFilters(Configurar el filtro)

    /// <summary>
    /// Aplica filtros a un modelo de entidad especificado.
    /// </summary>
    /// <param name="modelBuilder">El objeto <see cref="ModelBuilder"/> que se utiliza para configurar el modelo.</param>
    /// <param name="entityType">El tipo de entidad <see cref="IMutableEntityType"/> al que se le aplicarán los filtros.</param>
    /// <remarks>
    /// Este método busca un método privado llamado <c>ApplyFiltersImp</c> en la clase actual y lo invoca
    /// utilizando el tipo de entidad proporcionado. Esto permite aplicar filtros específicos para cada tipo de entidad.
    /// </remarks>
    protected virtual void ApplyFilters(ModelBuilder modelBuilder, IMutableEntityType entityType)
    {
        var method = GetType().GetMethod(nameof(ApplyFiltersImp), BindingFlags.Instance | BindingFlags.NonPublic);
        method?.MakeGenericMethod(entityType.ClrType).Invoke(this, new object[] { modelBuilder });
    }

    /// <summary>
    /// Aplica filtros a la entidad especificada en el <see cref="ModelBuilder"/>.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad sobre la cual se aplicarán los filtros.</typeparam>
    /// <param name="modelBuilder">El <see cref="ModelBuilder"/> que se utiliza para configurar la entidad.</param>
    /// <remarks>
    /// Este método verifica si el <see cref="FilterManager"/> está habilitado para la entidad especificada.
    /// Si es así, se obtiene la expresión de filtro correspondiente y se aplica al modelo.
    /// </remarks>
    /// <seealso cref="FilterManager"/>
    protected virtual void ApplyFiltersImp<TEntity>(ModelBuilder modelBuilder) where TEntity : class
    {
        if (FilterManager == null)
            return;
        if (FilterManager.IsEntityEnabled<TEntity>() == false)
            return;
        var expression = GetFilterExpression<TEntity>();
        if (expression == null)
            return;
        modelBuilder.Entity<TEntity>().HasQueryFilter(expression);
    }

    /// <summary>
    /// Obtiene una expresión de filtro para el tipo de entidad especificado.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad para el cual se genera la expresión de filtro.</typeparam>
    /// <returns>
    /// Una expresión que representa el filtro para la entidad especificada.
    /// </returns>
    /// <remarks>
    /// Esta función es virtual y puede ser sobreescrita en una clase derivada para proporcionar una lógica de filtrado personalizada.
    /// </remarks>
    /// <seealso cref="FilterManager.GetExpression{TEntity}(object)"/>
    protected virtual Expression<Func<TEntity, bool>> GetFilterExpression<TEntity>() where TEntity : class
    {
        return FilterManager.GetExpression<TEntity>(this);
    }

    #endregion

    #region ApplyExtraProperties(Configurar propiedades extendidas)

    /// <summary>
    /// Aplica propiedades adicionales a un tipo de entidad en el modelo.
    /// </summary>
    /// <param name="modelBuilder">El objeto <see cref="ModelBuilder"/> que se utiliza para configurar el modelo.</param>
    /// <param name="entityType">El tipo de entidad mutable al que se le aplicarán las propiedades adicionales.</param>
    /// <remarks>
    /// Este método verifica si el tipo de entidad implementa la interfaz <see cref="IExtraProperties"/>.
    /// Si es así, se añade una propiedad llamada "ExtraProperties" con un convertidor y un comparador de valores específicos.
    /// </remarks>
    protected virtual void ApplyExtraProperties(ModelBuilder modelBuilder, IMutableEntityType entityType)
    {
        if (typeof(IExtraProperties).IsAssignableFrom(entityType.ClrType) == false)
            return;
        modelBuilder.Entity(entityType.ClrType)
            .Property("ExtraProperties")
            .HasColumnName("ExtraProperties")
            .HasComment(R.ExtraProperties)
            .HasConversion(new ExtraPropertiesValueConverter())
            .Metadata.SetValueComparer(new ExtraPropertyDictionaryValueComparer());
    }

    #endregion

    #region ApplyVersion(Configurar el bloqueo optimista)

    /// <summary>
    /// Aplica la configuración de versión a una entidad en el modelo.
    /// </summary>
    /// <param name="modelBuilder">El constructor de modelo que se utiliza para configurar la entidad.</param>
    /// <param name="entityType">El tipo de entidad que se va a configurar.</param>
    /// <remarks>
    /// Este método verifica si el tipo de entidad implementa la interfaz <see cref="IVersion"/>.
    /// Si es así, se agrega una propiedad "Version" a la entidad, que se utiliza como un token de concurrencia.
    /// </remarks>
    protected virtual void ApplyVersion(ModelBuilder modelBuilder, IMutableEntityType entityType)
    {
        if (typeof(IVersion).IsAssignableFrom(entityType.ClrType) == false)
            return;
        modelBuilder.Entity(entityType.ClrType)
            .Property("Version")
            .HasColumnName("Version")
            .HasComment(R.Version)
            .IsConcurrencyToken();
    }

    #endregion

    #region ApplyIsDeleted(Configurar eliminación lógica)

    /// <summary>
    /// Aplica la configuración de la propiedad "IsDeleted" a la entidad especificada en el modelo.
    /// </summary>
    /// <param name="modelBuilder">El objeto <see cref="ModelBuilder"/> que se utiliza para configurar el modelo.</param>
    /// <param name="entityType">El tipo de entidad <see cref="IMutableEntityType"/> que se está configurando.</param>
    /// <remarks>
    /// Este método verifica si el tipo de entidad implementa la interfaz <see cref="IDelete"/>.
    /// Si es así, se agrega la propiedad "IsDeleted" a la entidad con el nombre de columna correspondiente y un comentario.
    /// </remarks>
    protected virtual void ApplyIsDeleted(ModelBuilder modelBuilder, IMutableEntityType entityType)
    {
        if (typeof(IDelete).IsAssignableFrom(entityType.ClrType) == false)
            return;
        modelBuilder.Entity(entityType.ClrType)
            .Property("IsDeleted")
            .HasColumnName("IsDeleted")
            .HasComment(R.IsDeleted);
    }

    #endregion

    #region ApplyTenantId(Configurar la identificación del inquilino)

    /// <summary>
    /// Aplica el identificador de inquilino a la configuración del modelo.
    /// </summary>
    /// <param name="modelBuilder">El constructor de modelo que se utiliza para configurar las entidades.</param>
    /// <param name="entityType">El tipo de entidad que se está configurando.</param>
    /// <remarks>
    /// Este método verifica si el tipo de entidad implementa la interfaz <see cref="ITenant"/>.
    /// Si es así, se agrega una propiedad "TenantId" a la entidad, que se mapea a la columna "TenantId" en la base de datos.
    /// </remarks>
    protected virtual void ApplyTenantId(ModelBuilder modelBuilder, IMutableEntityType entityType)
    {
        if (typeof(ITenant).IsAssignableFrom(entityType.ClrType) == false)
            return;
        modelBuilder.Entity(entityType.ClrType)
            .Property("TenantId")
            .HasColumnName("TenantId")
            .HasComment(R.TenantId);
    }

    #endregion

    #region ApplyUtc(Configurar la fecha UTC)

    /// <summary>
    /// Aplica la conversión de propiedades de tipo <see cref="DateTime"/> y <see cref="Nullable{DateTime}"/> 
    /// a UTC en el modelo de datos.
    /// </summary>
    /// <param name="modelBuilder">El objeto <see cref="ModelBuilder"/> que se utiliza para configurar el modelo.</param>
    /// <param name="entityType">El tipo de entidad que se está configurando.</param>
    /// <remarks>
    /// Este método verifica si la opción de uso de UTC está habilitada. Si no lo está, no realiza ninguna acción.
    /// Si está habilitada, busca todas las propiedades de tipo <see cref="DateTime"/> y <see cref="Nullable{DateTime}"/> 
    /// que son escribibles y que no están marcadas con el atributo <see cref="NotMappedAttribute"/>. 
    /// Luego, aplica un convertidor de valores para asegurar que se almacenen en UTC.
    /// </remarks>
    protected virtual void ApplyUtc(ModelBuilder modelBuilder, IMutableEntityType entityType)
    {
        if (TimeOptions.IsUseUtc == false)
            return;
        var properties = entityType.ClrType.GetProperties()
            .Where(property => (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?)) && property.CanWrite &&
                                property.GetCustomAttribute<NotMappedAttribute>() == null)
            .ToList();
        properties.ForEach(property =>
        {
            modelBuilder.Entity(entityType.ClrType)
                .Property(property.Name)
                .HasConversion(new DateTimeValueConverter());
        });
    }

    #endregion

    #region ApplyTrimString(Configurar para eliminar cadenas vacías)

    /// <summary>
    /// Aplica un convertidor de cadena recortada a las propiedades de tipo cadena de una entidad.
    /// </summary>
    /// <param name="modelBuilder">El objeto <see cref="ModelBuilder"/> que se utiliza para configurar el modelo.</param>
    /// <param name="entityType">El tipo de entidad <see cref="IMutableEntityType"/> al que se le aplicará el recorte de cadenas.</param>
    /// <remarks>
    /// Este método verifica si la opción de recortar cadenas está habilitada. Si no lo está, el método termina sin realizar ninguna acción.
    /// Si está habilitada, busca todas las propiedades de tipo cadena que son escribibles y que no están marcadas como no mapeadas.
    /// Luego, aplica un convertidor que recorta los espacios en blanco de las cadenas antes de almacenarlas en la base de datos.
    /// </remarks>
    protected virtual void ApplyTrimString(ModelBuilder modelBuilder, IMutableEntityType entityType)
    {
        if (IsTrimString == false)
            return;
        var properties = entityType.ClrType.GetProperties()
            .Where(property => property.PropertyType == typeof(string) && property.CanWrite &&
                                property.GetCustomAttribute<NotMappedAttribute>() == null)
            .ToList();
        properties.ForEach(property =>
        {
            modelBuilder.Entity(entityType.ClrType)
                .Property(property.Name)
                .HasConversion(new TrimStringValueConverter());
        });
    }

    #endregion

    #region CommitAsync(Enviar)

    /// <summary>
    /// Guarda los cambios de manera asíncrona en el contexto de la base de datos.
    /// </summary>
    /// <returns>
    /// Un <see cref="Task{int}"/> que representa la operación asíncrona, 
    /// que contiene el número de entidades afectadas por la operación de guardado.
    /// </returns>
    /// <exception cref="ConcurrencyException">
    /// Se lanza cuando ocurre una excepción de concurrencia al intentar guardar los cambios.
    /// </exception>
    /// <remarks>
    /// Este método utiliza <see cref="SaveChangesAsync"/> para realizar la operación de guardado. 
    /// Si se produce una excepción de concurrencia, se captura y se lanza una nueva excepción 
    /// de tipo <see cref="ConcurrencyException"/>.
    /// </remarks>
    public async Task<int> CommitAsync()
    {
        try
        {
            return await SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException(ex);
        }
    }

    #endregion

    #region SaveChangesAsync(Guardar)

    /// <summary>
    /// Guarda los cambios de forma asíncrona en el contexto actual.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación opcional para la operación asíncrona.</param>
    /// <returns>
    /// Un <see cref="Task{Int32}"/> que representa la tarea asíncrona que contiene el número de cambios realizados en la base de datos.
    /// </returns>
    /// <remarks>
    /// Este método sobrescribe el método <see cref="DbContext.SaveChangesAsync(CancellationToken)"/> 
    /// para permitir la ejecución de lógica adicional antes y después de guardar los cambios.
    /// </remarks>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await SaveChangesBefore();
        var result = await base.SaveChangesAsync(cancellationToken);
        await SaveChangesAfter();
        return result;
    }

    #endregion

    #region SaveChangesBefore(Operaciones antes de guardar)

    /// <summary>
    /// Guarda los cambios antes de realizar la operación de guardado en la base de datos.
    /// </summary>
    /// <remarks>
    /// Este método recorre todas las entradas en el ChangeTracker y actualiza el ID del inquilino, 
    /// agrega eventos de dominio y maneja las entradas según su estado (Agregado, Modificado, Eliminado).
    /// Al final, 
    protected virtual async Task SaveChangesBefore()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            UpdateTenantId(entry);
            AddDomainEvents(entry);
            switch (entry.State)
            {
                case EntityState.Added:
                    AddBefore(entry);
                    break;
                case EntityState.Modified:
                    UpdateBefore(entry);
                    break;
                case EntityState.Deleted:
                    DeleteBefore(entry);
                    break;
            }
        }
        await PublishSaveBeforeEventsAsync();
    }

    #endregion

    #region UpdateTenantId(Actualizar la identificación del inquilino)

    /// <summary>
    /// Actualiza el identificador del inquilino (tenant) en la entidad proporcionada.
    /// </summary>
    /// <param name="entry">La entrada de la entidad que se va a actualizar.</param>
    /// <remarks>
    /// Este método verifica si el manejo de inquilinos está habilitado y si la entidad
    /// implementa la interfaz <see cref="ITenant"/>. Si el identificador del inquilino
    /// está disponible, se asigna a la propiedad <c>TenantId</c> de la entidad.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="entry"/> es null.</exception>
    protected virtual void UpdateTenantId(EntityEntry entry)
    {
        if (TenantManager.Enabled() == false)
            return;
        if (entry.Entity is not ITenant tenant)
            return;
        var tenantId = TenantManager.GetTenantId();
        if (tenantId.IsEmpty())
            return;
        tenant.TenantId = tenantId;
    }

    #endregion

    #region AddDomainEvents(Agregar eventos de dominio)

    /// <summary>
    /// Agrega eventos de dominio a las colecciones correspondientes según su tipo.
    /// </summary>
    /// <param name="entry">La entrada de entidad que contiene el evento de dominio.</param>
    /// <remarks>
    /// Este método verifica si la entidad asociada a la entrada implementa la interfaz <see cref="IDomainEventManager"/>.
    /// Si es así, se revisan los eventos de dominio asociados. Los eventos que implementan <see cref="IIntegrationEvent"/> 
    /// se agregan a la colección <see cref="SaveAfterEvents"/>, mientras que los demás se agregan a <see cref="SaveBeforeEvents"/>.
    /// Finalmente, se limpian los eventos de dominio de la entidad.
    /// </remarks>
    protected virtual void AddDomainEvents(EntityEntry entry)
    {
        if (entry.Entity is not IDomainEventManager eventManager)
            return;
        if (eventManager.DomainEvents == null)
            return;
        foreach (var domainEvent in eventManager.DomainEvents)
        {
            if (domainEvent is IIntegrationEvent)
            {
                SaveAfterEvents.Add(domainEvent);
                continue;
            }
            SaveBeforeEvents.Add(domainEvent);
        }
        eventManager.ClearDomainEvents();
    }

    #endregion

    #region AddBefore(Agregar operación anterior)

    /// <summary>
    /// Realiza las configuraciones necesarias en una entidad antes de ser añadida.
    /// </summary>
    /// <param name="entry">La entrada de la entidad que se va a modificar.</param>
    /// <remarks>
    /// Este método establece la auditoría de creación y modificación, 
    /// así como la versión de la entidad. Además, agrega un evento 
    /// que indica que la entidad ha sido creada.
    /// </remarks>
    protected virtual void AddBefore(EntityEntry entry)
    {
        SetCreationAudited(entry.Entity);
        SetModificationAudited(entry.Entity);
        SetVersion(entry.Entity);
        AddEntityCreatedEvent(entry.Entity);
    }

    #endregion

    #region UpdateBefore(Operación antes de la modificación)

    /// <summary>
    /// Actualiza la entidad antes de que se realice la operación de guardado.
    /// </summary>
    /// <param name="entry">La entrada de la entidad que se va a actualizar.</param>
    protected virtual void UpdateBefore(EntityEntry entry)
    {
        SetModificationAudited(entry.Entity);
        SetVersion(entry.Entity);
        AddEntityUpdatedEvent(entry);
    }

    #endregion

    #region DeleteBefore(Eliminar operación anterior)

    /// <summary>
    /// Elimina la entidad antes de realizar la operación correspondiente.
    /// </summary>
    /// <param name="entry">La entrada de la entidad que se va a eliminar.</param>
    /// <remarks>
    /// Este método se encarga de auditar la modificación de la entidad y de agregar un evento
    /// que indica que la entidad ha sido eliminada. Se debe llamar antes de la eliminación real
    /// de la entidad en la base de datos.
    /// </remarks>
    protected virtual void DeleteBefore(EntityEntry entry)
    {
        SetModificationAudited(entry.Entity);
        AddEntityDeletedEvent(entry.Entity);
    }

    #endregion

    #region SetCreationAudited(Configurar la creación de información de auditoría)

    /// <summary>
    /// Establece la información de auditoría de creación para una entidad.
    /// </summary>
    /// <param name="entity">La entidad para la cual se establecerá la información de auditoría.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar 
    /// una implementación personalizada de la auditoría de creación.
    /// </remarks>
    /// <seealso cref="CreationAuditedSetter"/>
    protected virtual void SetCreationAudited(object entity)
    {
        CreationAuditedSetter.Set(entity, GetUserId());
    }

    #endregion

    #region SetModificationAudited(Configurar la modificación de la información de auditoría)

    /// <summary>
    /// Establece la auditoría de modificación para la entidad especificada.
    /// </summary>
    /// <param name="entity">La entidad para la cual se establecerá la auditoría de modificación.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una funcionalidad adicional
    /// o modificar el comportamiento de la auditoría de modificación.
    /// </remarks>
    protected virtual void SetModificationAudited(object entity)
    {
        ModificationAuditedSetter.Set(entity, GetUserId());
    }

    #endregion

    #region SetVersion(Configurar el número de versión)

    /// <summary>
    /// Establece la versión en el objeto proporcionado si este implementa la interfaz <see cref="IVersion"/>.
    /// </summary>
    /// <param name="obj">El objeto al que se le establecerá la versión.</param>
    /// <remarks>
    /// Este método verifica si el objeto dado implementa la interfaz <see cref="IVersion"/>.
    /// Si es así, intenta obtener la versión actual mediante el método <see cref="GetVersion"/>.
    /// Si se obtiene una versión válida, se asigna a la propiedad <see cref="IVersion.Version"/> del objeto.
    /// </remarks>
    /// <seealso cref="IVersion"/>
    /// <seealso cref="GetVersion"/>
    protected virtual void SetVersion(object obj)
    {
        if (!(obj is IVersion entity))
            return;
        var version = GetVersion();
        if (version == null)
            return;
        entity.Version = version;
    }

    #endregion

    #region GetVersion(Obtener número de versión)

    /// <summary>
    /// Obtiene la versión en forma de un arreglo de bytes.
    /// </summary>
    /// <returns>
    /// Un arreglo de bytes que representa un identificador único (GUID) convertido a una cadena en formato UTF-8.
    /// </returns>
    protected virtual byte[] GetVersion()
    {
        return Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
    }

    #endregion

    #region AddEntityChangedEvent(Agregar evento de cambio de entidad)

    /// <summary>
    /// Agrega un evento de cambio de entidad a la colección de eventos a guardar.
    /// </summary>
    /// <param name="entity">La entidad que ha cambiado.</param>
    /// <param name="changeType">El tipo de cambio que se ha realizado en la entidad.</param>
    /// <param name="changeValues">Una colección de valores que han cambiado en la entidad. Este parámetro es opcional.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar
    /// una funcionalidad adicional o personalizada al manejar eventos de cambio de entidad.
    /// </remarks>
    /// <seealso cref="EntityChangeType"/>
    /// <seealso cref="ChangeValueCollection"/>
    /// <seealso cref="IEvent"/>
    protected virtual void AddEntityChangedEvent(object entity, EntityChangeType changeType, ChangeValueCollection changeValues = null)
    {
        var eventType = typeof(EntityChangedEvent<>).MakeGenericType(entity.GetType());
        var @event = Reflection.CreateInstance<IEvent>(eventType, entity, changeType, changeValues);
        SaveAfterEvents.Add(@event);
    }

    #endregion

    #region AddEntityCreatedEvent(Agregar evento de creación de entidad)

    /// <summary>
    /// Agrega un evento que indica que se ha creado una entidad.
    /// </summary>
    /// <param name="entity">La entidad que ha sido creada.</param>
    /// <remarks>
    /// Este método crea un evento de tipo <see cref="EntityCreatedEvent{T}"/> 
    /// a partir de la entidad proporcionada y lo añade a la lista de eventos 
    /// que se guardarán después. Además, se registra un cambio de entidad 
    /// del tipo <see cref="EntityChangeType.Created"/>.
    /// </remarks>
    protected virtual void AddEntityCreatedEvent(object entity)
    {
        var @event = CreateEntityEvent(typeof(EntityCreatedEvent<>), entity);
        SaveAfterEvents.Add(@event);
        AddEntityChangedEvent(entity, EntityChangeType.Created);
    }

    /// <summary>
    /// Crea una instancia de un evento de tipo genérico basado en el tipo de evento y la entidad proporcionados.
    /// </summary>
    /// <param name="eventType">El tipo de evento que se desea crear.</param>
    /// <param name="entity">La entidad asociada al evento.</param>
    /// <param name="parameter">Un parámetro opcional adicional que puede ser utilizado al crear el evento.</param>
    /// <returns>Una instancia de <see cref="IEvent"/> correspondiente al tipo de evento creado.</returns>
    /// <remarks>
    /// Este método utiliza reflexión para crear una instancia del evento genérico, 
    /// permitiendo que se pase una entidad y un parámetro opcional.
    /// </remarks>
    protected IEvent CreateEntityEvent(Type eventType, object entity, object parameter = null)
    {
        var eventGenericType = eventType.MakeGenericType(entity.GetType());
        if (parameter == null)
            return Reflection.CreateInstance<IEvent>(eventGenericType, entity);
        return Reflection.CreateInstance<IEvent>(eventGenericType, entity, parameter);
    }

    #endregion

    #region AddEntityUpdatedEvent(Agregar evento de modificación de entidad)

    /// <summary>
    /// Maneja el evento de actualización de una entidad.
    /// </summary>
    /// <param name="entry">La entrada de la entidad que ha sido actualizada.</param>
    /// <remarks>
    /// Este método verifica si la entidad ha sido marcada como eliminada. Si es así, 
    /// se llama al método <see cref="AddEntityDeletedEvent(EntityEntry)"/> para manejar 
    /// la eliminación. De lo contrario, se obtienen los valores de cambio de la entidad 
    /// y se crea un evento de actualización que se agrega a la lista de eventos a guardar.
    /// Finalmente, se registra el cambio de la entidad como actualizado.
    /// </remarks>
    /// <seealso cref="AddEntityDeletedEvent(EntityEntry)"/>
    /// <seealso cref="GetChangeValues(EntityEntry)"/>
    /// <seealso cref="CreateEntityEvent(Type, object, object)"/>
    /// <seealso cref="AddEntityChangedEvent(object, EntityChangeType, object)"/>
    protected virtual void AddEntityUpdatedEvent(EntityEntry entry)
    {
        var entity = entry.Entity;
        if (entity is IDelete { IsDeleted: true })
        {
            AddEntityDeletedEvent(entity);
            return;
        }
        var changeValues = GetChangeValues(entry);
        var @event = CreateEntityEvent(typeof(EntityUpdatedEvent<>), entity, changeValues);
        SaveAfterEvents.Add(@event);
        AddEntityChangedEvent(entity, EntityChangeType.Updated, changeValues);
    }

    #endregion

    #region GetChangeValues(Obtener conjunto de valores de cambio)

    /// <summary>
    /// Obtiene una colección de valores que han cambiado para una entrada de entidad específica.
    /// </summary>
    /// <param name="entry">La entrada de entidad de la cual se obtendrán los valores cambiados.</param>
    /// <returns>
    /// Una colección de <see cref="ChangeValueCollection"/> que contiene los valores que han cambiado.
    /// </returns>
    /// <remarks>
    /// Este método itera a través de las propiedades de la entrada de entidad y verifica si han cambiado.
    /// Si la propiedad es "ExtraProperties", se obtienen los cambios específicos de esa propiedad.
    /// Para otras propiedades, se obtiene el valor cambiado utilizando el método <see cref="GetPropertyChangeValue"/>.
    /// </remarks>
    protected virtual ChangeValueCollection GetChangeValues(EntityEntry entry)
    {
        var result = new ChangeValueCollection();
        var properties = entry.Metadata.GetProperties();
        foreach (var property in properties)
        {
            var propertyEntry = entry.Property(property.Name);
            if (property.Name == "ExtraProperties")
            {
                var changeValues = GetExtraPropertiesChangeValues(property, propertyEntry);
                changeValues.ForEach(value =>
                {
                    if (value != null)
                        result.Add(value);
                });
                continue;
            }
            var changeValue = GetPropertyChangeValue(property, propertyEntry);
            if (changeValue == null)
                continue;
            result.Add(changeValue);
        }
        return result;
    }

    /// <summary>
    /// Obtiene los valores de cambio de propiedades adicionales para una propiedad específica.
    /// </summary>
    /// <param name="property">La propiedad para la cual se están obteniendo los valores de cambio.</param>
    /// <param name="propertyEntry">La entrada de propiedad que contiene los valores actuales y originales.</param>
    /// <returns>
    /// Una lista de objetos <see cref="ChangeValue"/> que representan los cambios en las propiedades adicionales.
    /// Si no hay cambios, se devuelve una lista vacía.
    /// </returns>
    /// <remarks>
    /// Este método compara los valores actuales y originales de un diccionario de propiedades adicionales.
    /// Si ambos valores son del tipo <see cref="ExtraPropertyDictionary"/>, se itera sobre las claves del valor actual
    /// y se comparan con los valores originales, generando un objeto <see cref="ChangeValue"/> para cada cambio detectado.
    /// </remarks>
    protected virtual List<ChangeValue> GetExtraPropertiesChangeValues(IProperty property, PropertyEntry propertyEntry)
    {
        var result = new List<ChangeValue>();
        if (propertyEntry.CurrentValue is not ExtraPropertyDictionary currentExtraValue)
            return result;
        if (propertyEntry.OriginalValue is not ExtraPropertyDictionary originalExtraValue)
            return result;
        foreach (var key in currentExtraValue.Keys)
        {
            currentExtraValue.TryGetValue(key, out var current);
            originalExtraValue.TryGetValue(key, out var original);
            var currentValue = Util.Helpers.Json.ToJson(current);
            var originalValue = Util.Helpers.Json.ToJson(original);
            result.Add(ToChangeValue(key, property.Name, originalValue, currentValue));
        }
        return result;
    }

    /// <summary>
    /// Convierte los valores originales y actuales en un objeto <see cref="ChangeValue"/> si han cambiado.
    /// </summary>
    /// <param name="name">El nombre del cambio.</param>
    /// <param name="description">Una descripción del cambio.</param>
    /// <param name="originalValue">El valor original antes del cambio.</param>
    /// <param name="currentValue">El valor actual después del cambio.</param>
    /// <returns>
    /// Un objeto <see cref="ChangeValue"/> que representa el cambio, o <c>null</c> si no hay cambios.
    /// </returns>
    /// <remarks>
    /// Este método verifica si los valores originales y actuales son iguales. Si son iguales,
    /// se devuelve <c>null</c>. Si alguno de los valores es "[]", se convierte a una cadena vacía.
    /// </remarks>
    protected virtual ChangeValue ToChangeValue(string name, string description, string originalValue, string currentValue)
    {
        if (originalValue == "[]")
            originalValue = string.Empty;
        if (currentValue == "[]")
            currentValue = string.Empty;
        if (originalValue == currentValue)
            return null;
        return new ChangeValue(name, description, originalValue, currentValue);
    }
    /// <summary>
    /// Obtiene el valor de cambio de una propiedad específica.
    /// </summary>
    /// <param name="property">La propiedad de la cual se desea obtener el valor de cambio.</param>
    /// <param name="propertyEntry">La entrada de propiedad que contiene los valores original y actual.</param>
    /// <returns>Un objeto <see cref="ChangeValue"/> que representa el cambio en la propiedad.</returns>
    /// <remarks>
    /// Este método utiliza la reflexión para obtener el nombre o la descripción de la propiedad 
    /// y luego crea un objeto <see cref="ChangeValue"/> que incluye el nombre de la propiedad, 
    /// su descripción y los valores original y actual en formato seguro.
    /// </remarks>
    protected virtual ChangeValue GetPropertyChangeValue(IProperty property, PropertyEntry propertyEntry)
    {
        var description = Reflection.GetDisplayNameOrDescription(property.PropertyInfo);
        return ToChangeValue(property.Name, description, propertyEntry.OriginalValue.SafeString(), propertyEntry.CurrentValue.SafeString());
    }

    #endregion

    #region AddEntityDeletedEvent(Agregar evento de eliminación de entidad)

    /// <summary>
    /// Agrega un evento que indica que una entidad ha sido eliminada.
    /// </summary>
    /// <param name="entity">La entidad que ha sido eliminada.</param>
    /// <remarks>
    /// Este método crea un evento de tipo <see cref="EntityDeletedEvent{T}"/> 
    /// y lo añade a la lista de eventos a guardar después. 
    /// También se registra un cambio de entidad con el tipo de cambio 
    /// <see cref="EntityChangeType.Deleted"/>.
    /// </remarks>
    protected virtual void AddEntityDeletedEvent(object entity)
    {
        var @event = CreateEntityEvent(typeof(EntityDeletedEvent<>), entity);
        SaveAfterEvents.Add(@event);
        AddEntityChangedEvent(entity, EntityChangeType.Deleted);
    }

    #endregion

    #region PublishSaveBeforeEventsAsync(Publicar evento antes de guardar)

    /// <summary>
    /// Publica de manera asíncrona los eventos que deben ser guardados antes de realizar una acción.
    /// </summary>
    /// <remarks>
    /// Este método verifica si hay eventos en la colección <c>SaveBeforeEvents</c>. Si la colección está vacía, el método retorna sin realizar ninguna acción.
    /// En caso contrario, se crea una copia de los eventos, se limpia la colección original y se 
    protected virtual async Task PublishSaveBeforeEventsAsync()
    {
        if (SaveBeforeEvents.Count == 0)
            return;
        var events = new List<IEvent>(SaveBeforeEvents);
        SaveBeforeEvents.Clear();
        await EventBus.PublishAsync(events);
    }

    #endregion

    #region SaveChangesAfter(Operaciones después de guardar)

    /// <summary>
    /// Guarda los cambios después de realizar las operaciones necesarias.
    /// </summary>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar
    /// una funcionalidad adicional después de guardar los cambios.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asincrónica.
    /// </returns>
    /// <seealso cref="PublishSaveAfterEventsAsync"/>
    /// <seealso cref="ExecuteActionsAsync"/>
    protected virtual async Task SaveChangesAfter()
    {
        await PublishSaveAfterEventsAsync();
        await ExecuteActionsAsync();
    }

    #endregion

    #region PublishSaveAfterEventsAsync(Publicar evento después de guardar)

    /// <summary>
    /// Publica eventos que deben ser guardados después de una operación.
    /// </summary>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas.
    /// Se encarga de recopilar los eventos almacenados en la lista <c>SaveAfterEvents</c>,
    /// limpiar dicha lista y luego 
    protected virtual async Task PublishSaveAfterEventsAsync()
    {
        var events = new List<IEvent>(SaveAfterEvents);
        SaveAfterEvents.Clear();
        await EventBus.PublishAsync(events);
    }

    #endregion

    #region ExecuteActionsAsync(Ejecutar conjunto de operaciones de unidad de trabajo)

    /// <summary>
    /// Ejecuta de manera asíncrona las acciones definidas en el gestor de acciones.
    /// </summary>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar
    /// una implementación personalizada de la ejecución de acciones.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona.
    /// </returns>
    protected virtual async Task ExecuteActionsAsync()
    {
        await ActionManager.ExecuteAsync();
    }

    #endregion
}