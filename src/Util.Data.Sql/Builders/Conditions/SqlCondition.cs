namespace Util.Data.Sql.Builders.Conditions; 

/// <summary>
/// Representa una condición SQL que puede ser utilizada en consultas.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ISqlCondition"/> y proporciona
/// métodos y propiedades para construir condiciones SQL de manera programática.
/// </remarks>
public class SqlCondition : ISqlCondition {
    private readonly string _condition;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqlCondition"/> con la condición especificada.
    /// </summary>
    /// <param name="condition">La condición SQL que se asignará a la instancia.</param>
    public SqlCondition( string condition ) {
        _condition = condition;
    }

    /// <summary>
    /// Agrega la condición al final del <see cref="StringBuilder"/> proporcionado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le añadirá la condición.</param>
    public void AppendTo( StringBuilder builder ) {
        builder.Append( _condition );
    }
}