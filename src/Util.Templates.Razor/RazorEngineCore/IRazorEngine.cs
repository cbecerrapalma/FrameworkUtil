using System;
using System.Threading.Tasks;

namespace RazorEngineCore; 

/// <summary>
/// Define la interfaz para un motor de plantillas Razor.
/// </summary>
public interface IRazorEngine
{
    /// <summary>
    /// Compila una plantilla Razor a partir del contenido proporcionado.
    /// </summary>
    /// <typeparam name="T">El tipo de la plantilla que implementa <see cref="IRazorEngineTemplate"/>.</typeparam>
    /// <param name="content">El contenido de la plantilla Razor que se desea compilar.</param>
    /// <param name="builderAction">Una acción opcional que permite configurar las opciones de compilación.</param>
    /// <returns>Una instancia de <typeparamref name="T"/> que representa la plantilla compilada.</returns>
    /// <remarks>
    /// Este método permite compilar dinámicamente contenido Razor en tiempo de ejecución.
    /// Se puede utilizar para generar contenido HTML o cualquier otro tipo de salida basado en la plantilla.
    /// </remarks>
    /// <seealso cref="IRazorEngineTemplate"/>
    IRazorEngineCompiledTemplate<T> Compile<T>(string content, Action<IRazorEngineCompilationOptionsBuilder> builderAction = null) 
        where T : IRazorEngineTemplate;
        
    /// <summary>
    /// Compila una plantilla Razor de forma asíncrona.
    /// </summary>
    /// <typeparam name="T">El tipo de la plantilla Razor que implementa <see cref="IRazorEngineTemplate"/>.</typeparam>
    /// <param name="content">El contenido de la plantilla Razor que se va a compilar.</param>
    /// <param name="builderAction">Una acción opcional que permite configurar las opciones de compilación.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que contiene la plantilla compilada.</returns>
    /// <remarks>
    /// Este método permite compilar una plantilla Razor en un hilo de trabajo separado, lo que mejora la 
    /// capacidad de respuesta de la aplicación al evitar bloqueos en el hilo principal.
    /// </remarks>
    /// <seealso cref="IRazorEngineTemplate"/>
    /// <seealso cref="IRazorEngineCompilationOptionsBuilder"/>
    Task<IRazorEngineCompiledTemplate<T>> CompileAsync<T>(string content, Action<IRazorEngineCompilationOptionsBuilder> builderAction = null) 
        where T : IRazorEngineTemplate;
        
    /// <summary>
    /// Compila una plantilla Razor a partir del contenido proporcionado.
    /// </summary>
    /// <param name="content">El contenido de la plantilla Razor que se desea compilar.</param>
    /// <param name="builderAction">Una acción opcional que permite configurar las opciones de compilación.</param>
    /// <returns>Una instancia de <see cref="IRazorEngineCompiledTemplate"/> que representa la plantilla compilada.</returns>
    /// <remarks>
    /// Este método permite la compilación dinámica de plantillas Razor, lo que facilita la generación de contenido
    /// basado en plantillas en tiempo de ejecución. Se puede utilizar para crear vistas en aplicaciones web,
    /// correos electrónicos o cualquier otro tipo de contenido que requiera una representación dinámica.
    /// </remarks>
    IRazorEngineCompiledTemplate Compile(string content, Action<IRazorEngineCompilationOptionsBuilder> builderAction = null);
        
    /// <summary>
    /// Compila una plantilla Razor de forma asíncrona utilizando el contenido proporcionado.
    /// </summary>
    /// <param name="content">El contenido de la plantilla Razor que se desea compilar.</param>
    /// <param name="builderAction">Una acción opcional que permite configurar las opciones de compilación.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado de la tarea es una instancia de <see cref="IRazorEngineCompiledTemplate"/> que representa la plantilla compilada.</returns>
    /// <remarks>
    /// Este método permite compilar plantillas Razor de manera eficiente y flexible, permitiendo la personalización de las opciones de compilación a través del parámetro <paramref name="builderAction"/>.
    /// </remarks>
    /// <seealso cref="IRazorEngineCompiledTemplate"/>
    Task<IRazorEngineCompiledTemplate> CompileAsync(string content, Action<IRazorEngineCompilationOptionsBuilder> builderAction = null);
}