using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace RazorEngineCore; 

/// <summary>
/// Representa una plantilla compilada de Razor que implementa la interfaz <see cref="IRazorEngineCompiledTemplate{T}"/>.
/// </summary>
/// <typeparam name="T">El tipo de la plantilla que debe implementar <see cref="IRazorEngineTemplate"/>.</typeparam>
public class RazorEngineCompiledTemplate<T> : IRazorEngineCompiledTemplate<T> where T : IRazorEngineTemplate
{
    private readonly MemoryStream assemblyByteCode;
    private readonly Type templateType;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RazorEngineCompiledTemplate"/>.
    /// </summary>
    /// <param name="assemblyByteCode">Un <see cref="MemoryStream"/> que contiene el código de bytes del ensamblado compilado.</param>
    /// <remarks>
    /// Este constructor carga el ensamblado desde el flujo de memoria proporcionado y obtiene el tipo de plantilla
    /// correspondiente a "TemplateNamespace.Template".
    /// </remarks>
    internal RazorEngineCompiledTemplate(MemoryStream assemblyByteCode)
    {
        this.assemblyByteCode = assemblyByteCode;

        Assembly assembly = Assembly.Load(assemblyByteCode.ToArray());
        this.templateType = assembly.GetType("TemplateNamespace.Template");
    }

    /// <summary>
    /// Carga una plantilla Razor compilada desde un archivo especificado.
    /// </summary>
    /// <param name="fileName">El nombre del archivo que contiene la plantilla Razor.</param>
    /// <returns>
    /// Una instancia de <see cref="IRazorEngineCompiledTemplate{T}"/> que representa la plantilla cargada.
    /// </returns>
    /// <remarks>
    /// Este método es sincrónico y espera a que se complete la carga de la plantilla.
    /// Si se requiere un enfoque asincrónico, se recomienda utilizar el método <see cref="LoadFromFileAsync(string)"/>.
    /// </remarks>
    /// <seealso cref="LoadFromFileAsync(string)"/>
    public static IRazorEngineCompiledTemplate<T> LoadFromFile(string fileName)
    {
        return LoadFromFileAsync(fileName: fileName).GetAwaiter().GetResult();
    }
        
    /// <summary>
    /// Carga un archivo de plantilla Razor de forma asíncrona desde el sistema de archivos.
    /// </summary>
    /// <param name="fileName">El nombre del archivo que contiene la plantilla Razor.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El resultado de la tarea es una instancia de 
    /// <see cref="IRazorEngineCompiledTemplate{T}"/> que representa la plantilla cargada.
    /// </returns>
    /// <remarks>
    /// Este método abre el archivo especificado, copia su contenido a un flujo de memoria y 
    /// devuelve una plantilla compilada que puede ser utilizada para renderizar contenido.
    /// </remarks>
    /// <exception cref="FileNotFoundException">Se lanza si el archivo especificado no se encuentra.</exception>
    /// <exception cref="IOException">Se lanza si ocurre un error de entrada/salida al abrir o leer el archivo.</exception>
    public static async Task<IRazorEngineCompiledTemplate<T>> LoadFromFileAsync(string fileName)
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
            
