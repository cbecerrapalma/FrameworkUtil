using System;
using System.IO;
using System.Threading.Tasks;

namespace RazorEngineCore; 

/// <summary>
/// Define una interfaz para una plantilla compilada de Razor que produce un tipo específico.
/// </summary>
/// <typeparam name="T">El tipo de resultado que produce la plantilla.</typeparam>
public interface IRazorEngineCompiledTemplate<out T> 
    where T : IRazorEngineTemplate
{
    /// <summary>
    /// Guarda los datos en el flujo especificado.
    /// </summary>
    /// <param name="stream">El flujo en el que se guardarán los datos.</param>
    /// <remarks>
    /// Este método permite serializar los datos actuales en el flujo proporcionado.
    /// Asegúrese de que el flujo esté abierto y sea accesible para escritura.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="stream"/> es null.</exception>
    /// <exception cref="IOException">Se lanza si ocurre un error de entrada/salida durante la operación.</exception>
    void SaveToStream(Stream stream);
        
    /// <summary>
    /// Guarda de manera asíncrona los datos en el flujo especificado.
    /// </summary>
    /// <param name="stream">El flujo en el que se guardarán los datos.</param>
    /// <returns>Una tarea que representa la operación asíncrona de guardado.</returns>
    /// <remarks>
    /// Este método permite guardar datos en un flujo de salida, como un archivo o un flujo de red.
    /// Asegúrese de que el flujo esté abierto y sea accesible para escritura antes de llamar a este método.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="stream"/> es null.</exception>
    /// <seealso cref="LoadFromStreamAsync(Stream)"/>
    Task SaveToStreamAsync(Stream stream);
        
    /// <summary>
    /// Guarda la información en un archivo con el nombre especificado.
    /// </summary>
    /// <param name="fileName">El nombre del archivo donde se guardará la información.</param>
    /// <remarks>
    /// Este método sobrescribirá el archivo si ya existe. Asegúrese de tener los permisos necesarios para escribir en la ubicación especificada.
    /// </remarks>
    void SaveToFile(string fileName);
        
    /// <summary>
    /// Guarda de manera asíncrona los datos en un archivo con el nombre especificado.
    /// </summary>
    /// <param name="fileName">El nombre del archivo donde se guardarán los datos.</param>
    /// <returns>Una tarea que representa la operación asíncrona de guardar el archivo.</returns>
    /// <remarks>
    /// Este método permite guardar datos en un archivo de forma asíncrona, lo que evita bloquear el hilo de ejecución.
    /// Asegúrese de que el nombre del archivo incluya la ruta completa si se desea guardar en un directorio específico.
    /// </remarks>
    /// <seealso cref="LoadFromFileAsync"/>
    Task SaveToFileAsync(string fileName);
        
    /// <summary>
    /// Ejecuta una acción de inicialización sobre un objeto de tipo T y devuelve una cadena de resultado.
    /// </summary>
    /// <param name="initializer">La acción que se ejecutará para inicializar el objeto.</param>
    /// <returns>Una cadena que representa el resultado de la ejecución.</returns>
    /// <typeparam name="T">El tipo del objeto que se inicializa.</typeparam>
    /// <remarks>
    /// Este método permite ejecutar una acción de inicialización que puede modificar el estado del objeto de tipo T. 
    /// El resultado de la ejecución se devuelve como una cadena, lo que permite su uso en contextos donde se requiere 
    /// una representación textual del resultado.
    /// </remarks>
    /// <seealso cref="Action{T}"/>
    string Run(Action<T> initializer);
        
    /// <summary>
    /// Ejecuta una acción asincrónica que inicializa un objeto de tipo <typeparamref name="T"/>.
    /// </summary>
    /// <param name="initializer">La acción que se utilizará para inicializar el objeto.</param>
    /// <returns>Una tarea que representa la operación asincrónica, que contiene un resultado de tipo <see cref="string"/>.</returns>
    /// <typeparam name="T">El tipo del objeto que será inicializado por la acción.</typeparam>
    /// <remarks>
    /// Este método permite ejecutar una acción de inicialización de forma asincrónica, 
    /// lo que puede ser útil para realizar configuraciones o preparaciones que 
    /// no bloqueen el hilo principal de ejecución.
    /// </remarks>
    /// <seealso cref="Task{TResult}"/>
    Task<string> RunAsync(Action<T> initializer);
}