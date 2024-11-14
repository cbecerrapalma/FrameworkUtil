namespace Util.ObjectMapping; 

/// <summary>
/// Define un contrato para la conversión entre diferentes tipos de objetos.
/// </summary>
public interface IObjectMapper {
    /// <summary>
    /// Mapea un objeto de tipo <typeparamref name="TSource"/> a un objeto de tipo <typeparamref name="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">El tipo del objeto fuente que se va a mapear.</typeparam>
    /// <typeparam name="TDestination">El tipo del objeto de destino al que se va a mapear.</typeparam>
    /// <param name="source">El objeto fuente que se desea mapear.</param>
    /// <returns>Un objeto de tipo <typeparamref name="TDestination"/> que representa el resultado del mapeo.</returns>
    /// <remarks>
    /// Este método realiza la conversión de propiedades del objeto fuente a las propiedades del objeto de destino,
    /// utilizando un mapeo automático basado en convenciones. Asegúrese de que las propiedades coincidan
    /// en nombre y tipo para que el mapeo se realice correctamente.
    /// </remarks>
    /// <seealso cref="Map{TSource, TDestination}"/>
    TDestination Map<TSource, TDestination>( TSource source );
    /// <summary>
    /// Mapea un objeto de tipo <typeparamref name="TSource"/> a un objeto de tipo <typeparamref name="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">El tipo del objeto fuente que se va a mapear.</typeparam>
    /// <typeparam name="TDestination">El tipo del objeto de destino al que se va a mapear.</typeparam>
    /// <param name="source">El objeto fuente que se va a mapear.</param>
    /// <param name="destination">El objeto de destino que recibirá los valores mapeados.</param>
    /// <returns>El objeto de destino con los valores mapeados desde el objeto fuente.</returns>
    /// <remarks>
    /// Este método es útil para transformar datos entre diferentes tipos de objetos, 
    /// facilitando la conversión de propiedades de un objeto a otro.
    /// </remarks>
    /// <seealso cref="Map{TSource, TDestination}(TSource, TDestination)"/>
    TDestination Map<TSource, TDestination>( TSource source, TDestination destination );
}