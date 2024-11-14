using Util.Data.Filters;
using Util.Dependency;
using Util.Domain;

namespace Util.Data.Stores; 

/// <summary>
/// Interfaz que define un almacén de consultas genérico para entidades.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que implementa la interfaz <see cref="IKey{Guid}"/>.</typeparam>
/// <remarks>
/// Esta interfaz hereda de <see cref="IQueryStore{TEntity, TKey}"/> utilizando <see cref="Guid"/> como tipo de clave.
/// </remarks>
public interface IQueryStore<TEntity> : IQueryStore<TEntity, Guid> where TEntity : class, IKey<Guid> {
}

/// <summary>
/// Define una interfaz para un almacén de consultas que permite realizar operaciones de filtrado
/// y gestión del alcance de dependencia para entidades específicas.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que representa el almacén de consultas.</typeparam>
/// <typeparam name="TKey">El tipo de la clave que identifica de manera única a la entidad.</typeparam>
/// <remarks>
/// Esta interfaz hereda de <see cref="IFilterOperation"/> para permitir operaciones de filtrado,
/// de <see cref="IScopeDependency"/> para gestionar dependencias de alcance, y de <see cref="IDisposable"/>
/// para liberar recursos no administrados.
/// </remarks>
public interface IQueryStore<TEntity, in TKey> : IFilterOperation,IScopeDependency,IDisposable where TEntity : class, IKey<TKey> {
    /// <summary>
    /// Obtiene una colección de entidades de tipo <typeparamref name="TEntity"/>.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IQueryable{T}"/> que representa la colección de entidades.
    /// </returns>
    /// <typeparam name="TEntity">
    /// El tipo de entidad que se va a recuperar.
    /// </typeparam>
    /// <remarks>
    /// Este método permite realizar consultas sobre la colección de entidades de manera diferida,
    /// lo que significa que la consulta no se ejecuta hasta que se itera sobre el resultado.
    /// </remarks>
    /// <seealso cref="IQueryable{T}"/>
    IQueryable<TEntity> Find();
    /// <summary>
    /// Busca entidades que cumplen con una condición específica.
    /// </summary>
    /// <param name="condition">La condición que deben cumplir las entidades a buscar.</param>
    /// <returns>Una colección de entidades que cumplen con la condición especificada.</returns>
    /// <remarks>
    /// Este método permite realizar consultas sobre un conjunto de entidades utilizando una condición definida por el usuario.
    /// Asegúrese de que la implementación de <see cref="ICondition{TEntity}"/> esté correctamente definida para obtener resultados precisos.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se está buscando.</typeparam>
    /// <seealso cref="ICondition{TEntity}"/>
    IQueryable<TEntity> Find( ICondition<TEntity> condition );
    /// <summary>
    /// Busca entidades que cumplen con la condición especificada.
    /// </summary>
    /// <param name="condition">Una expresión que representa la condición que deben cumplir las entidades.</param>
    /// <returns>Una colección de entidades que cumplen con la condición.</returns>
    /// <remarks>
    /// Este método permite realizar consultas sobre un conjunto de entidades utilizando una expresión lambda.
    /// La expresión debe ser una función que toma un objeto de tipo <typeparamref name="TEntity"/> y devuelve un valor booleano.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se está buscando.</typeparam>
    /// <seealso cref="IQueryable{TEntity}"/>
    IQueryable<TEntity> Find( Expression<Func<TEntity, bool>> condition );
    /// <summary>
    /// Busca una entidad por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único de la entidad que se desea buscar.</param>
    /// <returns>La entidad correspondiente al identificador proporcionado, o null si no se encuentra.</returns>
    /// <remarks>
    /// Este método es útil para recuperar una entidad específica de la base de datos
    /// utilizando su identificador. Asegúrese de que el identificador proporcionado
    /// sea del tipo correcto y que exista en la base de datos.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de la entidad que se está buscando.</typeparam>
    /// <seealso cref="FindAll"/>
    TEntity FindById( object id );
    /// <summary>
    /// Busca una entidad por su identificador de manera asíncrona.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de la entidad que se busca.</typeparam>
    /// <param name="id">El identificador de la entidad que se desea encontrar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que contiene la entidad encontrada, o null si no se encuentra ninguna entidad.</returns>
    /// <remarks>
    /// Este método permite realizar una búsqueda de una entidad en la base de datos de manera eficiente y no bloqueante. 
    /// Es recomendable utilizar el token de cancelación para manejar situaciones donde la operación pueda tardar demasiado.
    /// </remarks>
    /// <seealso cref="FindAllAsync"/>
    Task<TEntity> FindByIdAsync( object id, CancellationToken cancellationToken = default );
    /// <summary>
    /// Busca entidades en la base de datos utilizando un array de identificadores.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se está buscando.</typeparam>
    /// <typeparam name="TKey">El tipo del identificador de la entidad.</typeparam>
    /// <param name="ids">Un array de identificadores de las entidades a buscar.</param>
    /// <returns>Una lista de entidades que coinciden con los identificadores proporcionados.</returns>
    /// <remarks>
    /// Este método permite recuperar múltiples entidades a la vez, facilitando la 
    /// obtención de datos basados en sus identificadores únicos. Si un identificador 
    /// no corresponde a ninguna entidad, esa entidad no será incluida en la lista de resultados.
    /// </remarks>
    /// <seealso cref="FindById(TKey id)"/>
    List<TEntity> FindByIds( params TKey[] ids );
    /// <summary>
    /// Busca entidades en la base de datos utilizando una colección de identificadores.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se está buscando.</typeparam>
    /// <typeparam name="TKey">El tipo de la clave que se utiliza para identificar las entidades.</typeparam>
    /// <param name="ids">Una colección de identificadores de las entidades que se desean encontrar.</param>
    /// <returns>
    /// Una lista de entidades que coinciden con los identificadores proporcionados.
    /// Si no se encuentran entidades, se devuelve una lista vacía.
    /// </returns>
    /// <remarks>
    /// Este método es útil para recuperar múltiples entidades a la vez, 
    /// lo que puede mejorar el rendimiento en comparación con la búsqueda 
    /// de entidades individuales.
    /// </remarks>
    /// <seealso cref="FindById(TKey)"/>
    List<TEntity> FindByIds( IEnumerable<TKey> ids );
    /// <summary>
    /// Busca una lista de entidades del tipo <typeparamref name="TEntity"/> utilizando una cadena de identificadores.
    /// </summary>
    /// <param name="ids">Una cadena que contiene los identificadores separados por comas.</param>
    /// <returns>Una lista de entidades que coinciden con los identificadores proporcionados.</returns>
    /// <remarks>
    /// Este método es útil para recuperar múltiples entidades de la base de datos en una sola operación.
    /// Asegúrese de que la cadena de identificadores esté correctamente formateada antes de llamar a este método.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se busca.</typeparam>
    /// <seealso cref="List{TEntity}"/>
    List<TEntity> FindByIds( string ids );
    /// <summary>
    /// Busca entidades en la base de datos utilizando un conjunto de identificadores.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se está buscando.</typeparam>
    /// <typeparam name="TKey">El tipo de la clave que identifica la entidad.</typeparam>
    /// <param name="ids">Una lista de identificadores de las entidades a buscar.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El resultado contiene una lista de entidades encontradas.</returns>
    /// <remarks>
    /// Este método permite recuperar múltiples entidades de forma asíncrona.
    /// Si no se encuentran entidades con los identificadores proporcionados, se devolverá una lista vacía.
    /// </remarks>
    /// <seealso cref="FindByIdAsync(TKey id)"/>
    Task<List<TEntity>> FindByIdsAsync( params TKey[] ids );
    /// <summary>
    /// Busca entidades por sus identificadores.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de la entidad que se está buscando.</typeparam>
    /// <typeparam name="TKey">El tipo de la clave que identifica a la entidad.</typeparam>
    /// <param name="ids">Una colección de identificadores de las entidades a buscar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de entidades encontradas.</returns>
    /// <remarks>
    /// Este método permite buscar múltiples entidades en una sola operación.
    /// Si alguno de los identificadores no corresponde a una entidad existente, 
    /// esa entidad no será incluida en el resultado.
    /// </remarks>
    /// <seealso cref="FindByIdAsync"/>
    Task<List<TEntity>> FindByIdsAsync( IEnumerable<TKey> ids, CancellationToken cancellationToken = default );
    /// <summary>
    /// Busca entidades por una lista de identificadores.
    /// </summary>
    /// <param name="ids">Una cadena que contiene los identificadores de las entidades a buscar, separados por comas.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de entidades del tipo <typeparamref name="TEntity"/> como resultado.</returns>
    /// <remarks>
    /// Este método permite realizar búsquedas de múltiples entidades en una sola llamada, mejorando la eficiencia 
    /// al evitar múltiples consultas a la base de datos. Asegúrese de que los identificadores proporcionados sean válidos.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se está buscando.</typeparam>
    /// <seealso cref="CancellationToken"/>
    Task<List<TEntity>> FindByIdsAsync( string ids, CancellationToken cancellationToken = default );
    /// <summary>
    /// Obtiene una entidad que cumple con la condición especificada.
    /// </summary>
    /// <param name="condition">Una expresión que define la condición que debe cumplir la entidad.</param>
    /// <returns>La entidad que cumple con la condición especificada.</returns>
    /// <exception cref="InvalidOperationException">Se lanza si no se encuentra ninguna entidad que cumpla con la condición.</exception>
    /// <remarks>
    /// Este método busca en la colección de entidades y devuelve la primera entidad que coincide con la condición dada.
    /// Si se encuentran múltiples entidades que cumplen la condición, se devolverá solo la primera.
    /// </remarks>
    TEntity Single( Expression<Func<TEntity, bool>> condition );
    /// <summary>
    /// Obtiene un único elemento de una colección que cumple con una condición especificada.
    /// </summary>
    /// <param name="condition">Una expresión que define la condición que debe cumplir el elemento.</param>
    /// <param name="action">Una función que permite aplicar acciones adicionales sobre la consulta antes de obtener el resultado.</param>
    /// <returns>El elemento que cumple con la condición especificada.</returns>
    /// <remarks>
    /// Si se encuentra más de un elemento que cumple con la condición, se lanzará una excepción.
    /// Si no se encuentra ningún elemento, se devolverá el valor predeterminado de TEntity.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se está consultando.</typeparam>
    /// <seealso cref="IQueryable{T}"/>
    TEntity Single( Expression<Func<TEntity, bool>> condition, Func<IQueryable<TEntity>, IQueryable<TEntity>> action );
    /// <summary>
    /// Obtiene un único elemento que cumple con la condición especificada de manera asíncrona.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se está consultando.</typeparam>
    /// <param name="condition">Una expresión que define la condición que debe cumplir el elemento.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, que contiene el elemento que cumple con la condición.</returns>
    /// <remarks>
    /// Este método lanzará una excepción si se encuentra más de un elemento que cumple con la condición.
    /// Si no se encuentra ningún elemento, se devolverá null.
    /// </remarks>
    /// <seealso cref="SingleOrDefaultAsync"/>
    Task<TEntity> SingleAsync( Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default );
    /// <summary>
    /// Obtiene un único elemento de tipo <typeparamref name="TEntity"/> que cumple con la condición especificada de manera asíncrona.
    /// </summary>
    /// <param name="condition">Una expresión que representa la condición que debe cumplir el elemento.</param>
    /// <param name="action">Una función que permite aplicar transformaciones adicionales a la consulta antes de obtener el resultado.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, que contiene el elemento encontrado de tipo <typeparamref name="TEntity"/>.</returns>
    /// <remarks>
    /// Este método es útil para obtener un único elemento de una colección que cumple con una condición específica,
    /// permitiendo además aplicar transformaciones a la consulta mediante el parámetro <paramref name="action"/>.
    /// Si no se encuentra ningún elemento que cumpla con la condición, se lanzará una excepción.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se está consultando.</typeparam>
    /// <seealso cref="IQueryable{T}"/>
    Task<TEntity> SingleAsync( Expression<Func<TEntity, bool>> condition, Func<IQueryable<TEntity>, IQueryable<TEntity>> action, CancellationToken cancellationToken = default );
    /// <summary>
    /// Busca y devuelve una lista de entidades que cumplen con la condición especificada.
    /// </summary>
    /// <param name="condition">Una expresión que define la condición que deben cumplir las entidades. Si es <c>null</c>, se devolverán todas las entidades.</param>
    /// <returns>Una lista de entidades que cumplen con la condición especificada.</returns>
    /// <remarks>
    /// Este método permite filtrar las entidades de tipo <typeparamref name="TEntity"/> según una condición dada.
    /// Si no se proporciona ninguna condición, se retornarán todas las entidades disponibles.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se está buscando.</typeparam>
    List<TEntity> FindAll( Expression<Func<TEntity, bool>> condition = null );
    /// <summary>
    /// Busca todos los elementos de tipo <typeparamref name="TEntity"/> que cumplen con la condición especificada.
    /// </summary>
    /// <param name="condition">Una expresión que define la condición que deben cumplir los elementos. Si es <c>null</c>, se devolverán todos los elementos.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, que contiene una lista de elementos que cumplen con la condición.</returns>
    /// <remarks>
    /// Este método es útil para realizar consultas asíncronas sobre una colección de elementos.
    /// Asegúrese de manejar adecuadamente el <paramref name="cancellationToken"/> para evitar bloqueos en la aplicación.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se está buscando.</typeparam>
    /// <seealso cref="FindAsync"/>
    Task<List<TEntity>> FindAllAsync( Expression<Func<TEntity, bool>> condition = null, CancellationToken cancellationToken = default );
    /// <summary>
    /// Verifica si existen elementos en la colección con las claves especificadas.
    /// </summary>
    /// <param name="ids">Un arreglo de claves que se utilizarán para verificar la existencia de los elementos.</param>
    /// <returns>Devuelve <c>true</c> si al menos uno de los elementos con las claves especificadas existe; de lo contrario, devuelve <c>false</c>.</returns>
    /// <remarks>
    /// Este método permite comprobar la existencia de múltiples elementos a la vez, facilitando la validación de claves en la colección.
    /// </remarks>
    /// <typeparam name="TKey">El tipo de las claves que se utilizan para identificar los elementos en la colección.</typeparam>
    /// <seealso cref="Exists(TKey)"/>
    bool Exists( params TKey[] ids );
    /// <summary>
    /// Verifica si existe al menos un elemento en la colección que cumple con la condición especificada.
    /// </summary>
    /// <param name="condition">Una expresión que representa la condición que deben cumplir los elementos.</param>
    /// <returns>Devuelve <c>true</c> si existe al menos un elemento que cumple con la condición; de lo contrario, <c>false</c>.</returns>
    /// <remarks>
    /// Este método es útil para determinar la existencia de elementos sin necesidad de recuperar todos los datos.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se está evaluando.</typeparam>
    /// <seealso cref="Any(Expression{Func{TEntity, bool}})"/>
    bool Exists( Expression<Func<TEntity, bool>> condition );
    /// <summary>
    /// Verifica si existen elementos en la base de datos con las claves especificadas.
    /// </summary>
    /// <param name="ids">Un arreglo de claves que se utilizarán para verificar la existencia de los elementos.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El valor devuelto es verdadero si al menos uno de los elementos con las claves especificadas existe; de lo contrario, falso.</returns>
    /// <remarks>
    /// Este método permite realizar una verificación eficiente de la existencia de múltiples elementos a la vez,
    /// lo que puede ser útil para evitar operaciones innecesarias en la base de datos.
    /// </remarks>
    /// <seealso cref="ExistsAsync(TKey)"/>
    Task<bool> ExistsAsync( params TKey[] ids );
    /// <summary>
    /// Verifica si existe al menos un elemento que cumpla con la condición especificada.
    /// </summary>
    /// <param name="condition">Una expresión que define la condición que deben cumplir los elementos.</param>
    /// <param name="cancellationToken">Token para cancelar la operación, si es necesario.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El valor de la tarea es <c>true</c> si existe al menos un elemento que cumple con la condición; de lo contrario, <c>false</c>.</returns>
    /// <remarks>
    /// Este método es útil para determinar la existencia de elementos en una colección sin necesidad de cargar todos los elementos en memoria.
    /// </remarks>
    /// <seealso cref="ExistsAsync(Expression{Func{TEntity, bool}}, CancellationToken)"/>
    Task<bool> ExistsAsync( Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default );
    /// <summary>
    /// Cuenta el número de entidades que cumplen con una condición especificada.
    /// </summary>
    /// <param name="condition">Una expresión que define la condición que deben cumplir las entidades. Si es <c>null</c>, se contarán todas las entidades.</param>
    /// <returns>El número de entidades que cumplen con la condición especificada.</returns>
    /// <remarks>
    /// Este método es útil para obtener el total de registros en una base de datos que satisfacen una determinada condición.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se está contando.</typeparam>
    int Count( Expression<Func<TEntity, bool>> condition = null );
    /// <summary>
    /// Cuenta el número de entidades que cumplen con la condición especificada.
    /// </summary>
    /// <param name="condition">Una expresión que representa la condición que deben cumplir las entidades. Si es <c>null</c>, se contarán todas las entidades.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un valor entero que indica el número de entidades que cumplen con la condición.</returns>
    /// <remarks>
    /// Este método permite realizar un conteo de entidades de manera eficiente y asíncrona, 
    /// lo que es útil en escenarios donde se requiere un rendimiento óptimo al trabajar con bases de datos.
    /// </remarks>
    /// <seealso cref="Task{TResult}"/>
    Task<int> CountAsync( Expression<Func<TEntity, bool>> condition = null, CancellationToken cancellationToken = default );
}