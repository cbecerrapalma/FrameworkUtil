using Util.Helpers;

namespace Util.Data.EntityFrameworkCore.Migrations;

/// <summary>
/// Proporciona servicios para manejar archivos de migración.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IMigrationFileService"/> y contiene métodos
/// para gestionar la creación, lectura y eliminación de archivos de migración.
/// </remarks>
public class MigrationFileService : IMigrationFileService
{
    private readonly ILogger<MigrationFileService> _logger;
    private string _migrationsPath;
    private string _migrationName;
    private bool _isRemoveForeignKeys;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MigrationFileService"/>.
    /// </summary>
    /// <param name="logger">El objeto <see cref="ILogger{T}"/> utilizado para registrar información de la migración. Si es nulo, se utilizará un logger nulo.</param>
    public MigrationFileService(ILogger<MigrationFileService> logger)
    {
        _logger = logger ?? NullLogger<MigrationFileService>.Instance;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece la ruta de los archivos de migración.
    /// </summary>
    /// <param name="path">La ruta donde se encuentran los archivos de migración.</param>
    /// <returns>Una instancia del servicio de archivos de migración.</returns>
    /// <remarks>
    /// Este método permite configurar la ubicación de los archivos de migración
    /// que serán utilizados por el servicio. Asegúrese de que la ruta proporcionada
    /// sea válida y accesible.
    /// </remarks>
    public IMigrationFileService MigrationsPath(string path)
    {
        _migrationsPath = path;
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el nombre de la migración.
    /// </summary>
    /// <param name="name">El nombre que se asignará a la migración.</param>
    /// <returns>La instancia actual de <see cref="IMigrationFileService"/>.</returns>
    /// <remarks>
    /// Este método permite configurar el nombre de la migración que se utilizará en el proceso de migración.
    /// </remarks>
    public IMigrationFileService MigrationName(string name)
    {
        _migrationName = name;
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina las claves foráneas del servicio de migración.
    /// </summary>
    /// <returns>
    /// Una instancia del servicio de migración de archivos.
    /// </returns>
    /// <remarks>
    /// Este método establece un indicador interno que indica que las claves foráneas deben ser eliminadas.
    /// </remarks>
    public IMigrationFileService RemoveForeignKeys()
    {
        _isRemoveForeignKeys = true;
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene la ruta del archivo de migración correspondiente al nombre de migración especificado.
    /// </summary>
    /// <returns>
    /// La ruta completa del archivo de migración si se encuentra; de lo contrario, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si la ruta de migraciones y el nombre de migración no están vacíos antes de intentar
    /// localizar el archivo. Busca archivos con la extensión ".cs" en la ruta de migraciones y devuelve la ruta
    /// del archivo que coincide con el nombre de migración.
    /// </remarks>
    public string GetFilePath()
    {
        if (_migrationsPath.IsEmpty())
            return null;
        if (_migrationName.IsEmpty())
            return null;
        var files = Util.Helpers.File.GetAllFiles(_migrationsPath, "*.cs");
        var file = files.FirstOrDefault(t => t.Name.EndsWith($"{_migrationName}.cs"));
        if (file == null)
            return null;
        return file.FullName;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el contenido del archivo especificado por la ruta.
    /// </summary>
    /// <returns>
    /// Devuelve el contenido del archivo como una cadena de texto. 
    /// Si la ruta del archivo está vacía, se devuelve null.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el método <see cref="Util.Helpers.File.ReadToString(string)"/> 
    /// para leer el contenido del archivo.
    /// </remarks>
    public string GetContent()
    {
        var filePath = GetFilePath();
        if (filePath.IsEmpty())
            return null;
        return Util.Helpers.File.ReadToString(filePath);
    }

    /// <inheritdoc />
    /// <summary>
    /// Guarda el contenido modificado en un archivo especificado.
    /// </summary>
    /// <param name="filePath">La ruta del archivo donde se guardará el contenido. Si es nula o vacía, se utilizará la ruta predeterminada obtenida mediante <see cref="GetFilePath"/>.</param>
    /// <remarks>
    /// Este método solo realiza la operación de guardado si la propiedad <c>_isRemoveForeignKeys</c> es verdadera.
    /// El contenido se procesa para eliminar las claves foráneas y se limpia el formato antes de guardarlo.
    /// </remarks>
    /// <seealso cref="GetFilePath"/>
    /// <seealso cref="GetContent"/>
    /// <seealso cref="Util.Helpers.File.Write"/>
    /// <seealso cref="_logger"/>
    public void Save(string filePath = null)
    {
        if (_isRemoveForeignKeys == false)
            return;
        if (filePath.IsEmpty())
            filePath = GetFilePath();
        var content = GetContent();
        var pattern = @"table.ForeignKey\([\s\S]+?\);";
        var result = Util.Helpers.Regex.Replace(content, pattern, "");
        pattern = @$"\s+{Common.Line}\s+{Common.Line}";
        result = Util.Helpers.Regex.Replace(result, pattern, Common.Line);
        Util.Helpers.File.Write(filePath, result);
        _logger.LogTrace($"Modifica el archivo de migración y guarda con éxito, ruta: {filePath}");
    }
}