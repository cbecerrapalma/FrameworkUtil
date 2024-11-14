using Microsoft.AspNetCore.Mvc.Rendering;
using Util.Ui.Angular.Extensions;
using Util.Ui.Configs;
using Util.Ui.Extensions;

namespace Util.Ui.Angular.Builders;

/// <summary>
/// Clase abstracta que representa un generador de etiquetas Angular.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="Util.Ui.Builders.TagBuilder"/> y proporciona funcionalidades específicas
/// para construir etiquetas utilizadas en aplicaciones Angular.
/// </remarks>
public abstract class AngularTagBuilder : Util.Ui.Builders.TagBuilder {
    private readonly Config _config;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="AngularTagBuilder"/>.
    /// </summary>
    /// <param name="config">La configuración que se utilizará para construir el tag.</param>
    /// <param name="tagName">El nombre del tag que se va a construir.</param>
    /// <param name="renderMode">El modo de renderizado del tag. Por defecto es <see cref="TagRenderMode.Normal"/>.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="config"/> es <c>null</c>.</exception>
    protected AngularTagBuilder( Config config, string tagName, TagRenderMode renderMode = TagRenderMode.Normal ) : base( tagName, renderMode ) {
        _config = config ?? throw new ArgumentNullException( nameof( config ) );
    }

    /// <inheritdoc />
    /// <summary>
    /// Configura la instancia actual utilizando la configuración proporcionada.
    /// </summary>
    /// <remarks>
    /// Este método sobrescribe la implementación base para aplicar configuraciones específicas
    /// de la clase actual. Se llama a los métodos de configuración base y de contenido para
    /// asegurar que todas las configuraciones necesarias se apliquen correctamente.
    /// </remarks>
    public override void Config() {
        ConfigBase( _config );
        ConfigContent( _config );
    }

    /// <summary>
    /// Configura la base del objeto utilizando la configuración proporcionada.
    /// </summary>
    /// <param name="config">La configuración que se aplicará al objeto.</param>
    /// <remarks>
    /// Este método sobrescribe la implementación base para aplicar configuraciones adicionales
    /// específicas del objeto, incluyendo la configuración angular, el ID de configuración
    /// y el menú contextual.
    /// </remarks>
    public override void ConfigBase( Config config ) {
        base.ConfigBase( config );
        this.Angular( config );
        ConfigId( config );
        ConfigOnContextmenu( config );
    }

    /// <summary>
    /// Configura el identificador utilizando la configuración proporcionada.
    /// </summary>
    /// <param name="config">La configuración que se utilizará para establecer el identificador.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito por clases derivadas para proporcionar una implementación específica.
    /// Se llama a los métodos <see cref="RawId(Config)"/> y <see cref="Id(Config)"/> para realizar la configuración.
    /// </remarks>
    protected virtual void ConfigId( Config config ) {
        this.RawId( config );
        this.Id( config );
    }

    /// <summary>
    /// Configura el comportamiento del menú contextual utilizando la configuración proporcionada.
    /// </summary>
    /// <param name="config">La configuración que se utilizará para establecer el comportamiento del menú contextual.</param>
    /// <remarks>
    /// Este método es virtual, lo que permite que las clases derivadas lo sobreescriban para proporcionar una implementación específica.
    /// </remarks>
    protected virtual void ConfigOnContextmenu(Config config) 
    {
        AttributeIfNotEmpty("(contextmenu)", _config.GetValue(UiConst.OnContextmenu));
    }

    /// <summary>
    /// Configura el contenido del objeto actual utilizando la configuración proporcionada.
    /// </summary>
    /// <param name="config">La configuración que se utilizará para configurar el contenido.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar
    /// una implementación personalizada de la configuración del contenido.
    /// </remarks>
    protected virtual void ConfigContent( Config config ) {
        config.Content.AppendTo( this );
    }
}