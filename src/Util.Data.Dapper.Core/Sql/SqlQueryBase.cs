using Util.Data.Dapper.Properties;
using Util.Data.Sql;
using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Clauses;
using Util.Data.Sql.Builders.Core;
using Util.Data.Sql.Builders.Params;
using Util.Data.Sql.Builders.Sets;
using Util.Data.Sql.Configs;
using Util.Data.Sql.Database;
using Util.Helpers;

namespace Util.Data.Dapper.Sql;

/// <summary>
/// Clase base abstracta que representa una consulta SQL.
/// </summary>
/// <remarks>
/// Esta clase implementa múltiples interfaces que permiten el acceso a partes de la consulta SQL,
/// opciones de configuración, gestión de parámetros, conexiones y transacciones.
/// </remarks>
public abstract class SqlQueryBase : ISqlQuery, ISqlPartAccessor, ISqlOptionsAccessor, IGetParameter, IClearParameters, IConnectionManager, ITransactionManager
{

    #region Campo

    private IDatabase _database;
    protected readonly SqlOptions Options;
    private ISqlBuilder _sqlBuilder;
    private IDbConnection _connection;
    private IDbTransaction _transaction;
    private string _sql;
    private DynamicParameters _parameters;
    private IParamLiteralsResolver _paramLiteralsResolver;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqlQueryBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver dependencias.</param>
    /// <param name="options">Las opciones de configuración para la conexión SQL.</param>
    /// <param name="database">La instancia de la base de datos que se utilizará.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="serviceProvider"/> o <paramref name="options"/> son nulos.</exception>
    /// <remarks>
    /// Este constructor establece las propiedades necesarias para la ejecución de consultas SQL,
    /// incluyendo la creación de un logger y la generación de un identificador de contexto.
    /// </remarks>
    protected SqlQueryBase(IServiceProvider serviceProvider, SqlOptions options, IDatabase database)
    {
        ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        Options = options ?? throw new ArgumentNullException(nameof(options));
        Logger = CreateLogger();
        _connection = options.Connection;
        _database = database;
        ContextId = Id.Create();
    }

    /// <summary>
    /// Crea un registrador (logger) utilizando la fábrica de registradores proporcionada por el contenedor de servicios.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa <see cref="ILogger"/>. Si la fábrica de registradores no está disponible, se devuelve <see cref="NullLogger.Instance"/>.
    /// </returns>
    /// <remarks>
    /// Este método intenta obtener una instancia de <see cref="ILoggerFactory"/> del contenedor de servicios.
    /// Si la instancia no se encuentra, se devuelve un registrador nulo que no realiza ninguna operación de registro.
    /// </remarks>
    private ILogger CreateLogger()
    {
        var loggerFactory = ServiceProvider.GetService<ILoggerFactory>();
        if (loggerFactory == null)
            return NullLogger.Instance;
        return loggerFactory.CreateLogger(Options.LogCategory);
    }

    #endregion

    #region atributo

