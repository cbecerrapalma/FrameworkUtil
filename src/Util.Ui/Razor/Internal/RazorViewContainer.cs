namespace Util.Ui.Razor.Internal;

/// <summary>
/// Representa un contenedor para vistas Razor.
/// </summary>
public class RazorViewContainer {
    private readonly Dictionary<string, RazorView> _views;
    private readonly IPartViewPathResolver _partViewPathResolver;
    private readonly IViewContentResolver _contentResolver;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RazorViewContainer"/>.
    /// </summary>
    /// <param name="partViewPathResolver">El resolvedor de rutas de vistas parciales.</param>
    /// <param name="contentResolver">El resolvedor de contenido de vistas. Si es <c>null</c>, se utilizará una nueva instancia de <see cref="ViewContentResolver"/>.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="partViewPathResolver"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Esta clase se utiliza para gestionar las vistas Razor y su contenido asociado.
    /// </remarks>
    public RazorViewContainer( IPartViewPathResolver partViewPathResolver, IViewContentResolver contentResolver = null ) {
        _partViewPathResolver = partViewPathResolver ?? throw new ArgumentNullException( nameof( partViewPathResolver ) );
        _contentResolver = contentResolver ?? new ViewContentResolver();
        _views = [];
    }

    /// <summary>
    /// Obtiene una lista de todas las vistas disponibles.
    /// </summary>
    /// <returns>
    /// Una lista que contiene todas las instancias de <see cref="RazorView"/>.
    /// </returns>
    public List<RazorView> GetAllViews() {
        return _views.Values.ToList();
    }

    /// <summary>
    /// Obtiene una lista de las vistas principales.
    /// </summary>
    /// <returns>
    /// Una lista de objetos <see cref="RazorView"/> que representan las vistas principales.
    /// </returns>
    /// <remarks>
    /// Este método filtra las vistas almacenadas en el diccionario interno <c>_views</c>
    /// y devuelve solo aquellas que son del tipo <c>MainView</c>.
    /// </remarks>
    public List<RazorView> GetMainViews() {
        return _views.Values.Where( view => view is MainView ).ToList();
    }

    /// <summary>
    /// Obtiene una lista de rutas de las vistas principales.
    /// </summary>
    /// <returns>
    /// Una lista de cadenas que representan las rutas de las vistas principales.
    /// </returns>
    public List<string> GetMainViewPaths() {
        return GetMainViews().Select( t => t.Path ).ToList();
    }

    /// <summary>
    /// Obtiene una lista de rutas aleatorias de vistas principales.
    /// </summary>
    /// <returns>
    /// Una lista que contiene hasta tres rutas aleatorias de vistas que son del tipo <see cref="MainView"/>.
    /// </returns>
    public List<string> GetRandomPaths() {
        return Util.Helpers.Random.GetValues( _views.Values.Where( view => view is MainView ).Select( t => t.Path ), 3 );
    }

    /// <summary>
    /// Busca una vista en la colección de vistas utilizando la ruta proporcionada.
    /// </summary>
    /// <param name="path">La ruta de la vista que se desea buscar.</param>
    /// <returns>
    /// Devuelve un objeto <see cref="RazorView"/> si se encuentra la vista correspondiente a la ruta especificada; de lo contrario, devuelve null.
    /// </returns>
    /// <remarks>
    /// Este método convierte la ruta a minúsculas y utiliza una representación segura de la cadena para evitar problemas con caracteres no válidos.
    /// Si la ruta está vacía, se devuelve null inmediatamente.
    /// </remarks>
    public RazorView FindView( string path ) {
        if ( path.IsEmpty() )
            return null;
        return _views.TryGetValue( path.SafeString().ToLower(), out var view ) ? view : null;
    }

    /// <summary>
    /// Agrega una vista al contenedor de vistas.
    /// </summary>
    /// <param name="view">La vista que se va a agregar. Si es <c>null</c>, no se realiza ninguna acción.</param>
    /// <remarks>
    /// Este método utiliza un diccionario para almacenar las vistas, donde la clave es la ruta de la vista en minúsculas.
    /// Si la vista ya existe en el diccionario, no se sobrescribirá.
    /// </remarks>
    public void AddView( RazorView view ) {
        if ( view == null )
            return;
        _views.TryAdd( view.Path.SafeString().ToLower(), view );
    }

