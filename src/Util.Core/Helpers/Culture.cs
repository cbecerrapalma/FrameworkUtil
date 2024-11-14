namespace Util.Helpers; 

/// <summary>
/// Proporciona métodos y propiedades para trabajar con la cultura y la localización en la aplicación.
/// </summary>
public static class Culture {
    /// <summary>
    /// Obtiene la cultura actual del hilo en ejecución.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="CultureInfo"/> que representa la cultura actual.
    /// </returns>
    /// <remarks>
    /// La cultura actual se utiliza para formatear datos como fechas, números y monedas
    /// de acuerdo con las convenciones culturales del usuario.
    /// </remarks>
    public static CultureInfo GetCurrentCulture() {
        return CultureInfo.CurrentCulture;
    }

    /// <summary>
    /// Obtiene la cultura de la interfaz de usuario actual.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="CultureInfo"/> que representa la cultura de la interfaz de usuario actual.
    /// </returns>
    public static CultureInfo GetCurrentUICulture() {
        return CultureInfo.CurrentUICulture;
    }

    /// <summary>
    /// Obtiene el nombre de la cultura actual del hilo.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el nombre de la cultura actual, 
    /// en el formato "idioma-país" (por ejemplo, "es-ES" para español de España).
    /// </returns>
    public static string GetCurrentCultureName() {
        return CultureInfo.CurrentCulture.Name;
    }

    /// <summary>
    /// Obtiene el nombre de la cultura de la interfaz de usuario actual.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el nombre de la cultura de la interfaz de usuario actual.
    /// </returns>
    public static string GetCurrentUICultureName() {
        return CultureInfo.CurrentUICulture.Name;
    }

    /// <summary>
    /// Obtiene una lista de las culturas actuales basadas en la cultura del sistema.
    /// </summary>
    /// <returns>
    /// Una lista de objetos <see cref="CultureInfo"/> que representan las culturas actuales.
    /// </returns>
    /// <seealso cref="GetCultures(CultureInfo)"/>
    /// <seealso cref="GetCurrentCulture"/>
    public static List<CultureInfo> GetCurrentCultures() {
        return GetCultures( GetCurrentCulture() );
    }

    /// <summary>
    /// Obtiene una lista de las culturas de interfaz de usuario actuales.
    /// </summary>
    /// <returns>
    /// Una lista de objetos <see cref="CultureInfo"/> que representan las culturas de interfaz de usuario actuales.
    /// </returns>
    public static List<CultureInfo> GetCurrentUICultures() {
        return GetCultures( GetCurrentUICulture() );
    }

    /// <summary>
    /// Obtiene una lista de culturas a partir de una cultura dada, incluyendo la cultura padre 
    /// hasta llegar a la raíz de la jerarquía de culturas.
    /// </summary>
    /// <param name="culture">La cultura de la cual se desea obtener la jerarquía de culturas.</param>
    /// <returns>Una lista de objetos <see cref="CultureInfo"/> que representan la cultura dada y sus culturas padres.</returns>
    /// <remarks>
    /// Si el parámetro <paramref name="culture"/> es nulo, se devolverá una lista vacía.
    /// La lista resultante incluirá la cultura proporcionada y todas sus culturas padres hasta la raíz.
    /// </remarks>
    public static List<CultureInfo> GetCultures( CultureInfo culture ) {
        var result = new List<CultureInfo>();
        if ( culture == null )
            return result;
        while ( culture.Equals( culture.Parent ) == false ) {
            result.Add( culture );
            culture = culture.Parent;
        }
        return result;
    }
}