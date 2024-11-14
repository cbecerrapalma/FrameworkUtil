using System;
using System.Runtime.Serialization;

namespace RazorEngineCore; 

/// <summary>
/// Excepción que se produce en el motor de Razor.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="Exception"/> y se utiliza para manejar errores específicos del motor de Razor.
/// </remarks>
public class RazorEngineException : Exception
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RazorEngineException"/>.
    /// </summary>
    /// <remarks>
    /// Esta clase representa una excepción específica que se produce en el contexto de la 
    /// ejecución de Razor Engine. Puede ser utilizada para manejar errores relacionados 
    /// con el procesamiento de plantillas Razor.
    /// </remarks>
    public RazorEngineException()
    {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RazorEngineException"/>.
    /// </summary>
    /// <param name="info">El objeto <see cref="SerializationInfo"/> que contiene la información de serialización sobre la excepción.</param>
    /// <param name="context">El objeto <see cref="StreamingContext"/> que contiene el contexto de la transmisión de la excepción.</param>
    protected RazorEngineException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RazorEngineException"/>.
    /// </summary>
    /// <param name="message">El mensaje que describe el error.</param>
    /// <returns>Una nueva instancia de <see cref="RazorEngineException"/>.</returns>
    public RazorEngineException(string message) : base(message)
    {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RazorEngineException"/>.
    /// </summary>
    /// <param name="message">El mensaje que describe el error.</param>
    /// <param name="innerException">La excepción interna que causó este error.</param>
    public RazorEngineException(string message, Exception innerException) : base(message, innerException)
    {
    }
}