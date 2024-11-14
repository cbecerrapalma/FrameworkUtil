using Util.Data.Queries;
using Util.Data.Stores;
using Util.Domain;

namespace Util.Data; 

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="Store"/>.
/// </summary>
public static class StoreExtensions {
    /// <summary>
    /// Establece el contexto de consulta como "sin seguimiento" para la fuente de datos especificada.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se está consultando.</typeparam>
    /// <typeparam name="TKey">El tipo de clave que identifica de manera única a la entidad.</typeparam>
    /// <param name="source">La fuente de datos que implementa <see cref="IQueryStore{TEntity, TKey}"/>.</param>
    /// <returns>La fuente de datos original con el contexto de consulta establecido como "sin seguimiento".</returns>
    /// <remarks>
    /// Este método es útil cuando se desea realizar consultas sin que el contexto de datos realice un seguimiento de los cambios en las entidades.
    /// Esto puede mejorar el rendimiento en escenarios donde no se necesita modificar las entidades recuperadas.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <seealso cref="IQueryStore{TEntity, TKey}"/>
    /// <seealso cref="ITrack.NoTracking"/>
    public static IQueryStore<TEntity, TKey> NoTracking<TEntity, TKey>( this IQueryStore<TEntity, TKey> source ) where TEntity : class, IKey<TKey> {
        source.CheckNull( nameof( source ) );
        if( source is ITrack store )
            store.NoTracking();
        return source;
    }
}