using Util.Domain.Entities;

namespace Util.Domain.Compare; 

/// <summary>
/// Interfaz que define un contrato para comparar cambios en objetos de dominio.
/// </summary>
/// <typeparam name="T">El tipo de objeto de dominio que se va a comparar. Debe implementar <see cref="IDomainObject"/>.</typeparam>
public interface ICompareChange<in T> where T : IDomainObject {
    /// <summary>
    /// Obtiene los cambios entre la instancia actual y otra instancia de tipo <typeparamref name="T"/>.
    /// </summary>
    /// <param name="other">La otra instancia de tipo <typeparamref name="T"/> con la que se compararán los cambios.</param>
    /// <returns>Una colección de cambios que representan las diferencias entre las dos instancias.</returns>
    /// <typeparam name="T">El tipo de las instancias que se comparan.</typeparam>
    /// <remarks>
    /// Este método compara las propiedades de la instancia actual con las propiedades de la instancia proporcionada.
    /// Los cambios se devuelven en una colección que puede ser utilizada para aplicar las diferencias o para
    /// realizar un seguimiento de los cambios.
    /// </remarks>
    /// <seealso cref="ChangeValueCollection"/>
    ChangeValueCollection GetChanges( T other );
}