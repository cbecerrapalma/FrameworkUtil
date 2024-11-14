using Util.Data.Sql.Builders.Params;

namespace Util.Data.Sql.Builders.Conditions; 

/// <summary>
/// Representa una condición SQL que verifica si un valor está dentro de un conjunto de valores especificados.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ISqlCondition"/> y se utiliza para construir condiciones SQL
/// que utilizan la cláusula IN.
/// </remarks>
public class InCondition : ISqlCondition {
    protected readonly IParameterManager ParameterManager;
    protected readonly string Column;
    protected readonly object Value;
    protected readonly bool IsParameterization;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="InCondition"/>.
    /// </summary>
    /// <param name="parameterManager">El administrador de parámetros que se utilizará para gestionar los parámetros.</param>
    /// <param name="column">El nombre de la columna que se utilizará en la condición.</param>
    /// <param name="value">El valor que se comparará en la condición.</param>
    /// <param name="isParameterization">Indica si la condición debe ser parametrizada.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="parameterManager"/> o <paramref name="column"/> son nulos o están vacíos.</exception>
    /// <remarks>
    /// Esta clase se utiliza para representar una condición de tipo IN en una consulta, permitiendo especificar una columna y un valor, así como si se debe utilizar la parametrización.
    /// </remarks>
    public InCondition( IParameterManager parameterManager, string column, object value, bool isParameterization ) {
        ParameterManager = parameterManager ?? throw new ArgumentNullException( nameof( parameterManager ) );
        if( string.IsNullOrWhiteSpace( column ) )
            throw new ArgumentNullException( nameof( column ) );
        Column = column;
        Value = value;
        IsParameterization = isParameterization;
    }

    /// <summary>
    /// Agrega una representación de la condición actual al objeto <see cref="StringBuilder"/> proporcionado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se añadirá la condición.</param>
    /// <remarks>
    /// Este método verifica si el valor actual es un <see cref="ISqlBuilder"/> y, si es así, 
    /// llama a <see cref="AppendSqlBuilder"/> para agregar la representación SQL. 
    /// Si no hay valores disponibles, el método no realiza ninguna acción. 
    /// Dependiendo de la propiedad <see cref="IsParameterization"/>, 
    /// se utilizará un método diferente para agregar la condición: 
    /// <see cref="AppendParameterizedCondition"/> o <see cref="AppendNonParameterizedCondition"/>.
    /// </remarks>
    public void AppendTo( StringBuilder builder ) {
        if( Value is ISqlBuilder sqlBuilder ) {
            AppendSqlBuilder( builder, Column, sqlBuilder );
            return;
        }
        var values = GetValues();
        if( values.Count == 0 )
            return;
        if ( IsParameterization ) {
            AppendParameterizedCondition( builder,values );
            return;
        }
        AppendNonParameterizedCondition( builder,values );
    }

    /// <summary>
    /// Agrega una construcción SQL al generador de SQL proporcionado.
    /// </summary>
    /// <param name="builder">El objeto StringBuilder que se utilizará para construir la cadena SQL.</param>
    /// <param name="column">El nombre de la columna que se incluirá en la construcción SQL.</param>
    /// <param name="sqlBuilder">El objeto ISqlBuilder que se encargará de agregar la parte correspondiente de la consulta SQL.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito por clases derivadas para proporcionar una funcionalidad adicional.
    /// </remarks>
    protected virtual void AppendSqlBuilder(StringBuilder builder, string column, ISqlBuilder sqlBuilder) {
        builder.AppendFormat("{0} {1} ", column, GetOperator());
        builder.Append("(");
        sqlBuilder.AppendTo(builder);
        builder.Append(")");
    }

    /// <summary>
    /// Obtiene el operador utilizado en la lógica de la clase.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el operador, en este caso "In".
    /// </returns>
    protected virtual string GetOperator() 
    { 
        return "In"; 
    }

    /// <summary>
    /// Obtiene una lista de valores a partir de la propiedad <c>Value</c>.
    /// </summary>
    /// <returns>
    /// Una lista de objetos que contiene los valores no nulos de <c>Value</c>,
    /// o <c>null</c> si <c>Value</c> no es enumerable.
    /// </returns>
    /// <remarks>
    /// Este método verifica si la propiedad <c>Value</c> es de tipo <c>IEnumerable</c>.
    /// Si no lo es, devuelve <c>null</c>. Si es enumerable, itera a través de sus elementos
    /// y agrega aquellos que no son nulos a la lista de resultados.
    /// </remarks>
    private List<object> GetValues() {
        var values = Value as IEnumerable;
        if( values == null )
            return null;
        var result = new List<object>();
        foreach ( var value in values ) {
            if ( value == null )
                continue;
            result.Add( value );
        }
        return result;
    }

    /// <summary>
    /// Agrega una condición parametrizada a un objeto <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> donde se añadirá la condición.</param>
    /// <param name="values">Una lista de valores que se utilizarán como parámetros en la condición.</param>
    /// <remarks>
    /// Este método genera una cadena que representa una condición SQL parametrizada,
    /// utilizando un operador obtenido de <see cref="GetOperator"/> y los nombres de parámetros
    /// generados por <see cref="ParameterManager.GenerateName"/>.
    /// Los valores se añaden a <see cref="ParameterManager"/> para su posterior uso.
    /// </remarks>
    /// <seealso cref="GetOperator"/>
    /// <seealso cref="ParameterManager"/>
    protected virtual void AppendParameterizedCondition(StringBuilder builder, List<object> values) {
        builder.AppendFormat("{0} {1} (", Column, GetOperator());
        foreach (var value in values) {
            var paramName = ParameterManager.GenerateName();
            builder.AppendFormat("{0},", paramName);
            ParameterManager.Add(paramName, value);
        }
        builder.RemoveEnd(",").Append(")");
    }

    /// <summary>
    /// Agrega una condición no parametrizada a un objeto StringBuilder.
    /// </summary>
    /// <param name="builder">El objeto StringBuilder al que se le añadirá la condición.</param>
    /// <param name="values">Una lista de valores que se utilizarán en la condición.</param>
    /// <remarks>
    /// Este método construye una cadena que representa una condición en formato SQL, 
    /// utilizando el nombre de la columna y un operador específico. Los valores se 
    /// formatean y se añaden entre paréntesis, separados por comas.
    /// </remarks>
    protected virtual void AppendNonParameterizedCondition(StringBuilder builder, List<object> values) {
        builder.AppendFormat("{0} {1} (", Column, GetOperator());
        foreach (var value in values) {
            builder.AppendFormat("{0},", GetFormattedValue(value));
        }
        builder.RemoveEnd(",").Append(")");
    }

    /// <summary>
    /// Formatea el valor proporcionado en función de su tipo.
    /// </summary>
    /// <param name="value">El valor que se desea formatear.</param>
    /// <returns>
    /// Una representación en forma de cadena del valor formateado. 
    /// Si el valor es de tipo <see cref="System.String"/>, se devuelve entre comillas simples.
    /// En caso contrario, se devuelve la representación por defecto del valor.
    /// </returns>
    private string GetFormattedValue( object value ) {
        switch ( value.GetType().ToString() ) {
            case "System.String":
                return $"'{value}'";
            default:
                return value.ToString();
        }
    }
}