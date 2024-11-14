namespace Util.Helpers;

/// <summary>
/// Clase que implementa un observador de archivos, permitiendo monitorear cambios en archivos y directorios.
/// </summary>
public class FileWatcher : IDisposable {
    private readonly FileSystemWatcher _watcher;
    private int _debounceInterval;
    private readonly FileWatcherEventFilter _fileWatcherEventFilter;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="FileWatcher"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor configura un <see cref="FileSystemWatcher"/> para observar cambios en archivos y directorios.
    /// Se establecen los filtros de notificación para detectar cambios en el nombre del directorio, 
    /// nombre del archivo, última escritura, creación, último acceso, atributos, tamaño y seguridad.
    /// Además, se inicializa el intervalo de debounce en 200 milisegundos y se crea una instancia de 
    /// <see cref="FileWatcherEventFilter"/> para filtrar eventos específicos.
    /// </remarks>
    public FileWatcher() {
        _watcher = new FileSystemWatcher();
        _watcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite
                                | NotifyFilters.CreationTime | NotifyFilters.LastAccess
                                | NotifyFilters.Attributes | NotifyFilters.Size | NotifyFilters.Security;
        _debounceInterval = 200;
        _fileWatcherEventFilter = new FileWatcherEventFilter();
    }

    /// <summary>
    /// Establece el intervalo de debounce para el observador de archivos.
    /// </summary>
    /// <param name="interval">El intervalo de debounce en milisegundos.</param>
    /// <returns>La instancia actual de <see cref="FileWatcher"/> para permitir la encadenación de métodos.</returns>
    public FileWatcher Debounce( int interval ) {
        _debounceInterval = interval;
        return this;
    }

    /// <summary>
    /// Establece la ruta a observar y si se deben incluir subdirectorios.
    /// </summary>
    /// <param name="path">La ruta del directorio que se desea observar.</param>
    /// <param name="includeSubdirectories">Indica si se deben incluir subdirectorios en la observación. Por defecto es <c>true</c>.</param>
    /// <returns>Devuelve la instancia actual de <see cref="FileWatcher"/>.</returns>
    public FileWatcher Path( string path, bool includeSubdirectories = true ) {
        _watcher.Path = path;
        _watcher.IncludeSubdirectories = includeSubdirectories;
        return this;
    }

    /// <summary>
    /// Establece el filtro de notificación para el observador de archivos.
    /// </summary>
    /// <param name="notifyFilters">Los filtros de notificación que se aplicarán al observador de archivos.</param>
    /// <returns>La instancia actual de <see cref="FileWatcher"/>.</returns>
    public FileWatcher NotifyFilter( NotifyFilters notifyFilters ) {
        _watcher.NotifyFilter = notifyFilters;
        return this;
    }

    /// <summary>
    /// Establece el filtro para el observador de archivos.
    /// </summary>
    /// <param name="filter">El patrón de filtro que se aplicará para observar cambios en los archivos.</param>
    /// <returns>Devuelve la instancia actual de <see cref="FileWatcher"/> para permitir la encadenación de métodos.</returns>
    public FileWatcher Filter( string filter ) {
        _watcher.Filter = filter;
        return this;
    }

    /// <summary>
    /// Registra una acción que se ejecutará cuando se cree un archivo en el sistema de archivos observado.
    /// </summary>
    /// <param name="action">La acción que se ejecutará, que recibe el origen del evento y los argumentos del evento de creación de archivo.</param>
    /// <returns>Devuelve la instancia actual de <see cref="FileWatcher"/> para permitir el encadenamiento de métodos.</returns>
    /// <remarks>
    /// Este método utiliza un filtro de eventos para determinar si el evento de creación de archivo es válido
    /// antes de ejecutar la acción proporcionada. El filtro se basa en la ruta completa del archivo y un intervalo
    /// de debounce configurado.
    /// </remarks>
    public FileWatcher OnCreated( Action<object, FileSystemEventArgs> action ) {
        _watcher.Created += ( source, e ) => {
            if ( _fileWatcherEventFilter.IsValid( e.FullPath, _debounceInterval ) )
                action( source, e );
        };
        return this;
    }

