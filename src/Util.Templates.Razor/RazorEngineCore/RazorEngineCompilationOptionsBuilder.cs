using System;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace RazorEngineCore; 

/// <summary>
/// Clase que permite construir opciones de compilación para el motor Razor.
/// Implementa la interfaz <see cref="IRazorEngineCompilationOptionsBuilder"/>.
/// </summary>
public class RazorEngineCompilationOptionsBuilder : IRazorEngineCompilationOptionsBuilder
{
    /// <summary>
    /// Obtiene o establece las opciones de compilación para Razor Engine.
    /// </summary>
    /// <value>
    /// Un objeto <see cref="RazorEngineCompilationOptions"/> que contiene las opciones de compilación.
    /// </value>
    public RazorEngineCompilationOptions Options { get; set; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RazorEngineCompilationOptionsBuilder"/>.
    /// </summary>
    /// <param name="options">
    /// Opciones de compilación de Razor. Si se proporciona <c>null</c>, se inicializará con una nueva instancia de <see cref="RazorEngineCompilationOptions"/>.
    /// </param>
    public RazorEngineCompilationOptionsBuilder(RazorEngineCompilationOptions options = null)
    {
        this.Options = options ?? new RazorEngineCompilationOptions();
    }

    /// <summary>
    /// Agrega una referencia de ensamblado utilizando el nombre del ensamblado.
    /// </summary>
    /// <param name="assemblyName">El nombre del ensamblado que se desea agregar como referencia.</param>
    /// <remarks>
    /// Este método carga el ensamblado especificado por el nombre y lo agrega a la colección de referencias de ensamblados.
    /// Si el ensamblado no se encuentra, se lanzará una excepción.
    /// </remarks>
    /// <exception cref="System.IO.FileNotFoundException">
    /// Se lanza cuando el ensamblado especificado no se encuentra.
    /// </exception>
    public void AddAssemblyReferenceByName(string assemblyName)
    {
        Assembly assembly = Assembly.Load(new AssemblyName(assemblyName));
        this.AddAssemblyReference(assembly);
    }

    /// <summary>
    /// Agrega una referencia de ensamblado a las opciones del objeto.
    /// </summary>
    /// <param name="assembly">El ensamblado que se va a agregar como referencia.</param>
    /// <remarks>
    /// Este método permite incluir un ensamblado específico en la lista de referencias
    /// utilizadas por el objeto, lo que puede ser útil para la resolución de tipos y
    /// miembros durante la compilación o ejecución.
    /// </remarks>
    public void AddAssemblyReference(Assembly assembly)
    {
        this.Options.ReferencedAssemblies.Add(assembly);
    }

    /// <summary>
    /// Agrega una referencia de ensamblado para el tipo especificado.
    /// </summary>
    /// <param name="type">El tipo del cual se agregará la referencia de ensamblado.</param>
    /// <remarks>
    /// Este método también agrega referencias de ensamblado para los tipos genéricos 
    /// que son argumentos del tipo especificado.
    /// </remarks>
    /// <seealso cref="AddAssemblyReference(Assembly)"/>
    public void AddAssemblyReference(Type type)
    {
        this.AddAssemblyReference(type.Assembly);

        foreach (Type argumentType in type.GenericTypeArguments)
        {
            this.AddAssemblyReference(argumentType);
        }
    }

    /// <summary>
    /// Agrega una referencia de metadatos a las opciones de compilación.
    /// </summary>
    /// <param name="reference">La referencia de metadatos que se va a agregar.</param>
    /// <remarks>
    /// Este método permite incluir referencias adicionales que son necesarias para la compilación
    /// del código, facilitando el acceso a tipos y miembros definidos en las bibliotecas referenciadas.
    /// </remarks>
    public void AddMetadataReference(MetadataReference reference)
    {
        this.Options.MetadataReferences.Add(reference);
    }

    /// <summary>
    /// Agrega un espacio de nombres a la lista de usings predeterminados.
    /// </summary>
    /// <param name="namespaceName">El nombre del espacio de nombres que se va a agregar.</param>
    public void AddUsing(string namespaceName)
    {
        this.Options.DefaultUsings.Add(namespaceName);
    }

    /// <summary>
    /// Establece el tipo que se hereda en las opciones.
    /// </summary>
    /// <param name="type">El tipo que se desea heredar.</param>
    /// <remarks>
    /// Este método actualiza la propiedad <c>Inherits</c> de las opciones con el nombre del tipo proporcionado
    /// y agrega una referencia al ensamblado correspondiente.
    /// </remarks>
    public void Inherits(Type type)
    {
        this.Options.Inherits = this.RenderTypeName(type);
        this.AddAssemblyReference(type);
    }

    /// <summary>
    /// Renderiza el nombre completo de un tipo, incluyendo su espacio de nombres y los argumentos de tipo genérico si los tiene.
    /// </summary>
    /// <param name="type">El tipo del cual se desea obtener el nombre renderizado.</param>
    /// <returns>
    /// Un string que representa el nombre completo del tipo, incluyendo el espacio de nombres y los argumentos de tipo genérico.
    /// </returns>
    /// <remarks>
    /// Si el tipo es genérico, se incluirán los argumentos de tipo en la forma adecuada. 
    /// Si el tipo tiene un carácter '`', este será eliminado del nombre renderizado.
    /// </remarks>
    private string RenderTypeName(Type type)
    {
        string result = type.Namespace + "." + type.Name;

        if (result.Contains('`'))
        {
            result = result.Substring(0, result.IndexOf("`"));
        }

        if (type.GenericTypeArguments.Length == 0)
        {
            return result;
        }

        return result + "<" + string.Join(",", type.GenericTypeArguments.Select(this.RenderTypeName)) + ">";
    }
}