    /// <summary>
    /// Obtiene el identificador del contexto.
    /// </summary>
    /// <remarks>
    /// Este identificador es de solo lectura y se establece en el momento de la creación del objeto.
    /// </remarks>
    /// <value>
    /// Un string que representa el identificador del contexto.
    /// </value>
    public string ContextId { get; private set; }
    /// <summary>
    /// Obtiene el proveedor de servicios asociado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso al contenedor de servicios, permitiendo la resolución de dependencias
    /// y la gestión del ciclo de vida de los servicios.
    /// </remarks>
    /// <value>
    /// Un objeto que implementa <see cref="IServiceProvider"/>.
    /// </value>
    protected IServiceProvider ServiceProvider { get; }
    /// <summary>
    /// Obtiene el logger utilizado para registrar información, advertencias y errores.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a una instancia de <see cref="ILogger"/> 
    /// que puede ser utilizada para realizar el registro de eventos en la aplicación.
    /// </remarks>
    protected ILogger Logger { get; }
    /// <summary>
    /// Obtiene la instancia de la base de datos.
    /// </summary>
    /// <remarks>
    /// Si la instancia de la base de datos no ha sido creada, se invoca el método <see cref="CreateDatabase"/> 
    /// para crearla. Utiliza la asignación condicional para garantizar que solo se crea una instancia.
    /// </remarks>
    /// <value>
    /// Una instancia de <see cref="IDatabase"/> que representa la base de datos.
    /// </value>
    protected IDatabase Database => _database ??= CreateDatabase();
    /// <summary>
    /// Obtiene una instancia de <see cref="ISqlBuilder"/>. 
    /// Si la instancia no ha sido creada previamente, se genera una nueva utilizando el método <see cref="CreateSqlBuilder"/>.
    /// </summary>
    /// <value>
    /// Una instancia de <see cref="ISqlBuilder"/> que se utiliza para construir consultas SQL.
    /// </value>
    /// <remarks>
    /// Este acceso a la propiedad utiliza el operador de asignación condicional (null-coalescing assignment) 
    /// para asegurar que solo se crea una nueva instancia si no existe una previamente.
    /// </remarks>
    public ISqlBuilder SqlBuilder => _sqlBuilder ??= CreateSqlBuilder();
    /// <summary>
    /// Obtiene el dialecto SQL asociado al constructor de consultas.
    /// </summary>
    /// <value>
    /// Un objeto que implementa la interfaz <see cref="IDialect"/> que representa el dialecto SQL.
    /// </value>
    /// <remarks>
    /// Este miembro permite acceder al dialecto utilizado por el <see cref="SqlBuilder"/> 
    /// a través de la interfaz <see cref="ISqlPartAccessor"/>. 
    /// Es útil para realizar operaciones específicas del dialecto en uso.
    /// </remarks>
    public IDialect Dialect => ((ISqlPartAccessor)SqlBuilder).Dialect;
    /// <summary>
    /// Obtiene el administrador de parámetros asociado con el constructor SQL.
    /// </summary>
    /// <remarks>
    /// Este miembro permite acceder al <see cref="IParameterManager"/> a través de la interfaz <see cref="ISqlPartAccessor"/>.
    /// </remarks>
    /// <value>
    /// Un objeto que implementa la interfaz <see cref="IParameterManager"/>.
    /// </value>
    /// <seealso cref="IParameterManager"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public IParameterManager ParameterManager => ((ISqlPartAccessor)SqlBuilder).ParameterManager;
    /// <summary>
    /// Obtiene una instancia de <see cref="IParamLiteralsResolver"/>.
    /// </summary>
    /// <remarks>
    /// Si la instancia de <see cref="_paramLiteralsResolver"/> es nula, se crea una nueva instancia utilizando el método <see cref="CreateParamLiteralsResolver"/>.
    /// </remarks>
    /// <value>
    /// Una instancia de <see cref="IParamLiteralsResolver"/> que se utiliza para resolver literales de parámetros.
    /// </value>
    protected IParamLiteralsResolver ParamLiteralsResolver => _paramLiteralsResolver ??= CreateParamLiteralsResolver();
    /// <summary>
    /// Obtiene la cláusula de inicio de la consulta SQL.
    /// </summary>
    /// <value>
    /// La cláusula de inicio que implementa la interfaz <see cref="IStartClause"/>.
    /// </value>
    /// <remarks>
    /// Esta propiedad permite acceder a la cláusula de inicio de la construcción de la consulta SQL
    /// a través del objeto <see cref="SqlBuilder"/>. 
    /// </remarks>
    public IStartClause StartClause => ((ISqlPartAccessor)SqlBuilder).StartClause;
    /// <summary>
    /// Obtiene la cláusula FROM de la consulta SQL.
    /// </summary>
    /// <value>
    /// La cláusula FROM representada como un objeto que implementa <see cref="IFromClause"/>.
    /// </value>
    /// <remarks>
    /// Esta propiedad permite acceder a la cláusula FROM de la consulta SQL construida por el <see cref="SqlBuilder"/>.
    /// </remarks>
    public IFromClause FromClause => ((ISqlPartAccessor)SqlBuilder).FromClause;
    /// <summary>
    /// Obtiene la cláusula de unión de la consulta SQL.
    /// </summary>
    /// <value>
    /// La cláusula de unión que se utiliza en la construcción de la consulta SQL.
    /// </value>
    /// <remarks>
    /// Esta propiedad permite acceder a la cláusula de unión actual del constructor de SQL,
    /// lo que facilita la manipulación y construcción de consultas complejas que requieren
    /// uniones entre diferentes tablas.
    /// </remarks>
    /// <seealso cref="ISqlPartAccessor"/>
    /// <seealso cref="IJoinClause"/>
    public IJoinClause JoinClause => ((ISqlPartAccessor)SqlBuilder).JoinClause;
    /// <summary>
    /// Obtiene la cláusula WHERE de la consulta SQL.
    /// </summary>
    /// <value>
    /// La cláusula WHERE como un objeto que implementa <see cref="IWhereClause"/>.
    /// </value>
    /// <remarks>
    /// Esta propiedad permite acceder a la cláusula WHERE de la consulta SQL construida
    /// a través del <see cref="SqlBuilder"/>. Es útil para agregar condiciones a la consulta
    /// antes de ejecutarla.
    /// </remarks>
    public IWhereClause WhereClause => ((ISqlPartAccessor)SqlBuilder).WhereClause;
    /// <summary>
    /// Obtiene la cláusula de inserción del constructor SQL.
    /// </summary>
    /// <value>
    /// La cláusula de inserción representada por un objeto que implementa <see cref="IInsertClause"/>.
    /// </value>
    /// <remarks>
    /// Esta propiedad permite acceder a la cláusula de inserción que se está construyendo en el contexto del generador SQL.
    /// </remarks>
    public IInsertClause InsertClause => ((ISqlPartAccessor)SqlBuilder).InsertClause;
    /// <summary>
    /// Obtiene la cláusula SELECT de la consulta SQL.
    /// </summary>
    /// <value>
    /// La cláusula SELECT representada por un objeto que implementa <see cref="ISelectClause"/>.
    /// </value>
    /// <remarks>
    /// Esta propiedad accede a la cláusula SELECT a través del objeto <see cref="SqlBuilder"/> 
    /// que implementa la interfaz <see cref="ISqlPartAccessor"/>.
    /// </remarks>
    public ISelectClause SelectClause => ((ISqlPartAccessor)SqlBuilder).SelectClause;
    /// <summary>
    /// Obtiene la cláusula GROUP BY de la consulta SQL.
    /// </summary>
    /// <value>
    /// La cláusula GROUP BY que se está utilizando en la construcción de la consulta SQL.
    /// </value>
    /// <remarks>
    /// Esta propiedad permite acceder a la cláusula GROUP BY a través de la interfaz <see cref="ISqlPartAccessor"/>.
    /// </remarks>
    public IGroupByClause GroupByClause => ((ISqlPartAccessor)SqlBuilder).GroupByClause;
    /// <summary>
    /// Obtiene la cláusula ORDER BY de la consulta SQL.
    /// </summary>
    /// <value>
    /// La cláusula ORDER BY implementada por <see cref="ISqlPartAccessor"/>.
    /// </value>
    /// <remarks>
    /// Esta propiedad permite acceder a la cláusula ORDER BY que se ha configurado en el constructor del 
    /// objeto <see cref="SqlBuilder"/>. Es útil para construir consultas SQL que requieren un orden específico 
    /// de los resultados.
    /// </remarks>
    public IOrderByClause OrderByClause => ((ISqlPartAccessor)SqlBuilder).OrderByClause;
    /// <summary>
    /// Obtiene la cláusula final de la consulta SQL.
    /// </summary>
    /// <value>
    /// La cláusula final que implementa <see cref="IEndClause"/>.
    /// </value>
    /// <remarks>
    /// Esta propiedad permite acceder a la cláusula final de una consulta SQL construida
    /// mediante el objeto <see cref="SqlBuilder"/>. Se utiliza para definir el final de
    /// la consulta en el contexto de la construcción de SQL.
    /// </remarks>
    public IEndClause EndClause => ((ISqlPartAccessor)SqlBuilder).EndClause;
    /// <summary>
    /// Obtiene el conjunto de constructores SQL asociado.
    /// </summary>
    /// <value>
    /// Un objeto que implementa <see cref="ISqlBuilderSet"/> que representa el conjunto de constructores SQL.
    /// </value>
    /// <remarks>
    /// Este miembro accede a la propiedad <see cref="SqlBuilder"/> y la convierte a <see cref="ISqlPartAccessor"/> 
    /// para obtener el conjunto de constructores SQL.
    /// </remarks>
    public ISqlBuilderSet SqlBuilderSet => ((ISqlPartAccessor)SqlBuilder).SqlBuilderSet;
    /// <summary>
    /// Obtiene un conjunto de parámetros dinámicos utilizados para las operaciones de base de datos.
    /// </summary>
    /// <remarks>
    /// Este método inicializa una instancia de <see cref="DynamicParameters"/> y la llena con los parámetros
    /// obtenidos de <see cref="ParameterManager.GetParams"/> y <see cref="ParameterManager.GetDynamicParams"/>.
    /// Los parámetros se añaden a la colección utilizando sus propiedades como nombre, valor, tipo de base de datos,
    /// dirección, tamaño, precisión y escala.
    /// </remarks>
    /// <returns>
    /// Un objeto <see cref="DynamicParameters"/> que contiene todos los parámetros necesarios para la operación.
    /// </returns>
    protected DynamicParameters Params
    {
        get
        {
            _parameters = new DynamicParameters();
            ParameterManager.GetParams().ToList().ForEach(t => _parameters.Add(t.Name, t.Value, t.DbType, t.Direction, t.Size, t.Precision, t.Scale));
            ParameterManager.GetDynamicParams().ToList().ForEach(_parameters.AddDynamicParams);
            return _parameters;
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el resultado de la consulta SQL anterior.
    /// </summary>
    /// <remarks>
    /// Esta propiedad almacena el resultado de la ejecución de la consulta SQL previa,
    /// permitiendo acceder a los datos generados o a cualquier error que haya ocurrido.
    /// </remarks>
    /// <value>
    /// Un objeto de tipo <see cref="SqlBuilderResult"/> que representa el resultado
    /// de la consulta SQL anterior.
    /// </value>
    public SqlBuilderResult PreviousSql { get; private set; }

    #endregion

    #region Método de fábrica

    /// <summary>
    /// Crea una instancia de un constructor de SQL.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="ISqlBuilder"/>.
    /// </returns>
    /// <remarks>
    /// Este método es abstracto y debe ser implementado por las clases derivadas.
    /// </remarks>
    protected abstract ISqlBuilder CreateSqlBuilder();

    /// <summary>
    /// Crea una instancia de una base de datos utilizando la cadena de conexión especificada en las opciones.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IDatabase"/> que representa la base de datos creada.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Se lanza si la cadena de conexión está vacía o si la fábrica de base de datos es nula.
    /// </exception>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación personalizada.
    /// </remarks>
    protected virtual IDatabase CreateDatabase()
    {
        if (Options.ConnectionString.IsEmpty())
            throw new InvalidOperationException(UtilDataDapperResource.ConnectionIsEmpty);
        var factory = CreateDatabaseFactory();
        if (factory == null)
            throw new InvalidOperationException(UtilDataDapperResource.DatabaseFactoryIsNull);
        return factory.Create(Options.ConnectionString);
    }

    /// <summary>
    /// Crea una instancia de un <see cref="IDatabaseFactory"/>.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="IDatabaseFactory"/>.
    /// </returns>
    /// <remarks>
    /// Este método es abstracto y debe ser implementado en una clase derivada.
    /// </remarks>
    protected abstract IDatabaseFactory CreateDatabaseFactory();

    /// <summary>
    /// Crea un generador de SQL para verificar la existencia de registros.
    /// </summary>
    /// <param name="sqlBuilder">El generador de SQL que se utilizará para construir la consulta.</param>
    /// <returns>Un objeto que implementa <see cref="IExistsSqlBuilder"/> para construir consultas de existencia.</returns>
    /// <remarks>
    /// Este método es abstracto y debe ser implementado en una clase derivada.
    /// </remarks>
    protected abstract IExistsSqlBuilder CreatExistsSqlBuilder(ISqlBuilder sqlBuilder);

    /// <summary>
    /// Crea una instancia de <see cref="IParamLiteralsResolver"/>.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="IParamLiteralsResolver"/>.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación personalizada.
    /// </remarks>
    protected virtual IParamLiteralsResolver CreateParamLiteralsResolver()
    {
        return new ParamLiteralsResolver();
    }

    #endregion

    #region GetOptions(Obtener configuración de Sql.)

    /// <summary>
    /// Obtiene las opciones de configuración de SQL.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="SqlOptions"/> que contiene las opciones de configuración.
    /// </returns>
    public SqlOptions GetOptions()
    {
        return Options;
    }

    #endregion

    #region GetParam(Obtener el valor de los parámetros de SQL.)

    /// <summary>
    /// Obtiene un parámetro del conjunto de parámetros actual o del conjunto de parámetros anterior.
    /// </summary>
    /// <typeparam name="T">El tipo del parámetro que se desea obtener.</typeparam>
    /// <param name="name">El nombre del parámetro que se desea recuperar.</param>
    /// <returns>El valor del parámetro del tipo especificado, o el valor predeterminado de T si no se encuentra.</returns>
    /// <remarks>
    /// Este método verifica primero si el conjunto de parámetros actual es nulo. Si es así, intenta obtener el parámetro 
    /// del conjunto de parámetros de la consulta SQL anterior. Si el conjunto de parámetros actual no es nulo, 
    /// se obtiene el parámetro directamente de él.
    /// </remarks>
    /// <seealso cref="PreviousSql"/>
    /// <seealso cref="Params"/>
    public virtual T GetParam<T>(string name)
    {
        if (_parameters == null && PreviousSql != null)
            return PreviousSql.GetParam<T>(name);
        return Params.Get<T>(name);
    }

    #endregion

    #region ClearParams(Limpiar parámetros de SQL)

    /// <summary>
    /// Limpia los parámetros actuales, restableciendo la colección de parámetros a su estado inicial.
    /// </summary>
    /// <remarks>
    /// Este método invoca el método <see cref="ParameterManager.Clear"/> para eliminar todos los parámetros 
    /// gestionados por el administrador de parámetros y luego establece la variable de instancia 
    /// <c>_parameters</c> a <c>null</c>.
    /// </remarks>
    public void ClearParams()
    {
        ParameterManager.Clear();
        _parameters = null;
    }

    #endregion

    #region GetConnection(Obtener conexión a la base de datos.)

    /// <summary>
    /// Obtiene una conexión a la base de datos.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa <see cref="IDbConnection"/> que representa la conexión a la base de datos.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Se lanza cuando la conexión a la base de datos es nula después de intentar obtenerla.
    /// </exception>
    public IDbConnection GetConnection()
    {
        if (_connection != null)
            return _connection;
        _connection = Database.GetConnection();
        if (_connection == null)
            throw new InvalidOperationException(UtilDataDapperResource.ConnectionIsEmpty);
        return _connection;
    }

    #endregion

    #region SetConnection(Configurar la conexión a la base de datos.)

    /// <summary>
    /// Establece la conexión a la base de datos.
    /// </summary>
    /// <param name="connection">La conexión a la base de datos que se va a establecer. Si es <c>null</c>, no se realiza ninguna acción.</param>
    public void SetConnection(IDbConnection connection)
    {
        if (connection == null)
            return;
        _connection = connection;
    }

    #endregion

    #region GetTransaction(Obtener transacciones de la base de datos.)

    /// <summary>
    /// Obtiene la transacción de base de datos actual.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="IDbTransaction"/> que representa la transacción activa.
    /// </returns>
    public IDbTransaction GetTransaction()
    {
        return _transaction;
    }

    #endregion

    #region SetTransaction(Configurar transacciones de base de datos.)

    /// <summary>
    /// Establece la transacción actual para la operación.
    /// </summary>
    /// <param name="transaction">La transacción a establecer. Si es <c>null</c>, no se realiza ninguna acción.</param>
    public void SetTransaction(IDbTransaction transaction)
    {
        if (transaction == null)
            return;
        _transaction = transaction;
        _connection = transaction.Connection;
    }

    #endregion

    #region BeginTransaction(Iniciar transacción)

    /// <summary>
    /// Inicia una nueva transacción en la base de datos.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="IDbTransaction"/> que representa la transacción iniciada.
    /// </returns>
    /// <remarks>
    /// Este método llama a la implementación interna <see cref="BeginTransactionImpl"/> 
    /// con un parámetro nulo, lo que puede implicar que se utilizarán los valores predeterminados 
    /// para la configuración de la transacción.
    /// </remarks>
    public IDbTransaction BeginTransaction()
    {
        return BeginTransactionImpl(null);
    }

    /// <summary>
    /// Inicia una nueva transacción con el nivel de aislamiento especificado.
    /// </summary>
    /// <param name="isolationLevel">El nivel de aislamiento que se aplicará a la transacción.</param>
    /// <returns>Una instancia de <see cref="IDbTransaction"/> que representa la transacción iniciada.</returns>
    /// <remarks>
    /// Este método permite controlar el comportamiento de la transacción en relación con otras transacciones concurrentes.
    /// Los niveles de aislamiento disponibles pueden variar según el proveedor de la base de datos.
    /// </remarks>
    public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
    {
        return BeginTransactionImpl(isolationLevel);
    }

    /// <summary>
    /// Inicia una transacción en la conexión actual.
    /// </summary>
    /// <param name="isolationLevel">El nivel de aislamiento de la transacción. Si es nulo, se utilizará el nivel de aislamiento predeterminado.</param>
    /// <returns>Un objeto <see cref="IDbTransaction"/> que representa la transacción iniciada.</returns>
    /// <remarks>
    /// Este método verifica si ya existe una transacción activa. Si es así, devuelve la transacción existente.
    /// Si no hay una transacción activa, se obtiene la conexión y se abre si está cerrada. Luego, se inicia una nueva transacción
    /// con el nivel de aislamiento especificado o el predeterminado si no se proporciona ninguno.
    /// En caso de que ocurra un error durante el inicio de la transacción, se cierra la conexión si está abierta
    /// y se dispone de la transacción activa antes de lanzar la excepción nuevamente.
    /// </remarks>
    /// <exception cref="Exception">Se lanza si ocurre un error al iniciar la transacción.</exception>
    private IDbTransaction BeginTransactionImpl(IsolationLevel? isolationLevel)
    {
        try
        {
            if (_transaction != null)
                return _transaction;
            var connection = GetConnection();
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            _transaction = isolationLevel == null ? connection.BeginTransaction() : connection.BeginTransaction(isolationLevel.SafeValue());
            return _transaction;
        }
        catch
        {
            if (_connection?.State == ConnectionState.Open)
                _connection?.Close();
            _transaction?.Dispose();
            throw;
        }
    }

    #endregion

    #region CommitTransaction(Enviar transacción)

    /// <summary>
    /// Confirma la transacción actual. Si ocurre un error durante la confirmación,
    /// se realiza un rollback de la transacción.
    /// </summary>
    /// <remarks>
    /// Este método intenta confirmar la transacción activa. Si la confirmación falla,
    /// se revierte la transacción para mantener la integridad de los datos. Al final,
    /// se cierra la conexión si está abierta y se libera la transacción.
    /// </remarks>
    public void CommitTransaction()
    {
        try
        {
            _transaction?.Commit();
        }
        catch
        {
            _transaction?.Rollback();
            throw;
        }
        finally
        {
            if (_connection?.State == ConnectionState.Open)
                _connection?.Close();
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    #endregion

    #region RollbackTransaction(Revertir transacción.)

    /// <summary>
    /// Revierte la transacción actual si está activa.
    /// </summary>
    /// <remarks>
    /// Este método verifica el estado de la conexión y, si la conexión está abierta,
    /// intenta revertir la transacción activa. Al finalizar, cierra la conexión si está abierta
    /// y libera los recursos asociados a la transacción.
    /// </remarks>
    public void RollbackTransaction()
    {
        try
        {
            if (_connection.State != ConnectionState.Closed)
                _transaction?.Rollback();
        }
        finally
        {
            if (_connection?.State == ConnectionState.Open)
                _connection?.Close();
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    #endregion

    #region ExecuteExists(Determinar si existe.)

    /// <summary>
    /// Ejecuta una consulta para verificar la existencia de un registro en la base de datos.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si el registro existe; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método primero ejecuta un procedimiento previo a la consulta mediante el método <see cref="ExecuteBefore"/>.
    /// Luego, construye la consulta SQL utilizando el método <see cref="CreatExistsSqlBuilder"/> y ejecuta la consulta
    /// utilizando <see cref="GetConnection"/> y <see cref="ExecuteScalar"/>. Finalmente, se llama a <see cref="ExecuteAfter"/>
    /// para realizar cualquier acción necesaria después de la ejecución.
    /// </remarks>
    /// <exception cref="Exception">
    /// Se puede lanzar una excepción si ocurre un error durante la ejecución de la consulta.
    /// </exception>
    public bool ExecuteExists()
    {
        object result = null;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = CreatExistsSqlBuilder(SqlBuilder).GetSql();
            var connection = GetConnection();
            result = connection.ExecuteScalar(GetSql(), Params, GetTransaction());
            return Util.Helpers.Convert.ToBool(result);
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    #endregion

    #region ExecuteExistsAsync(Determinar si existe.)

    /// <summary>
    /// Ejecuta una consulta asíncrona para verificar la existencia de un registro en la base de datos.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si el registro existe; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método primero verifica si se puede ejecutar la operación mediante el método <see cref="ExecuteBefore"/>.
    /// Luego, construye la consulta SQL utilizando <see cref="CreatExistsSqlBuilder"/> y ejecuta la consulta.
    /// Finalmente, se llama al método <see cref="ExecuteAfter"/> para realizar cualquier acción necesaria después de la ejecución.
    /// </remarks>
    /// <exception cref="Exception">
    /// Se lanzará una excepción si ocurre un error durante la ejecución de la consulta.
    /// </exception>
    public async Task<bool> ExecuteExistsAsync()
    {
        object result = null;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = CreatExistsSqlBuilder(SqlBuilder).GetSql();
            var connection = GetConnection();
            result = await connection.ExecuteScalarAsync(GetSql(), Params, GetTransaction());
            return Util.Helpers.Convert.ToBool(result);
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    #endregion

    #region ExecuteScalar(Obtener un solo valor.)

    /// <summary>
    /// Ejecuta una consulta SQL y devuelve el primer valor de la primera fila del conjunto de resultados.
    /// </summary>
    /// <param name="timeout">El tiempo máximo en segundos que se permitirá para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>
    /// Un objeto que representa el primer valor de la primera fila del conjunto de resultados, o <c>null</c> si no se devuelve ninguna fila.
    /// </returns>
    /// <remarks>
    /// Este método se asegura de que se ejecute una acción antes y después de la ejecución de la consulta.
    /// Si la ejecución previa falla, se devuelve el valor predeterminado del tipo de retorno.
    /// </remarks>
    /// <exception cref="Exception">Se lanzará una excepción si ocurre un error durante la ejecución de la consulta.</exception>
    public virtual object ExecuteScalar(int? timeout = null)
    {
        object result = null;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            var connection = GetConnection();
            result = connection.ExecuteScalar(GetSql(), Params, GetTransaction(), timeout);
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta SQL y devuelve el primer valor de la primera fila 
    /// del resultado, convertido al tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo al que se convertirá el resultado de la consulta.</typeparam>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución de la consulta. 
    /// Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>El primer valor de la primera fila del resultado de la consulta, 
    /// convertido al tipo especificado.</returns>
    /// <remarks>
    /// Este método es útil para consultas que devuelven un solo valor, como 
    /// funciones de agregación o consultas que devuelven un único campo.
    /// </remarks>
    /// <seealso cref="ExecuteScalar(int?)"/>
    public virtual T ExecuteScalar<T>(int? timeout = null)
    {
        return Util.Helpers.Convert.To<T>(ExecuteScalar(timeout));
    }

    #endregion

    #region ExecuteScalarAsync(Obtener un valor único.)

    /// <summary>
    /// Ejecuta una consulta SQL y devuelve el primer valor de la primera fila del resultado.
    /// </summary>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>
    /// Un objeto que representa el primer valor de la primera fila del resultado de la consulta. 
    /// Si no hay filas, se devuelve null.
    /// </returns>
    /// <remarks>
    /// Este método se ejecuta de manera asíncrona y puede ser utilizado para consultas que se espera que devuelvan un solo valor.
    /// Se ejecutan métodos adicionales antes y después de la ejecución de la consulta para manejar la lógica de negocio necesaria.
    /// </remarks>
    /// <exception cref="Exception">Se lanzará una excepción si ocurre un error durante la ejecución de la consulta.</exception>
    /// <seealso cref="GetConnection"/>
    /// <seealso cref="GetSql"/>
    /// <seealso cref="Params"/>
    /// <seealso cref="GetTransaction"/>
    /// <seealso cref="ExecuteBefore"/>
    /// <seealso cref="ExecuteAfter"/>
    public virtual async Task<object> ExecuteScalarAsync(int? timeout = null)
    {
        object result = null;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            var connection = GetConnection();
            result = await connection.ExecuteScalarAsync(GetSql(), Params, GetTransaction(), timeout);
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una operación asincrónica que devuelve un único valor de la base de datos.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se espera recibir como resultado de la operación.</typeparam>
    /// <param name="timeout">El tiempo máximo en segundos que se permitirá para la ejecución de la operación. Si es null, se utilizará el valor predeterminado.</param>
    /// <returns>
    /// Un objeto de tipo <typeparamref name="T"/> que representa el resultado de la operación.
    /// </returns>
    /// <remarks>
    /// Este método es útil para ejecutar consultas que devuelven un solo valor, como una suma o un conteo.
    /// </remarks>
    /// <seealso cref="ExecuteScalarAsync(int?)"/>
    public virtual async Task<T> ExecuteScalarAsync<T>(int? timeout = null)
    {
        var result = await ExecuteScalarAsync(timeout);
        return Util.Helpers.Convert.To<T>(result);
    }

    #endregion

    #region ExecuteProcedureScalar(Ejecutar un procedimiento almacenado para obtener un solo valor.)

    /// <summary>
    /// Ejecuta un procedimiento almacenado y devuelve el resultado como un objeto.
    /// </summary>
    /// <param name="procedure">El nombre del procedimiento almacenado a ejecutar.</param>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>El resultado del procedimiento almacenado, o el valor predeterminado si la ejecución falla.</returns>
    /// <remarks>
    /// Este método primero verifica si se puede ejecutar antes de proceder. Si la ejecución previa falla, se devuelve el valor predeterminado.
    /// Luego, obtiene la conexión y ejecuta el procedimiento almacenado utilizando el método ExecuteScalar.
    /// Finalmente, se ejecuta la lógica de limpieza o seguimiento después de la ejecución.
    /// </remarks>
    /// <exception cref="Exception">Lanza una excepción si ocurre un error durante la ejecución del procedimiento.</exception>
    public virtual object ExecuteProcedureScalar(string procedure, int? timeout = null)
    {
        object result = null;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = GetProcedure(procedure);
            var connection = GetConnection();
            result = connection.ExecuteScalar(GetSql(), Params, GetTransaction(), timeout, GetProcedureCommandType());
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }



    /// <summary>
    /// Ejecuta un procedimiento almacenado y devuelve un valor escalar.
    /// </summary>
    /// <typeparam name="T">El tipo de dato del valor escalar que se espera recibir.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo de espera opcional en segundos para la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>El valor escalar devuelto por el procedimiento almacenado, convertido al tipo especificado.</returns>
    /// <remarks>
    /// Este método es útil para ejecutar procedimientos que devuelven un solo valor, como conteos o sumas.
    /// </remarks>
    /// <seealso cref="ExecuteProcedureScalar{T}(string, int?)"/>
    public virtual T ExecuteProcedureScalar<T>(string procedure, int? timeout = null)
    {
        return Util.Helpers.Convert.To<T>(ExecuteProcedureScalar(procedure, timeout));
    }

    #endregion

    #region ExecuteProcedureScalarAsync(Ejecutar un procedimiento almacenado para obtener un solo valor.)

    /// <summary>
    /// Ejecuta un procedimiento almacenado de forma asíncrona y devuelve un resultado escalar.
    /// </summary>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>El resultado escalar devuelto por el procedimiento almacenado, o null si la ejecución falla o no se ejecuta.</returns>
    /// <remarks>
    /// Este método realiza una serie de pasos antes y después de la ejecución del procedimiento almacenado.
    /// Se puede personalizar el comportamiento mediante los métodos <c>ExecuteBefore</c> y <c>ExecuteAfter</c>.
    /// </remarks>
    /// <exception cref="Exception">Lanza una excepción si ocurre un error durante la ejecución del procedimiento.</exception>
    /// <seealso cref="GetProcedure(string)"/>
    /// <seealso cref="GetConnection()"/>
    /// <seealso cref="ExecuteBefore()"/>
    /// <seealso cref="ExecuteAfter(object)"/>
    /// <seealso cref="GetSql()"/>
    /// <seealso cref="Params"/>
    /// <seealso cref="GetTransaction()"/>
    /// <seealso cref="GetProcedureCommandType()"/>
    public virtual async Task<object> ExecuteProcedureScalarAsync(string procedure, int? timeout = null)
    {
        object result = null;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = GetProcedure(procedure);
            var connection = GetConnection();
            result = await connection.ExecuteScalarAsync(GetSql(), Params, GetTransaction(), timeout, GetProcedureCommandType());
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta un procedimiento almacenado de forma asíncrona y devuelve un valor escalar.
    /// </summary>
    /// <typeparam name="T">El tipo de dato al que se convertirá el resultado del procedimiento.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo de espera en segundos para la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>Un valor del tipo especificado <typeparamref name="T"/> que representa el resultado del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método es útil para obtener un único valor de un procedimiento almacenado que devuelve un resultado escalar,
    /// como un conteo, suma, o cualquier otro valor único.
    /// </remarks>
    /// <seealso cref="ExecuteProcedureScalarAsync{T}(string, int?)"/>
    public virtual async Task<T> ExecuteProcedureScalarAsync<T>(string procedure, int? timeout = null)
    {
        var result = await ExecuteProcedureScalarAsync(procedure, timeout);
        return Util.Helpers.Convert.To<T>(result);
    }

    #endregion

    #region ExecuteSingle(Obtener una sola entidad.)

    /// <summary>
    /// Ejecuta una consulta SQL y devuelve un único resultado de tipo especificado.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se espera como resultado de la consulta.</typeparam>
    /// <param name="timeout">El tiempo máximo en segundos que se permitirá para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>
    /// Un objeto de tipo <typeparamref name="TEntity"/> que representa el resultado de la consulta, 
    /// o el valor predeterminado de <typeparamref name="TEntity"/> si no se encuentra ningún resultado 
    /// o si la ejecución previa falla.
    /// </returns>
    /// <remarks>
    /// Este método realiza una ejecución de consulta SQL utilizando una conexión a la base de datos. 
    /// Se ejecuta un método antes de la consulta y otro después de la misma, asegurando que se manejen 
    /// adecuadamente los recursos y el estado de la operación.
    /// </remarks>
    /// <seealso cref="GetConnection"/>
    /// <seealso cref="GetSql"/>
    /// <seealso cref="ExecuteBefore"/>
    /// <seealso cref="ExecuteAfter"/>
    public virtual TEntity ExecuteSingle<TEntity>(int? timeout = null)
    {
        TEntity result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            var connection = GetConnection();
            result = connection.QueryFirstOrDefault<TEntity>(GetSql(), Params, GetTransaction(), timeout);
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    #endregion

    #region ExecuteSingleAsync(Obtener una sola entidad.)

    /// <summary>
    /// Ejecuta una consulta SQL de forma asíncrona y devuelve un único resultado.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se espera como resultado de la consulta.</typeparam>
    /// <param name="timeout">El tiempo máximo en milisegundos que se permitirá para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>
    /// Un objeto de tipo <typeparamref name="TEntity"/> que representa el resultado de la consulta, o el valor predeterminado si no se encuentra ningún resultado.
    /// </returns>
    /// <remarks>
    /// Este método se asegura de que se ejecute una lógica antes y después de la ejecución de la consulta mediante los métodos <c>ExecuteBefore</c> y <c>ExecuteAfter</c>.
    /// Si <c>ExecuteBefore</c> devuelve <c>false</c>, el método retornará el valor predeterminado sin ejecutar la consulta.
    /// </remarks>
    /// <seealso cref="GetConnection"/>
    /// <seealso cref="GetSql"/>
    /// <seealso cref="Params"/>
    /// <seealso cref="GetTransaction"/>
    public virtual async Task<TEntity> ExecuteSingleAsync<TEntity>(int? timeout = null)
    {
        TEntity result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            var connection = GetConnection();
            result = await connection.QueryFirstOrDefaultAsync<TEntity>(GetSql(), Params, GetTransaction(), timeout);
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    #endregion

    #region ExecuteProcedureSingle(Ejecutar un procedimiento almacenado para obtener una única entidad.)

    /// <summary>
    /// Ejecuta un procedimiento almacenado y devuelve un solo resultado.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se espera como resultado del procedimiento.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado a ejecutar.</param>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>Un objeto del tipo <typeparamref name="TEntity"/> que representa el resultado del procedimiento almacenado, o el valor predeterminado si no se encuentra ningún resultado.</returns>
    /// <remarks>
    /// Este método se encarga de manejar la conexión a la base de datos y la ejecución del procedimiento almacenado.
    /// Se ejecutan métodos antes y después de la llamada al procedimiento para permitir la personalización de la lógica de ejecución.
    /// </remarks>
    /// <exception cref="Exception">Lanza una excepción si ocurre un error durante la ejecución del procedimiento almacenado.</exception>
    /// <seealso cref="GetProcedure(string)"/>
    /// <seealso cref="GetConnection()"/>
    /// <seealso cref="ExecuteBefore()"/>
    /// <seealso cref="ExecuteAfter(TEntity)"/>
    public virtual TEntity ExecuteProcedureSingle<TEntity>(string procedure, int? timeout = null)
    {
        TEntity result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = GetProcedure(procedure);
            var connection = GetConnection();
            result = connection.QueryFirstOrDefault<TEntity>(GetSql(), Params, GetTransaction(), timeout, GetProcedureCommandType());
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    #endregion

    #region ExecuteProcedureSingleAsync(Ejecutar un procedimiento almacenado para obtener una entidad única.)

    /// <summary>
    /// Ejecuta un procedimiento almacenado de forma asíncrona y devuelve un solo resultado.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se espera como resultado del procedimiento.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo máximo en segundos para la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>
    /// Un objeto de tipo <typeparamref name="TEntity"/> que representa el resultado del procedimiento almacenado, o el valor predeterminado si no se encuentra ningún resultado.
    /// </returns>
    /// <remarks>
    /// Este método se asegura de que se ejecuten las operaciones necesarias antes y después de la ejecución del procedimiento.
    /// Si la ejecución previa falla, se devuelve el valor predeterminado.
    /// </remarks>
    /// <seealso cref="GetProcedure(string)"/>
    /// <seealso cref="GetConnection()"/>
    /// <seealso cref="ExecuteBefore()"/>
    /// <seealso cref="ExecuteAfter(TEntity)"/>
    /// <seealso cref="GetTransaction()"/>
    /// <seealso cref="GetSql()"/>
    /// <seealso cref="GetProcedureCommandType()"/>
    public virtual async Task<TEntity> ExecuteProcedureSingleAsync<TEntity>(string procedure, int? timeout = null)
    {
        TEntity result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = GetProcedure(procedure);
            var connection = GetConnection();
            result = await connection.QueryFirstOrDefaultAsync<TEntity>(GetSql(), Params, GetTransaction(), timeout, GetProcedureCommandType());
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    #endregion

    #region ExecuteQuery(Obtener conjunto de entidades)

    /// <summary>
    /// Ejecuta una consulta SQL y devuelve los resultados como una lista dinámica.
    /// </summary>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución de la consulta. Si es null, se utilizará el valor predeterminado de la conexión.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria (true) o si se deben leer de forma no almacenada (false).</param>
    /// <returns>
    /// Una lista dinámica que contiene los resultados de la consulta. 
    /// Si la ejecución de la consulta falla o si <c>ExecuteBefore()</c> devuelve false, se devolverá null.
    /// </returns>
    /// <remarks>
    /// Este método se asegura de que se realicen las operaciones necesarias antes y después de la ejecución de la consulta 
    /// mediante las llamadas a <c>ExecuteBefore()</c> y <c>ExecuteAfter()</c>, respectivamente.
    /// </remarks>
    /// <seealso cref="GetConnection"/>
    /// <seealso cref="GetSql"/>
    /// <seealso cref="GetTransaction"/>
    public virtual List<dynamic> ExecuteQuery(int? timeout = null, bool buffered = true)
    {
        List<dynamic> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            var connection = GetConnection();
            result = connection.Query(GetSql(), Params, GetTransaction(), buffered, timeout).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta y devuelve una lista de entidades del tipo especificado.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se espera obtener como resultado de la consulta.</typeparam>
    /// <param name="timeout">El tiempo máximo en segundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria (true) o si deben ser leídos de manera diferida (false).</param>
    /// <returns>
    /// Una lista de entidades del tipo especificado, o null si la consulta no se ejecutó correctamente.
    /// </returns>
    /// <remarks>
    /// Este método se encarga de ejecutar una consulta SQL utilizando una conexión a la base de datos.
    /// Se ejecutan métodos antes y después de la consulta para manejar cualquier lógica adicional necesaria.
    /// </remarks>
    /// <seealso cref="GetConnection"/>
    /// <seealso cref="GetSql"/>
    /// <seealso cref="GetTransaction"/>
    /// <seealso cref="ExecuteBefore"/>
    /// <seealso cref="ExecuteAfter"/>
    public virtual List<TEntity> ExecuteQuery<TEntity>(int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            var connection = GetConnection();
            result = connection.Query<TEntity>(GetSql(), Params, GetTransaction(), buffered, timeout).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad resultante del mapeo.</typeparam>
    /// <param name="map">Función que define cómo mapear los resultados de la consulta a una entidad.</param>
    /// <param name="timeout">Tiempo máximo en segundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria (true) o no (false).</param>
    /// <returns>Una lista de entidades resultantes de la consulta.</returns>
    /// <remarks>
    /// Este método se encarga de ejecutar una consulta SQL y mapear los resultados a una lista de entidades del tipo especificado.
    /// Se asegura de realizar acciones antes y después de la ejecución de la consulta mediante los métodos <c>ExecuteBefore</c> y <c>ExecuteAfter</c>.
    /// </remarks>
    /// <seealso cref="ExecuteBefore"/>
    /// <seealso cref="ExecuteAfter"/>
    /// <seealso cref="GetConnection"/>
    /// <seealso cref="GetSql"/>
    /// <seealso cref="GetTransaction"/>
    public virtual List<TEntity> ExecuteQuery<T1, T2, TEntity>(Func<T1, T2, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            var connection = GetConnection();
            result = connection.Query(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir de los resultados de la consulta.</typeparam>
    /// <param name="map">Función que define cómo mapear los resultados de la consulta a una entidad.</param>
    /// <param name="timeout">Tiempo máximo en segundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una lista de entidades del tipo especificado, o null si la ejecución de la consulta falla.</returns>
    /// <remarks>
    /// Este método se encarga de gestionar la conexión a la base de datos, así como de ejecutar la consulta SQL definida por el método <see cref="GetSql"/>.
    /// Se asegura de ejecutar las operaciones necesarias antes y después de la consulta mediante los métodos <see cref="ExecuteBefore"/> y <see cref="ExecuteAfter"/>.
    /// </remarks>
    public virtual List<TEntity> ExecuteQuery<T1, T2, T3, TEntity>(Func<T1, T2, T3, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            var connection = GetConnection();
            result = connection.Query(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se devolverá en la lista.</typeparam>
    /// <param name="map">Función que mapea los resultados de la consulta a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">Tiempo máximo en segundos para la ejecución de la consulta. Si es null, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria o no.</param>
    /// <returns>Una lista de entidades de tipo <typeparamref name="TEntity"/> resultantes de la consulta.</returns>
    /// <remarks>
    /// Este método se encarga de ejecutar una consulta SQL utilizando una conexión a la base de datos,
    /// aplicando un mapeo a los resultados obtenidos. Se asegura de ejecutar acciones antes y después
    /// de la consulta mediante los métodos <c>ExecuteBefore</c> y <c>ExecuteAfter</c>.
    /// </remarks>
    /// <seealso cref="ExecuteBefore"/>
    /// <seealso cref="ExecuteAfter"/>
    /// <seealso cref="GetConnection"/>
    /// <seealso cref="GetSql"/>
    /// <seealso cref="GetTransaction"/>
    public virtual List<TEntity> ExecuteQuery<T1, T2, T3, T4, TEntity>(Func<T1, T2, T3, T4, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            var connection = GetConnection();
            result = connection.Query(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se devolverá en la lista.</typeparam>
    /// <param name="map">Función que mapea los resultados de la consulta a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">Tiempo máximo en segundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una lista de entidades de tipo <typeparamref name="TEntity"/> resultantes de la consulta.</returns>
    /// <remarks>
    /// Este método se asegura de ejecutar acciones antes y después de la consulta mediante los métodos 
    /// <c>ExecuteBefore</c> y <c>ExecuteAfter</c>, respectivamente. Si <c>ExecuteBefore</c> devuelve falso, 
    /// se retornará un valor por defecto.
    /// </remarks>
    /// <seealso cref="GetConnection"/>
    /// <seealso cref="GetSql"/>
    /// <seealso cref="GetTransaction"/>
    public virtual List<TEntity> ExecuteQuery<T1, T2, T3, T4, T5, TEntity>(Func<T1, T2, T3, T4, T5, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            var connection = GetConnection();
            result = connection.Query(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro del mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro del mapeo.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro del mapeo.</typeparam>
    /// <typeparam name="T6">El tipo del sexto parámetro del mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad resultante.</typeparam>
    /// <param name="map">Función que define cómo mapear los resultados de la consulta a una entidad.</param>
    /// <param name="timeout">Tiempo máximo en segundos para la ejecución de la consulta. Si es nulo, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una lista de entidades mapeadas a partir de los resultados de la consulta.</returns>
    /// <remarks>
    /// Este método ejecuta una consulta SQL y utiliza la función de mapeo proporcionada para convertir los resultados en una lista de entidades.
    /// Se asegura de que se ejecute un proceso antes y después de la consulta.
    /// </remarks>
    /// <seealso cref="GetConnection"/>
    /// <seealso cref="GetSql"/>
    /// <seealso cref="ExecuteBefore"/>
    /// <seealso cref="ExecuteAfter"/>
    public virtual List<TEntity> ExecuteQuery<T1, T2, T3, T4, T5, T6, TEntity>(Func<T1, T2, T3, T4, T5, T6, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            var connection = GetConnection();
            result = connection.Query(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro del mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro del mapeo.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro del mapeo.</typeparam>
    /// <typeparam name="T6">El tipo del sexto parámetro del mapeo.</typeparam>
    /// <typeparam name="T7">El tipo del séptimo parámetro del mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad a la que se mapean los resultados.</typeparam>
    /// <param name="map">Función que mapea los parámetros de la consulta a una entidad.</param>
    /// <param name="timeout">Tiempo máximo en segundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados de la consulta deben ser almacenados en memoria.</param>
    /// <returns>Una lista de entidades mapeadas a partir de los resultados de la consulta.</returns>
    /// <remarks>
    /// Este método ejecuta una consulta SQL y utiliza la función de mapeo proporcionada para transformar los resultados en una lista de entidades.
    /// Se asegura de que se realicen las operaciones necesarias antes y después de la ejecución de la consulta.
    /// </remarks>
    /// <exception cref="Exception">Lanza una excepción si ocurre un error durante la ejecución de la consulta.</exception>
    public virtual List<TEntity> ExecuteQuery<T1, T2, T3, T4, T5, T6, T7, TEntity>(Func<T1, T2, T3, T4, T5, T6, T7, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            var connection = GetConnection();
            result = connection.Query(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    #endregion

    #region ExecuteQueryAsync(Obtener conjunto de entidades)

    /// <summary>
    /// Ejecuta una consulta de forma asíncrona y devuelve una lista de resultados dinámicos.
    /// </summary>
    /// <param name="timeout">El tiempo máximo en segundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de resultados dinámicos.</returns>
    /// <remarks>
    /// Este método se asegura de que se ejecute una lógica antes y después de la ejecución de la consulta.
    /// Si la ejecución previa falla, se devolverá un valor nulo.
    /// </remarks>
    /// <exception cref="Exception">Lanza una excepción si ocurre un error durante la ejecución de la consulta.</exception>
    /// <seealso cref="GetConnection"/>
    /// <seealso cref="GetSql"/>
    /// <seealso cref="Params"/>
    /// <seealso cref="GetTransaction"/>
    /// <seealso cref="ExecuteBefore"/>
    /// <seealso cref="ExecuteAfter"/>
    public virtual async Task<List<dynamic>> ExecuteQueryAsync(int? timeout = null)
    {
        List<dynamic> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            var connection = GetConnection();
            result = (await connection.QueryAsync(GetSql(), Params, GetTransaction(), timeout)).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta de forma asíncrona y devuelve una lista de entidades.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se espera recibir como resultado de la consulta.</typeparam>
    /// <param name="timeout">El tiempo máximo en segundos para la ejecución de la consulta. Si es nulo, se usará el valor predeterminado.</param>
    /// <returns>
    /// Una lista de entidades del tipo especificado, o null si la ejecución no se realizó correctamente.
    /// </returns>
    /// <remarks>
    /// Este método se encarga de manejar la conexión a la base de datos y ejecutar la consulta SQL definida en el método <see cref="GetSql"/>.
    /// Se ejecutan métodos adicionales antes y después de la consulta mediante <see cref="ExecuteBefore"/> y <see cref="ExecuteAfter"/> respectivamente.
    /// </remarks>
    /// <seealso cref="GetSql"/>
    /// <seealso cref="GetConnection"/>
    /// <seealso cref="ExecuteBefore"/>
    /// <seealso cref="ExecuteAfter"/>
    public virtual async Task<List<TEntity>> ExecuteQueryAsync<TEntity>(int? timeout = null)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            var connection = GetConnection();
            result = (await connection.QueryAsync<TEntity>(GetSql(), Params, GetTransaction(), timeout)).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta asíncrona y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro utilizado en el mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro utilizado en el mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad resultante del mapeo.</typeparam>
    /// <param name="map">Función que define cómo mapear los resultados de la consulta a una entidad.</param>
    /// <param name="timeout">Tiempo máximo en milisegundos para la ejecución de la consulta. Si es null, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria (true) o no (false).</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de entidades como resultado.</returns>
    /// <remarks>
    /// Este método se asegura de ejecutar lógica antes y después de la consulta mediante los métodos <c>ExecuteBefore</c> y <c>ExecuteAfter</c>.
    /// Si <c>ExecuteBefore</c> devuelve false, se retorna null sin ejecutar la consulta.
    /// </remarks>
    /// <seealso cref="ExecuteBefore"/>
    /// <seealso cref="ExecuteAfter"/>
    /// <seealso cref="GetConnection"/>
    /// <seealso cref="GetSql"/>
    /// <seealso cref="Params"/>
    /// <seealso cref="GetTransaction"/>
    public virtual async Task<List<TEntity>> ExecuteQueryAsync<T1, T2, TEntity>(Func<T1, T2, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            var connection = GetConnection();
            result = (await connection.QueryAsync(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout)).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta asíncrona y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro del mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad resultante del mapeo.</typeparam>
    /// <param name="map">Función que define cómo mapear los parámetros a una entidad.</param>
    /// <param name="timeout">Tiempo de espera opcional para la consulta en milisegundos.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria.</param>
    /// <returns>Una lista de entidades del tipo especificado.</returns>
    /// <remarks>
    /// Este método ejecuta una consulta SQL utilizando una conexión a la base de datos y aplica una función de mapeo 
    /// para transformar los resultados en una lista de entidades. Se asegura de que se ejecuten las operaciones 
    /// necesarias antes y después de la ejecución de la consulta.
    /// </remarks>
    /// <seealso cref="GetConnection"/>
    /// <seealso cref="GetSql"/>
    /// <seealso cref="ExecuteBefore"/>
    /// <seealso cref="ExecuteAfter"/>
    public virtual async Task<List<TEntity>> ExecuteQueryAsync<T1, T2, T3, TEntity>(Func<T1, T2, T3, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            var connection = GetConnection();
            result = (await connection.QueryAsync(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout)).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta asíncrona y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro para el mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro para el mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro para el mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro para el mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad resultante del mapeo.</typeparam>
    /// <param name="map">Función que define cómo mapear los parámetros a una entidad.</param>
    /// <param name="timeout">Tiempo de espera opcional para la consulta, en milisegundos.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de entidades como resultado.</returns>
    /// <remarks>
    /// Este método se asegura de ejecutar acciones antes y después de la consulta mediante los métodos 
    /// <see cref="ExecuteBefore"/> y <see cref="ExecuteAfter"/>. 
    /// Si <see cref="ExecuteBefore"/> devuelve false, se retorna null sin ejecutar la consulta.
    /// </remarks>
    public virtual async Task<List<TEntity>> ExecuteQueryAsync<T1, T2, T3, T4, TEntity>(Func<T1, T2, T3, T4, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            var connection = GetConnection();
            result = (await connection.QueryAsync(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout)).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta asíncrona y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad resultante del mapeo.</typeparam>
    /// <param name="map">Función que mapea los resultados de la consulta a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">Tiempo máximo en milisegundos para la ejecución de la consulta. Si es nulo, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser procesados.</param>
    /// <returns>Una lista de entidades de tipo <typeparamref name="TEntity"/> resultantes de la consulta.</returns>
    /// <remarks>
    /// Este método ejecuta una consulta SQL y utiliza la función de mapeo proporcionada para transformar los resultados en una lista de entidades.
    /// Se asegura de que se ejecuten los métodos <c>ExecuteBefore</c> y <c>ExecuteAfter</c> para manejar la lógica antes y después de la ejecución de la consulta.
    /// </remarks>
    /// <seealso cref="ExecuteBefore"/>
    /// <seealso cref="ExecuteAfter"/>
    /// <seealso cref="GetConnection"/>
    /// <seealso cref="GetSql"/>
    /// <seealso cref="GetTransaction"/>
    public virtual async Task<List<TEntity>> ExecuteQueryAsync<T1, T2, T3, T4, T5, TEntity>(Func<T1, T2, T3, T4, T5, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            var connection = GetConnection();
            result = (await connection.QueryAsync(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout)).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta asíncrona y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de mapeo.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro de mapeo.</typeparam>
    /// <typeparam name="T6">El tipo del sexto parámetro de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad resultante.</typeparam>
    /// <param name="map">Función que define cómo mapear los parámetros a una entidad.</param>
    /// <param name="timeout">Tiempo máximo en milisegundos para la ejecución de la consulta. Si es nulo, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una lista de entidades de tipo <typeparamref name="TEntity"/> resultantes de la consulta.</returns>
    /// <remarks>
    /// Este método se asegura de ejecutar acciones antes y después de la consulta mediante los métodos <c>ExecuteBefore</c> y <c>ExecuteAfter</c>.
    /// Si <c>ExecuteBefore</c> devuelve <c>false</c>, se retornará <c>null</c> sin ejecutar la consulta.
    /// </remarks>
    /// <seealso cref="GetConnection"/>
    /// <seealso cref="GetSql"/>
    /// <seealso cref="GetTransaction"/>
    public virtual async Task<List<TEntity>> ExecuteQueryAsync<T1, T2, T3, T4, T5, T6, TEntity>(Func<T1, T2, T3, T4, T5, T6, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            var connection = GetConnection();
            result = (await connection.QueryAsync(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout)).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta asíncrona y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T6">El tipo del sexto parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="T7">El tipo del séptimo parámetro de la función de mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se devolverá en la lista.</typeparam>
    /// <param name="map">Función que mapea los parámetros de la consulta a una entidad.</param>
    /// <param name="timeout">Tiempo máximo en milisegundos para la ejecución de la consulta. Si es nulo, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una lista de entidades resultantes de la consulta.</returns>
    /// <remarks>
    /// Este método se asegura de ejecutar acciones antes y después de la consulta mediante los métodos 
    /// <c>ExecuteBefore</c> y <c>ExecuteAfter</c>. Si <c>ExecuteBefore</c> devuelve false, 
    /// el método retorna null sin ejecutar la consulta.
    /// </remarks>
    /// <seealso cref="ExecuteBefore"/>
    /// <seealso cref="ExecuteAfter"/>
    /// <seealso cref="GetConnection"/>
    /// <seealso cref="GetSql"/>
    /// <seealso cref="Params"/>
    /// <seealso cref="GetTransaction"/>
    public virtual async Task<List<TEntity>> ExecuteQueryAsync<T1, T2, T3, T4, T5, T6, T7, TEntity>(Func<T1, T2, T3, T4, T5, T6, T7, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            var connection = GetConnection();
            result = (await connection.QueryAsync(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout)).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    #endregion

    #region ExecuteProcedureQuery(Ejecutar un procedimiento almacenado para obtener un conjunto de entidades.)

    /// <summary>
    /// Ejecuta una consulta de procedimiento almacenado y devuelve los resultados como una lista dinámica.
    /// </summary>
    /// <param name="procedure">El nombre del procedimiento almacenado a ejecutar.</param>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado de la conexión.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria (true) o si se deben procesar de forma secuencial (false).</param>
    /// <returns>
    /// Una lista dinámica que contiene los resultados de la consulta. Si la ejecución falla o no se ejecuta, se devuelve null.
    /// </returns>
    /// <remarks>
    /// Este método se encarga de manejar la conexión a la base de datos, ejecutar el procedimiento almacenado especificado y procesar los resultados.
    /// Se ejecutan métodos adicionales antes y después de la consulta para realizar tareas de configuración y limpieza.
    /// </remarks>
    /// <exception cref="Exception">Lanza una excepción si ocurre un error durante la ejecución del procedimiento almacenado.</exception>
    public virtual List<dynamic> ExecuteProcedureQuery(string procedure, int? timeout = null, bool buffered = true)
    {
        List<dynamic> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = GetProcedure(procedure);
            var connection = GetConnection();
            result = connection.Query(GetSql(), Params, GetTransaction(), buffered, timeout, GetProcedureCommandType()).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta a un procedimiento almacenado y devuelve una lista de resultados.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se espera como resultado de la consulta.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo máximo en segundos que se permite para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos. El valor predeterminado es verdadero.</param>
    /// <returns>Una lista de entidades del tipo especificado que resultan de la ejecución del procedimiento almacenado. Si la ejecución falla, se devuelve el valor predeterminado de la lista.</returns>
    /// <remarks>
    /// Este método se asegura de que se ejecute una lógica antes y después de la ejecución de la consulta.
    /// Si la ejecución previa falla, se devuelve el valor predeterminado sin ejecutar la consulta.
    /// </remarks>
    /// <exception cref="Exception">Se lanzará una excepción si ocurre un error durante la ejecución de la consulta.</exception>
    /// <seealso cref="GetConnection"/>
    /// <seealso cref="GetProcedure"/>
    /// <seealso cref="ExecuteBefore"/>
    /// <seealso cref="ExecuteAfter"/>
    public virtual List<TEntity> ExecuteProcedureQuery<TEntity>(string procedure, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = GetProcedure(procedure);
            var connection = GetConnection();
            result = connection.Query<TEntity>(GetSql(), Params, GetTransaction(), buffered, timeout, GetProcedureCommandType()).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta de procedimiento almacenado y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de entrada del procedimiento almacenado.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de entrada del procedimiento almacenado.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir del mapeo de los resultados.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado a ejecutar.</param>
    /// <param name="map">Una función que define cómo mapear los resultados del procedimiento a una entidad.</param>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una lista de entidades mapeadas a partir de los resultados del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método se asegura de que se ejecute cualquier lógica previa y posterior a la ejecución del procedimiento,
    /// utilizando los métodos <see cref="ExecuteBefore"/> y <see cref="ExecuteAfter"/>.
    /// </remarks>
    /// <seealso cref="ExecuteBefore"/>
    /// <seealso cref="ExecuteAfter"/>
    /// <seealso cref="GetProcedure(string)"/>
    /// <seealso cref="GetConnection()"/>
    /// <seealso cref="GetSql()"/>
    /// <seealso cref="GetTransaction()"/>
    /// <seealso cref="GetProcedureCommandType()"/>
    public virtual List<TEntity> ExecuteProcedureQuery<T1, T2, TEntity>(string procedure, Func<T1, T2, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = GetProcedure(procedure);
            var connection = GetConnection();
            result = connection.Query(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout, GetProcedureCommandType()).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta de procedimiento almacenado y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir de los resultados del procedimiento.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado a ejecutar.</param>
    /// <param name="map">Función que define cómo mapear los resultados del procedimiento a una entidad.</param>
    /// <param name="timeout">Tiempo máximo en segundos para la ejecución de la consulta. Si es nulo, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria o no.</param>
    /// <returns>Una lista de entidades generadas a partir de los resultados del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método se encarga de manejar la conexión a la base de datos, ejecutar el procedimiento almacenado y mapear los resultados
    /// a la entidad especificada. Se ejecutan métodos antes y después de la ejecución para permitir la personalización del comportamiento.
    /// </remarks>
    /// <seealso cref="GetProcedure(string)"/>
    /// <seealso cref="GetConnection()"/>
    /// <seealso cref="ExecuteBefore()"/>
    /// <seealso cref="ExecuteAfter(List{TEntity})"/>
    public virtual List<TEntity> ExecuteProcedureQuery<T1, T2, T3, TEntity>(string procedure, Func<T1, T2, T3, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = GetProcedure(procedure);
            var connection = GetConnection();
            result = connection.Query(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout, GetProcedureCommandType()).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta de procedimiento almacenado y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad resultante del mapeo.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado a ejecutar.</param>
    /// <param name="map">Función que mapea los resultados del procedimiento a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">Tiempo máximo en segundos para la ejecución de la consulta. Si es null, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una lista de entidades de tipo <typeparamref name="TEntity"/> resultantes de la ejecución del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método se asegura de ejecutar acciones antes y después de la ejecución de la consulta,
    /// permitiendo la personalización del comportamiento mediante los métodos <c>ExecuteBefore</c> y <c>ExecuteAfter</c>.
    /// </remarks>
    /// <seealso cref="ExecuteBefore"/>
    /// <seealso cref="ExecuteAfter"/>
    /// <seealso cref="GetConnection"/>
    /// <seealso cref="GetProcedure"/>
    /// <seealso cref="GetSql"/>
    /// <seealso cref="GetTransaction"/>
    /// <seealso cref="GetProcedureCommandType"/>
    public virtual List<TEntity> ExecuteProcedureQuery<T1, T2, T3, T4, TEntity>(string procedure, Func<T1, T2, T3, T4, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = GetProcedure(procedure);
            var connection = GetConnection();
            result = connection.Query(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout, GetProcedureCommandType()).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta a un procedimiento almacenado y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir del mapeo.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado a ejecutar.</param>
    /// <param name="map">Función que define cómo mapear los resultados del procedimiento a una entidad.</param>
    /// <param name="timeout">Tiempo de espera en segundos para la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una lista de entidades generadas a partir del mapeo de los resultados del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método se encarga de ejecutar un procedimiento almacenado y manejar la conexión a la base de datos.
    /// Se asegura de que las operaciones previas y posteriores a la ejecución del procedimiento se realicen correctamente.
    /// </remarks>
    /// <seealso cref="GetProcedure(string)"/>
    /// <seealso cref="GetConnection()"/>
    /// <seealso cref="ExecuteBefore()"/>
    /// <seealso cref="ExecuteAfter(List{TEntity})"/>
    public virtual List<TEntity> ExecuteProcedureQuery<T1, T2, T3, T4, T5, TEntity>(string procedure, Func<T1, T2, T3, T4, T5, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = GetProcedure(procedure);
            var connection = GetConnection();
            result = connection.Query(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout, GetProcedureCommandType()).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta de procedimiento almacenado y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T6">El tipo del sexto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se devolverá como resultado de la consulta.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado a ejecutar.</param>
    /// <param name="map">Una función que mapea los resultados del procedimiento a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una lista de entidades de tipo <typeparamref name="TEntity"/> resultantes de la ejecución del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método permite ejecutar procedimientos almacenados con hasta seis parámetros y mapear los resultados a una entidad específica.
    /// Se recomienda manejar excepciones que puedan surgir durante la ejecución de la consulta.
    /// </remarks>
    /// <seealso cref="GetProcedure(string)"/>
    /// <seealso cref="GetConnection()"/>
    /// <seealso cref="ExecuteBefore()"/>
    /// <seealso cref="ExecuteAfter(List{TEntity})"/>
    public virtual List<TEntity> ExecuteProcedureQuery<T1, T2, T3, T4, T5, T6, TEntity>(string procedure, Func<T1, T2, T3, T4, T5, T6, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = GetProcedure(procedure);
            var connection = GetConnection();
            result = connection.Query(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout, GetProcedureCommandType()).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta de procedimiento almacenado y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro de entrada.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro de entrada.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro de entrada.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro de entrada.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro de entrada.</typeparam>
    /// <typeparam name="T6">El tipo del sexto parámetro de entrada.</typeparam>
    /// <typeparam name="T7">El tipo del séptimo parámetro de entrada.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se va a devolver.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado a ejecutar.</param>
    /// <param name="map">Función que mapea los resultados del procedimiento a una entidad.</param>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una lista de entidades mapeadas a partir de los resultados del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método se encarga de manejar la conexión a la base de datos y la ejecución del procedimiento almacenado.
    /// Se ejecutan métodos antes y después de la consulta para permitir la personalización del comportamiento.
    /// </remarks>
    /// <exception cref="Exception">Lanza una excepción si ocurre un error durante la ejecución del procedimiento.</exception>
    public virtual List<TEntity> ExecuteProcedureQuery<T1, T2, T3, T4, T5, T6, T7, TEntity>(string procedure, Func<T1, T2, T3, T4, T5, T6, T7, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = GetProcedure(procedure);
            var connection = GetConnection();
            result = connection.Query(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout, GetProcedureCommandType()).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    #endregion

    #region ExecuteProcedureQueryAsync(Ejecutar un procedimiento almacenado para obtener un conjunto de entidades.)

    /// <summary>
    /// Ejecuta un procedimiento almacenado de manera asíncrona y devuelve los resultados como una lista dinámica.
    /// </summary>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo máximo en segundos para esperar la ejecución del procedimiento. Si es nulo, se usará el valor predeterminado.</param>
    /// <returns>
    /// Una lista de objetos dinámicos que representan los resultados del procedimiento almacenado.
    /// Si la ejecución del procedimiento falla o si <c>ExecuteBefore</c> devuelve falso, se devolverá <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método maneja la conexión a la base de datos y asegura que se ejecute un método de limpieza después de la ejecución,
    /// independientemente de si la ejecución fue exitosa o no.
    /// </remarks>
    /// <exception cref="Exception">Lanza una excepción si ocurre un error durante la ejecución del procedimiento.</exception>
    public virtual async Task<List<dynamic>> ExecuteProcedureQueryAsync(string procedure, int? timeout = null)
    {
        List<dynamic> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = GetProcedure(procedure);
            var connection = GetConnection();
            result = (await connection.QueryAsync(GetSql(), Params, GetTransaction(), timeout, GetProcedureCommandType())).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta de procedimiento almacenado de manera asíncrona y devuelve una lista de entidades.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se espera como resultado de la consulta.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="timeout">El tiempo máximo en segundos que se permite para la ejecución de la consulta. Si es null, se utilizará el valor predeterminado.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El valor de la tarea es una lista de entidades del tipo especificado.
    /// </returns>
    /// <remarks>
    /// Este método ejecuta un procedimiento almacenado en la base de datos y devuelve los resultados como una lista de entidades. 
    /// Se ejecutan métodos antes y después de la consulta para manejar cualquier lógica adicional necesaria.
    /// </remarks>
    /// <exception cref="Exception">Lanza una excepción en caso de que ocurra un error durante la ejecución del procedimiento.</exception>
    /// <seealso cref="GetProcedure(string)"/>
    /// <seealso cref="GetConnection()"/>
    /// <seealso cref="ExecuteBefore()"/>
    /// <seealso cref="ExecuteAfter(List{TEntity})"/>
    public virtual async Task<List<TEntity>> ExecuteProcedureQueryAsync<TEntity>(string procedure, int? timeout = null)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = GetProcedure(procedure);
            var connection = GetConnection();
            result = (await connection.QueryAsync<TEntity>(GetSql(), Params, GetTransaction(), timeout, GetProcedureCommandType())).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta a un procedimiento almacenado de forma asíncrona y mapea los resultados a un tipo específico.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro que se pasa al mapeo.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro que se pasa al mapeo.</typeparam>
    /// <typeparam name="TEntity">El tipo del objeto que se generará a partir del mapeo.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="map">Función que define cómo mapear los resultados del procedimiento a un objeto de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">Tiempo máximo en segundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una lista de objetos de tipo <typeparamref name="TEntity"/> resultantes de la ejecución del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método permite ejecutar procedimientos almacenados y mapear los resultados a un tipo específico utilizando una función de mapeo proporcionada por el usuario.
    /// Se asegura de que se realicen las operaciones necesarias antes y después de la ejecución del procedimiento.
    /// </remarks>
    /// <seealso cref="ExecuteBefore"/>
    /// <seealso cref="ExecuteAfter"/>
    /// <seealso cref="GetConnection"/>
    /// <seealso cref="GetProcedure"/>
    /// <seealso cref="GetSql"/>
    /// <seealso cref="GetTransaction"/>
    /// <seealso cref="GetProcedureCommandType"/>
    public virtual async Task<List<TEntity>> ExecuteProcedureQueryAsync<T1, T2, TEntity>(string procedure, Func<T1, T2, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = GetProcedure(procedure);
            var connection = GetConnection();
            result = (await connection.QueryAsync(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout, GetProcedureCommandType())).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta a un procedimiento almacenado de forma asíncrona y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir del mapeo.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado a ejecutar.</param>
    /// <param name="map">Función que mapea los resultados del procedimiento a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">Tiempo de espera opcional en segundos para la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos. El valor predeterminado es verdadero.</param>
    /// <returns>Una lista de entidades de tipo <typeparamref name="TEntity"/> resultantes de la ejecución del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método se ejecuta de forma asíncrona y utiliza una conexión a la base de datos para realizar la consulta.
    /// Se asegura de ejecutar cualquier lógica previa y posterior a la consulta mediante los métodos <c>ExecuteBefore</c> y <c>ExecuteAfter</c>.
    /// </remarks>
    /// <exception cref="Exception">Lanza una excepción si ocurre un error durante la ejecución de la consulta.</exception>
    public virtual async Task<List<TEntity>> ExecuteProcedureQueryAsync<T1, T2, T3, TEntity>(string procedure, Func<T1, T2, T3, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = GetProcedure(procedure);
            var connection = GetConnection();
            result = (await connection.QueryAsync(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout, GetProcedureCommandType())).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta un procedimiento almacenado de manera asíncrona y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir del mapeo.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="map">Función que mapea los resultados del procedimiento a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">Tiempo de espera en segundos para la ejecución del procedimiento. Si es nulo, se usará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria (true) o no (false).</param>
    /// <returns>Una lista de entidades de tipo <typeparamref name="TEntity"/> resultantes de la ejecución del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método realiza una llamada asíncrona a un procedimiento almacenado y espera los resultados. 
    /// Se ejecutan métodos antes y después de la ejecución del procedimiento para manejar la lógica adicional necesaria.
    /// </remarks>
    /// <exception cref="Exception">Lanza una excepción si ocurre un error durante la ejecución del procedimiento.</exception>
    /// <seealso cref="GetProcedure(string)"/>
    /// <seealso cref="GetConnection()"/>
    /// <seealso cref="ExecuteBefore()"/>
    /// <seealso cref="ExecuteAfter(List{TEntity})"/>
    public virtual async Task<List<TEntity>> ExecuteProcedureQueryAsync<T1, T2, T3, T4, TEntity>(string procedure, Func<T1, T2, T3, T4, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = GetProcedure(procedure);
            var connection = GetConnection();
            result = (await connection.QueryAsync(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout, GetProcedureCommandType())).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta una consulta de procedimiento almacenado de forma asíncrona y mapea los resultados a un tipo específico.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="TEntity">El tipo del resultado que se devolverá.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado a ejecutar.</param>
    /// <param name="map">Una función que mapea los resultados del procedimiento a un objeto de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">El tiempo máximo en segundos para la ejecución de la consulta. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una lista de objetos de tipo <typeparamref name="TEntity"/> resultantes de la ejecución del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método permite la ejecución de procedimientos almacenados con múltiples parámetros y el mapeo de los resultados a un tipo específico.
    /// Se asegura de que se ejecuten las operaciones necesarias antes y después de la consulta.
    /// </remarks>
    /// <seealso cref="GetProcedure(string)"/>
    /// <seealso cref="GetConnection()"/>
    /// <seealso cref="ExecuteBefore()"/>
    /// <seealso cref="ExecuteAfter(List{TEntity})"/>
    public virtual async Task<List<TEntity>> ExecuteProcedureQueryAsync<T1, T2, T3, T4, T5, TEntity>(string procedure, Func<T1, T2, T3, T4, T5, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = GetProcedure(procedure);
            var connection = GetConnection();
            result = (await connection.QueryAsync(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout, GetProcedureCommandType())).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta un procedimiento almacenado de forma asíncrona y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T6">El tipo del sexto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se generará a partir de los resultados del procedimiento.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado a ejecutar.</param>
    /// <param name="map">Una función que mapea los resultados del procedimiento a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">El tiempo de espera en segundos para la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de entidades de tipo <typeparamref name="TEntity"/> como resultado.</returns>
    /// <remarks>
    /// Este método permite ejecutar procedimientos almacenados de manera asíncrona y transformar los resultados en una lista de entidades utilizando una función de mapeo proporcionada.
    /// Asegúrese de que el procedimiento almacenado y los tipos de parámetros coincidan con los tipos especificados.
    /// </remarks>
    /// <seealso cref="ExecuteBefore"/>
    /// <seealso cref="ExecuteAfter"/>
    public virtual async Task<List<TEntity>> ExecuteProcedureQueryAsync<T1, T2, T3, T4, T5, T6, TEntity>(string procedure, Func<T1, T2, T3, T4, T5, T6, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = GetProcedure(procedure);
            var connection = GetConnection();
            result = (await connection.QueryAsync(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout, GetProcedureCommandType())).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    /// <summary>
    /// Ejecuta un procedimiento almacenado de forma asíncrona y mapea los resultados a una lista de entidades.
    /// </summary>
    /// <typeparam name="T1">El tipo del primer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T2">El tipo del segundo parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T3">El tipo del tercer parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T4">El tipo del cuarto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T5">El tipo del quinto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T6">El tipo del sexto parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="T7">El tipo del séptimo parámetro del procedimiento almacenado.</typeparam>
    /// <typeparam name="TEntity">El tipo de la entidad que se devolverá en la lista.</typeparam>
    /// <param name="procedure">El nombre del procedimiento almacenado que se va a ejecutar.</param>
    /// <param name="map">Función que mapea los resultados del procedimiento a una entidad de tipo <typeparamref name="TEntity"/>.</param>
    /// <param name="timeout">Tiempo máximo en segundos para la ejecución del procedimiento. Si es nulo, se utilizará el valor predeterminado.</param>
    /// <param name="buffered">Indica si los resultados deben ser almacenados en memoria antes de ser devueltos.</param>
    /// <returns>Una lista de entidades de tipo <typeparamref name="TEntity"/> resultantes de la ejecución del procedimiento almacenado.</returns>
    /// <remarks>
    /// Este método se asegura de ejecutar acciones antes y después de la ejecución del procedimiento mediante los métodos <c>ExecuteBefore</c> y <c>ExecuteAfter</c>.
    /// Si <c>ExecuteBefore</c> devuelve falso, el método retornará un valor nulo sin ejecutar el procedimiento.
    /// </remarks>
    /// <seealso cref="ExecuteBefore"/>
    /// <seealso cref="ExecuteAfter"/>
    /// <seealso cref="GetProcedure"/>
    /// <seealso cref="GetConnection"/>
    /// <seealso cref="GetSql"/>
    /// <seealso cref="GetTransaction"/>
    /// <seealso cref="GetProcedureCommandType"/>
    public virtual async Task<List<TEntity>> ExecuteProcedureQueryAsync<T1, T2, T3, T4, T5, T6, T7, TEntity>(string procedure, Func<T1, T2, T3, T4, T5, T6, T7, TEntity> map, int? timeout = null, bool buffered = true)
    {
        List<TEntity> result = default;
        try
        {
            if (ExecuteBefore() == false)
                return default;
            _sql = GetProcedure(procedure);
            var connection = GetConnection();
            result = (await connection.QueryAsync(GetSql(), map, Params, GetTransaction(), buffered, "Id", timeout, GetProcedureCommandType())).ToList();
            return result;
        }
        finally
        {
            ExecuteAfter(result);
        }
    }

    #endregion

    #region Método de gancho

    /// <summary>
    /// Ejecuta una acción antes de realizar la operación principal.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si la acción se ejecutó correctamente; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación específica.
    /// </remarks>
    protected virtual bool ExecuteBefore()
    {
        return true;
    }


    /// <summary>
    /// Ejecuta acciones después de un proceso, utilizando el resultado proporcionado.
    /// </summary>
    /// <param name="result">El resultado del proceso anterior que se utilizará para las acciones posteriores.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar
    /// una implementación específica de las acciones a realizar después de la ejecución.
    /// </remarks>
    protected virtual void ExecuteAfter(object result)
    {
        SetPreviousSql();
        Clear();
        WriteLog();
    }


    #endregion

    #region GetSql(Obtener la sentencia SQL.)

    /// <summary>
    /// Obtiene la cadena SQL generada por el constructor de SQL.
    /// </summary>
    /// <returns>
    /// Una cadena que representa la consulta SQL. Si ya se ha generado previamente, se devuelve la misma instancia; 
    /// de lo contrario, se genera una nueva consulta SQL utilizando el <see cref="SqlBuilder"/>.
    /// </returns>
    public string GetSql()
    {
        return _sql ??= SqlBuilder.GetSql();
    }

    #endregion

    #region SetSql(Configurar la sentencia SQL.)

    /// <summary>
    /// Establece la cadena SQL que se utilizará en la operación.
    /// </summary>
    /// <param name="sql">La cadena SQL que se desea establecer.</param>
    protected void SetSql(string sql)
    {
        _sql = sql;
    }


    #endregion

    #region GetProcedure(Obtener el nombre del procedimiento almacenado.)

    /// <summary>
    /// Obtiene el procedimiento en forma de cadena a partir del nombre del procedimiento especificado.
    /// </summary>
    /// <param name="procedure">El nombre del procedimiento que se desea obtener.</param>
    /// <returns>Una cadena que representa el resultado del procedimiento.</returns>
    /// <remarks>
    /// Este método crea una nueva instancia de <see cref="TableItem"/> utilizando el dialecto actual y el nombre del procedimiento,
    /// y luego llama al método <see cref="ToResult"/> para obtener el resultado.
    /// </remarks>
    protected virtual string GetProcedure(string procedure)
    {
        return new TableItem(Dialect, procedure).ToResult();
    }


    #endregion

    #region GetProcedureCommandType(Obtener el tipo de comando del procedimiento almacenado.)

    /// <summary>
    /// Obtiene el tipo de comando para procedimientos almacenados.
    /// </summary>
    /// <returns>
    /// Devuelve un valor de <see cref="CommandType"/> que representa el tipo de comando.
    /// </returns>
    protected virtual CommandType GetProcedureCommandType()
    {
        return CommandType.StoredProcedure;
    }


    #endregion

    #region SetPreviousSql(Configurar la SQL y los parámetros de la última ejecución.)

    /// <summary>
    /// Establece la consulta SQL anterior utilizando los parámetros obtenidos.
    /// </summary>
    /// <remarks>
    /// Este método crea una instancia de <see cref="SqlBuilderResult"/> 
    /// con la consulta SQL actual y los parámetros asociados.
    /// </remarks>
    protected virtual void SetPreviousSql()
    {
        var sqlParams = GetSqlParams();
        PreviousSql = new SqlBuilderResult(_sql, sqlParams, ParamLiteralsResolver, ParameterManager);
    }


    #endregion

    #region GetSqlParams(Obtener lista de parámetros de Sql)

    /// <summary>
    /// Obtiene una lista de parámetros SQL a partir de los parámetros definidos.
    /// </summary>
    /// <returns>
    /// Una lista de objetos <see cref="SqlParam"/> que representan los parámetros SQL.
    /// </returns>
    /// <remarks>
    /// Este método verifica si la colección de parámetros (_parameters) es nula. Si es así, retorna una lista vacía.
    /// Para cada nombre de parámetro en _parameters, se verifica si existe en el <see cref="ParameterManager"/>.
    /// Si el parámetro es de tipo de dirección diferente a <c>Input</c>, se crea un nuevo objeto <see cref="SqlParam"/> 
    /// con los valores correspondientes. Si el parámetro no se encuentra en el <see cref="ParameterManager"/>, 
    /// se crea un nuevo <see cref="SqlParam"/> utilizando el nombre normalizado.
    /// </remarks>
    protected List<SqlParam> GetSqlParams()
    {
        var result = new List<SqlParam>();
        if (_parameters == null)
            return result;
        foreach (var parameterName in _parameters.ParameterNames)
        {
            if (ParameterManager.Contains(parameterName))
            {
                var param = ParameterManager.GetParam(parameterName);
                if (param.Direction != ParameterDirection.Input)
                    param = new SqlParam(param.Name, _parameters.Get<object>(parameterName), param.DbType, param.Direction, param.Size, param.Precision, param.Scale);
                result.Add(param);
                continue;
            }
            var sqlParam = new SqlParam(ParameterManager.NormalizeName(parameterName), _parameters.Get<object>(parameterName));
            result.Add(sqlParam);
        }
        return result;
    }


    #endregion

    #region Clear(Limpieza)

    /// <summary>
    /// Limpia el estado actual del objeto, eliminando la consulta SQL y los parámetros asociados.
    /// </summary>
    /// <remarks>
    /// Este método establece la propiedad <c>_sql</c> a <c>null</c>, 
    /// llama al método <c>Clear</c> de <c>SqlBuilder</c> para limpiar cualquier 
    /// construcción SQL previa y también invoca <c>ClearParams</c> para 
    /// eliminar los parámetros almacenados.
    /// </remarks>
    protected void Clear()
    {
        _sql = null;
        SqlBuilder.Clear();
        ClearParams();
    }

    #endregion

    #region WriteLog(Escribir un diario.)

    /// <summary>
    /// Registra información de SQL en el log si el nivel de log está habilitado para Trace.
    /// </summary>
    /// <remarks>
    /// Este método construye un mensaje de log que incluye el SQL original, el SQL de depuración y los parámetros utilizados en la consulta.
    /// Cada parámetro se detalla con su nombre, valor, tipo, dirección, tamaño, precisión y escala, si están disponibles.
    /// </remarks>
    /// <param name="PreviousSql">Una instancia que contiene la información del SQL previo, incluyendo el SQL original, el SQL de depuración y los parámetros.</param>
    /// <seealso cref="Logger"/>
    /// <seealso cref="LogLevel"/>
    protected virtual void WriteLog()
    {
        if (Logger.IsEnabled(LogLevel.Trace) == false)
            return;
        var message = new StringBuilder();
        message.AppendLine("Título: Registro SQL");
        message.AppendLine("SQL original:");
        message.AppendLine(PreviousSql.GetSql());
        message.AppendLine("Depurar SQL:");
        message.AppendLine(PreviousSql.GetDebugSql());
        message.AppendLine("Parámetros SQL:");
        foreach (var param in PreviousSql.GetParams())
        {
            message.Append("Nombre del parámetro: ");
            message.Append(param.Name);
            if (param.Value != null)
            {
                message.Append(",Valores de parámetros: ");
                message.Append(param.Value);
            }
            if (param.DbType != null)
            {
                message.Append(",Tipo de parámetro: ");
                message.Append(param.DbType);
            }
            if (param.Direction != null)
            {
                message.Append(",Dirección de parámetros: ");
                message.Append(param.Direction);
            }
            if (param.Size != null)
            {
                message.Append(",Size: ");
                message.Append(param.Size);
            }
            if (param.Precision != null)
            {
                message.Append(",Precision: ");
                message.Append(param.Precision);
            }
            if (param.Scale != null)
            {
                message.Append(",Scale: ");
                message.Append(param.Scale);
            }
            message.AppendLine();
        }
        Logger.LogTrace(message.ToString());
    }

    #endregion

    #region Dispose(Liberar recursos)

    /// <summary>
    /// Libera los recursos utilizados por la instancia actual de la clase.
    /// </summary>
    /// <remarks>
    /// Este método cierra la conexión a la base de datos si está abierta y establece la transacción en null.
    /// Asegúrese de llamar a este método cuando haya terminado de usar la instancia para evitar fugas de memoria.
    /// </remarks>
    public void Dispose()
    {
        if (_connection != null)
            _connection.Dispose();
        _transaction = null;
    }

    #endregion
}