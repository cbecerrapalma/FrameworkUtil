using Util.Ui.Configs;

namespace Util.Ui.Expressions; 

/// <summary>
/// Clase base abstracta para la carga de expresiones.
/// </summary>
/// <remarks>
/// Esta clase proporciona una implementación base para los cargadores de expresiones,
/// permitiendo la creación de diferentes tipos de cargadores que heredan de esta clase.
/// </remarks>
public abstract class ExpressionLoaderBase : IExpressionLoader {
    /// <inheritdoc />
    /// <summary>
    /// Carga la configuración especificada en el objeto actual.
    /// </summary>
    /// <param name="config">El objeto de configuración que contiene los datos a cargar.</param>
    /// <param name="expressionPropertyName">El nombre de la propiedad de expresión que se utilizará para cargar la configuración. Por defecto es "for".</param>
    /// <remarks>
    /// Este método verifica si el objeto de configuración es nulo y si contiene la propiedad de expresión especificada antes de proceder a cargar los datos.
    /// Si la propiedad de expresión no se encuentra, el método no realiza ninguna acción.
    /// </remarks>
    /// <seealso cref="ResolveModelExpression(Config, string)"/>
    /// <seealso cref="Load(Config, ModelInfo)"/>
    public virtual void Load( Config config, string expressionPropertyName = "for" ) {
        if ( config == null )
            return;
        if ( config.Contains( expressionPropertyName ) == false )
            return;
        var info = ResolveModelExpression( config, expressionPropertyName );
        info.ExpressionPropertyName = expressionPropertyName;
        Load( config, info );
    }

    /// <summary>
    /// Resuelve una expresión de modelo a partir de la configuración y el nombre de la propiedad de expresión.
    /// </summary>
    /// <param name="config">La configuración que contiene la expresión a resolver.</param>
    /// <param name="expressionPropertyName">El nombre de la propiedad de expresión que se desea resolver.</param>
    /// <returns>
    /// Un objeto <see cref="ModelExpressionInfo"/> que representa la expresión de modelo resuelta.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una implementación personalizada.
    /// </remarks>
    protected virtual ModelExpressionInfo ResolveModelExpression(Config config, string expressionPropertyName) {
        var expression = config.GetValue<ModelExpression>(expressionPropertyName);
        var resolver = CreateExpressionResolver(config);
        return resolver.Resolve(expression);
    }

    /// <summary>
    /// Crea una instancia de un resolvedor de expresiones.
    /// </summary>
    /// <param name="config">La configuración utilizada para crear el resolvedor de expresiones.</param>
    /// <returns>Una instancia de <see cref="IExpressionResolver"/>.</returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una implementación personalizada
    /// del resolvedor de expresiones.
    /// </remarks>
    protected virtual IExpressionResolver CreateExpressionResolver(Config config) 
    {
        return new ExpressionResolver();
    }

    /// <summary>
    /// Carga la configuración en el modelo especificado.
    /// </summary>
    /// <param name="config">La configuración que se va a cargar.</param>
    /// <param name="info">La información del modelo que se está utilizando.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito por clases derivadas para proporcionar una implementación específica de la carga de configuración.
    /// </remarks>
    protected virtual void Load( Config config, ModelExpressionInfo info ) {
    }
}