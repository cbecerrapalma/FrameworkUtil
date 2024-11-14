using Util.Data.Queries;
using Util.Data.Sql.Builders.Params;

namespace Util.Data.Sql.Builders.Conditions; 

/// <summary>
/// Representa una condición de segmento para ser utilizada en consultas SQL.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ISqlCondition"/> y permite definir condiciones específicas
/// que pueden ser aplicadas en la construcción de sentencias SQL.
/// </remarks>
public class SegmentCondition : ISqlCondition {
    protected readonly IParameterManager ParameterManager;
    protected readonly string Column;
    protected readonly object MinValue;
    protected readonly object MaxValue;
    protected Boundary Boundary;
    protected readonly bool IsParameterization;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SegmentCondition"/>.
    /// </summary>
    /// <param name="parameterManager">El administrador de parámetros que se utilizará para gestionar los parámetros de la condición.</param>
    /// <param name="column">El nombre de la columna que se evaluará en la condición.</param>
    /// <param name="minValue">El valor mínimo de la condición.</param>
    /// <param name="maxValue">El valor máximo de la condición.</param>
    /// <param name="boundary">El límite que define cómo se interpretan los valores mínimo y máximo.</param>
    /// <param name="isParameterization">Indica si la condición utiliza parametrización.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="parameterManager"/> o <paramref name="column"/> son nulos o vacíos.</exception>
    /// <remarks>
    /// Esta clase se utiliza para definir condiciones de segmento basadas en rangos de valores 
    /// en una columna específica, permitiendo la parametrización de los valores.
    /// </remarks>
    public SegmentCondition( IParameterManager parameterManager, string column, object minValue, object maxValue, Boundary boundary, bool isParameterization ) {
        ParameterManager = parameterManager ?? throw new ArgumentNullException( nameof( parameterManager ) );
        if( string.IsNullOrWhiteSpace( column ) )
            throw new ArgumentNullException( nameof( column ) );
        Column = column;
        MinValue = minValue;
        MaxValue = maxValue;
        Boundary = boundary;
        IsParameterization = isParameterization;
    }

    /// <summary>
    /// Agrega una condición compuesta a un objeto <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le añadirá la condición.</param>
    /// <remarks>
    /// Este método crea una condición AND utilizando dos condiciones generadas por los métodos 
    /// <see cref="CreateLeftCondition"/> y <see cref="CreateRightCondition"/>. Luego, 
    /// la condición resultante se añade al <paramref name="builder"/>.
    /// </remarks>
    public void AppendTo( StringBuilder builder ) {
        new AndCondition( CreateLeftCondition(), CreateRightCondition() ).AppendTo( builder );
    }

    /// <summary>
    /// Crea una condición SQL para el límite izquierdo basado en el valor mínimo y el tipo de límite especificado.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="ISqlCondition"/> que representa la condición SQL para el límite izquierdo.
    /// Si el valor mínimo es nulo o vacío, se devuelve una instancia de <see cref="NullCondition"/>.
    /// </returns>
    /// <remarks>
    /// Este método evalúa el tipo de límite (izquierdo, ambos o derecho) y crea la condición correspondiente.
    /// Si el límite es izquierdo o ambos, se utiliza una condición de mayor o igual.
    /// Si el límite es derecho, se utiliza una condición de mayor.
    /// </remarks>
    private ISqlCondition CreateLeftCondition() {
        if( string.IsNullOrWhiteSpace( MinValue.SafeString() ) )
            return NullCondition.Instance;
        switch( Boundary ) {
            case Boundary.Left:
                return new GreaterEqualCondition( ParameterManager, Column, MinValue, IsParameterization );
            case Boundary.Both:
                return new GreaterEqualCondition( ParameterManager, Column, MinValue, IsParameterization );
            default:
                return new GreaterCondition( ParameterManager, Column, MinValue, IsParameterization );
        }
    }

    /// <summary>
    /// Crea una condición SQL para el límite derecho basado en el valor máximo definido.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="ISqlCondition"/> que representa la condición SQL
    /// correspondiente al límite derecho. Si el valor máximo es nulo o vacío, se devuelve
    /// una instancia de <see cref="NullCondition"/>.
    /// </returns>
    /// <remarks>
    /// Este método evalúa el tipo de límite definido en la propiedad <see cref="Boundary"/> 
    /// y crea la condición adecuada. Si el límite es "Derecho" o "Ambos", se utiliza 
    /// <see cref="LessEqualCondition"/>; de lo contrario, se utiliza <see cref="LessCondition"/>.
    /// </remarks>
    private ISqlCondition CreateRightCondition() {
        if( string.IsNullOrWhiteSpace( MaxValue.SafeString() ) )
            return NullCondition.Instance;
        switch( Boundary ) {
            case Boundary.Right:
                return new LessEqualCondition( ParameterManager, Column, MaxValue, IsParameterization );
            case Boundary.Both:
                return new LessEqualCondition( ParameterManager, Column, MaxValue, IsParameterization );
            default:
                return new LessCondition( ParameterManager, Column, MaxValue, IsParameterization );
        }
    }
}