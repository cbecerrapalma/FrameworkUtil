namespace Util.Ui.Razor; 

/// <summary>
/// Representa las opciones de configuración para el motor de vistas Razor.
/// </summary>
public class RazorOptions {
    /// <summary>
    /// Obtiene o establece el directorio raíz de Razor.
    /// </summary>
    /// <remarks>
    /// Este directorio se utiliza como base para las rutas de los archivos Razor en la aplicación.
    /// El valor predeterminado es "/ClientApp".
    /// </remarks>
    /// <value>
    /// Un string que representa la ruta del directorio raíz de Razor.
    /// </value>
    public string RazorRootDirectory { get; set; } = "/ClientApp";
    /// <summary>
    /// Obtiene o establece la ruta raíz del cliente.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se inicializa con el valor predeterminado "ClientApp".
    /// </remarks>
    /// <value>
    /// Una cadena que representa la ruta raíz del cliente.
    /// </value>
    public string RootPath { get; set; } = "ClientApp";
    /// <summary>
    /// Obtiene o establece un valor que indica si se debe generar HTML.
    /// </summary>
    /// <value>
    /// <c>true</c> si se debe generar HTML; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsGenerateHtml { get; set; }
    /// <summary>
    /// Obtiene o establece el nombre de la carpeta donde se generará el HTML.
    /// </summary>
    /// <remarks>
    /// El valor predeterminado de esta propiedad es "html".
    /// </remarks>
    /// <returns>
    /// Un string que representa el nombre de la carpeta para la generación de HTML.
    /// </returns>
    public string GenerateHtmlFolder { get; set; } = "html";
    /// <summary>
    /// Obtiene o establece un valor que indica si la funcionalidad de WatchRazor está habilitada.
    /// </summary>
    /// <remarks>
    /// Cuando esta propiedad está establecida en <c>true</c>, se habilita la funcionalidad de WatchRazor,
    /// lo que permite la recarga automática de las vistas Razor durante el desarrollo.
    /// </remarks>
    /// <value>
    /// <c>true</c> si la funcionalidad de WatchRazor está habilitada; de lo contrario, <c>false</c>.
    /// </value>
    public bool EnableWatchRazor { get; set; } = true;
    /// <summary>
    /// Obtiene o establece el retraso inicial en milisegundos antes de que comience la operación.
    /// </summary>
    /// <remarks>
    /// El valor predeterminado es 1000 milisegundos (1 segundo).
    /// </remarks>
    /// <value>
    /// Un entero que representa el tiempo de retraso inicial en milisegundos.
    /// </value>
    public int StartInitDelay { get; set; } = 1000;
    /// <summary>
    /// Obtiene o establece el retraso en milisegundos para renderizar el HTML 
    /// cuando se detecta un cambio en Razor.
    /// </summary>
    /// <remarks>
    /// Este valor determina cuánto tiempo se espera antes de que se inicie el 
    /// proceso de renderizado después de que se haya realizado un cambio. 
    /// Un valor más alto puede ayudar a evitar renderizados innecesarios 
    /// si se realizan múltiples cambios en un corto período de tiempo.
    /// </remarks>
    /// <value>
    /// Un entero que representa el retraso en milisegundos. El valor predeterminado es 300.
    /// </value>
    public int HtmlRenderDelayOnRazorChange { get; set; } = 300;    
    /// <summary>
    /// Obtiene o establece un valor que indica si el precalentamiento está habilitado.
    /// </summary>
    /// <remarks>
    /// El valor predeterminado es <c>true</c>, lo que significa que el precalentamiento está habilitado al iniciar.
    /// </remarks>
    /// <value>
    /// <c>true</c> si el precalentamiento está habilitado; de lo contrario, <c>false</c>.
    /// </value>
    public bool EnablePreheat { get; set; } = true;
    /// <summary>
    /// Obtiene o establece un valor que indica si se permite la sobreescritura de HTML.
    /// </summary>
    /// <remarks>
    /// Este valor se utiliza para habilitar o deshabilitar la capacidad de modificar el HTML generado.
    /// Por defecto, está establecido en <c>true</c>.
    /// </remarks>
    /// <value>
    /// <c>true</c> si se permite la sobreescritura de HTML; de lo contrario, <c>false</c>.
    /// </value>
    public bool EnableOverrideHtml { get; set; } = true;
    /// <summary>
    /// Obtiene o establece un valor que indica si se debe generar todo el HTML.
    /// </summary>
    /// <value>
    /// <c>true</c> si se debe generar todo el HTML; de lo contrario, <c>false</c>.
    /// </value>
    public bool EnableGenerateAllHtml { get; set; }
    /// <summary>
    /// Obtiene o establece el tiempo de espera para el inicio.
    /// </summary>
    /// <remarks>
    /// Este valor determina cuánto tiempo se esperará antes de considerar que el inicio ha fallado.
    /// El valor predeterminado es 300 segundos.
    /// </remarks>
    /// <returns>
    /// Un objeto <see cref="TimeSpan"/> que representa el tiempo de espera para el inicio.
    /// </returns>
    public TimeSpan StartupTimeout { get; set; } = TimeSpan.FromSeconds( 300 );
}