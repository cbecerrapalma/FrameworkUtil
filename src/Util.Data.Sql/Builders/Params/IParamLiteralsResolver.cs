namespace Util.Data.Sql.Builders.Params; 

/// <summary>
/// Define un contrato para resolver literales de parámetros.
/// </summary>
public interface IParamLiteralsResolver {
    /// <summary>
    /// Obtiene una representación en forma de cadena de los literales de un parámetro dado.
    /// </summary>
    /// <param name="value">El objeto del cual se obtendrán los literales.</param>
    /// <returns>Una cadena que representa los literales del parámetro.</returns>
    /// <remarks>
    /// Este método puede manejar diferentes tipos de objetos y convertirá su representación a una cadena.
    /// Asegúrese de que el objeto no sea nulo antes de llamar a este método para evitar excepciones.
    /// </remarks>
    /// <seealso cref="System.String"/>
    string GetParamLiterals( object value );
}