using Util.Data.Sql.Builders.Caches;
using Util.Data.Sql.Builders.Clauses;
using Util.Data.Sql.Builders.Params;
using Util.Data.Sql.Builders.Sets;
using Util.Helpers;

namespace Util.Data.Sql.Builders;

/// <summary>
/// Clase base abstracta que proporciona una implementación común para construir consultas SQL.
/// </summary>
/// <remarks>
/// Esta clase implementa las interfaces <see cref="ISqlBuilder"/> y <see cref="ISqlPartAccessor"/>.
/// Se espera que las clases derivadas proporcionen la lógica específica para construir diferentes tipos de consultas SQL.
/// </remarks>
public abstract class SqlBuilderBase : ISqlBuilder, ISqlPartAccessor
{

    #region Campo

    private IDialect _dialect;
    private IColumnCache _columnCache;
    private IParameterManager _parameterManager;
    private IConditionFactory _conditionFactory;
    private IStartClause _startClause;
    private IInsertClause _insertClause;
    private ISelectClause _selectClause;
    private IFromClause _fromClause;
    private IJoinClause _joinClause;
    private IWhereClause _whereClause;
    private IGroupByClause _groupByClause;
    private IOrderByClause _orderByClause;
    private IEndClause _endClause;
    private ISqlBuilderSet _sqlBuilderSet;

    #endregion

    #region Método constructor

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqlBuilderBase"/>.
    /// </summary>
    /// <param name="parameterManager">
    /// Un objeto que implementa <see cref="IParameterManager"/> que se utilizará para gestionar los parámetros. 
    /// Si se proporciona un valor nulo, se utilizará un administrador de parámetros predeterminado.
    /// </param>
    protected SqlBuilderBase(IParameterManager parameterManager = null)
    {
        _parameterManager = parameterManager;
    }

    #endregion

    #region atributo

