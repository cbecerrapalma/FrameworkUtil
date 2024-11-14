using Util.Dependency;

namespace Util.Data.EntityFrameworkCore.Migrations;

/// <summary>
/// Define un servicio de migración que se utiliza para gestionar las migraciones de datos.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ITransientDependency"/> lo que indica que su ciclo de vida es transitorio.
/// </remarks>
public interface IMigrationService : ITransientDependency {
    /// <summary>
    /// Instala la herramienta de Entity Framework (EF) para la migración.
    /// </summary>
    /// <param name="version">La versión específica de la herramienta EF a instalar. Si se omite, se instalará la versión predeterminada.</param>
    /// <returns>Una instancia de <see cref="IMigrationService"/> que representa el servicio de migración instalado.</returns>
    /// <remarks>
    /// Este método permite a los desarrolladores instalar la herramienta de migración de EF, facilitando la gestión de migraciones en la base de datos.
    /// </remarks>
    /// <seealso cref="IMigrationService"/>
    IMigrationService InstallEfTool( string version = null );
    /// <summary>
    /// Desinstala la herramienta de Entity Framework.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="IMigrationService"/> que representa el servicio de migración después de la desinstalación.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para eliminar la herramienta de Entity Framework del entorno actual.
    /// Asegúrese de que no haya migraciones pendientes antes de llamar a este método.
    /// </remarks>
    IMigrationService UninstallEfTool();
    /// <summary>
    /// Actualiza la herramienta de Entity Framework a la versión especificada.
    /// </summary>
    /// <param name="version">La versión a la que se desea actualizar. Si se establece en <c>null</c>, se actualizará a la última versión disponible.</param>
    /// <returns>Una instancia de <see cref="IMigrationService"/> que representa el servicio de migración actualizado.</returns>
    /// <remarks>
    /// Este método permite gestionar la actualización de la herramienta de Entity Framework, facilitando la migración de bases de datos 
    /// a nuevas versiones y asegurando que se apliquen las últimas mejoras y correcciones de errores.
    /// </remarks>
    /// <seealso cref="IMigrationService"/>
    IMigrationService UpdateEfTool( string version = null );
    /// <summary>
    /// Agrega una nueva migración a la base de datos.
    /// </summary>
    /// <param name="migrationName">El nombre de la migración que se va a agregar.</param>
    /// <param name="dbContextRootPath">La ruta raíz del contexto de la base de datos.</param>
    /// <param name="isRemoveForeignKeys">Indica si se deben eliminar las claves foráneas. El valor predeterminado es <c>false</c>.</param>
    /// <returns>Una instancia de <see cref="IMigrationService"/> que representa el servicio de migración.</returns>
    /// <remarks>
    /// Este método se utiliza para crear y aplicar una nueva migración en el contexto de la base de datos especificado.
    /// Si <paramref name="isRemoveForeignKeys"/> se establece en <c>true</c>, se eliminarán las claves foráneas durante el proceso de migración.
    /// </remarks>
    IMigrationService AddMigration( string migrationName, string dbContextRootPath, bool isRemoveForeignKeys = false );
    /// <summary>
    /// Realiza la migración de la base de datos utilizando el contexto especificado.
    /// </summary>
    /// <param name="dbContextRootPath">La ruta raíz del contexto de la base de datos que se va a migrar.</param>
    /// <remarks>
    /// Este método se encarga de aplicar las migraciones pendientes a la base de datos
    /// definida en el contexto. Asegúrese de que la ruta proporcionada sea correcta
    /// y que el contexto esté configurado adecuadamente para evitar errores durante
    /// el proceso de migración.
    /// </remarks>
    /// <seealso cref="DbContext"/>
    void Migrate( string dbContextRootPath );
}