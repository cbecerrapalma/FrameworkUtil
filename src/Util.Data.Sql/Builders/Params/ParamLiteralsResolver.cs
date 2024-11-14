namespace Util.Data.Sql.Builders.Params; 

/// <summary>
/// Clase que implementa la interfaz <see cref="IParamLiteralsResolver"/>.
/// Esta clase se encarga de resolver literales de parámetros en una aplicación.
/// </summary>
public class ParamLiteralsResolver : IParamLiteralsResolver {
    /// <summary>
    /// Obtiene la representación en forma de literal de un parámetro basado en su tipo.
    /// </summary>
    /// <param name="value">El valor del cual se desea obtener la representación literal.</param>
    /// <returns>
    /// Una cadena que representa el valor en forma de literal. 
    /// Si el valor es nulo, se devuelve una cadena vacía. 
    /// Para valores booleanos, se devuelve "1" para verdadero y "0" para falso. 
    /// Para tipos numéricos, se devuelve su representación en cadena. 
    /// Para otros tipos, se devuelve el valor entre comillas simples.
    /// </returns>
    public string GetParamLiterals( object value ) {
        if( value == null )
            return "''";
        switch( value.GetType().Name.ToLower() ) {
            case "boolean":
                return Helpers.Convert.ToBool( value ) ? "1" : "0";
            case "int16":
            case "int32":
            case "int64":
            case "single":
            case "double":
            case "decimal":
                return value.SafeString();
            default:
                return $"'{value}'";
        }
    }
}