    /// <summary>
    /// Inicializa el objeto con una lista de rutas.
    /// </summary>
    /// <param name="paths">Una lista de cadenas que representan las rutas a agregar.</param>
    /// <remarks>
    /// Este método verifica si la lista de rutas es nula. Si no lo es, itera a través de cada ruta
    /// y llama al método <see cref="AddView(string)"/> para agregarla.
    /// </remarks>
    public void Init( List<string> paths ) {
        if ( paths == null )
            return;
        foreach ( var path in paths )
            AddView( path );
    }

    /// <summary>
    /// Agrega una vista Razor a partir de la ruta especificada.
    /// </summary>
    /// <param name="path">La ruta de la vista que se desea agregar.</param>
    /// <returns>
    /// Retorna un objeto <see cref="RazorView"/> que representa la vista agregada,
    /// o <c>null</c> si la ruta está vacía o si el contenido de la vista es vacío.
    /// </returns>
    /// <remarks>
    /// Este método busca el contenido de la vista en la ruta proporcionada y, si se encuentra,
    /// crea o encuentra una vista Razor existente. Además, resuelve las rutas de las vistas parciales
    /// asociadas y las agrega a la vista principal.
    /// </remarks>
    /// <seealso cref="FindOrCreateView(string, string)"/>
    /// <seealso cref="_partViewPathResolver.Resolve(string, string)"/>
    /// <seealso cref="AddView(RazorView)"/>
    public RazorView AddView( string path ) {
        if ( path.IsEmpty() )
            return null;
        var content = GetContent( path );
        if ( content.IsEmpty() )
            return null;
        RazorView view = FindOrCreateView( path, content );
        var partViewPaths = _partViewPathResolver.Resolve( path, content ) ?? [];
        foreach ( var partViewPath in partViewPaths ) {
            var partView = AddView( partViewPath );
            if ( partView != null )
                view.AddPartView( (PartView)partView );
        }
        AddView( view );
        return view;
    }

    /// <summary>
    /// Obtiene el contenido a partir de una ruta relativa.
    /// </summary>
    /// <param name="relativePath">La ruta relativa del contenido que se desea obtener.</param>
    /// <returns>El contenido asociado a la ruta relativa especificada.</returns>
    protected string GetContent(string relativePath) 
    { 
        return _contentResolver.Resolve(relativePath); 
    }

    /// <summary>
    /// Busca una vista en la ruta especificada o crea una nueva vista si no se encuentra.
    /// </summary>
    /// <param name="path">La ruta de la vista que se desea encontrar o crear.</param>
    /// <param name="content">El contenido de la vista que se utilizará para determinar el tipo de vista a crear.</param>
    /// <returns>
    /// Una instancia de <see cref="RazorView"/> que representa la vista encontrada o creada.
    /// </returns>
    /// <remarks>
    /// Si la vista ya existe en la ruta especificada, se devuelve la vista existente.
    /// Si no existe, se determina si el contenido corresponde a una vista parcial o a una vista principal
    /// y se crea la instancia correspondiente.
    /// </remarks>
    protected RazorView FindOrCreateView(string path, string content) {
        var result = FindView(path);
        if (result != null)
            return result;
        return IsPartView(content) ? new PartView(path) : new MainView(path);
    }

    /// <summary>
    /// Determina si el contenido proporcionado es una vista parcial.
    /// </summary>
    /// <param name="content">El contenido que se evaluará para determinar si es una vista parcial.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el contenido no comienza con la cadena "@page"; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    protected bool IsPartView( string content ) {
        return content.SafeString().StartsWith( "@page" ) == false;
    }

    /// <summary>
    /// Obtiene las rutas de vista asociadas a un camino específico.
    /// </summary>
    /// <param name="path">El camino del cual se desean obtener las rutas de vista.</param>
    /// <returns>
    /// Una lista de cadenas que representan las rutas de vista. 
    /// Retorna una lista vacía si el camino está vacío o si no se encuentra la vista.
    /// </returns>
    public List<string> GetViewPaths( string path ) {
        if ( path.IsEmpty() )
            return new List<string>();
        var view = FindView( path );
        if ( view == null )
            return new List<string>();
        return view.GetMainViewPaths();
    }
}