namespace Util.Data.Sql.Builders.Conditions; 

/// <summary>
/// Representa una condición SQL que verifica la no existencia de un conjunto de resultados.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ISqlCondition"/> y se utiliza para construir
/// condiciones SQL que se basan en la ausencia de registros en una consulta.
/// </remarks>
public class NotExistsCondition : ISqlCondition {
    private readonly ISqlBuilder _sqlBuilder;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="NotExistsCondition"/>.
    /// </summary>
    /// <param name="builder">El constructor SQL que se utilizará para crear la consulta.</param>
    public NotExistsCondition( ISqlBuilder builder ) {
        _sqlBuilder = builder;
    }

    /// <summary>
    /// Agrega una cláusula "Not Exists" a un objeto <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le añadirá la cláusula.</param>
    /// <remarks>
    /// Este método verifica si el objeto <c>_sqlBuilder</c> es nulo antes de intentar agregar la cláusula.
    /// Si <c>_sqlBuilder</c> no es nulo, se llama a su método <c>AppendTo</c> para construir la parte interna de la cláusula.
    /// </remarks>
    public void AppendTo( StringBuilder builder ) {
        if ( _sqlBuilder == null )
            return;
        builder.Append( "Not Exists (" );
        _sqlBuilder.AppendTo( builder );
        builder.Append( ")" );
    }
}