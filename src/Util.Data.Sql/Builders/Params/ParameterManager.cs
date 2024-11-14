namespace Util.Data.Sql.Builders.Params; 

/// <summary>
/// Clase que gestiona los parámetros de configuración.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IParameterManager"/> y proporciona métodos para 
/// obtener y establecer parámetros de configuración en la aplicación.
/// </remarks>
public class ParameterManager : IParameterManager {
    protected readonly IDialect Dialect;
    protected readonly IDictionary<string, SqlParam> SqlParams;
    protected readonly List<object> DynamicParams;
    protected int ParamIndex;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ParameterManager"/>.
    /// </summary>
    /// <param name="dialect">El dialecto de base de datos que se utilizará para la gestión de parámetros.</param>
    /// <remarks>
    /// Este constructor establece el dialecto proporcionado y inicializa el índice de parámetros a cero.
    /// También se inicializan las colecciones necesarias para almacenar los parámetros SQL y los parámetros dinámicos.
    /// </remarks>
    public ParameterManager( IDialect dialect ) {
        Dialect = dialect;
        ParamIndex = 0;
        SqlParams = new Dictionary<string, SqlParam>();
        DynamicParams = new List<object>();
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ParameterManager"/> 
    /// a partir de otra instancia existente.
    /// </summary>
    /// <param name="manager">La instancia de <see cref="ParameterManager"/> 
    /// que se utilizará para copiar los valores.</param>
    public ParameterManager( ParameterManager manager ) {
        Dialect = manager.Dialect;
        ParamIndex = manager.ParamIndex;
        SqlParams = new Dictionary<string, SqlParam>( manager.SqlParams );
        DynamicParams = new List<object>( manager.DynamicParams );
    }

    /// <summary>
    /// Genera un nombre basado en el prefijo del dialecto y el índice del parámetro.
    /// </summary>
    /// <returns>
    /// Un string que representa el nombre generado, que incluye el prefijo del dialecto y el índice del parámetro.
    /// </returns>
    public virtual string GenerateName() {
        var result = $"{Dialect.GetPrefix()}_p_{ParamIndex}";
        ParamIndex++;
        return result;
    }

    /// <summary>
    /// Normaliza el nombre proporcionado agregando un prefijo si no está presente.
    /// </summary>
    /// <param name="name">El nombre que se desea normalizar.</param>
    /// <returns>
    /// Devuelve el nombre normalizado con el prefijo correspondiente si no lo tenía.
    /// Si el nombre está vacío, se devuelve tal cual.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el nombre está vacío y, en caso afirmativo, lo retorna sin cambios.
    /// Si el nombre no está vacío, se le quitan los espacios en blanco al inicio y al final.
    /// Luego, se comprueba si el nombre ya comienza con el prefijo del dialecto.
    /// Si no es así, se le añade el prefijo al nombre.
    /// </remarks>
    public virtual string NormalizeName( string name ) {
        if ( name.IsEmpty() )
            return name;
        name = name.Trim();
        if ( name.StartsWith( Dialect.GetPrefix() ) )
            return name;
        return $"{Dialect.GetPrefix()}{name}";
    }

    /// <summary>
    /// Agrega parámetros dinámicos a la colección si el parámetro proporcionado no es nulo.
    /// </summary>
    /// <param name="param">El objeto que se desea agregar a la colección de parámetros dinámicos.</param>
    /// <remarks>
    /// Este método es virtual, lo que permite que las clases derivadas lo sobreescriban si es necesario.
    /// Si el parámetro es nulo, el método no realiza ninguna acción.
    /// </remarks>
    public virtual void AddDynamicParams( object param ) {
        if ( param == null )
            return;
        DynamicParams.Add( param );
    }

    /// <summary>
    /// Agrega un nuevo parámetro SQL a la colección de parámetros.
    /// </summary>
    /// <param name="name">El nombre del parámetro. No debe estar vacío.</param>
    /// <param name="value">El valor del parámetro. Por defecto es null.</param>
    /// <param name="dbType">El tipo de datos del parámetro en la base de datos. Por defecto es null.</param>
    /// <param name="direction">La dirección del parámetro (entrada, salida, etc.). Por defecto es null.</param>
    /// <param name="size">El tamaño del parámetro. Por defecto es null.</param>
    /// <param name="precision">La precisión del parámetro. Por defecto es null.</param>
    /// <param name="scale">La escala del parámetro. Por defecto es null.</param>
    /// <remarks>
    /// Si el nombre del parámetro está vacío, el método no realiza ninguna acción.
    /// Si el parámetro ya existe en la colección, se elimina antes de agregar el nuevo.
    /// El nombre del parámetro se normaliza antes de ser agregado.
    /// </remarks>
    /// <seealso cref="SqlParam"/>
    public virtual void Add( string name, object value = null, DbType? dbType = null, ParameterDirection? direction = null, int? size = null, byte? precision = null, byte? scale = null ) {
        if ( name.IsEmpty() )
            return;
        name = NormalizeName( name );
        if ( SqlParams.ContainsKey( name ) )
            SqlParams.Remove( name );
        value = ConvertValue( value );
        var param = new SqlParam( name, value, dbType, direction, size, precision, scale );
        SqlParams.Add( name, param );
    }

    /// <summary>
    /// Convierte el valor proporcionado y lo devuelve sin modificaciones.
    /// </summary>
    /// <param name="value">El valor que se desea convertir.</param>
    /// <returns>El valor original sin cambios.</returns>
    protected virtual object ConvertValue(object value) {
        return value;
    }

    /// <summary>
    /// Obtiene una lista de parámetros dinámicos.
    /// </summary>
    /// <returns>
    /// Una lista de solo lectura que contiene los parámetros dinámicos.
    /// </returns>
    public IReadOnlyList<object> GetDynamicParams() {
        return DynamicParams;
    }

    /// <summary>
    /// Obtiene una lista de parámetros SQL.
    /// </summary>
    /// <returns>
    /// Una lista de solo lectura que contiene los parámetros SQL.
    /// </returns>
    public IReadOnlyList<SqlParam> GetParams() {
        return SqlParams.Values.ToList();
    }

    /// <summary>
    /// Verifica si el nombre especificado está presente en los parámetros SQL.
    /// </summary>
    /// <param name="name">El nombre que se desea verificar en los parámetros SQL.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el nombre está presente en los parámetros SQL; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    public virtual bool Contains( string name ) {
        name = NormalizeName( name );
        return SqlParams.ContainsKey( name );
    }

