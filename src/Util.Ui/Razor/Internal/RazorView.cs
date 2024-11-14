namespace Util.Ui.Razor.Internal;

/// <summary>
/// Representa una vista Razor que se puede renderizar en una aplicación web.
/// </summary>
public class RazorView {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RazorView"/>.
    /// </summary>
    /// <param name="path">La ruta del archivo de vista.</param>
    /// <exception cref="ArgumentNullException">Se lanza cuando <paramref name="path"/> está vacío.</exception>
    protected RazorView(string path)
    {
        if (path.IsEmpty())
            throw new ArgumentNullException(nameof(path));
        Path = path;
        PartViews = [];
    }

    /// <summary>
    /// Obtiene la ruta como una cadena de texto.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y devuelve la ruta actual.
    /// </remarks>
    /// <returns>
    /// Una cadena que representa la ruta.
    /// </returns>
    public string Path { get; }

    /// <summary>
    /// Obtiene o establece la lista de vistas de partes.
    /// </summary>
    /// <remarks>
    /// Esta propiedad almacena una colección de objetos <see cref="PartView"/> 
    /// que representan las diferentes vistas de partes en el sistema.
    /// </remarks>
    /// <value>
    /// Una lista de <see cref="PartView"/> que contiene las vistas de partes.
    /// </value>
    public List<PartView> PartViews { get; set; }

    /// <summary>
    /// Agrega una vista de parte al conjunto de vistas de parte.
    /// </summary>
    /// <param name="partView">La vista de parte que se va a agregar. No puede ser nula.</param>
    /// <remarks>
    /// Si <paramref name="partView"/> es nulo o si ya existe una vista de parte con la misma ruta,
    /// la operación no se llevará a cabo.
    /// </remarks>
    /// <seealso cref="PartView"/>
    public virtual void AddPartView( PartView partView ) {
        if ( partView == null )
            return;
        if ( PartViews.Any( t => t.Path == partView.Path ) )
            return;
        partView.AddMainView( this );
        PartViews.Add( partView );
    }

    /// <summary>
    /// Obtiene una lista de rutas principales de vista.
    /// </summary>
    /// <returns>
    /// Una lista de cadenas que representan las rutas principales de vista.
    /// </returns>
    public virtual List<string> GetMainViewPaths() {
        return [Path];
    }
}