using Util.Ui.Configs;

namespace Util.Ui.Expressions;

/// <summary>
/// Clase que se encarga de cargar expresiones desde una fuente específica.
/// Hereda de <see cref="ExpressionLoaderBase"/>.
/// </summary>
public class ExpressionLoader : ExpressionLoaderBase {
    /// <summary>
    /// Carga la configuración del modelo utilizando la información proporcionada.
    /// </summary>
    /// <param name="config">La configuración que se va a cargar.</param>
    /// <param name="info">La información del modelo que se utilizará durante la carga.</param>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar una implementación específica
    /// que carga el nombre, el nombre para mostrar y la información de requerimiento
    /// del modelo a partir de la configuración proporcionada.
    /// </remarks>
    protected override void Load(Config config, ModelExpressionInfo info) 
    {
        LoadName(config, info);
        LoadDisplayName(config, info);
        LoadRequired(config, info);
    }

    /// <summary>
    /// Carga el nombre de la propiedad en la configuración especificada.
    /// </summary>
    /// <param name="config">La configuración en la que se establecerá el nombre.</param>
    /// <param name="info">Información sobre la expresión del modelo que contiene el nombre de la propiedad.</param>
    protected virtual void LoadName(Config config, ModelExpressionInfo info) 
    { 
        config.SetAttribute(UiConst.Name, info.PropertyName, false); 
    }

    /// <summary>
    /// Carga el nombre para mostrar en la configuración especificada.
    /// </summary>
    /// <param name="config">La configuración donde se establecerá el nombre para mostrar.</param>
    /// <param name="info">La información del modelo que contiene el nombre para mostrar.</param>
    protected virtual void LoadDisplayName(Config config, ModelExpressionInfo info)
    {
        config.SetAttribute(UiConst.Title, info.DisplayName, false);
    }

    /// <summary>
    /// Carga los atributos requeridos en la configuración si el modelo es obligatorio.
    /// </summary>
    /// <param name="config">La configuración a la que se le agregarán los atributos.</param>
    /// <param name="info">La información del modelo que contiene el estado de requerimiento y el mensaje asociado.</param>
    /// <remarks>
    /// Este método verifica si el modelo es requerido. Si no lo es, no realiza ninguna acción.
    /// Si el modelo es requerido, establece el atributo 'Required' en 'true' y 
    /// el mensaje de requerimiento en la configuración proporcionada.
    /// </remarks>
    protected virtual void LoadRequired(Config config, ModelExpressionInfo info) {
        if (info.IsRequired == false)
            return;
        config.SetAttribute(UiConst.Required, "true", false);
        config.SetAttribute(UiConst.RequiredMessage, info.RequiredMessage, false);
    }
}