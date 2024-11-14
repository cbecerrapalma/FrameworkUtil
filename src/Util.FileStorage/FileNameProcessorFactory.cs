namespace Util.FileStorage;

/// <summary>
/// Clase que implementa la interfaz <see cref="IFileNameProcessorFactory"/> 
/// para crear instancias de procesadores de nombres de archivo.
/// </summary>
public class FileNameProcessorFactory : IFileNameProcessorFactory
{
    private readonly ISession _session;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="FileNameProcessorFactory"/>.
    /// </summary>
    /// <param name="session">La sesión que se utilizará para procesar nombres de archivos. Si es <c>null</c>, se utilizará una instancia de sesión nula.</param>
    public FileNameProcessorFactory(ISession session)
    {
        _session = session ?? NullSession.Instance;
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea un procesador de nombres de archivo basado en la política especificada.
    /// </summary>
    /// <param name="policy">La política que determina qué tipo de procesador de nombres de archivo se debe crear.</param>
    /// <returns>Un objeto que implementa <see cref="IFileNameProcessor"/> correspondiente a la política dada.</returns>
    /// <exception cref="NotImplementedException">Se lanza si la política especificada no está implementada.</exception>
    /// <remarks>
    /// Si la política está vacía, se devuelve un nuevo <see cref="FileNameProcessor"/> por defecto.
    /// Si la política coincide con <see cref="UserTimeFileNameProcessor.Policy"/>, se devuelve un nuevo <see cref="UserTimeFileNameProcessor"/>.
    /// </remarks>
    public IFileNameProcessor CreateProcessor(string policy)
    {
        if (policy.IsEmpty())
            return new FileNameProcessor();
        if (policy.ToUpperInvariant() == UserTimeFileNameProcessor.Policy)
            return new UserTimeFileNameProcessor(_session);
        throw new NotImplementedException($"El manejo de la estrategia del nombre de archivo {policy} no está implementado.");
    }
}