    /// <summary>
    /// Registra un manejador de eventos que se ejecuta de manera asíncrona cuando se crea un archivo.
    /// </summary>
    /// <param name="action">La acción asíncrona que se ejecutará cuando se detecte la creación de un archivo. 
    /// Recibe como parámetros el origen del evento y los argumentos del evento de sistema de archivos.</param>
    /// <returns>Devuelve la instancia actual de <see cref="FileWatcher"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método utiliza un filtro de eventos para determinar si el evento de creación de archivo es válido 
    /// según la ruta completa del archivo y un intervalo de debounce especificado.
    /// </remarks>
    public FileWatcher OnCreatedAsync( Func<object, FileSystemEventArgs, Task> action ) {
        _watcher.Created += async ( source, e ) => {
            if ( _fileWatcherEventFilter.IsValid( e.FullPath, _debounceInterval ) )
                await action( source, e );
        };
        return this;
    }

    /// <summary>
    /// Registra una acción que se ejecutará cuando se produzca un cambio en el sistema de archivos.
    /// </summary>
    /// <param name="action">La acción que se ejecutará cuando se detecte un cambio. Debe aceptar dos parámetros: el origen del evento y los argumentos del evento de sistema de archivos.</param>
    /// <returns>Devuelve la instancia actual de <see cref="FileWatcher"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método utiliza un filtro de eventos para determinar si el cambio detectado es válido antes de ejecutar la acción proporcionada.
    /// El filtro se basa en la ruta completa del archivo y un intervalo de debounce especificado.
    /// </remarks>
    public FileWatcher OnChanged( Action<object, FileSystemEventArgs> action ) {
        _watcher.Changed += ( source, e ) => {
            if ( _fileWatcherEventFilter.IsValid( e.FullPath, _debounceInterval ) )
                action( source, e );
        };
        return this;
    }

    /// <summary>
    /// Registra un controlador asíncrono que se ejecutará cuando se produzca un cambio en el sistema de archivos.
    /// </summary>
    /// <param name="action">La acción asíncrona a ejecutar cuando se detecte un cambio. Debe aceptar un objeto de origen y un <see cref="FileSystemEventArgs"/> como parámetros.</param>
    /// <returns>Devuelve la instancia actual de <see cref="FileWatcher"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método se utiliza para agregar un evento de cambio al observador de archivos. 
    /// La acción proporcionada se ejecutará solo si el evento cumple con los criterios de filtrado especificados.
    /// </remarks>
    public FileWatcher OnChangedAsync( Func<object, FileSystemEventArgs, Task> action ) {
        _watcher.Changed += async ( source, e ) => {
            if ( _fileWatcherEventFilter.IsValid( e.FullPath, _debounceInterval ) )
                await action( source, e );
        };
        return this;
    }

    /// <summary>
    /// Registra una acción que se ejecutará cuando un archivo sea eliminado.
    /// </summary>
    /// <param name="action">La acción que se ejecutará al detectar la eliminación de un archivo. 
    /// Recibe como parámetros el origen del evento y los argumentos del evento de sistema de archivos.</param>
    /// <returns>Devuelve la instancia actual de <see cref="FileWatcher"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método utiliza un filtro de eventos para determinar si la eliminación del archivo es válida 
    /// según el intervalo de debounce configurado. Solo se ejecutará la acción si el evento es considerado válido.
    /// </remarks>
    public FileWatcher OnDeleted( Action<object, FileSystemEventArgs> action ) {
        _watcher.Deleted += ( source, e ) => {
            if ( _fileWatcherEventFilter.IsValid( e.FullPath, _debounceInterval ) )
                action( source, e );
        };
        return this;
    }

    /// <summary>
    /// Registra un evento que se ejecutará de forma asíncrona cuando un archivo sea eliminado.
    /// </summary>
    /// <param name="action">La acción asíncrona que se ejecutará cuando se detecte la eliminación de un archivo.</param>
    /// <returns>La instancia actual de <see cref="FileWatcher"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método utiliza un filtro de eventos para determinar si la eliminación del archivo es válida
    /// según el intervalo de debounce configurado. Si el evento es válido, se ejecutará la acción proporcionada.
    /// </remarks>
    public FileWatcher OnDeletedAsync( Func<object, FileSystemEventArgs, Task> action ) {
        _watcher.Deleted += async ( source, e ) => {
            if ( _fileWatcherEventFilter.IsValid( e.FullPath, _debounceInterval ) )
                await action( source, e );
        };
        return this;
    }

