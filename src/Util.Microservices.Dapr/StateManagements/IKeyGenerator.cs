namespace Util.Microservices.Dapr.StateManagements; 

/// <summary>
/// Interfaz que define un generador de claves.
/// </summary>
/// <remarks>
/// Esta interfaz se utiliza para implementar diferentes estrategias de generación de claves,
/// asegurando que cada implementación sea un singleton.
/// </remarks>
public interface IKeyGenerator : ISingletonDependency {
    /// <summary>
    /// Crea una clave única basada en el identificador proporcionado.
    /// </summary>
    /// <typeparam name="TValue">El tipo de valor que implementa la interfaz <see cref="IDataKey"/>.</typeparam>
    /// <param name="id">El identificador que se utilizará para generar la clave.</param>
    /// <returns>Una cadena que representa la clave única generada.</returns>
    /// <remarks>
    /// Este método es genérico y permite crear claves para diferentes tipos que implementan <see cref="IDataKey"/>.
    /// Asegúrese de que el tipo proporcionado cumpla con los requisitos de la interfaz.
    /// </remarks>
    /// <seealso cref="IDataKey"/>
    string CreateKey<TValue>( string id ) where TValue : IDataKey;
}