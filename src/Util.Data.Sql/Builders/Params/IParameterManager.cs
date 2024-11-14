namespace Util.Data.Sql.Builders.Params; 

/// <summary>
/// Define un contrato para la gestión de parámetros.
/// </summary>
public interface IParameterManager {
    /// <summary>
    /// Genera un nombre aleatorio.
    /// </summary>
    /// <returns>
    /// Un string que representa un nombre generado de manera aleatoria.
    /// </returns>
    /// <remarks>
    /// Este método puede ser utilizado en situaciones donde se necesite un nombre ficticio, 
    /// como en pruebas o en la creación de datos de ejemplo.
    /// </remarks>
    string GenerateName();
    /// <summary>
    /// Normaliza el nombre proporcionado, eliminando caracteres no deseados 
    /// y ajustando el formato a un estándar específico.
    /// </summary>
    /// <param name="name">El nombre que se desea normalizar.</param>
    /// <returns>El nombre normalizado.</returns>
    /// <remarks>
    /// Este método puede ser útil para asegurar que los nombres se almacenen 
    /// de manera consistente en una base de datos o se muestren de forma 
    /// uniforme en la interfaz de usuario.
    /// </remarks>
    /// <example>
    /// <code>
    /// string nombreNormalizado = NormalizeName(" Juan   Pérez ");
    /// </code>
    /// </example>
    string NormalizeName( string name );
    /// <summary>
    /// Agrega parámetros dinámicos a la colección.
    /// </summary>
    /// <param name="param">El objeto que representa los parámetros a agregar.</param>
    /// <remarks>
    /// Este método permite la adición de parámetros de forma dinámica,
    /// lo que facilita la configuración de opciones o ajustes en tiempo de ejecución.
    /// Asegúrese de que el objeto proporcionado sea del tipo esperado para evitar errores.
    /// </remarks>
    void AddDynamicParams( object param );
    /// <summary>
    /// Agrega un nuevo parámetro a la colección de parámetros.
    /// </summary>
    /// <param name="name">El nombre del parámetro.</param>
    /// <param name="value">El valor del parámetro. Si no se proporciona, se establece en <c>null</c>.</param>
    /// <param name="dbType">El tipo de datos del parámetro. Si no se proporciona, se establece en <c>null</c>.</param>
    /// <param name="direction">La dirección del parámetro, que puede ser entrada, salida o entrada/salida. Si no se proporciona, se establece en <c>null</c>.</param>
    /// <param name="size">El tamaño del parámetro, si es aplicable. Si no se proporciona, se establece en <c>null</c>.</param>
    /// <param name="precision">La precisión del parámetro, si es aplicable. Si no se proporciona, se establece en <c>null</c>.</param>
    /// <param name="scale">La escala del parámetro, si es aplicable. Si no se proporciona, se establece en <c>null</c>.</param>
    /// <remarks>
    /// Este método permite agregar parámetros de manera flexible, permitiendo especificar solo aquellos atributos que son necesarios.
    /// </remarks>
    /// <seealso cref="DbType"/>
    /// <seealso cref="ParameterDirection"/>
    void Add( string name, object value = null, DbType? dbType = null, ParameterDirection? direction = null, int? size = null, byte? precision = null, byte? scale = null );
    /// <summary>
    /// Obtiene una lista de parámetros dinámicos de solo lectura.
    /// </summary>
    /// <returns>
    /// Una lista de objetos que representan los parámetros dinámicos.
    /// </returns>
    /// <remarks>
    /// Este método es útil para obtener una colección de parámetros que pueden ser utilizados en 
    /// diferentes contextos sin permitir modificaciones a la lista original.
    /// </remarks>
    IReadOnlyList<object> GetDynamicParams();
    /// <summary>
    /// Obtiene una lista de parámetros SQL de solo lectura.
    /// </summary>
    /// <returns>
    /// Una lista de tipo <see cref="IReadOnlyList{SqlParam}"/> que contiene los parámetros SQL.
    /// </returns>
    /// <remarks>
    /// Este método es útil para acceder a los parámetros necesarios para realizar consultas SQL sin permitir modificaciones a la lista.
    /// </remarks>
    IReadOnlyList<SqlParam> GetParams();
    /// <summary>
    /// Determina si la colección contiene un elemento con el nombre especificado.
    /// </summary>
    /// <param name="name">El nombre del elemento que se va a buscar en la colección.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la colección contiene un elemento con el nombre especificado; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método realiza una búsqueda en la colección y puede ser sensible a mayúsculas y minúsculas,
    /// dependiendo de la implementación de la colección.
    /// </remarks>
    bool Contains(string name);
    /// <summary>
    /// Obtiene un parámetro SQL por su nombre.
    /// </summary>
    /// <param name="name">El nombre del parámetro SQL que se desea obtener.</param>
    /// <returns>Un objeto <see cref="SqlParam"/> que representa el parámetro SQL solicitado.</returns>
    /// <remarks>
    /// Este método busca un parámetro en una colección de parámetros SQL y devuelve el que coincide con el nombre proporcionado.
    /// Si no se encuentra un parámetro con el nombre especificado, el comportamiento puede variar según la implementación.
    /// </remarks>
    /// <seealso cref="SqlParam"/>
    SqlParam GetParam( string name );
    /// <summary>
    /// Obtiene el valor asociado a un nombre específico.
    /// </summary>
    /// <param name="name">El nombre del cual se desea obtener el valor.</param>
    /// <returns>El valor asociado al nombre proporcionado. Puede ser de cualquier tipo.</returns>
    /// <remarks>
    /// Este método busca un valor en una colección o base de datos utilizando el nombre proporcionado.
    /// Si no se encuentra un valor asociado, puede devolver null o un valor por defecto, dependiendo de la implementación.
    /// </remarks>
    /// <seealso cref="SetValue(string, object)"/>
    object GetValue( string name );
    /// <summary>
    /// Limpia el contenido actual.
    /// </summary>
    /// <remarks>
    /// Este método elimina todos los elementos o datos almacenados, dejando el estado inicial.
    /// </remarks>
    void Clear();
    /// <summary>
    /// Clona la instancia actual de <see cref="IParameterManager"/>.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="IParameterManager"/> que es una copia de la instancia actual.
    /// </returns>
    /// <remarks>
    /// Este método permite crear una copia independiente del objeto, lo que puede ser útil
    /// para mantener la integridad de los datos al modificar parámetros en diferentes contextos.
    /// </remarks>
    /// <seealso cref="IParameterManager"/>
    IParameterManager Clone();
}