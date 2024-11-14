using Util.Helpers;

namespace Util.Data.EntityFrameworkCore.ValueConverters; 

/// <summary>
/// Convierte valores de tipo <see cref="DateTime?"/> a <see cref="DateTime?"/> y viceversa.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="ValueConverter{TSource, TTarget}"/> y proporciona
/// funcionalidad para la conversión de valores de fecha y hora que pueden ser nulos.
/// </remarks>
/// <typeparam name="TSource">El tipo de origen, que en este caso es <see cref="DateTime?"/>.</typeparam>
/// <typeparam name="TTarget">El tipo de destino, que también es <see cref="DateTime?"/>.</typeparam>
public class DateTimeValueConverter : ValueConverter<DateTime?, DateTime?> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DateTimeValueConverter"/>.
    /// </summary>
    /// <remarks>
    /// Esta clase se utiliza para convertir valores de fecha y hora, normalizando la fecha y convirtiéndola a la hora local.
    /// </remarks>
    /// <param name="date">La fecha que se va a normalizar y convertir.</param>
    /// <returns>Un objeto <see cref="DateTime"/> que representa la fecha normalizada y convertida a la hora local.</returns>
    public DateTimeValueConverter()
        : base( date => Normalize( date ), date => ToLocalTime( date ) ) {
    }

    /// <summary>
    /// Normaliza una fecha si tiene un valor.
    /// </summary>
    /// <param name="date">La fecha a normalizar, que puede ser nula.</param>
    /// <returns>
    /// Una fecha normalizada si <paramref name="date"/> tiene un valor; de lo contrario, devuelve null.
    /// </returns>
    public static DateTime? Normalize( DateTime? date ) {
        return date.HasValue ? Time.Normalize( date.Value ) : null;
    }

    /// <summary>
    /// Convierte un objeto <see cref="DateTime"/> nullable de UTC a hora local.
    /// </summary>
    /// <param name="date">Un objeto <see cref="DateTime?"/> que representa la fecha y hora en UTC.</param>
    /// <returns>
    /// Un objeto <see cref="DateTime?"/> que representa la fecha y hora en hora local si <paramref name="date"/> tiene un valor; de lo contrario, <c>null</c>.</returns>
    /// <remarks>
    /// Este método es útil para convertir fechas y horas almacenadas en formato UTC a la zona horaria local del sistema.
    /// </remarks>
    public static DateTime? ToLocalTime( DateTime? date ) {
        return date.HasValue ? Time.UtcToLocalTime( date.Value ) : null;
    }
}