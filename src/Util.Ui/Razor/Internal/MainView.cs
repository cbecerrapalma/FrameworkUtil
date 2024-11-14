namespace Util.Ui.Razor.Internal;

/// <summary>
/// Representa la vista principal en la aplicación.
/// Esta clase hereda de <see cref="RazorView"/> y se encarga de renderizar la interfaz de usuario.
/// </summary>
public class MainView : RazorView {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MainView"/>.
    /// </summary>
    /// <param name="path">La ruta que se utilizará para inicializar la vista.</param>
    public MainView( string path ) : base( path ) {
    }
}