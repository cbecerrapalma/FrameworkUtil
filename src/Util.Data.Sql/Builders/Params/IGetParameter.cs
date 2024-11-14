namespace Util.Data.Sql.Builders.Params; 

/// <summary>
/// Define un contrato para obtener parámetros.
/// </summary>
public interface IGetParameter {
    /// <summary>
    /// Obtiene un parámetro de tipo T a partir de su nombre.
    /// </summary>
    /// <typeparam name="T">El tipo del parámetro que se desea obtener.</typeparam>
    /// <param name="name">El nombre del parámetro que se desea recuperar.</param>
    /// <returns>El valor del parámetro de tipo T.</returns>
    /// <remarks>
    /// Este método busca un parámetro en una colección y lo devuelve como el tipo especificado.
    /// Si el parámetro no se encuentra o no puede ser convertido al tipo T, se puede lanzar una excepción.
    /// </remarks>
    /// <seealso cref="System.Exception"/>
    T GetParam<T>( string name );
}