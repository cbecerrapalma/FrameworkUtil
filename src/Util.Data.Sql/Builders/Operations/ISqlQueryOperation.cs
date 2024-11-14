namespace Util.Data.Sql.Builders.Operations; 

/// <summary>
/// Define una interfaz para realizar operaciones de consulta SQL.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de múltiples interfaces que representan diferentes partes de una consulta SQL,
/// permitiendo construir consultas complejas de manera fluida y estructurada.
/// </remarks>
public interface ISqlQueryOperation : IStart, ISelect, IFrom, IJoin, IWhere, IGroupBy, IOrderBy, IEnd, ISqlParameter, ISet {
}