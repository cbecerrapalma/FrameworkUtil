namespace Util.Events; 

/// <summary>
/// Interfaz que representa un bus de eventos local.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="IEventBus"/> y se utiliza para la 
/// comunicación de eventos dentro de un contexto local, permitiendo 
/// la suscripción y 
public interface ILocalEventBus : IEventBus {
}