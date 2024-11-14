namespace Util.Data.Sql.Builders.Conditions; 

/// <summary>
/// Representa una condición SQL que verifica si un valor es nulo.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ISqlCondition"/> y se utiliza para construir
/// condiciones SQL que evalúan si un campo específico tiene un valor nulo.
/// </remarks>
public class NullCondition : ISqlCondition {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="NullCondition"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor es privado y se utiliza para evitar la creación de instancias de la clase <see cref="NullCondition"/>.
    /// </remarks>
    private NullCondition() {
    }

    public static readonly NullCondition Instance = new NullCondition();

    /// <summary>
    /// Agrega contenido a un objeto <see cref="StringBuilder"/> existente.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le añadirá contenido.</param>
    /// <remarks>
    /// Este método permite modificar el contenido del <paramref name="builder"/> 
    /// al agregarle nuevos datos. Asegúrese de que el objeto <paramref name="builder"/> 
    /// no sea nulo antes de llamar a este método.
    /// </remarks>
    public void AppendTo( StringBuilder builder ) {
    }
}