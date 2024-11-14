namespace Util.Configs; 

/// <summary>
/// Define un contrato para construir aplicaciones.
/// </summary>
public interface IAppBuilder {
    /// <summary>
    /// Obtiene el constructor de host que se utiliza para configurar y crear una aplicación.
    /// </summary>
    /// <remarks>
    /// Este objeto permite definir los servicios y la configuración de la aplicación, así como gestionar el ciclo de vida del host.
    /// </remarks>
    /// <value>
    /// Un objeto que implementa <see cref="IHostBuilder"/>.
    /// </value>
    public IHostBuilder Host { get; }
    /// <summary>
    /// Construye y configura una instancia de <see cref="IHost"/>.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IHost"/> que representa la aplicación configurada.
    /// </returns>
    /// <remarks>
    /// Este método es responsable de inicializar todos los servicios necesarios y 
    /// establecer la configuración del host para la aplicación. Asegúrese de que 
    /// todos los servicios requeridos estén registrados antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="IHost"/>
    public IHost Build();
}