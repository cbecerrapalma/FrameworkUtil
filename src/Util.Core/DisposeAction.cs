namespace Util; 

/// <summary>
/// Proporciona una acción que se ejecuta al liberar recursos.
/// Implementa la interfaz <see cref="IDisposable"/> para asegurar la correcta liberación de recursos.
/// </summary>
public class DisposeAction : IDisposable {
    private readonly Action _action;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DisposeAction"/>.
    /// </summary>
    /// <param name="action">La acción que se debe ejecutar al disponer.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="action"/> es nulo.</exception>
    public DisposeAction( Action action ) {
        action.CheckNull( nameof( action ) );
        _action = action;
    }

    public static readonly IDisposable Null = new DisposeAction( null );

    /// <summary>
    /// Libera los recursos utilizados por la instancia actual.
    /// </summary>
    /// <remarks>
    /// Este método invoca una acción, si está definida, para realizar cualquier limpieza necesaria.
    /// </remarks>
    public void Dispose() {
        _action?.Invoke();
    }
}