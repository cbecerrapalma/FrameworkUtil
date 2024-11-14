namespace Util.Ui.Expressions; 

/// <summary>
/// Define un contrato para resolver expresiones.
/// </summary>
public interface IExpressionResolver {
    /// <summary>
    /// Resuelve la información del modelo a partir de una expresión de modelo dada.
    /// </summary>
    /// <param name="expression">La expresión de modelo que se desea resolver.</param>
    /// <returns>Un objeto <see cref="ModelExpressionInfo"/> que contiene la información resuelta del modelo.</returns>
    /// <remarks>
    /// Este método toma una expresión de modelo y devuelve un objeto que encapsula
    /// la información relevante sobre el modelo, lo que puede incluir detalles como
    /// el tipo de modelo, el nombre del modelo y otros metadatos asociados.
    /// </remarks>
    ModelExpressionInfo Resolve( ModelExpression expression );
}