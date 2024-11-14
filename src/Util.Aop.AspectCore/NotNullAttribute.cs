namespace Util.Aop; 

/// <summary>
/// Atributo que se utiliza para validar que un parámetro no sea nulo.
/// Hereda de <see cref="ParameterInterceptorBase"/>.
/// </summary>
public class NotNullAttribute : ParameterInterceptorBase {
    /// <summary>
    /// Invoca el siguiente delegado en la cadena de ejecución, después de validar el valor del parámetro.
    /// </summary>
    /// <param name="context">El contexto del aspecto del parámetro que contiene información sobre el parámetro actual.</param>
    /// <param name="next">El delegado que representa el siguiente paso en la cadena de ejecución.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si el valor del parámetro es nulo.</exception>
    public override Task Invoke( ParameterAspectContext context, ParameterAspectDelegate next ) {
        if( context.Parameter.Value == null )
            throw new ArgumentNullException( context.Parameter.Name );
        return next( context );
    }
}