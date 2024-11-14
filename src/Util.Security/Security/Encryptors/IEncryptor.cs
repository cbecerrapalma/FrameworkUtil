namespace Util.Security.Encryptors; 

/// <summary>
/// Define un contrato para clases que implementan funcionalidad de cifrado.
/// </summary>
public interface IEncryptor {
    /// <summary>
    /// Encripta una cadena de texto utilizando un algoritmo de cifrado específico.
    /// </summary>
    /// <param name="data">La cadena de texto que se desea encriptar.</param>
    /// <returns>Una cadena de texto encriptada.</returns>
    /// <remarks>
    /// Este método utiliza un algoritmo de cifrado simétrico para transformar la cadena de entrada en una representación encriptada.
    /// Asegúrese de que la cadena de entrada no sea nula o vacía antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="Decrypt(string)"/>
    string Encrypt( string data );
    /// <summary>
    /// Desencripta una cadena de texto que ha sido previamente encriptada.
    /// </summary>
    /// <param name="data">La cadena de texto encriptada que se desea desencriptar.</param>
    /// <returns>La cadena de texto desencriptada.</returns>
    /// <remarks>
    /// Este método utiliza un algoritmo de desencriptación específico. Asegúrese de que la cadena proporcionada
    /// sea válida y haya sido encriptada con el mismo método de encriptación que se utiliza para desencriptar.
    /// </remarks>
    /// <seealso cref="Encrypt(string)"/>
    string Decrypt( string data );
}