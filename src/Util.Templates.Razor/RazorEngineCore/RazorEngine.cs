using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection.Metadata;

namespace RazorEngineCore;

/// <summary>
/// Representa una implementación del motor Razor para la generación de contenido dinámico.
/// </summary>
/// <remarks>
/// Esta clase se encarga de procesar plantillas Razor y generar el contenido HTML correspondiente.
/// </remarks>
public class RazorEngine : IRazorEngine
{
    /// <summary>
    /// Compila una plantilla Razor a partir del contenido proporcionado.
    /// </summary>
    /// <typeparam name="T">El tipo de la plantilla que implementa <see cref="IRazorEngineTemplate"/>.</typeparam>
    /// <param name="content">El contenido de la plantilla Razor que se desea compilar.</param>
    /// <param name="builderAction">Una acción opcional que permite configurar las opciones de compilación.</param>
    /// <returns>
    /// Un objeto que implementa <see cref="IRazorEngineCompiledTemplate{T}"/> que representa la plantilla compilada.
    /// </returns>
    /// <remarks>
    /// Este método permite personalizar la compilación de la plantilla mediante el uso de un 
    /// <paramref name="builderAction"/> que puede modificar las opciones de compilación antes de 
    /// que se realice la compilación.
    /// </remarks>
    /// <seealso cref="IRazorEngineTemplate"/>
    /// <seealso cref="IRazorEngineCompiledTemplate{T}"/>
    public IRazorEngineCompiledTemplate<T> Compile<T>(string content, Action<IRazorEngineCompilationOptionsBuilder> builderAction = null) where T : IRazorEngineTemplate
    {
        IRazorEngineCompilationOptionsBuilder compilationOptionsBuilder = new RazorEngineCompilationOptionsBuilder();

        compilationOptionsBuilder.AddAssemblyReference(typeof(T).Assembly);
        compilationOptionsBuilder.Inherits(typeof(T));

        builderAction?.Invoke(compilationOptionsBuilder);

        MemoryStream memoryStream = this.CreateAndCompileToStream(content, compilationOptionsBuilder.Options);

        return new RazorEngineCompiledTemplate<T>(memoryStream);
    }

    /// <summary>
    /// Compila una plantilla Razor de forma asíncrona.
    /// </summary>
    /// <typeparam name="T">El tipo de la plantilla que implementa <see cref="IRazorEngineTemplate"/>.</typeparam>
    /// <param name="content">El contenido de la plantilla Razor que se va a compilar.</param>
    /// <param name="builderAction">Una acción opcional que permite configurar las opciones de compilación.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de compilación, que contiene el resultado de tipo <see cref="IRazorEngineCompiledTemplate{T}"/>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza <see cref="Task.Factory.StartNew"/> para ejecutar la compilación en un hilo separado.
    /// </remarks>
    /// <seealso cref="IRazorEngineTemplate"/>
    /// <seealso cref="IRazorEngineCompiledTemplate{T}"/>
    public Task<IRazorEngineCompiledTemplate<T>> CompileAsync<T>(string content, Action<IRazorEngineCompilationOptionsBuilder> builderAction = null) where T : IRazorEngineTemplate
    {
        return Task.Factory.StartNew(() => this.Compile<T>(content: content, builderAction: builderAction));
    }

    /// <summary>
    /// Compila una plantilla Razor a partir del contenido proporcionado.
    /// </summary>
    /// <param name="content">El contenido de la plantilla Razor que se va a compilar.</param>
    /// <param name="builderAction">Una acción opcional que permite configurar las opciones de compilación.</param>
    /// <returns>Una instancia de <see cref="IRazorEngineCompiledTemplate"/> que representa la plantilla compilada.</returns>
    /// <remarks>
    /// Este método utiliza un constructor de opciones de compilación para establecer la clase base de la plantilla.
    /// Si se proporciona <paramref name="builderAction"/>, se invocará para permitir configuraciones adicionales.
    /// </remarks>
    public IRazorEngineCompiledTemplate Compile(string content, Action<IRazorEngineCompilationOptionsBuilder> builderAction = null)
    {
        IRazorEngineCompilationOptionsBuilder compilationOptionsBuilder = new RazorEngineCompilationOptionsBuilder();
        compilationOptionsBuilder.Inherits(typeof(RazorEngineTemplateBase));

        builderAction?.Invoke(compilationOptionsBuilder);

        MemoryStream memoryStream = this.CreateAndCompileToStream(content, compilationOptionsBuilder.Options);

        return new RazorEngineCompiledTemplate(memoryStream);
    }

