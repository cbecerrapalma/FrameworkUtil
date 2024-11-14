using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;
using System;

namespace RazorEngineCore; 

/// <summary>
/// Representa las opciones de compilación para el motor Razor.
/// </summary>
public class RazorEngineCompilationOptions
{
    /// <summary>
    /// Obtiene o establece un conjunto de ensamblados referenciados.
    /// </summary>
    /// <remarks>
    /// Esta propiedad almacena un conjunto de objetos <see cref="Assembly"/> que representan los ensamblados
    /// que han sido referenciados por el componente o módulo en cuestión.
    /// </remarks>
    /// <value>
    /// Un <see cref="HashSet{Assembly}"/> que contiene los ensamblados referenciados.
    /// </value>
    public HashSet<Assembly> ReferencedAssemblies { get; set; }

    /// <summary>
    /// Obtiene o establece un conjunto de referencias de metadatos.
    /// </summary>
    /// <value>
    /// Un <see cref="HashSet{MetadataReference}"/> que contiene las referencias de metadatos.
    /// </value>
    public HashSet<MetadataReference> MetadataReferences { get; set; } = new HashSet<MetadataReference>();
    /// <summary>
    /// Obtiene o establece el espacio de nombres del template.
    /// </summary>
    /// <remarks>
    /// El valor predeterminado es "TemplateNamespace".
    /// </remarks>
    public string TemplateNamespace { get; set; } = "TemplateNamespace";
    /// <summary>
    /// Representa la clase base de la plantilla Razor que se utilizará para la herencia.
    /// </summary>
    /// <remarks>
    /// Este campo se inicializa con el valor por defecto "RazorEngineCore.RazorEngineTemplateBase".
    /// </remarks>
    /// <value>
    /// Una cadena que contiene el nombre de la clase base de la plantilla Razor.
    /// </value>
    public string Inherits { get; set; } = "RazorEngineCore.RazorEngineTemplateBase";

    /// <summary>
    /// Obtiene o establece el conjunto de espacios de nombres predeterminados utilizados en el proyecto.
    /// </summary>
    /// <value>
    /// Un <see cref="HashSet{T}"/> que contiene los espacios de nombres predeterminados.
    /// </value>
    /// <remarks>
    /// Este conjunto incluye espacios de nombres comunes como "System.Linq", "System.Collections" y "System.Collections.Generic".
    /// </remarks>
    public HashSet<string> DefaultUsings { get; set; } = new HashSet<string>()
    {
        "System.Linq",
        "System.Collections",
        "System.Collections.Generic"
    };

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RazorEngineCompilationOptions"/>.
    /// Configura las ensamblajes referenciados según el sistema operativo y el marco de trabajo en uso.
    /// </summary>
    /// <remarks>
    /// Este constructor determina si el sistema operativo es Windows y si se está utilizando el .NET Framework o .NET Core.
    /// Basado en esta información, se cargan los ensamblajes necesarios para la compilación de Razor.
    /// </remarks>
    public RazorEngineCompilationOptions()
    {
        bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        bool isFullFramework = RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework", StringComparison.OrdinalIgnoreCase);

        if (isWindows && isFullFramework)
        {
            this.ReferencedAssemblies = new HashSet<Assembly>()
            {
                typeof(object).Assembly,
                Assembly.Load(new AssemblyName("Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")),
                typeof(RazorEngineTemplateBase).Assembly,
                typeof(System.Runtime.GCSettings).Assembly,
                typeof(System.Collections.IList).Assembly,
                typeof(System.Collections.Generic.IEnumerable<>).Assembly,
                typeof(System.Linq.Enumerable).Assembly,
                typeof(System.Linq.Expressions.Expression).Assembly,
                Assembly.Load(new AssemblyName("netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51"))
            };
        }

        if (isWindows && !isFullFramework) // i.e. NETCore
        {
            this.ReferencedAssemblies = new HashSet<Assembly>()
            {
                typeof(object).Assembly,
                Assembly.Load(new AssemblyName("Microsoft.CSharp")),
                typeof(RazorEngineTemplateBase).Assembly,
                Assembly.Load(new AssemblyName("System.Runtime")),
                typeof(System.Collections.IList).Assembly,
                typeof(System.Collections.Generic.IEnumerable<>).Assembly,
                Assembly.Load(new AssemblyName("System.Linq")),
                Assembly.Load(new AssemblyName("System.Linq.Expressions")),
                Assembly.Load(new AssemblyName("netstandard"))
            };
        }

        if (!isWindows)
        {
            this.ReferencedAssemblies = new HashSet<Assembly>()
            {
                typeof(object).Assembly,
                Assembly.Load(new AssemblyName("Microsoft.CSharp")),
                typeof(RazorEngineTemplateBase).Assembly,
                Assembly.Load(new AssemblyName("System.Runtime")),
                typeof(System.Collections.IList).Assembly,
                typeof(System.Collections.Generic.IEnumerable<>).Assembly,
                Assembly.Load(new AssemblyName("System.Linq")),
                Assembly.Load(new AssemblyName("System.Linq.Expressions")),
                Assembly.Load(new AssemblyName("netstandard"))
            };
        }
    }
}