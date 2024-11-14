namespace Util.Data.EntityFrameworkCore;

/// <summary>
/// Representa las opciones de configuración para el uso de Entity Framework Core con Oracle.
/// </summary>
public class OracleEntityFrameworkCoreOptions {
    /// <summary>
    /// Obtiene o establece un valor que indica si el GUID se debe convertir a una cadena.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para determinar el formato de salida del GUID.
    /// Si se establece en verdadero, el GUID se convertirá a una representación de cadena.
    /// De lo contrario, se mantendrá en su formato original.
    /// </remarks>
    /// <value>
    /// <c>true</c> si el GUID se convierte a cadena; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsGuidToString { get; set; }
}