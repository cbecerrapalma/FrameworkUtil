namespace Util.Ui.Razor.Internal;

/// <summary>
/// Representa una vista parcial en el contexto de Razor.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="RazorView"/> y se utiliza para renderizar
/// vistas parciales en una aplicación ASP.NET.
/// </remarks>
public class PartView : RazorView {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PartView"/>.
    /// </summary>
    /// <param name="path">La ruta que se utilizará para inicializar la vista principal.</param>
    public PartView( string path ) : base( path ) {
        MainViews = [];
    }

    /// <summary>
    /// Obtiene o establece la lista de vistas Razor principales.
    /// </summary>
    /// <remarks>
    /// Esta propiedad almacena una colección de objetos <see cref="RazorView"/> 
    /// que representan las vistas Razor que se utilizan en la aplicación.
    /// </remarks>
    /// <value>
    /// Una lista de <see cref="RazorView"/> que contiene las vistas principales.
    /// </value>
    public List<RazorView> MainViews { get; set; }

    /// <summary>
    /// Agrega una vista principal a la colección de vistas principales.
    /// </summary>
    /// <param name="mainView">La vista principal que se desea agregar. Si es nula, no se realizará ninguna acción.</param>
    /// <remarks>
    /// Este método verifica si la vista principal ya existe en la colección antes de agregarla.
    /// Si la vista ya está presente, no se agregará nuevamente.
    /// </remarks>
    public void AddMainView( RazorView mainView ) {
        if ( mainView == null )
            return;
        if ( MainViews.Any( t => t.Path == mainView.Path ) )
            return;
        MainViews.Add( mainView );
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una lista de rutas de vistas principales.
    /// </summary>
    /// <returns>
    /// Una lista de cadenas que representan las rutas de las vistas principales.
    /// </returns>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar una implementación que
    /// recopila rutas de vistas de todos los elementos en la colección <c>MainViews</c>.
    /// </remarks>
    public override List<string> GetMainViewPaths() {
        var result = new List<string>();
        foreach (var view in MainViews )
            result.AddRange( view.GetMainViewPaths() );
        return result;
    }
}