namespace Util.FileStorage;

/// <summary>
/// Clase que implementa la interfaz <see cref="IBucketNameProcessorFactory"/>.
/// Proporciona métodos para crear instancias de procesadores de nombres de bucket.
/// </summary>
public class BucketNameProcessorFactory : IBucketNameProcessorFactory
{
    /// <inheritdoc />
    /// <summary>
    /// Crea un procesador de nombres de bucket basado en la política proporcionada.
    /// </summary>
    /// <param name="policy">La política que se utilizará para crear el procesador de nombres de bucket.</param>
    /// <returns>Un objeto que implementa <see cref="IBucketNameProcessor"/>.</returns>
    /// <exception cref="NotImplementedException">Se lanza si la política proporcionada no está implementada.</exception>
    /// <remarks>
    /// Si la política está vacía, se devuelve una nueva instancia de <see cref="BucketNameProcessor"/>.
    /// </remarks>
    public IBucketNameProcessor CreateProcessor(string policy)
    {
        if (policy.IsEmpty())
            return new BucketNameProcessor();
        throw new NotImplementedException($"La política de manejo del nombre del bucket {policy} no está implementada.");
    }
}