using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.CodeAnalysis;

namespace RazorEngineCore; 

/// <summary>
/// Excepción que se lanza cuando hay un error en la compilación del motor Razor.
/// </summary>
/// <remarks>
/// Esta excepción hereda de <see cref="RazorEngineException"/> y se utiliza para indicar problemas específicos
/// relacionados con la compilación de plantillas Razor.
/// </remarks>
public class RazorEngineCompilationException : RazorEngineException
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RazorEngineCompilationException"/>.
    /// </summary>
    /// <remarks>
    /// Esta excepción se utiliza para indicar un error en la compilación de una plantilla Razor.
    /// </remarks>
    public RazorEngineCompilationException()
    {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RazorEngineCompilationException"/>.
    /// </summary>
    /// <param name="info">El objeto <see cref="SerializationInfo"/> que contiene la información de la serialización.</param>
    /// <param name="context">El contexto de streaming que contiene información sobre el origen y el destino de la serialización.</param>
    protected RazorEngineCompilationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RazorEngineCompilationException"/>.
    /// </summary>
    /// <param name="message">El mensaje que describe el error.</param>
    /// <returns>Una nueva instancia de <see cref="RazorEngineCompilationException"/> con el mensaje especificado.</returns>
    public RazorEngineCompilationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RazorEngineCompilationException"/>.
    /// </summary>
    /// <param name="message">El mensaje que describe el error.</param>
    /// <param name="innerException">La excepción interna que representa el error original.</param>
    public RazorEngineCompilationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Obtiene o establece la lista de errores de diagnóstico.
    /// </summary>
    /// <remarks>
    /// Esta propiedad almacena una colección de objetos <see cref="Diagnostic"/> 
    /// que representan los errores encontrados durante el procesamiento.
    /// </remarks>
    /// <value>
    /// Una lista de objetos <see cref="Diagnostic"/> que contiene los errores.
    /// </value>
    public List<Diagnostic> Errors { get; set; }
    /// <summary>
    /// Obtiene o establece el código generado.
    /// </summary>
    /// <value>
    /// Una cadena que representa el código generado.
    /// </value>
    public string GeneratedCode { get; set; }
}