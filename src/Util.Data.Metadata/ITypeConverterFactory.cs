namespace Util.Data.Metadata; 

/// <summary>
/// Define una interfaz para la fábrica de convertidores de tipos.
/// </summary>
/// <remarks>
/// Esta interfaz se utiliza para crear instancias de convertidores de tipos,
/// permitiendo la conversión entre diferentes tipos de datos.
/// </remarks>
public interface ITypeConverterFactory : ITransientDependency {
    /// <summary>
    /// Crea una instancia de un convertidor de tipo basado en el tipo de base de datos especificado.
    /// </summary>
    /// <param name="dbType">El tipo de base de datos para el cual se desea crear el convertidor de tipo.</param>
    /// <returns>Una instancia de <see cref="ITypeConverter"/> correspondiente al tipo de base de datos proporcionado.</returns>
    /// <remarks>
    /// Este método permite la creación de convertidores de tipo específicos para diferentes tipos de bases de datos,
    /// facilitando la conversión de tipos de datos entre la base de datos y la aplicación.
    /// </remarks>
    /// <seealso cref="ITypeConverter"/>
    ITypeConverter Create( DatabaseType dbType );
}