    /// <summary>
    /// Obtiene el dialecto utilizado por la instancia actual.
    /// </summary>
    /// <value>
    /// Un objeto que implementa la interfaz <see cref="IDialect"/> que representa el dialecto.
    /// </value>
    /// <remarks>
    /// Si el dialecto aún no ha sido creado, se invoca el método <see cref="CreateDialect"/> 
    /// para inicializarlo antes de devolverlo.
    /// </remarks>
    public IDialect Dialect => _dialect ??= CreateDialect();
    /// <summary>
    /// Obtiene el caché de columnas.
    /// </summary>
    /// <remarks>
    /// Si el caché de columnas aún no ha sido creado, se inicializa utilizando el método <see cref="CreateColumnCache"/>.
    /// </remarks>
    /// <value>
    /// Un objeto que implementa la interfaz <see cref="IColumnCache"/> que representa el caché de columnas.
    /// </value>
    public IColumnCache ColumnCache => _columnCache ??= CreateColumnCache();
    /// <summary>
    /// Obtiene una instancia de <see cref="IParameterManager"/>.
    /// </summary>
    /// <remarks>
    /// Si la instancia de <see cref="_parameterManager"/> es nula, se crea una nueva utilizando el método <see cref="CreateParameterManager"/>.
    /// </remarks>
    /// <value>
    /// Una instancia de <see cref="IParameterManager"/> que gestiona los parámetros.
    /// </value>
    public IParameterManager ParameterManager => _parameterManager ??= CreateParameterManager();
    /// <summary>
    /// Obtiene la instancia de <see cref="IConditionFactory"/>. 
    /// Si la instancia no ha sido creada, se inicializa utilizando el método <see cref="CreateConditionFactory"/>.
    /// </summary>
    /// <value>
    /// La instancia de <see cref="IConditionFactory"/>.
    /// </value>
    /// <remarks>
    /// Este acceso a la propiedad es útil para asegurar que la fábrica de condiciones se crea solo una vez 
    /// y se reutiliza en llamadas posteriores.
    /// </remarks>
    public IConditionFactory ConditionFactory => _conditionFactory ??= CreateConditionFactory();
    /// <summary>
    /// Obtiene la cláusula de inicio, creando una nueva instancia si no existe.
    /// </summary>
    /// <value>
    /// La cláusula de inicio actual.
    /// </value>
    /// <remarks>
    /// Este acceso a la propiedad utiliza el operador de asignación condicional para
    /// inicializar la cláusula de inicio solo si es nula.
    /// </remarks>
    public IStartClause StartClause => _startClause ??= CreateStartClause();
    /// <summary>
    /// Obtiene la cláusula de selección.
    /// </summary>
    /// <value>
    /// La cláusula de selección, que es una instancia de <see cref="ISelectClause"/>.
    /// </value>
    /// <remarks>
    /// Si la cláusula de selección no ha sido creada aún, se invoca el método <see cref="CreateSelectClause"/> 
    /// para inicializarla.
    /// </remarks>
    public ISelectClause SelectClause => _selectClause ??= CreateSelectClause();
    /// <summary>
    /// Obtiene la cláusula FROM asociada.
    /// </summary>
    /// <value>
    /// La cláusula FROM actual. Si no existe, se crea una nueva instancia.
    /// </value>
    /// <remarks>
    /// Esta propiedad utiliza el operador de asignación de null coalescente para
    /// devolver la cláusula FROM existente o crear una nueva si no está disponible.
    /// </remarks>
    public IFromClause FromClause => _fromClause ??= CreateFromClause();
    /// <summary>
    /// Obtiene la cláusula de unión asociada.
    /// </summary>
    /// <value>
    /// La cláusula de unión actual. Si no existe, se crea una nueva instancia de la cláusula de unión.
    /// </value>
    /// <remarks>
    /// Este miembro utiliza la evaluación perezosa para crear la cláusula de unión solo cuando es necesario.
    /// </remarks>
    public IJoinClause JoinClause => _joinClause ??= CreateJoinClause();
    /// <summary>
    /// Obtiene la cláusula WHERE asociada.
    /// </summary>
    /// <value>
    /// Una instancia de <see cref="IWhereClause"/> que representa la cláusula WHERE.
    /// </value>
    /// <remarks>
    /// Si la cláusula WHERE no ha sido creada previamente, se invoca el método <see cref="CreatewWhereClause"/> 
    /// para crear una nueva instancia.
    /// </remarks>
    public IWhereClause WhereClause => _whereClause ??= CreatewWhereClause();
    /// <summary>
    /// Obtiene la cláusula GROUP BY asociada.
    /// </summary>
    /// <value>
    /// La cláusula GROUP BY que se utiliza en la consulta.
    /// </value>
    /// <remarks>
    /// Si la cláusula GROUP BY no ha sido creada previamente, se generará una nueva instancia
    /// utilizando el método <see cref="CreateGroupByClause"/>.
    /// </remarks>
    public IGroupByClause GroupByClause => _groupByClause ??= CreateGroupByClause();
    /// <summary>
    /// Obtiene la cláusula de ordenación asociada.
    /// </summary>
    /// <value>
    /// La cláusula de ordenación que se utiliza para ordenar los resultados.
    /// Si la cláusula de ordenación no ha sido inicializada, se crea una nueva instancia.
    /// </value>
    public IOrderByClause OrderByClause => _orderByClause ??= CreateOrderByClause();
    /// <summary>
    /// Obtiene la cláusula de inserción.
    /// </summary>
    /// <value>
    /// La cláusula de inserción actual. Si no existe, se crea una nueva instancia.
    /// </value>
    public IInsertClause InsertClause => _insertClause ??= CreateInsertClause();
    /// <summary>
    /// Obtiene la cláusula de finalización.
    /// </summary>
    /// <value>
    /// La cláusula de finalización, que se crea si no existe previamente.
    /// </value>
    /// <remarks>
    /// Esta propiedad utiliza la asignación condicional para inicializar la cláusula de finalización
    /// solo cuando se accede por primera vez. Esto ayuda a optimizar el rendimiento al evitar
    /// la creación de un objeto innecesario si no se necesita.
    /// </remarks>
    public IEndClause EndClause => _endClause ??= CreateEndClause();
    /// <summary>
    /// Obtiene el conjunto de generadores SQL.
    /// </summary>
    /// <value>
    /// Un objeto que implementa la interfaz <see cref="ISqlBuilderSet"/>.
    /// </value>
    /// <remarks>
    /// Si el conjunto de generadores SQL no ha sido creado, se inicializa mediante el método <see cref="CreateSqlBuilderSet"/>.
    /// </remarks>
    public ISqlBuilderSet SqlBuilderSet => _sqlBuilderSet ??= CreateSqlBuilderSet();

    #endregion

    #region Método de fábrica

    /// <summary>
    /// Crea una instancia de un dialecto específico.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="IDialect"/> que representa el dialecto creado.
    /// </returns>
    /// <remarks>
    /// Este método es abstracto y debe ser implementado por las clases derivadas.
    /// </remarks>
    protected abstract IDialect CreateDialect();

    /// <summary>
    /// Crea una instancia de un caché de columnas.
    /// </summary>
    /// <remarks>
    /// Este método es abstracto y debe ser implementado por las clases derivadas.
    /// La implementación debe proporcionar la lógica específica para crear un caché de columnas.
    /// </remarks>
    /// <returns>
    /// Una instancia que implementa la interfaz <see cref="IColumnCache"/>.
    /// </returns>
    protected abstract IColumnCache CreateColumnCache();