    /// <summary>
    /// Suscribe a un evento que se activa cuando un archivo es renombrado.
    /// </summary>
    /// <param name="action">La acción que se ejecutará cuando se produzca el evento de renombrado.</param>
    /// <returns>Devuelve la instancia actual de <see cref="FileWatcher"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método añade un manejador al evento <c>Renamed</c> del <c>FileSystemWatcher</c>. 
    /// La acción proporcionada se ejecutará solo si el evento cumple con los criterios de filtrado 
    /// definidos en <c>_fileWatcherEventFilter</c> y el intervalo de debounce especificado.
    /// </remarks>
    /// <seealso cref="FileWatcher"/>
    public FileWatcher OnRenamed( Action<object, RenamedEventArgs> action ) {
        _watcher.Renamed += ( source, e ) => {
            if ( _fileWatcherEventFilter.IsValid( e.FullPath, _debounceInterval ) )
                action( source, e );
        };
        return this;
    }

    /// <summary>
    /// Registra un manejador de eventos asíncrono que se ejecuta cuando un archivo es renombrado.
    /// </summary>
    /// <param name="action">La acción asíncrona que se ejecutará cuando se produzca el evento de renombrado. 
    /// Recibe como parámetros el objeto fuente del evento y los argumentos del evento de renombrado.</param>
    /// <returns>Devuelve la instancia actual de <see cref="FileWatcher"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método se asegura de que el evento de renombrado solo se procese si pasa el filtro de validez
    /// definido en <see cref="_fileWatcherEventFilter"/> y respeta el intervalo de debounce especificado.
    /// </remarks>
    public FileWatcher OnRenamedAsync( Func<object, RenamedEventArgs, Task> action ) {
        _watcher.Renamed += async ( source, e ) => {
            if ( _fileWatcherEventFilter.IsValid( e.FullPath, _debounceInterval ) )
                await action( source, e );
        };
        return this;
    }

    /// <summary>
    /// Suscribe una acción que se ejecutará cuando ocurra un error en el FileWatcher.
    /// </summary>
    /// <param name="action">La acción que se ejecutará, recibiendo el origen del evento y los detalles del error.</param>
    /// <returns>Devuelve la instancia actual de FileWatcher para permitir la encadenación de llamadas.</returns>
    public FileWatcher OnError( Action<object, ErrorEventArgs> action ) {
        _watcher.Error += ( source, e ) => {
            action( source, e );
        };
        return this;
    }

    /// <summary>
    /// Registra un controlador de eventos para manejar errores que ocurren en el FileWatcher.
    /// </summary>
    /// <param name="action">Una función asincrónica que se ejecutará cuando ocurra un error. 
    /// Recibe como parámetros el origen del evento y los argumentos del error.</param>
    /// <returns>Devuelve la instancia actual de <see cref="FileWatcher"/> para permitir el encadenamiento de métodos.</returns>
    /// <remarks>
    /// Este método permite a los usuarios definir cómo manejar los errores que se producen durante la 
    /// vigilancia de archivos, proporcionando un mecanismo para la gestión de excepciones de manera 
    /// asincrónica.
    /// </remarks>
    /// <seealso cref="FileWatcher"/>
    public FileWatcher OnErrorAsync( Func<object, ErrorEventArgs, Task> action ) {
        _watcher.Error += async ( source, e ) => {
            await action( source, e );
        };
        return this;
    }

    /// <summary>
    /// Registra un controlador de eventos que se ejecutará cuando el objeto <see cref="_watcher"/> sea dispuesto.
    /// </summary>
    /// <param name="action">La acción que se ejecutará al dispararse el evento <see cref="Disposed"/>.</param>
    /// <returns>El objeto actual <see cref="FileWatcher"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método permite suscribirse al evento de disposición del <see cref="_watcher"/> 
    /// para realizar acciones específicas cuando el objeto se libera de recursos.
    /// </remarks>
    /// <seealso cref="FileWatcher"/>
    public FileWatcher OnDisposed( Action<object, EventArgs> action ) {
        _watcher.Disposed += ( source, e ) => {
            action( source, e );
        };
        return this;
    }

