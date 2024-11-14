using Util.Dependency;

namespace Util.Data.EntityFrameworkCore.Migrations;

/// <summary>
/// Interfaz que define los servicios relacionados con la gestión de archivos de migración.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ITransientDependency"/> lo que indica que su ciclo de vida es transitorio.
/// Los servicios que implementan esta interfaz deben proporcionar funcionalidades para manejar archivos de migración
/// en el contexto de la aplicación.
/// </remarks>
public interface IMigrationFileService : ITransientDependency {
    /// <summary>
    /// Establece la ruta para los archivos de migración.
    /// </summary>
    /// <param name="path">La ruta donde se encuentran los archivos de migración.</param>
    /// <returns>Una instancia de <see cref="IMigrationFileService"/> configurada con la ruta especificada.</returns>
    IMigrationFileService MigrationsPath(string path);
    /// <summary>
    /// Interfaz que define los métodos para gestionar archivos de migración.
    /// </summary>
    IMigrationFileService MigrationName( string name );
    /// <summary>
    /// Elimina las claves foráneas de la base de datos.
    /// </summary>
    /// <returns>Una instancia de <see cref="IMigrationFileService"/> que representa el servicio de migración de archivos.</returns>
    /// <remarks>
    /// Este método se utiliza para limpiar las relaciones de claves foráneas antes de realizar otras operaciones de migración.
    /// </remarks>
    IMigrationFileService RemoveForeignKeys();
    /// <summary>
    /// Obtiene la ruta del archivo.
    /// </summary>
    /// <returns>
    /// La ruta del archivo como una cadena.
    /// </returns>
    /// <remarks>
    /// Este método puede ser utilizado para obtener la ubicación de un archivo específico
    /// en el sistema de archivos. Asegúrese de que el archivo exista en la ruta devuelta
    /// antes de intentar acceder a él.
    /// </remarks>
    string GetFilePath();
    /// <summary>
    /// Obtiene el contenido como una cadena de texto.
    /// </summary>
    /// <returns>Una cadena que representa el contenido.</returns>
    string GetContent();
    /// <summary>
    /// Guarda el contenido en el archivo especificado por la ruta.
    /// </summary>
    /// <param name="filePath">La ruta del archivo donde se guardará el contenido. Si es null, se usará una ruta predeterminada.</param>
    /// <remarks>
    /// Este método sobrescribirá el archivo existente si ya existe uno en la ruta especificada.
    /// </remarks>
    /// <exception cref="System.IO.IOException">
    /// Se produce si hay un error al intentar guardar el archivo.
    /// </exception>
    void Save(string filePath = null);
}