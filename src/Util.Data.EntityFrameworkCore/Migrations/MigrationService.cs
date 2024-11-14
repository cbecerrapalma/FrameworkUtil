using Util.Helpers;

namespace Util.Data.EntityFrameworkCore.Migrations;

/// <summary>
/// Proporciona servicios para la migración de datos entre diferentes sistemas.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IMigrationService"/> y contiene métodos
/// para realizar operaciones de migración de datos, incluyendo la configuración de las
/// conexiones y la ejecución de procesos de migración.
/// </remarks>
public class MigrationService : IMigrationService
{
    private readonly ILogger<MigrationService> _logger;
    private readonly IMigrationFileService _migrationFileService;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MigrationService"/>.
    /// </summary>
    /// <param name="logger">El registrador utilizado para registrar información y errores.</param>
    /// <param name="migrationFileService">El servicio encargado de manejar archivos de migración.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="migrationFileService"/> es <c>null</c>.</exception>
    public MigrationService(ILogger<MigrationService> logger, IMigrationFileService migrationFileService)
    {
        _logger = logger ?? NullLogger<MigrationService>.Instance;
        _migrationFileService = migrationFileService ?? throw new ArgumentNullException(nameof(migrationFileService));
    }

    /// <inheritdoc />
    /// <summary>
    /// Instala la herramienta global dotnet-ef.
    /// </summary>
    /// <param name="version">La versión específica de la herramienta dotnet-ef a instalar. Si no se especifica, se instalará la última versión disponible.</param>
    /// <returns>Una instancia del servicio de migración.</returns>
    /// <remarks>
    /// Este método registra un mensaje de traza antes de ejecutar el comando de instalación.
    /// Utiliza el comando 'dotnet tool install' para instalar la herramienta dotnet-ef.
    /// </remarks>
    /// <seealso cref="IMigrationService"/>
    public IMigrationService InstallEfTool(string version = null)
    {
        _logger.LogTrace("Preparándose para instalar la herramienta global dotnet-ef.");
        var versionArgs = version.IsEmpty() ? null : $" --version {version}";
        CommandLine.Create("dotnet", $"tool install -g dotnet-ef{versionArgs}")
            .OutputToMatch("dotnet-ef")
            .Log(_logger)
            .Execute();
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Desinstala la herramienta global dotnet-ef.
    /// </summary>
    /// <returns>
    /// Devuelve una instancia de <see cref="IMigrationService"/> para permitir el encadenamiento de llamadas.
    /// </returns>
    /// <remarks>
    /// Este método registra un mensaje de traza en el logger antes de ejecutar el comando para desinstalar la herramienta.
    /// Utiliza el comando de línea de comandos de .NET para llevar a cabo la desinstalación.
    /// </remarks>
    public IMigrationService UninstallEfTool()
    {
        _logger.LogTrace("Preparándose para desinstalar la herramienta global dotnet-ef.");
        CommandLine.Create("dotnet", "tool uninstall -g dotnet-ef")
            .OutputToMatch("dotnet-ef")
            .Log(_logger)
            .Execute();
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Actualiza la herramienta global dotnet-ef a la versión especificada.
    /// </summary>
    /// <param name="version">La versión a la que se desea actualizar la herramienta. Si se omite, se actualizará a la última versión disponible.</param>
    /// <returns>Una instancia del servicio de migración.</returns>
    /// <remarks>
    /// Este método utiliza la línea de comandos para ejecutar el comando de actualización de la herramienta dotnet-ef.
    /// Se registra un mensaje de traza antes de ejecutar el comando.
    /// </remarks>
    /// <seealso cref="IMigrationService"/>
    public IMigrationService UpdateEfTool(string version = null)
    {
        _logger.LogTrace("Preparándose para actualizar la herramienta global dotnet-ef.");
        var versionArgs = version.IsEmpty() ? null : $" --version {version}";
        CommandLine.Create("dotnet", $"tool update -g dotnet-ef{versionArgs}")
            .OutputToMatch("dotnet-ef")
            .Log(_logger)
            .Execute();
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Agrega una nueva migración a la base de datos utilizando Entity Framework.
    /// </summary>
    /// <param name="migrationName">El nombre de la migración que se va a agregar.</param>
    /// <param name="dbContextRootPath">La ruta absoluta del directorio raíz del proyecto del contexto de la base de datos.</param>
    /// <param name="isRemoveForeignKeys">Indica si se deben eliminar las claves foráneas de la migración. El valor predeterminado es <c>false</c>.</param>
    /// <returns>Una instancia de <see cref="IMigrationService"/> que representa el servicio de migración.</returns>
    /// <exception cref="ArgumentException">Se lanza si <paramref name="migrationName"/> o <paramref name="dbContextRootPath"/> están vacíos.</exception>
    /// <remarks>
    /// Este método ejecuta un comando de línea para agregar la migración y, si se especifica, elimina las claves foráneas de los archivos de migración.
    /// </remarks>
    public IMigrationService AddMigration(string migrationName, string dbContextRootPath, bool isRemoveForeignKeys = false)
    {
        if (migrationName.IsEmpty())
            throw new ArgumentException("Se debe establecer un nombre de migración.");
        if (dbContextRootPath.IsEmpty())
            throw new ArgumentException("Debes establecer la ruta absoluta del directorio raíz del proyecto del contexto de datos.");
        _logger.LogTrace("Preparándose para agregar migraciones de EF.");
        CommandLine.Create("dotnet", $"ef migrations add {migrationName}")
            .WorkingDirectory(dbContextRootPath)
            .OutputToMatch("Hecho.")
            .OutputToMatch("utilizado por una migración existente")
            .Log(_logger)
            .Execute();
        if (isRemoveForeignKeys)
            RemoveMigrationFileForeignKeys(migrationName, dbContextRootPath);
        return this;
    }

    /// <summary>
    /// Elimina las configuraciones de claves foráneas de un archivo de migración específico.
    /// </summary>
    /// <param name="migrationName">El nombre de la migración de la cual se eliminarán las claves foráneas.</param>
    /// <param name="dbContextRootPath">La ruta raíz del contexto de la base de datos donde se encuentran los archivos de migración.</param>
    /// <remarks>
    /// Este método registra una traza indicando que se está preparando para eliminar las configuraciones de claves foráneas,
    /// construye la ruta de acceso a los archivos de migración y utiliza el servicio de archivos de migración para
    /// eliminar las claves foráneas y guardar los cambios.
    /// </remarks>
    private void RemoveMigrationFileForeignKeys(string migrationName, string dbContextRootPath)
    {
        _logger.LogTrace("Prepárese para eliminar la configuración de clave externa en el archivo de migración.");
        var migrationsPath = Common.JoinPath(dbContextRootPath, "Migrations");
        _migrationFileService
            .MigrationsPath(migrationsPath)
            .MigrationName(migrationName)
            .RemoveForeignKeys()
            .Save();
    }

    /// <inheritdoc />
    /// <summary>
    /// Realiza la migración de la base de datos utilizando Entity Framework.
    /// </summary>
    /// <param name="dbContextRootPath">La ruta del directorio raíz del contexto de la base de datos.</param>
    /// <remarks>
    /// Este método ejecuta el comando de migración de Entity Framework para actualizar la base de datos.
    /// Se registran diferentes mensajes de salida para monitorear el progreso y los posibles errores durante la ejecución.
    /// </remarks>
    /// <seealso cref="CommandLine"/>
    public void Migrate(string dbContextRootPath)
    {
        _logger.LogTrace("Preparándose para ejecutar la migración de ef para actualizar la base de datos.");
        CommandLine.Create("dotnet", "ef database update")
            .WorkingDirectory(dbContextRootPath)
            .OutputToMatch("TLa propiedad ConnectionString no ha sido inicializada.")
            .OutputToMatch("El servidor no fue encontrado o no estaba accesible.")
            .OutputToMatch("Ya hay un objeto llamado")
            .OutputToMatch("Aplicando migración")
            .OutputToMatch("Hecho")
            .Log(_logger)
            .Execute();
    }
}