    /// <summary>
    /// Obtiene un parámetro SQL por su nombre.
    /// </summary>
    /// <param name="name">El nombre del parámetro SQL que se desea obtener.</param>
    /// <returns>
    /// Un objeto <see cref="SqlParam"/> que representa el parámetro SQL si se encuentra, de lo contrario, devuelve <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método normaliza el nombre del parámetro antes de buscarlo en la colección de parámetros SQL.
    /// Si el nombre no se encuentra, se devuelve <c>null</c>.
    /// </remarks>
    public virtual SqlParam GetParam( string name ) {
        name = NormalizeName( name );
        return SqlParams.TryGetValue( name, out var param ) ? param : null;
    }

    /// <summary>
    /// Obtiene el valor de un parámetro SQL dado su nombre.
    /// </summary>
    /// <param name="name">El nombre del parámetro del cual se desea obtener el valor.</param>
    /// <returns>
    /// El valor del parámetro si existe; de lo contrario, devuelve null.
    /// </returns>
    /// <remarks>
    /// Este método normaliza el nombre del parámetro antes de intentar buscarlo en la colección de parámetros SQL.
    /// </remarks>
    public virtual object GetValue( string name ) {
        name = NormalizeName( name );
        return SqlParams.TryGetValue( name, out var param ) ? param.Value : null;
    }

    /// <summary>
    /// Limpia los parámetros SQL y restablece el índice de parámetros.
    /// </summary>
    /// <remarks>
    /// Este método establece el índice de parámetros en cero y elimina todos los parámetros
    /// de la colección SqlParams.
    /// </remarks>
    public virtual void Clear() {
        ParamIndex = 0;
        SqlParams.Clear();
    }

    /// <summary>
    /// Crea una copia del objeto actual de tipo <see cref="IParameterManager"/>.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="IParameterManager"/> que es una copia del objeto actual.
    /// </returns>
    public IParameterManager Clone() {
        return new ParameterManager( this );
    }
}