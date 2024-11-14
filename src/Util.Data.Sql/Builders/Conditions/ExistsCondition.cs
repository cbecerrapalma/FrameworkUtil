namespace Util.Data.Sql.Builders.Conditions; 

/// <summary>
/// Representa una condición SQL que verifica la existencia de registros en una consulta.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ISqlCondition"/> y se utiliza para construir
/// condiciones SQL que utilizan la cláusula EXISTS.
/// </remarks>
public class ExistsCondition : ISqlCondition {
    private readonly ISqlBuilder _sqlBuilder;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ExistsCondition"/>.
    /// </summary>
    /// <param name="builder">El objeto <see cref="ISqlBuilder"/> que se utilizará para construir la consulta SQL.</param>
    public ExistsCondition( ISqlBuilder builder ) {
        _sqlBuilder = builder;
    }

    /// <summary>
    /// Agrega una cláusula "Exists" a un objeto StringBuilder.
    /// </summary>
    /// <param name="builder">El StringBuilder al que se le añadirá la cláusula.</param>
    /// <remarks>
    /// Este método verifica si el objeto _sqlBuilder no es nulo antes de proceder a 
    /// agregar la cláusula "Exists" y el contenido del _sqlBuilder al StringBuilder proporcionado.
    /// Si _sqlBuilder es nulo, el método no realiza ninguna acción.
    /// </remarks>
    public void AppendTo( StringBuilder builder ) {
        if ( _sqlBuilder == null )
            return;
        builder.Append( "Exists (" );
        _sqlBuilder.AppendTo( builder );
        builder.Append( ")" );
    }
}