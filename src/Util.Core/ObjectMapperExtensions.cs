using Util.ObjectMapping;

namespace Util; 

/// <summary>
/// Proporciona métodos de extensión para facilitar la conversión de objetos entre diferentes tipos.
/// </summary>
public static class ObjectMapperExtensions {
    private static IObjectMapper _mapper;

    /// <summary>
    /// Establece el mapeador de objetos utilizado por la aplicación.
    /// </summary>
    /// <param name="mapper">El mapeador de objetos que se va a establecer. No puede ser nulo.</param>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="mapper"/> es nulo.</exception>
    public static void SetMapper( IObjectMapper mapper ) {
        _mapper = mapper ?? throw new ArgumentNullException( nameof( mapper ) );
    }

    /// <summary>
    /// Mapea un objeto de origen a un objeto de destino del tipo especificado.
    /// </summary>
    /// <typeparam name="TDestination">El tipo del objeto de destino al que se va a mapear.</typeparam>
    /// <param name="source">El objeto de origen que se va a mapear.</param>
    /// <returns>Un objeto del tipo <typeparamref name="TDestination"/> que representa el resultado del mapeo.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si el mapeador (_mapper) es nulo.</exception>
    /// <remarks>
    /// Este método utiliza un mapeador configurado para realizar la conversión de tipos.
    /// Asegúrese de que el mapeador esté inicializado antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="AutoMapper.Mapper"/>
    public static TDestination MapTo<TDestination>( this object source ) {
        if ( _mapper == null )
            throw new ArgumentNullException( nameof(_mapper) );
        return _mapper.Map<object, TDestination>( source );
    }
        
    /// <summary>
    /// Mapea un objeto de tipo <typeparamref name="TSource"/> a un objeto de tipo <typeparamref name="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">El tipo del objeto fuente que se va a mapear.</typeparam>
    /// <typeparam name="TDestination">El tipo del objeto de destino al que se va a mapear.</typeparam>
    /// <param name="source">El objeto fuente que se va a mapear.</param>
    /// <param name="destination">El objeto de destino que recibirá los valores mapeados.</param>
    /// <returns>
    /// Un objeto de tipo <typeparamref name="TDestination"/> que contiene los valores mapeados desde el objeto fuente.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si el mapeador (_mapper) es nulo.</exception>
    /// <remarks>
    /// Este método utiliza un mapeador previamente configurado para realizar la operación de mapeo.
    /// Asegúrese de que el mapeador esté inicializado antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="AutoMapper.IMapper"/>
    public static TDestination MapTo<TSource, TDestination>( this TSource source, TDestination destination ) {
        if( _mapper == null )
            throw new ArgumentNullException( nameof( _mapper ) );
        return _mapper.Map( source, destination );
    }

    /// <summary>
    /// Mapea una colección enumerable a una lista de un tipo específico.
    /// </summary>
    /// <typeparam name="TDestination">El tipo de los elementos en la lista de destino.</typeparam>
    /// <param name="source">La colección enumerable que se va a mapear.</param>
    /// <returns>Una lista del tipo especificado que contiene los elementos mapeados de la colección fuente.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de las colecciones enumerable, permitiendo convertirlas fácilmente en listas.
    /// Asegúrese de que los elementos de la colección fuente sean del tipo adecuado o que sean convertibles al tipo de destino.
    /// </remarks>
    /// <seealso cref="MapTo{TDestination}(System.Collections.IEnumerable)"/>
    public static List<TDestination> MapToList<TDestination>( this System.Collections.IEnumerable source ) {
        return MapTo<List<TDestination>>( source );
    }
}