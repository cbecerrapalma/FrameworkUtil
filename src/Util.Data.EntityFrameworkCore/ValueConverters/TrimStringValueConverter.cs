namespace Util.Data.EntityFrameworkCore.ValueConverters; 

/// <summary>
/// Convertidor de valores que se encarga de recortar espacios en blanco al inicio y al final de una cadena.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="ValueConverter{TSource, TDestination}"/> y se utiliza para transformar cadenas
/// de texto eliminando los espacios en blanco no deseados.
/// </remarks>
/// <typeparam name="TSource">Tipo de la fuente, en este caso <see cref="string"/>.</typeparam>
/// <typeparam name="TDestination">Tipo de destino, en este caso <see cref="string"/>.</typeparam>
public class TrimStringValueConverter : ValueConverter<string, string> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TrimStringValueConverter"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor utiliza una función de transformación que asegura que el valor de entrada se convierta a una cadena segura
    /// y devuelve el mismo valor sin cambios.
    /// </remarks>
    /// <param name="value">El valor que se va a convertir.</param>
    /// <returns>El valor convertido a una cadena segura.</returns>
    public TrimStringValueConverter()
        : base( value => value.SafeString(), value => value ) {
    }
}