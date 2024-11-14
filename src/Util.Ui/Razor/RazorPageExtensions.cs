namespace Util.Ui.Razor; 

/// <summary>
/// Proporciona métodos de extensión para las páginas Razor.
/// </summary>
public static class RazorPageExtensions {
    /// <summary>
    /// Extiende la funcionalidad de <see cref="IMvcBuilder"/> para agregar convenciones de Razor Pages.
    /// </summary>
    /// <param name="builder">El constructor de MVC al que se le agregarán las convenciones.</param>
    /// <returns>El mismo <see cref="IMvcBuilder"/> con las convenciones añadidas.</returns>
    /// <remarks>
    /// Este método permite registrar convenciones personalizadas para las páginas Razor,
    /// incluyendo la adición de un <see cref="PageRouteConvention"/> y un filtro de generación de HTML
    /// para todas las páginas en la raíz de la aplicación.
    /// </remarks>
    /// <seealso cref="IMvcBuilder"/>
    /// <seealso cref="PageRouteConvention"/>
    /// <seealso cref="GenerateHtmlFilter"/>
    public static IMvcBuilder AddConventions( this IMvcBuilder builder ) {
        return builder.AddRazorPagesOptions( options => {
            options.Conventions.Add( new PageRouteConvention() );
            options.Conventions.AddFolderApplicationModelConvention( "/", model => model.Filters.Add( new GenerateHtmlFilter() ) );
        } );
    }
}