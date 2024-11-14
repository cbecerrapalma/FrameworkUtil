namespace Util.Microservices.Events;

public record PubsubArgument( string Pubsub, string Topic, object EventData, Dictionary<string, string> Metadata = null ) 
    : PubsubArgument<object>( Pubsub, Topic, EventData, Metadata ) {
}

public record PubsubArgument<TEventData>( string Pubsub, string Topic, TEventData EventData, Dictionary<string, string> Metadata = null ) {
    /// <summary>
    /// Obtiene los datos del evento como un tipo específico.
    /// </summary>
    /// <typeparam name="T">El tipo al que se desea convertir los datos del evento.</typeparam>
    /// <returns>
    /// Los datos del evento convertidos al tipo especificado.
    /// </returns>
    /// <remarks>
    /// Este método realiza una conversión de tipo de los datos del evento almacenados.
    /// Asegúrese de que el tipo especificado coincida con el tipo de los datos del evento,
    /// de lo contrario, se producirá una excepción en tiempo de ejecución.
    /// </remarks>
    public T GetEventData<T>() {
        return (T)(object)EventData;
    }
}