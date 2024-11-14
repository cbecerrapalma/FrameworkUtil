namespace Util.Data.Sql.Builders.Operations; 

/// <summary>
/// Interfaz que define las operaciones SQL básicas.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ISqlQueryOperation"/> y <see cref="IInsert"/>,
/// lo que permite realizar tanto consultas como inserciones en una base de datos.
/// </remarks>
public interface ISqlOperation : ISqlQueryOperation, IInsert {
}