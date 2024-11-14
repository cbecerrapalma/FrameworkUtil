namespace Util.Aop; 

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="System.Exception"/>.
/// </summary>
/// <remarks>
/// Esta clase está diseñada para agregar funcionalidad adicional a las excepciones en C#,
/// permitiendo un manejo más flexible y útil de los errores.
/// </remarks>
public static class ExceptionExtensions {
    /// <summary>
    /// Obtiene la excepción original de una excepción proporcionada, 
    /// recorriendo las excepciones internas si es necesario.
    /// </summary>
    /// <param name="exception">La excepción de la cual se desea obtener la excepción original.</param>
    /// <returns>
    /// La excepción original si se encuentra, de lo contrario, 
    /// devuelve <c>null</c> si la excepción proporcionada es <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método es útil para extraer la causa raíz de una excepción que puede haber sido 
    /// envuelta en múltiples capas de excepciones, especialmente en el contexto de 
    /// AspectCore y sus excepciones de invocación.
    /// </remarks>
    /// <seealso cref="AspectCore.DynamicProxy.AspectInvocationException"/>
    public static Exception GetRawException( this Exception exception ) {
        if( exception == null )
            return null;
        if( exception is AspectCore.DynamicProxy.AspectInvocationException aspectInvocationException ) {
            if( aspectInvocationException.InnerException == null )
                return aspectInvocationException;
            return GetRawException( aspectInvocationException.InnerException );
        }
        return exception;
    }
}