        return new RazorEngineCompiledTemplate<T>(memoryStream);
    }

    /// <summary>
    /// Carga una plantilla Razor compilada desde un flujo de datos.
    /// </summary>
    /// <param name="stream">El flujo de datos desde el cual se cargará la plantilla.</param>
    /// <returns>
    /// Una instancia de <see cref="IRazorEngineCompiledTemplate{T}"/> que representa la plantilla cargada.
    /// </returns>
    /// <remarks>
    /// Este método es una versión sincrónica de <see cref="LoadFromStreamAsync(Stream)"/>.
    /// Se recomienda utilizar la versión asincrónica para evitar bloqueos en el hilo de llamada.
    /// </remarks>
    public static IRazorEngineCompiledTemplate<T> LoadFromStream(Stream stream)
    {
        return LoadFromStreamAsync(stream).GetAwaiter().GetResult();
    }
        
    /// <summary>
    /// Carga un template Razor compilado desde un flujo de datos de forma asíncrona.
    /// </summary>
    /// <param name="stream">El flujo de datos desde el cual se cargará el template.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que es una instancia de <see cref="IRazorEngineCompiledTemplate{T}"/>.</returns>
    /// <remarks>
    /// Este método copia el contenido del flujo proporcionado a un <see cref="MemoryStream"/> y luego inicializa
    /// un nuevo <see cref="RazorEngineCompiledTemplate{T}"/> con el contenido del <see cref="MemoryStream"/>.
    /// </remarks>
    /// <typeparam name="T">El tipo de modelo que se utilizará con el template Razor.</typeparam>
    public static async Task<IRazorEngineCompiledTemplate<T>> LoadFromStreamAsync(Stream stream)
    {
        MemoryStream memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
            
        return new RazorEngineCompiledTemplate<T>(memoryStream);
    }

    /// <summary>
    /// Guarda los datos en el flujo especificado de manera sincrónica.
    /// </summary>
    /// <param name="stream">El flujo en el que se guardarán los datos.</param>
    /// <remarks>
    /// Este método llama a <see cref="SaveToStreamAsync(Stream)"/> de manera sincrónica
    /// utilizando <see cref="GetAwaiter"/> y <see cref="GetResult"/>. 
    /// Se recomienda utilizar el método asíncrono para evitar bloqueos en el hilo de ejecución.
    /// </remarks>
    public void SaveToStream(Stream stream)
    {
        this.SaveToStreamAsync(stream).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Guarda el código de bytes de la asamblea en un flujo de forma asíncrona.
    /// </summary>
    /// <param name="stream">El flujo en el que se guardará el código de bytes de la asamblea.</param>
    /// <returns>Una tarea que representa la operación asíncrona de guardado.</returns>
    public Task SaveToStreamAsync(Stream stream)
    {
        return this.assemblyByteCode.CopyToAsync(stream);
    }
        
    /// <summary>
    /// Guarda el contenido en un archivo de forma sincrónica.
    /// </summary>
    /// <param name="fileName">El nombre del archivo donde se guardará el contenido.</param>
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
    /// Guarda el contenido del código de bytes del ensamblado en un archivo de forma asíncrona.
    /// </summary>
    /// <param name="fileName">El nombre del archivo en el que se guardará el contenido.</param>
    /// <returns>Una tarea que representa la operación asíncrona de guardar el archivo.</returns>
    /// <remarks>
    /// Este método crea un nuevo archivo con el nombre especificado. Si el archivo ya existe, se lanzará una excepción.
    /// Asegúrese de que el nombre del archivo sea válido y que tenga los permisos necesarios para crear un archivo en la ubicación especificada.
    /// </remarks>
    public Task SaveToFileAsync(string fileName)
    {
        using (FileStream fileStream = new FileStream(
                   path: fileName, 
                   mode: FileMode.CreateNew, 
                   access: FileAccess.Write,
                   share: FileShare.None,
                   bufferSize: 4096, 
                   useAsync: true))
        {
            return assemblyByteCode.CopyToAsync(fileStream);
        }
    }

    /// <summary>
    /// Ejecuta una acción de inicialización de forma sincrónica.
    /// </summary>
    /// <param name="initializer">La acción que se utilizará para inicializar.</param>
    /// <returns>Una cadena que representa el resultado de la ejecución.</returns>
    /// <remarks>
    /// Este método es una envoltura sincrónica para el método asíncrono <see cref="RunAsync(Action{T})"/>.
    /// Utiliza <see cref="GetAwaiter"/> y <see cref="GetResult"/> para bloquear el hilo actual hasta que se complete la tarea.
    /// </remarks>
    /// <typeparam name="T">El tipo de parámetro que se pasa a la acción de inicialización.</typeparam>
    public string Run(Action<T> initializer)
    {
        return this.RunAsync(initializer).GetAwaiter().GetResult();
    }
        
    /// <summary>
    /// Ejecuta de manera asíncrona una acción de inicialización sobre una instancia de tipo <typeparamref name="T"/>.
    /// </summary>
    /// <param name="initializer">La acción que inicializa la instancia de tipo <typeparamref name="T"/>.</param>
    /// <typeparam name="T">El tipo de la instancia que se va a crear y ejecutar.</typeparam>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado de tipo <see cref="string"/>.</returns>
    /// <remarks>
    /// Este método crea una instancia del tipo especificado por <see cref="templateType"/>,
    /// aplica la acción de inicialización y luego ejecuta un método asíncrono en la instancia.
    /// Finalmente, devuelve el resultado de la instancia como una cadena.
    /// </remarks>
    /// <exception cref="InvalidOperationException">Se lanza si la creación de la instancia falla.</exception>
    public async Task<string> RunAsync(Action<T> initializer)
    {
        T instance = (T) Activator.CreateInstance(this.templateType);
        initializer(instance);

        await instance.ExecuteAsync();

        return instance.Result();
    }
}