    /// <summary>
    /// Compila el contenido proporcionado de una plantilla Razor de forma asíncrona.
    /// </summary>
    /// <param name="content">El contenido de la plantilla Razor que se desea compilar.</param>
    /// <param name="builderAction">Una acción opcional que permite configurar las opciones de compilación.</param>
    /// <returns>Una tarea que representa la operación de compilación asíncrona, que contiene el resultado de tipo <see cref="IRazorEngineCompiledTemplate"/>.</returns>
    /// <remarks>
    /// Este método utiliza <see cref="Task.Factory.StartNew"/> para ejecutar la compilación en un hilo separado.
    /// Asegúrese de manejar adecuadamente las excepciones que puedan surgir durante la compilación.
    /// </remarks>
    /// <seealso cref="IRazorEngineCompiledTemplate"/>
    /// <seealso cref="IRazorEngineCompilationOptionsBuilder"/>
    public Task<IRazorEngineCompiledTemplate> CompileAsync(string content, Action<IRazorEngineCompilationOptionsBuilder> builderAction = null)
    {
        return Task.Factory.StartNew(() => this.Compile(content: content, builderAction: builderAction));
    }

    /// <summary>
    /// Crea y compila un documento Razor a un flujo de memoria.
    /// </summary>
    /// <param name="templateSource">El código fuente del template Razor que se va a compilar.</param>
    /// <param name="options">Opciones de compilación que incluyen el espacio de nombres y referencias de ensamblado.</param>
    /// <returns>Un <see cref="MemoryStream"/> que contiene el resultado de la compilación del template.</returns>
    /// <remarks>
    /// Este método utiliza el motor de Razor para procesar el código fuente del template y compilarlo en un ensamblado dinámico.
    /// Si la compilación falla, se lanza una excepción <see cref="RazorEngineCompilationException"/> que contiene detalles sobre los errores.
    /// </remarks>
    /// <exception cref="RazorEngineCompilationException">
    /// Se lanza cuando la compilación del template falla.
    /// </exception>
    private MemoryStream CreateAndCompileToStream(string templateSource, RazorEngineCompilationOptions options)
    {
        templateSource = this.WriteDirectives(templateSource, options);

        RazorProjectEngine engine = RazorProjectEngine.Create(
            RazorConfiguration.Default,
            RazorProjectFileSystem.Create(@"."),
            (builder) =>
            {
                builder.SetNamespace(options.TemplateNamespace);
            });

        string fileName = Path.GetRandomFileName();

        RazorSourceDocument document = RazorSourceDocument.Create(templateSource, fileName);

        RazorCodeDocument codeDocument = engine.Process(
            document,
            null,
            new List<RazorSourceDocument>(),
            new List<TagHelperDescriptor>());

        RazorCSharpDocument razorCSharpDocument = codeDocument.GetCSharpDocument();

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(razorCSharpDocument.GeneratedCode);

        CSharpCompilation compilation = CSharpCompilation.Create(
            fileName,
            new[]
            {
                syntaxTree
            },
            options.ReferencedAssemblies
                .Select(ass =>
                {
#if NETSTANDARD2_0
                            return  MetadataReference.CreateFromFile(ass.Location); 
#else
                    unsafe
                    {
                        ass.TryGetRawMetadata(out byte* blob, out int length);
                        var moduleMetadata = ModuleMetadata.CreateFromMetadata((IntPtr)blob, length);
                        var assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);
                        var metadataReference = assemblyMetadata.GetReference();
                        return metadataReference;
                    }
#endif
                })
                .Concat(options.MetadataReferences)
                .ToList(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        MemoryStream memoryStream = new MemoryStream();

        EmitResult emitResult = compilation.Emit(memoryStream);

        if (!emitResult.Success)
        {
            List<Diagnostic> errors = emitResult.Diagnostics.ToList();

            RazorEngineCompilationException exception = new RazorEngineCompilationException($"No se puede compilar la plantilla: {errors.FirstOrDefault()}")
            {
                Errors = errors,
                GeneratedCode = razorCSharpDocument.GeneratedCode
            };

            throw exception;
        }

        memoryStream.Position = 0;

        return memoryStream;
    }

    /// <summary>
    /// Genera directivas para un archivo Razor, incluyendo la herencia y los usings predeterminados.
    /// </summary>
    /// <param name="content">El contenido del archivo Razor al que se le agregarán las directivas.</param>
    /// <param name="options">Las opciones de compilación de Razor que contienen la información de herencia y usings.</param>
    /// <returns>Una cadena que representa el contenido del archivo Razor con las directivas añadidas.</returns>
    /// <remarks>
    /// Este método utiliza un StringBuilder para construir el contenido final, comenzando con la directiva de herencia 
    /// y añadiendo cada uno de los usings predeterminados especificados en las opciones.
    /// </remarks>
    private string WriteDirectives(string content, RazorEngineCompilationOptions options)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"@inherits {options.Inherits}");

        foreach (string entry in options.DefaultUsings)
        {
            stringBuilder.AppendLine($"@using {entry}");
        }

        stringBuilder.Append(content);

        return stringBuilder.ToString();
    }
}