    /// <summary>
    /// Crea una instancia de <see cref="IParameterManager"/>.
    /// </summary>
    /// <returns>
    /// Un objeto de tipo <see cref="IParameterManager"/> que gestiona los parámetros.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito por clases derivadas para proporcionar una implementación personalizada.
    /// </remarks>
    protected virtual IParameterManager CreateParameterManager()
    {
        return new ParameterManager(Dialect);
    }

    /// <summary>
    /// Crea una instancia de <see cref="IConditionFactory"/>.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="IConditionFactory"/> que utiliza <see cref="SqlConditionFactory"/>.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito por clases derivadas para proporcionar una implementación diferente de <see cref="IConditionFactory"/>.
    /// </remarks>
    protected virtual IConditionFactory CreateConditionFactory()
    {
        return new SqlConditionFactory(ParameterManager);
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="IStartClause"/>.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IStartClause"/> que representa la cláusula de inicio.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar
    /// una implementación personalizada de la cláusula de inicio.
    /// </remarks>
    protected virtual IStartClause CreateStartClause()
    {
        return new StartClause(this);
    }

    /// <summary>
    /// Crea una cláusula de inserción.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IInsertClause"/> que representa la cláusula de inserción.
    /// </returns>
    protected virtual IInsertClause CreateInsertClause()
    {
        return new InsertClause(this);
    }

    /// <summary>
    /// Crea una cláusula de selección.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="ISelectClause"/> que representa la cláusula de selección.
    /// </returns>
    protected virtual ISelectClause CreateSelectClause()
    {
        return new SelectClause(this);
    }

    /// <summary>
    /// Crea una instancia de la cláusula FROM.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="IFromClause"/> que representa la cláusula FROM.
    /// </returns>
    protected virtual IFromClause CreateFromClause()
    {
        return new FromClause(this);
    }

    /// <summary>
    /// Crea una cláusula de unión.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IJoinClause"/> que representa la cláusula de unión creada.
    /// </returns>
    protected virtual IJoinClause CreateJoinClause()
    {
        return new JoinClause(this);
    }

    /// <summary>
    /// Crea una instancia de un objeto <see cref="IWhereClause"/>.
    /// </summary>
    /// <returns>
    /// Un nuevo objeto <see cref="IWhereClause"/> que representa la cláusula WHERE.
    /// </returns>
    protected virtual IWhereClause CreatewWhereClause()
    {
        return new WhereClause(this);
    }

    /// <summary>
    /// Crea una instancia de la cláusula GroupBy.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="IGroupByClause"/> que representa la cláusula GroupBy.
    /// </returns>
    protected virtual IGroupByClause CreateGroupByClause()
    {
        return new GroupByClause(this);
    }

    /// <summary>
    /// Crea una cláusula de ordenamiento.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IOrderByClause"/> que representa la cláusula de ordenamiento.
    /// </returns>
    protected virtual IOrderByClause CreateOrderByClause()
    {
        return new OrderByClause(this);
    }

    /// <summary>
    /// Crea una instancia de la cláusula final.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="IEndClause"/> que representa la cláusula final.
    /// </returns>
    protected virtual IEndClause CreateEndClause()
    {
        return new EndClause(this);
    }

    /// <summary>
    /// Crea una instancia de <see cref="ISqlBuilderSet"/>.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="ISqlBuilderSet"/> que representa un conjunto de constructores de SQL.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito por clases derivadas para proporcionar una implementación personalizada.
    /// </remarks>
    protected virtual ISqlBuilderSet CreateSqlBuilderSet()
    {
        return new SqlBuilderSet(this);
    }

    #endregion

    #region Clone(Generador de SQL de copia)

    /// <inheritdoc />
    /// <summary>
    /// Clona la instancia actual de <see cref="ISqlBuilder"/>.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="ISqlBuilder"/> que es una copia de la instancia actual.
    /// </returns>
    /// <remarks>
    /// Este método debe ser implementado por las clases que heredan de <see cref="ISqlBuilder"/> 
    /// para proporcionar una funcionalidad de clonación específica.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    public abstract ISqlBuilder Clone();

    /// <summary>
    /// Clona las propiedades de un objeto <see cref="SqlBuilderBase"/> fuente en el objeto actual.
    /// </summary>
    /// <param name="source">El objeto <see cref="SqlBuilderBase"/> que se va a clonar.</param>
    /// <remarks>
    /// Este método copia las propiedades del objeto fuente, incluyendo el dialecto, el caché de columnas,
    /// y los diferentes cláusulas SQL, asegurando que el nuevo objeto tenga la misma configuración
    /// que el objeto fuente.
    /// </remarks>
    protected void Clone(SqlBuilderBase source)
    {
        _dialect = source._dialect;
        _columnCache = source._columnCache;
        _parameterManager = source._parameterManager?.Clone();
        _startClause = source._startClause?.Clone(this);
        _insertClause = source._insertClause?.Clone(this);
        _selectClause = source._selectClause?.Clone(this);
        _fromClause = source._fromClause?.Clone(this);
        _joinClause = source._joinClause?.Clone(this);
        _whereClause = source._whereClause?.Clone(this);
        _groupByClause = source._groupByClause?.Clone(this);
        _orderByClause = source._orderByClause?.Clone(this);
        _endClause = source._endClause?.Clone(this);
        _sqlBuilderSet = source._sqlBuilderSet?.Clone(this);
    }

    #endregion

    #region Clear(Limpiar)

    /// <summary>
    /// Limpia todos los componentes del generador de SQL, restableciendo su estado a uno inicial.
    /// </summary>
    /// <returns>
    /// Devuelve la instancia actual de <see cref="ISqlBuilder"/> para permitir la encadenación de métodos.
    /// </returns>
    /// <remarks>
    /// Este método elimina todos los parámetros y cláusulas que se han configurado previamente,
    /// permitiendo comenzar una nueva construcción de consulta SQL desde cero.
    /// </remarks>
    public ISqlBuilder Clear()
    {
        _parameterManager?.Clear();
        _startClause?.Clear();
        _insertClause?.Clear();
        _selectClause?.Clear();
        _fromClause?.Clear();
        _joinClause?.Clear();
        _whereClause?.Clear();
        _groupByClause?.Clear();
        _orderByClause?.Clear();
        _endClause?.Clear();
        _sqlBuilderSet?.Clear();
        return this;
    }

    #endregion

    #region New(Crear un generador de SQL.)

    /// <summary>
    /// Crea una nueva instancia de un constructor de consultas SQL.
    /// </summary>
    /// <returns>
    /// Una instancia que implementa la interfaz <see cref="ISqlBuilder"/>.
    /// </returns>
    /// <remarks>
    /// Este método es abstracto y debe ser implementado por las clases derivadas.
    /// </remarks>
    public abstract ISqlBuilder New();

    #endregion

    #region GetSql(Obtener la sentencia SQL.)

    /// <summary>
    /// Obtiene la cadena SQL generada por el conjunto de constructores de SQL.
    /// </summary>
    /// <returns>
    /// Una cadena que representa la consulta SQL generada.
    /// </returns>
    public virtual string GetSql()
    {
        return SqlBuilderSet.ToResult();
    }

    #endregion

    #region AppendTo(Agregar al generador de cadenas.)

    /// <summary>
    /// Agrega las cláusulas SQL a un objeto <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">El objeto <see cref="StringBuilder"/> al que se le agregarán las cláusulas SQL.</param>
    /// <remarks>
    /// Este método verifica que el objeto <paramref name="builder"/> no sea nulo antes de proceder a agregar las cláusulas.
    /// Las cláusulas se agregan en el siguiente orden: 
    /// 1. _startClause
    /// 2. _insertClause
    /// 3. _selectClause
    /// 4. _fromClause
    /// 5. _joinClause
    /// 6. _whereClause
    /// 7. _groupByClause
    /// 8. _orderByClause
    /// 9. _endClause
    /// Al finalizar, se elimina el último carácter de nueva línea agregado al final de la cadena.
    /// </remarks>
    public void AppendTo(StringBuilder builder)
    {
        builder.CheckNull(nameof(builder));
        AppendSql(builder, _startClause);
        AppendSql(builder, _insertClause);
        AppendSql(builder, _selectClause);
        AppendSql(builder, _fromClause);
        AppendSql(builder, _joinClause);
        AppendSql(builder, _whereClause);
        AppendSql(builder, _groupByClause);
        AppendSql(builder, _orderByClause);
        AppendSql(builder, _endClause);
        builder.RemoveEnd($" {Common.Line}");
    }

    /// <summary>
    /// Agrega el contenido de una cláusula SQL al <see cref="StringBuilder"/> proporcionado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se añadirá el contenido SQL.</param>
    /// <param name="content">La cláusula SQL que se desea agregar.</param>
    /// <remarks>
    /// Este método verifica si el contenido es nulo y si es válido antes de agregarlo al <paramref name="builder"/>.
    /// Si el contenido es nulo o no es válido, no se realiza ninguna acción.
    /// </remarks>
    protected void AppendSql(StringBuilder builder, ISqlClause content)
    {
        if (content == null)
            return;
        if (content.Validate() == false)
            return;
        content.AppendTo(builder);
        builder.AppendLine(" ");
    }

    #endregion
}