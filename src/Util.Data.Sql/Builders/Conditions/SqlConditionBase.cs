using Util.Data.Sql.Builders.Params;

namespace Util.Data.Sql.Builders.Conditions; 

/// <summary>
/// Clase base abstracta que representa una condición SQL.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ISqlCondition"/> y proporciona una base para 
/// crear condiciones SQL específicas. Las clases derivadas deben implementar la lógica 
/// necesaria para construir la representación SQL de la condición.
/// </remarks>
public abstract class SqlConditionBase : ISqlCondition {
    protected readonly IParameterManager ParameterManager;
    protected readonly string Column;
    protected readonly object Value;
    protected readonly bool IsParameterization;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqlConditionBase"/>.
    /// </summary>
    /// <param name="parameterManager">El administrador de parámetros que se utilizará para manejar los parámetros de la consulta SQL.</param>
    /// <param name="column">El nombre de la columna sobre la cual se aplicará la condición.</param>
    /// <param name="value">El valor que se comparará con el valor de la columna.</param>
    /// <param name="isParameterization">Indica si se debe utilizar la parametrización en la consulta SQL.</param>
    /// <exception cref="ArgumentNullException">
    /// Se lanza si <paramref name="parameterManager"/> es <c>null</c>, o si <paramref name="column"/> es una cadena vacía o solo contiene espacios en blanco.
    /// </exception>
    protected SqlConditionBase( IParameterManager parameterManager, string column, object value, bool isParameterization ) {
        ParameterManager = parameterManager ?? throw new ArgumentNullException( nameof( parameterManager ) );
        if( string.IsNullOrWhiteSpace( column ) )
            throw new ArgumentNullException( nameof( column ) );
        Column = column;
        Value = value;
        IsParameterization = isParameterization;
    }

    /// <summary>
    /// Agrega una representación de la condición actual al <see cref="StringBuilder"/> proporcionado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se añadirá la representación de la condición.</param>
    /// <remarks>
    /// Este método verifica si el valor actual es un <see cref="ISqlBuilder"/> y, de ser así, 
    /// llama al método <see cref="AppendSqlBuilder"/> para agregar la representación SQL. 
    /// Si la condición está parametrizada, se utiliza <see cref="AppendParameterizedCondition"/>; 
    /// de lo contrario, se llama a <see cref="AppendNonParameterizedCondition"/>.
    /// </remarks>
    public virtual void AppendTo( StringBuilder builder ) {
        if ( Value is ISqlBuilder sqlBuilder ) {
            AppendSqlBuilder( builder,Column, sqlBuilder );
            return;
        }
        if ( IsParameterization ) {
            AppendParameterizedCondition( builder );
            return;
        }
        AppendNonParameterizedCondition( builder );
    }

    /// <summary>
    /// Agrega un constructor SQL al generador de SQL especificado.
    /// </summary>
    /// <param name="builder">El objeto <see cref="StringBuilder"/> que se utilizará para construir la cadena SQL.</param>
    /// <param name="column">El nombre de la columna que se va a agregar al generador SQL.</param>
    /// <param name="sqlBuilder">El objeto <see cref="ISqlBuilder"/> que se utilizará para construir la consulta SQL.</param>
    /// <exception cref="NotImplementedException">Se lanza cuando el método no ha sido implementado.</exception>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación específica.
    /// </remarks>
    protected virtual void AppendSqlBuilder(StringBuilder builder, string column, ISqlBuilder sqlBuilder) {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Agrega una condición parametrizada al <see cref="StringBuilder"/> especificado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le añadirá la condición.</param>
    /// <remarks>
    /// Este método genera un nombre de parámetro único, obtiene el valor correspondiente y lo añade a la gestión de parámetros.
    /// Luego, se llama al método <see cref="AppendCondition"/> para agregar la condición al <paramref name="builder"/>.
    /// </remarks>
    protected virtual void AppendParameterizedCondition(StringBuilder builder) {
        var paramName = GenerateParamName();
        var value = GetValue();
        ParameterManager.Add(paramName, value);
        AppendCondition(builder, Column, paramName);
    }

    /// <summary>
    /// Genera un nombre de parámetro utilizando el administrador de parámetros.
    /// </summary>
    /// <returns>
    /// Un string que representa el nombre del parámetro generado.
    /// </returns>
    protected virtual string GenerateParamName() {
        return ParameterManager.GenerateName();
    }

    /// <summary>
    /// Obtiene el valor de la propiedad.
    /// </summary>
    /// <returns>
    /// El valor de la propiedad.
    /// </returns>
    protected virtual object GetValue() {
        return Value;
    }

    /// <summary>
    /// Método abstracto que permite agregar una condición a un objeto <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le añadirá la condición.</param>
    /// <param name="column">El nombre de la columna sobre la cual se aplicará la condición.</param>
    /// <param name="value">El valor que se utilizará en la condición.</param>
    /// <remarks>
    /// Este método debe ser implementado por las clases derivadas para definir cómo se 
    /// construyen las condiciones específicas para la consulta.
    /// </remarks>
    protected abstract void AppendCondition(StringBuilder builder, string column, object value);

    /// <summary>
    /// Agrega una condición no parametrizada al <see cref="StringBuilder"/> especificado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le agregará la condición.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación personalizada.
    /// </remarks>
    protected virtual void AppendNonParameterizedCondition(StringBuilder builder) 
    {
        AppendCondition(builder, Column, GetValue());
    }
}