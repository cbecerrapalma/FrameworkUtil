namespace Util.Scheduling; 

/// <summary>
/// Representa el contexto de ejecución para las tareas programadas con Hangfire.
/// </summary>
public class HangfireExecutionContext {
    private readonly object _data;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="HangfireExecutionContext"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver dependencias.</param>
    /// <param name="data">Los datos asociados al contexto de ejecución.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="serviceProvider"/> es <c>null</c>.</exception>
    public HangfireExecutionContext( IServiceProvider serviceProvider,object data ) {
        ServiceProvider = serviceProvider ?? throw new ArgumentNullException( nameof( serviceProvider ) );
        _data = data;
    }

    /// <summary>
    /// Obtiene el proveedor de servicios asociado.
    /// </summary>
    /// <remarks>
    /// Este proveedor de servicios se utiliza para resolver dependencias dentro de la aplicación.
    /// </remarks>
    /// <value>
    /// Un objeto que implementa <see cref="IServiceProvider"/>.
    /// </value>
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Obtiene una instancia del servicio especificado por el tipo generico <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del servicio que se desea obtener.</typeparam>
    /// <returns>
    /// Una instancia del servicio del tipo <typeparamref name="T"/> si está registrado en el contenedor de servicios; de lo contrario, devuelve <c>null</c>.
    /// </returns>
    public T GetService<T>() {
        return ServiceProvider.GetService<T>();
    }

    /// <summary>
    /// Obtiene los datos convertidos al tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo al que se desea convertir los datos.</typeparam>
    /// <returns>
    /// Un objeto del tipo especificado que representa los datos convertidos.
    /// Si la conversión inicial falla, se intentará convertir los datos desde una cadena JSON.
    /// </returns>
    /// <remarks>
    /// Este método primero intenta convertir los datos utilizando un método de conversión genérico.
    /// Si la conversión resulta en un valor nulo, se intentará deserializar los datos desde una cadena JSON segura.
    /// </remarks>
    /// <seealso cref="Util.Helpers.Convert"/>
    /// <seealso cref="Util.Helpers.Json.ToObject{T}"/>
    public T GetData<T>() {
        var result = Util.Helpers.Convert.To<T>( _data );
        return result ?? Util.Helpers.Json.ToObject<T>( _data.SafeString() );
    }
}