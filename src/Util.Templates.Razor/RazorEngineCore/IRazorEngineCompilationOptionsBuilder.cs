using System;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace RazorEngineCore; 

/// <summary>
/// Define un contrato para construir opciones de compilación para el motor Razor.
/// </summary>
public interface IRazorEngineCompilationOptionsBuilder
{
    /// <summary>
    /// Obtiene o establece las opciones de compilación para RazorEngine.
    /// </summary>
    /// <value>
    /// Un objeto <see cref="RazorEngineCompilationOptions"/> que contiene las opciones de compilación.
    /// </value>
    RazorEngineCompilationOptions Options { get; set; }
        
    /// <summary>
    /// Agrega una referencia a un ensamblado utilizando su nombre.
    /// </summary>
    /// <param name="assemblyName">El nombre del ensamblado que se desea agregar como referencia.</param>
    /// <remarks>
    /// Este método busca el ensamblado especificado por su nombre y lo agrega a la colección de referencias del proyecto.
    /// Si el ensamblado no se encuentra, se lanzará una excepción.
    /// </remarks>
    /// <exception cref="System.Exception">Se lanza si el ensamblado no se encuentra o no se puede agregar.</exception>
    void AddAssemblyReferenceByName(string assemblyName);
        
    /// <summary>
    /// Agrega una referencia a un ensamblado especificado.
    /// </summary>
    /// <param name="assembly">El ensamblado que se va a agregar como referencia.</param>
    /// <remarks>
    /// Este método permite incluir un ensamblado en el contexto actual, lo que puede ser útil para 
    /// acceder a tipos y miembros definidos en el ensamblado especificado.
    /// </remarks>
    /// <seealso cref="RemoveAssemblyReference(Assembly)"/>
    void AddAssemblyReference(Assembly assembly);
        
    /// <summary>
    /// Agrega una referencia de ensamblado basada en el tipo proporcionado.
    /// </summary>
    /// <param name="type">El tipo del cual se desea agregar la referencia de ensamblado.</param>
    /// <remarks>
    /// Este método permite incluir un ensamblado en el contexto actual, lo que puede ser útil 
    /// para la reflexión o para la carga dinámica de tipos.
    /// </remarks>
    /// <seealso cref="RemoveAssemblyReference(Type)"/>
    void AddAssemblyReference(Type type);

    /// <summary>
    /// Agrega una referencia de metadatos al contexto actual.
    /// </summary>
    /// <param name="reference">
    /// La referencia de metadatos que se va a agregar. Esta referencia debe ser válida y 
    /// puede provenir de un ensamblado, un archivo o una fuente de metadatos.
    /// </param>
    /// <remarks>
    /// Este método es útil para incluir referencias a tipos y miembros que se 
    /// utilizarán en la compilación o en la ejecución del código.
    /// </remarks>
    /// <seealso cref="MetadataReference"/>
    void AddMetadataReference(MetadataReference reference);
        
    /// <summary>
    /// Agrega una declaración de uso para el espacio de nombres especificado.
    /// </summary>
    /// <param name="namespaceName">El nombre del espacio de nombres que se desea agregar.</param>
    /// <remarks>
    /// Este método permite incluir un espacio de nombres en el contexto actual,
    /// facilitando el acceso a las clases y miembros definidos en dicho espacio.
    /// Asegúrese de que el espacio de nombres proporcionado sea válido y esté disponible
    /// en el contexto del proyecto.
    /// </remarks>
    /// <example>
    /// <code>
    /// AddUsing("System.Collections.Generic");
    /// </code>
    /// </example>
    void AddUsing(string namespaceName);
        
    /// <summary>
    /// Establece la relación de herencia para el tipo especificado.
    /// </summary>
    /// <param name="type">El tipo que se desea establecer como base para la herencia.</param>
    /// <remarks>
    /// Este método permite definir un tipo base del cual se heredarán las propiedades y métodos.
    /// Asegúrese de que el tipo proporcionado sea válido y que se pueda heredar.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="type"/> es null.</exception>
    /// <exception cref="ArgumentException">Se lanza si el tipo especificado no es un tipo válido para la herencia.</exception>
    void Inherits(Type type);
}