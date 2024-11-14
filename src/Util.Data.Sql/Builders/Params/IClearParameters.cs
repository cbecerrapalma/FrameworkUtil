namespace Util.Data.Sql.Builders.Params; 

/// <summary>
/// Define un contrato para las clases que implementan la funcionalidad de limpiar parámetros.
/// </summary>
public interface IClearParameters {
    /// <summary>
    /// Limpia todos los parámetros establecidos.
    /// </summary>
    /// <remarks>
    /// Este método restablece el estado de los parámetros a sus valores predeterminados,
    /// eliminando cualquier dato previamente almacenado.
    /// </remarks>
    void ClearParams();
}