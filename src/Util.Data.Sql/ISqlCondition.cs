using Util.Data.Sql.Builders;

namespace Util.Data.Sql; 

/// <summary>
/// Define una interfaz para las condiciones SQL.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ISqlContent"/> y se utiliza para representar
/// condiciones que pueden ser utilizadas en consultas SQL.
/// </remarks>
public interface ISqlCondition : ISqlContent {
}