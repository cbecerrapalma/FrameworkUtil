namespace Util.Data.Sql.Builders.Sets; 

/// <summary>
/// Representa un elemento de configuración para una cláusula SET en una consulta SQL.
/// </summary>
public class SqlBuilderSetItem {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqlBuilderSetItem"/>.
    /// </summary>
    /// <param name="operator">El operador SQL que se utilizará en la construcción de la consulta.</param>
    /// <param name="builder">Una instancia de <see cref="ISqlBuilder"/> que se utilizará para construir la consulta SQL.</param>
    public SqlBuilderSetItem( string @operator, ISqlBuilder builder ) {
        Operator = @operator;
        Builder = builder;
    }

    /// <summary>
    /// Representa el operador asociado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad solo tiene un getter, lo que significa que su valor es de solo lectura.
    /// </remarks>
    /// <value>
    /// Un string que contiene el operador.
    /// </value>
    public string Operator { get; }

    /// <summary>
    /// Obtiene una instancia de un generador de consultas SQL.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso al objeto que implementa la interfaz <see cref="ISqlBuilder"/>.
    /// Se utiliza para construir consultas SQL de manera programática.
    /// </remarks>
    /// <value>
    /// Una instancia de <see cref="ISqlBuilder"/> que permite la construcción de consultas SQL.
    /// </value>
    public ISqlBuilder Builder { get; }

    /// <summary>
    /// Crea una copia del objeto actual <see cref="SqlBuilderSetItem"/>.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="SqlBuilderSetItem"/> que es una copia del objeto actual.
    /// </returns>
    public SqlBuilderSetItem Clone() {
        return new SqlBuilderSetItem( Operator, Builder.Clone() );
    }
}