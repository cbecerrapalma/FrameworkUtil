namespace Util.Tenants.Managements;

/// <summary>
/// Clase que gestiona la información y operaciones relacionadas con los inquilinos.
/// Implementa la interfaz <see cref="ITenantManager"/>.
/// </summary>
public class TenantManager : ITenantManager {
    private static readonly AsyncLocal<string> _currentTenantId;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TenantManager"/>.
    /// Este constructor estático se utiliza para configurar el identificador del inquilino actual.
    /// </summary>
    static TenantManager() 
    { 
        _currentTenantId = new AsyncLocal<string>(); 
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TenantManager"/>.
    /// </summary>
    /// <param name="tenantStore">La tienda de inquilinos utilizada para gestionar inquilinos.</param>
    /// <param name="viewAllTenantManager">El gestor que permite ver todos los inquilinos.</param>
    /// <param name="switchTenantManager">El gestor que permite cambiar entre inquilinos.</param>
    /// <param name="session">La sesión actual del usuario.</param>
    /// <param name="options">Las opciones de configuración de inquilentes.</param>
    /// <exception cref="ArgumentNullException">
    /// Se lanza si <paramref name="tenantStore"/>, <paramref name="viewAllTenantManager"/>, 
    /// <paramref name="switchTenantManager"/> o <paramref name="session"/> son nulos.
    /// </exception>
    /// <remarks>
    /// Este constructor asegura que todos los parámetros requeridos sean proporcionados 
    /// y no sean nulos, lanzando una excepción en caso contrario.
    /// </remarks>
    public TenantManager( ITenantStore tenantStore, IViewAllTenantManager viewAllTenantManager, ISwitchTenantManager switchTenantManager,
        Sessions.ISession session, IOptions<TenantOptions> options ) {
        TenantStore = tenantStore ?? throw new ArgumentNullException( nameof( tenantStore ) );
        ViewAllTenantManager = viewAllTenantManager ?? throw new ArgumentNullException( nameof( viewAllTenantManager ) );
        SwitchTenantManager = switchTenantManager ?? throw new ArgumentNullException( nameof( switchTenantManager ) );
        Session = session ?? throw new ArgumentNullException( nameof( session ) );
        Options = options?.Value ?? TenantOptions.Null;
    }

    /// <summary>
    /// Obtiene la instancia del almacén de inquilinos.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso al almacén de inquilinos, que es responsable de gestionar la información relacionada con los inquilinos en la aplicación.
    /// </remarks>
    /// <value>
    /// Una instancia de <see cref="ITenantStore"/> que representa el almacén de inquilinos.
    /// </value>
    protected ITenantStore TenantStore { get; }
    /// <summary>
    /// Obtiene una instancia del administrador de visualización de todos los inquilinos.
    /// </summary>
    /// <value>
    /// Una implementación de <see cref="IViewAllTenantManager"/> que permite gestionar la visualización de todos los inquilinos.
    /// </value>
    protected IViewAllTenantManager ViewAllTenantManager { get; }
    /// <summary>
    /// Obtiene la instancia del administrador de cambio de inquilinos.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso al administrador que maneja el cambio de inquilinos en el sistema.
    /// </remarks>
    /// <value>
    /// Una instancia de <see cref="ISwitchTenantManager"/> que permite gestionar el cambio de inquilinos.
    /// </value>
    protected ISwitchTenantManager SwitchTenantManager { get; }
    /// <summary>
    /// Obtiene la sesión actual.
    /// </summary>
    /// <value>
    /// La sesión actual de tipo <see cref="Sessions.ISession"/>.
    /// </value>
    protected Sessions.ISession Session { get; }
    /// <summary>
    /// Obtiene las opciones del inquilino.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a la configuración específica del inquilino,
    /// permitiendo la personalización del comportamiento de la aplicación según las
    /// necesidades del inquilino.
    /// </remarks>
    /// <value>
    /// Un objeto de tipo <see cref="TenantOptions"/> que contiene las opciones del inquilino.
    /// </value>
    protected TenantOptions Options { get; }

    /// <summary>
    /// Obtiene o establece el identificador del inquilino actual.
    /// </summary>
    /// <remarks>
    /// Este identificador se utiliza para identificar el contexto del inquilino en una aplicación multitenant.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el identificador del inquilino actual.
    /// </value>
    public static string CurrentTenantId {
        get => _currentTenantId.Value;
        set => _currentTenantId.Value = value;
    }

    /// <inheritdoc />
    /// <summary>
    /// Determina si la opción está habilitada.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si la opción está habilitada; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica el estado de la propiedad <see cref="Options.IsEnabled"/> 
    /// para determinar si la funcionalidad asociada está activa.
    /// </remarks>
    public virtual bool Enabled() {
        return Options.IsEnabled;
    }

    /// <inheritdoc />
    /// <summary>
    /// Indica si se permite el uso de múltiples bases de datos.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si se permite el uso de múltiples bases de datos; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica la configuración actual para determinar si se permite la conexión a más de una base de datos.
    /// </remarks>
    public virtual bool AllowMultipleDatabase() {
        return Options.IsAllowMultipleDatabase;
    }

    /// <inheritdoc />
    /// <summary>
    /// Determina si el usuario actual es un host.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si el usuario está autenticado, tiene un ID de usuario válido,
    /// pertenece al inquilino actual y su ID de inquilino es igual al ID de inquilino por defecto;
    /// de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica varias condiciones relacionadas con la sesión actual del usuario
    /// para determinar si el usuario tiene privilegios de host.
    /// </remarks>
    public virtual bool IsHost() {
        if ( Session.IsAuthenticated == false )
            return false;
        if ( Session.UserId.IsEmpty() )
            return false;
        if ( Session.TenantId != CurrentTenantId )
            return false;
        return Session.TenantId == Options.DefaultTenantId;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el identificador del inquilino actual.
    /// </summary>
    /// <returns>
    /// Un string que representa el identificador del inquilino actual. 
    /// Si no se está en el modo host, se devuelve el identificador del inquilino actual. 
    /// Si se está en modo host y no hay un identificador de inquilino alternativo, se devuelve el identificador del inquilino actual.
    /// </returns>
    /// <remarks>
    /// Este método verifica si la instancia actual es un host. Si no lo es, simplemente devuelve el identificador del inquilino actual. 
    /// Si es un host, intenta obtener un identificador de inquilino alternativo a través del administrador de cambio de inquilino.
    /// </remarks>
    public virtual string GetTenantId() {
        if ( IsHost() == false )
            return CurrentTenantId;
        var switchTenantId = SwitchTenantManager.GetSwitchTenantId();
        return switchTenantId.IsEmpty() ? CurrentTenantId : switchTenantId;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene la información del inquilino actual.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="TenantInfo"/> que contiene la información del inquilino.
    /// </returns>
    /// <remarks>
    /// Este método llama al almacenamiento de inquilinos para recuperar la información del inquilino
    /// utilizando el identificador del inquilino actual.
    /// </remarks>
    /// <seealso cref="TenantStore"/>
    /// <seealso cref="GetTenantId()"/>
    public virtual TenantInfo GetTenant() {
        return TenantStore.GetTenant( GetTenantId() );
    }

    /// <inheritdoc />
    /// <summary>
    /// Determina si el filtro de inquilinos está deshabilitado.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si el filtro de inquilinos está deshabilitado; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el contexto actual es un host. Si no es un host, 
    /// el filtro de inquilinos no se deshabilita. Si es un host, se consulta 
    /// al administrador de inquilinos para determinar el estado del filtro.
    /// </remarks>
    /// <seealso cref="IsHost"/>
    /// <seealso cref="ViewAllTenantManager.IsDisableTenantFilter"/>
    public virtual bool IsDisableTenantFilter() {
        if ( IsHost() == false )
            return false;
        return ViewAllTenantManager.IsDisableTenantFilter();
    }
}