namespace Util.Data.Dapper.TypeHandlers; 

/// <summary>
/// Clase que maneja la conversión de tipos <see cref="Guid"/> para el mapeo de datos en SQL.
/// Implementa la interfaz <see cref="SqlMapper.ITypeHandler"/> para proporcionar funcionalidad personalizada
/// al trabajar con tipos GUID en bases de datos.
/// </summary>
public class GuidTypeHandler : SqlMapper.ITypeHandler {
    /// <summary>
    /// Establece el valor de un parámetro de base de datos de tipo Oracle.
    /// </summary>
    /// <param name="parameter">El parámetro de base de datos que se va a modificar.</param>
    /// <param name="value">El valor que se asignará al parámetro.</param>
    /// <remarks>
    /// Este método convierte el parámetro a un tipo específico de OracleParameter y establece su tipo de base de datos 
    /// como OracleDbType.Raw antes de asignarle el valor proporcionado.
    /// </remarks>
    /// <exception cref="InvalidCastException">Se produce si el parámetro no es de tipo OracleParameter.</exception>
    public void SetValue( IDbDataParameter parameter, object value ) {
        var oracleParameter = (OracleParameter)parameter;
        oracleParameter.OracleDbType = OracleDbType.Raw;
        parameter.Value = value;
    }

    /// <summary>
    /// Convierte el valor proporcionado al tipo de destino especificado.
    /// </summary>
    /// <param name="destinationType">El tipo al que se desea convertir el valor.</param>
    /// <param name="value">El valor que se desea convertir.</param>
    /// <returns>
    /// Un objeto que representa el valor convertido al tipo de destino especificado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la función <c>Util.Helpers.Convert.ToGuid</c> para realizar la conversión.
    /// </remarks>
    public object Parse( Type destinationType, object value ) {
        return Util.Helpers.Convert.ToGuid( value );
    }
}