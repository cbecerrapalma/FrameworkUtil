using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Util.Templates.Razor;
using Util.Templates.Razor.Helpers;

namespace RazorEngineCore; 

/// <summary>
/// Representa una plantilla compilada de Razor que implementa la interfaz <see cref="IRazorEngineCompiledTemplate"/>.
/// </summary>
/// <remarks>
/// Esta clase se utiliza para gestionar la ejecución de plantillas Razor ya compiladas, permitiendo la generación de contenido dinámico
/// a partir de estas plantillas.
/// </remarks>
public class RazorEngineCompiledTemplate : IRazorEngineCompiledTemplate
{
    private readonly MemoryStream assemblyByteCode;
    private readonly Type templateType;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RazorEngineCompiledTemplate"/>.
    /// </summary>
    /// <param name="assemblyByteCode">Un flujo de memoria que contiene el código de bytes del ensamblado compilado.</param>
    /// <remarks>
    /// Este constructor carga el ensamblado desde el flujo de memoria y obtiene el tipo de plantilla correspondiente.
    /// </remarks>
    internal RazorEngineCompiledTemplate(MemoryStream assemblyByteCode)
    {
        this.assemblyByteCode = assemblyByteCode;

        Assembly assembly = Assembly.Load(assemblyByteCode.ToArray());
        this.templateType = assembly.GetType("TemplateNamespace.Template");
    }