    /// <summary>
    /// Registra un controlador de eventos que se ejecutará de manera asíncrona cuando el objeto <see cref="_watcher"/> sea descartado.
    /// </summary>
    /// <param name="action">La acción asíncrona que se ejecutará al producirse el evento <see cref="Disposed"/>.</param>
    /// <returns>Devuelve la instancia actual de <see cref="FileWatcher"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método permite suscribirse al evento <see cref="Disposed"/> del objeto <see cref="_watcher"/> 
    /// y ejecutar una acción personalizada cuando se produzca dicho evento.
    /// </remarks>
    /// <seealso cref="FileWatcher"/>
    public FileWatcher OnDisposedAsync( Func<object, EventArgs, Task> action ) {
        _watcher.Disposed += async ( source, e ) => {
            await action( source, e );
        };
        return this;
    }

    /// <summary>
    /// Inicia el observador de archivos y habilita la generación de eventos.
    /// </summary>
    /// <returns>
    /// Devuelve la instancia actual de <see cref="FileWatcher"/>.
    /// </returns>
    public FileWatcher Start() {
        _watcher.EnableRaisingEvents = true;
        return this;
    }

    /// <summary>
    /// Detiene la vigilancia de cambios en el sistema de archivos.
    /// </summary>
    /// <returns>
    /// Devuelve la instancia actual de <see cref="FileWatcher"/> para permitir el encadenamiento de métodos.
    /// </returns>
    public FileWatcher Stop() {
        _watcher.EnableRaisingEvents = false;
        return this;
    }

    /// <summary>
    /// Libera los recursos utilizados por la instancia actual de la clase.
    /// </summary>
    /// <remarks>
    /// Este método se utiliza para liberar cualquier recurso no administrado que la clase pueda estar utilizando.
    /// Si el objeto <see cref="_watcher"/> está inicializado, se llama a su método <c>Dispose</c> para liberar los recursos asociados.
    /// </remarks>
    public void Dispose() {
        _watcher?.Dispose();
    }
}

/// <summary>
/// Clase que representa un filtro para eventos de un observador de archivos.
/// </summary>
/// <remarks>
/// Esta clase permite definir criterios específicos para filtrar eventos generados por un observador de archivos.
/// </remarks>
internal class FileWatcherEventFilter {
    private WatchFile _file;

    /// <summary>
    /// Verifica si la ruta del archivo es válida y si ha pasado el intervalo de debounce.
    /// </summary>
    /// <param name="path">La ruta del archivo que se va a validar.</param>
    /// <param name="debounceInterval">El intervalo de debounce en milisegundos.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la ruta del archivo es válida y ha pasado el intervalo de debounce; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método compara la ruta del archivo actual con la nueva ruta proporcionada. 
    /// Si son iguales y el tiempo transcurrido desde la última modificación es menor 
    /// que el intervalo de debounce especificado, el método devuelve <c>false</c>.
    /// En caso contrario, actualiza el archivo observado y devuelve <c>true</c>.
    /// </remarks>
    internal bool IsValid( string path, int debounceInterval ) {
        if ( _file != null && path == _file.Path && DateTime.Now - _file.Time < TimeSpan.FromMilliseconds( debounceInterval ) ) {
            _file = new WatchFile( path );
            return false;
        }
        _file = new WatchFile( path );
        return true;
    }
}

/// <summary>
/// Representa un archivo que se puede observar para detectar cambios.
/// </summary>
internal class WatchFile {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="WatchFile"/>.
    /// </summary>
    /// <param name="path">La ruta del archivo que se va a observar.</param>
    public WatchFile( string path ) {
        Path = path;
        Time = DateTime.Now;
    }

    /// <summary>
    /// Obtiene la ruta como una cadena de caracteres.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y devuelve la ruta actual.
    /// </remarks>
    /// <returns>Una cadena que representa la ruta.</returns>
    public string Path { get; }

    /// <summary>
    /// Obtiene la hora actual.
    /// </summary>
    /// <remarks>
    /// Esta propiedad devuelve un objeto <see cref="DateTime"/> que representa la hora actual.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="DateTime"/> que contiene la fecha y hora del sistema.
    /// </value>
    public DateTime Time { get; }
}