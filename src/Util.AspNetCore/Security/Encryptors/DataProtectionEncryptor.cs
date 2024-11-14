namespace Util.Security.Encryptors; 

/// <summary>
/// Clase que implementa la interfaz <see cref="IEncryptor"/> para proporcionar funcionalidades de cifrado de datos.
/// </summary>
/// <remarks>
/// Esta clase utiliza algoritmos de cifrado para proteger la información sensible.
/// </remarks>
public class DataProtectionEncryptor : IEncryptor {
    private readonly IDataProtector _dataProtector;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DataProtectionEncryptor"/>.
    /// </summary>
    /// <param name="dataProtector">Una instancia de <see cref="IDataProtector"/> que se utilizará para la protección de datos.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="dataProtector"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Esta clase se encarga de la encriptación y desencriptación de datos utilizando el mecanismo proporcionado por <paramref name="dataProtector"/>.
    /// </remarks>
    public DataProtectionEncryptor( IDataProtector dataProtector ) {
        _dataProtector = dataProtector ?? throw new ArgumentNullException( nameof( dataProtector ) );
    }

    /// <inheritdoc />
    /// <summary>
    /// Encripta los datos proporcionados utilizando un protector de datos.
    /// </summary>
    /// <param name="data">La cadena de texto que se desea encriptar.</param>
    /// <returns>
    /// La cadena encriptada si los datos no están vacíos; de lo contrario, devuelve null.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un objeto de tipo <see cref="_dataProtector"/> para realizar la encriptación.
    /// Asegúrese de que el objeto <see cref="_dataProtector"/> esté correctamente inicializado antes de llamar a este método.
    /// </remarks>
    public string Encrypt( string data ) {
        if ( data.IsEmpty() )
            return null;
        return _dataProtector.Protect( data );
    }

    /// <inheritdoc />
    /// <summary>
    /// Desencripta los datos proporcionados.
    /// </summary>
    /// <param name="data">La cadena de texto encriptada que se desea desencriptar.</param>
    /// <returns>
    /// La cadena de texto desencriptada, o null si la cadena de entrada está vacía.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un objeto de protección de datos para realizar la desencriptación.
    /// Si la cadena de entrada está vacía, se devolverá null.
    /// </remarks>
    /// <seealso cref="Encrypt(string)"/>
    public string Decrypt( string data ) {
        if ( data.IsEmpty() )
            return null;
        return _dataProtector.Unprotect( data );
    }
}