    /// <summary>
    /// Carga una plantilla Razor compilada desde un archivo especificado de manera sincrónica.
    /// </summary>
    /// <param name="fileName">El nombre del archivo que contiene la plantilla Razor.</param>
    /// <returns>
    /// Una instancia de <see cref="IRazorEngineCompiledTemplate"/> que representa la plantilla cargada.
    /// </returns>
    /// <remarks>
    /// Este método es una envoltura sincrónica para el método asíncrono <see cref="LoadFromFileAsync(string)"/>.
    /// Se recomienda utilizar el método asíncrono para evitar bloqueos en el hilo de ejecución.
    /// </remarks>
    /// <seealso cref="LoadFromFileAsync(string)"/>
    public static IRazorEngineCompiledTemplate LoadFromFile(string fileName)
    {
        return LoadFromFileAsync(fileName: fileName).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Carga un archivo de plantilla Razor de manera asíncrona desde el sistema de archivos.
    /// </summary>
    /// <param name="fileName">El nombre del archivo que contiene la plantilla Razor.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El resultado de la tarea es una instancia de 
    /// <see cref="IRazorEngineCompiledTemplate"/> que representa la plantilla cargada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un <see cref="FileStream"/> para abrir el archivo especificado y copiar su contenido 
    /// a un <see cref="MemoryStream"/>. Una vez que se ha copiado el contenido, se crea una nueva instancia de 
    /// <see cref="RazorEngineCompiledTemplate"/> utilizando el <see cref="MemoryStream"/>.
    /// </remarks>
    /// <exception cref="FileNotFoundException">Se lanza si el archivo especificado no se encuentra.</exception>
    /// <exception cref="UnauthorizedAccessException">Se lanza si no se tienen permisos para acceder al archivo.</exception>
    /// <seealso cref="IRazorEngineCompiledTemplate"/>
    /// <seealso cref="RazorEngineCompiledTemplate"/>
    public static async Task<IRazorEngineCompiledTemplate> LoadFromFileAsync(string fileName)
    {
        MemoryStream memoryStream = new MemoryStream();
            
        using (FileStream fileStream = new FileStream(
                   path: fileName, 
                   mode: FileMode.Open, 
                   access: FileAccess.Read,
                   share: FileShare.None,
                   bufferSize: 4096, 
                   useAsync: true))
        {
            await fileStream.CopyToAsync(memoryStream);
        }
            
        return new RazorEngineCompiledTemplate(memoryStream);
    }
        
    /// <summary>
    /// Carga una plantilla Razor compilada desde un flujo de datos.
    /// </summary>
    /// <param name="stream">El flujo de datos desde el cual se cargará la plantilla.</param>
    /// <returns>Una instancia de <see cref="IRazorEngineCompiledTemplate"/> que representa la plantilla cargada.</returns>
    /// <remarks>
    /// Este método es una versión sincrónica de <see cref="LoadFromStreamAsync(Stream)"/>.
    /// Se recomienda utilizar la versión asíncrona siempre que sea posible para evitar bloqueos en el hilo de ejecución.
    /// </remarks>
    /// <seealso cref="LoadFromStreamAsync(Stream)"/>
    public static IRazorEngineCompiledTemplate LoadFromStream(Stream stream)
    {
        return LoadFromStreamAsync(stream).GetAwaiter().GetResult();
    }
        
    /// <summary>
    /// Carga una plantilla Razor compilada desde un flujo de datos.
    /// </summary>
    /// <param name="stream">El flujo de datos desde el cual se cargará la plantilla.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un resultado que es una instancia de 
    /// <see cref="IRazorEngineCompiledTemplate"/> que representa la plantilla cargada.
    /// </returns>
    /// <remarks>
    /// Este método copia el contenido del flujo proporcionado a un <see cref="MemoryStream"/> 
    /// y establece la posición del flujo en 0 antes de devolver la plantilla compilada.
    /// </remarks>
    public static async Task<IRazorEngineCompiledTemplate> LoadFromStreamAsync(Stream stream)
    {
        MemoryStream memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
            
        return new RazorEngineCompiledTemplate(memoryStream);
    }

    /// <summary>
    /// Guarda los datos en el flujo especificado de manera sincrónica.
    /// </summary>
    /// <param name="stream">El flujo en el que se guardarán los datos.</param>
    /// <remarks>
    /// Este método llama a <see cref="SaveToStreamAsync(Stream)"/> de forma sincrónica,
    /// lo que puede bloquear el hilo actual hasta que la operación se complete.
    /// Se recomienda utilizar el método asincrónico directamente cuando sea posible.
    /// </remarks>
    public void SaveToStream(Stream stream)
    {
        this.SaveToStreamAsync(stream).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Guarda el código de bytes de la asamblea en un flujo de salida de forma asíncrona.
    /// </summary>
    /// <param name="stream">El flujo en el que se guardará el código de bytes de la asamblea.</param>
    /// <returns>Una tarea que representa la operación asíncrona de guardar en el flujo.</returns>
    public Task SaveToStreamAsync(Stream stream)
    {
        return this.assemblyByteCode.CopyToAsync(stream);
    }

    /// <summary>
    /// Guarda los datos en un archivo de forma sincrónica.
    /// </summary>
    /// <param name="fileName">El nombre del archivo donde se guardarán los datos.</param>
    /// <remarks>
    /// Este método llama a la versión asíncrona <see cref="SaveToFileAsync(string)"/> 
    /// y espera su finalización antes de continuar.
    /// </remarks>
    /// <seealso cref="SaveToFileAsync(string)"/>
    public void SaveToFile(string fileName)
    {
        this.SaveToFileAsync(fileName).GetAwaiter().GetResult();
    }
        
    /// <summary>
    /// Guarda el contenido del código de bytes de la asamblea en un archivo de forma asíncrona.
    /// </summary>
    /// <param name="fileName">El nombre del archivo en el que se guardará el contenido.</param>
    /// <returns>Una tarea que representa la operación asíncrona de guardar el archivo.</returns>
    /// <remarks>
    /// Este método crea un nuevo archivo o abre uno existente para escritura. 
    /// Si el archivo ya existe, su contenido se sobrescribirá.
    /// </remarks>
    /// <exception cref="System.IO.IOException">
    /// Se produce si hay un error de entrada/salida durante la operación de archivo.
    /// </exception>
    /// <seealso cref="System.IO.FileStream"/>
    /// <seealso cref="System.Threading.Tasks.Task"/>
    public Task SaveToFileAsync(string fileName)
    {
        using (FileStream fileStream = new FileStream(
                   path: fileName, 
                   mode: FileMode.OpenOrCreate, 
                   access: FileAccess.Write,
                   share: FileShare.None,
                   bufferSize: 4096, 
                   useAsync: true))
        {
            return assemblyByteCode.CopyToAsync(fileStream);
        }
    }
        
    /// <summary>
    /// Ejecuta de manera sincrónica un proceso utilizando el modelo proporcionado.
    /// </summary>
    /// <param name="model">El modelo que se utilizará en el proceso. Puede ser null.</param>
    /// <returns>Una cadena que representa el resultado del proceso ejecutado.</returns>
    /// <remarks>
    /// Este método es una envoltura sincrónica para el método asíncrono <see cref="RunAsync(object)"/>.
    /// Se recomienda utilizar el método asíncrono siempre que sea posible para evitar bloqueos en el hilo de ejecución.
    /// </remarks>
    /// <seealso cref="RunAsync(object)"/>
    public string Run(object model = null)
    {
        return this.RunAsync(model).GetAwaiter().GetResult();
    }
        
    /// <summary>
    /// Ejecuta de manera asíncrona una plantilla Razor con el modelo proporcionado.
    /// </summary>
    /// <param name="model">El modelo que se utilizará en la plantilla. Puede ser nulo o un objeto anónimo.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que es una cadena que contiene el HTML generado por la plantilla.</returns>
    /// <remarks>
    /// Si el modelo es un objeto anónimo, se envuelve en un <see cref="AnonymousTypeWrapper"/> antes de ser utilizado.
    /// Se crea una instancia de la plantilla Razor y se establece el modelo y el helper HTML.
    /// Luego, se ejecuta la plantilla y se devuelve el resultado generado.
    /// </remarks>
    public async Task<string> RunAsync(object model = null)
    {
        if (model != null && model.IsAnonymous())
        {
            model = new AnonymousTypeWrapper(model);
        }

        IRazorEngineTemplate instance = (IRazorEngineTemplate) Activator.CreateInstance(this.templateType);
        instance.Model = model;
        instance.Html = new HtmlHelper( new RazorTemplateEngine( new RazorEngine() ) );

        await instance.ExecuteAsync();

        return instance.Result();
    }
}