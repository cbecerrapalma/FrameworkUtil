namespace Util.Security.Encryptors; 

/// <summary>
/// Clase que implementa la interfaz <see cref="IEncryptor"/> para proporcionar una 
/// funcionalidad de cifrado nula, es decir, no realiza ninguna operación de cifrado.
/// </summary>
public class NullEncryptor : IEncryptor {
    public static readonly IEncryptor Instance = new NullEncryptor();

    /// <summary>
    /// Encripta la cadena de texto proporcionada.
    /// </summary>
    /// <param name="data">La cadena de texto que se desea encriptar.</param>
    /// <returns>Una cadena vacía, ya que la implementación de la encriptación no está definida.</returns>
    public string Encrypt( string data ) {
        return string.Empty;
    }

    /// <summary>
    /// Desencripta la cadena de datos proporcionada.
    /// </summary>
    /// <param name="data">La cadena de datos que se desea desencriptar.</param>
    /// <returns>Una cadena vacía, ya que la implementación de desencriptación no está definida.</returns>
    public string Decrypt( string data ) {
        return string.Empty;
    }
}