namespace Util.Microservices;

/// <summary>
/// Representa el resultado de un servicio que puede contener datos de tipo TData.
/// </summary>
/// <typeparam name="TData">El tipo de datos que se devolverán en el resultado del servicio.</typeparam>
public class ServiceResult<TData> {
    /// <summary>
    /// Obtiene o establece el código asociado.
    /// </summary>
    /// <value>
    /// Un string que representa el código.
    /// </value>
    public string Code { get; set; }
    /// <summary>
    /// Obtiene o establece el mensaje asociado.
    /// </summary>
    /// <value>
    /// Un string que representa el mensaje.
    /// </value>
    public string Message { get; set; }
    /// <summary>
    /// Obtiene o establece el dato de tipo <typeparamref name="TData"/>.
    /// </summary>
    /// <value>
    /// El dato de tipo <typeparamref name="TData"/> asociado.
    /// </value>
    /// <typeparam name="TData">
    /// El tipo de dato que se almacena en la propiedad <see cref="Data"/>.
    /// </typeparam>
    public TData Data { get; set; }
}