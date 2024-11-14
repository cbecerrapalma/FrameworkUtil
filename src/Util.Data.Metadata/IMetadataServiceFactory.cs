namespace Util.Data.Metadata; 

/// <summary>
/// Define una interfaz para la fábrica de servicios de metadatos.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ITransientDependency"/> lo que indica que las implementaciones de esta interfaz 
/// deben ser creadas como dependencias transitorias.
/// </remarks>
public interface IMetadataServiceFactory : ITransientDependency {
    /// <summary>
    /// Crea una instancia de <see cref="IMetadataService"/> según el tipo de base de datos especificado.
    /// </summary>
    /// <param name="dbType">El tipo de base de datos que se utilizará para crear el servicio.</param>
    /// <param name="connection">La cadena de conexión que se utilizará para conectarse a la base de datos.</param>
    /// <returns>Una instancia de <see cref="IMetadataService"/> configurada para el tipo de base de datos especificado.</returns>
    /// <remarks>
    /// Este método es útil para abstraer la creación de servicios de metadatos, permitiendo que el sistema 
    /// se adapte a diferentes tipos de bases de datos sin necesidad de cambiar el código que consume este servicio.
    /// </remarks>
    /// <seealso cref="IMetadataService"/>
    /// <seealso cref="DatabaseType"/>
    IMetadataService Create( DatabaseType dbType, string connection );
}