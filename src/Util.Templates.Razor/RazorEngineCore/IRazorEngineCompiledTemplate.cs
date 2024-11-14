using System.IO;
using System.Threading.Tasks;

namespace RazorEngineCore; 

/// <summary>
/// Define una interfaz para un motor de plantillas Razor compiladas.
/// </summary>
public interface IRazorEngineCompiledTemplate
{
    /// <summary>
    /// Guarda los datos en el flujo especificado.
    /// </summary>
    /// <param name="stream">El flujo en el que se guardarán los datos.</param>
    /// <remarks>
    /// Este método permite serializar la información de la instancia actual y escribirla en el flujo proporcionado.
    /// Asegúrese de que el flujo esté abierto y sea accesible para escritura antes de llamar a este método.
    /// </remarks>
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
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="stream"/> es nulo.</exception>
    /// <seealso cref="LoadFromStreamAsync(Stream)"/>
    Task SaveToStreamAsync(Stream stream);
        
    /// <summary>
    /// Guarda los datos en un archivo con el nombre especificado.
    /// </summary>
    /// <param name="fileName">El nombre del archivo donde se guardarán los datos.</param>
    /// <remarks>
    /// Este método sobrescribirá el archivo si ya existe. Asegúrese de tener los permisos necesarios para escribir en la ubicación especificada.
    /// </remarks>
    void SaveToFile(string fileName);
        
    /// <summary>
    /// Guarda de manera asíncrona los datos en un archivo con el nombre especificado.
    /// </summary>
    /// <param name="fileName">El nombre del archivo en el que se guardarán los datos.</param>
    /// <returns>Una tarea que representa la operación asíncrona de guardar el archivo.</returns>
    /// <remarks>
    /// Este método permite guardar datos en un archivo de forma asíncrona, lo que evita bloquear el hilo de ejecución principal.
    /// Asegúrese de que el nombre del archivo incluya la ruta correcta si desea guardarlo en un directorio específico.
    /// </remarks>
    /// <seealso cref="LoadFromFileAsync(string)"/>
    Task SaveToFileAsync(string fileName);
        
    /// <summary>
    /// Ejecuta un proceso basado en el modelo proporcionado.
    /// </summary>
    /// <param name="model">El modelo que se utilizará para ejecutar el proceso. Si es null, se utilizará un modelo por defecto.</param>
    /// <returns>Una cadena que representa el resultado de la ejecución.</returns>
    /// <remarks>
    /// Este método puede ser utilizado para iniciar operaciones que dependen de la configuración del modelo.
    /// Asegúrese de que el modelo proporcionado sea del tipo esperado para evitar errores en tiempo de ejecución.
    /// </remarks>
    string Run(object model = null);
        
    /// <summary>
    /// Ejecuta una tarea asíncrona que procesa el modelo proporcionado.
    /// </summary>
    /// <param name="model">El modelo a procesar. Puede ser null si no se proporciona ningún modelo.</param>
    /// <returns>Una tarea que representa el resultado de la operación asíncrona, que contiene un <see cref="string"/> como resultado.</returns>
    /// <remarks>
    /// Este método es útil para realizar operaciones que requieren tiempo, como llamadas a servicios externos o procesamiento de datos.
    /// Asegúrese de manejar adecuadamente las excepciones que puedan surgir durante la ejecución.
    /// </remarks>
    Task<string> RunAsync(object model = null);
}