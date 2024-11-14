using RSAExtensions;

namespace Util.Helpers;

/// <summary>
/// Proporciona métodos para realizar operaciones de cifrado y descifrado.
/// </summary>
public static class Encrypt
{

    #region Cifrado MD5

    /// <summary>
    /// Calcula el hash MD5 de una cadena y devuelve los primeros 16 caracteres del resultado en formato hexadecimal.
    /// </summary>
    /// <param name="value">La cadena de entrada para la cual se calculará el hash MD5.</param>
    /// <returns>Una cadena que representa los primeros 16 caracteres del hash MD5 en formato hexadecimal.</returns>
    public static string Md5By16(string value)
    {
        return Md5By16(value, Encoding.UTF8);
    }

    /// <summary>
    /// Calcula el hash MD5 de una cadena y devuelve una representación de 16 caracteres.
    /// </summary>
    /// <param name="value">La cadena de entrada que se desea hashear.</param>
    /// <param name="encoding">La codificación que se utilizará para convertir la cadena en bytes.</param>
    /// <returns>Una cadena que representa el hash MD5 de 16 caracteres.</returns>
    /// <remarks>
    /// Este método utiliza una función auxiliar llamada <see cref="Md5"/> para realizar el cálculo del hash.
    /// La representación de 16 caracteres se obtiene extrayendo una subcadena del resultado completo del hash.
    /// </remarks>
    public static string Md5By16(string value, Encoding encoding)
    {
        return Md5(value, encoding, 4, 8);
    }

    /// <summary>
    /// Calcula el hash MD5 de una cadena de texto utilizando la codificación especificada.
    /// </summary>
    /// <param name="value">La cadena de texto de la cual se calculará el hash MD5.</param>
    /// <param name="encoding">La codificación que se utilizará para convertir la cadena en un arreglo de bytes.</param>
    /// <param name="startIndex">La posición inicial en el arreglo de bytes desde donde se comenzará a calcular el hash. Puede ser nulo.</param>
    /// <param name="length">La longitud del segmento del arreglo de bytes que se utilizará para calcular el hash. Puede ser nulo.</param>
    /// <returns>Una representación en cadena del hash MD5 calculado, sin guiones.</returns>
    /// <remarks>
    /// Si la cadena de entrada es nula o está vacía, se devolverá una cadena vacía.
    /// El método utiliza la clase <see cref="MD5"/> para calcular el hash.
    /// </remarks>
    private static string Md5(string value, Encoding encoding, int? startIndex, int? length)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;
        var md5 = MD5.Create();
        string result;
        try
        {
            var hash = md5.ComputeHash(encoding.GetBytes(value));
            result = startIndex == null ? BitConverter.ToString(hash) : BitConverter.ToString(hash, startIndex.SafeValue(), length.SafeValue());
        }
        finally
        {
            md5.Clear();
        }
        return result.Replace("-", "");
    }

    /// <summary>
    /// Calcula el hash MD5 de una cadena de texto y lo devuelve en formato de 32 caracteres hexadecimales.
    /// </summary>
    /// <param name="value">La cadena de texto que se va a hashear.</param>
    /// <returns>Una representación en formato de 32 caracteres hexadecimales del hash MD5 de la cadena proporcionada.</returns>
    /// <seealso cref="Md5By32(string, Encoding)"/>
    public static string Md5By32(string value)
    {
        return Md5By32(value, Encoding.UTF8);
    }

    /// <summary>
    /// Calcula el hash MD5 de una cadena de texto y lo devuelve en formato de 32 caracteres hexadecimales.
    /// </summary>
    /// <param name="value">La cadena de texto que se va a hashear.</param>
    /// <param name="encoding">La codificación que se utilizará para convertir la cadena en un arreglo de bytes.</param>
    /// <returns>Una representación en formato de 32 caracteres hexadecimales del hash MD5 de la cadena proporcionada.</returns>
    /// <seealso cref="Md5(string, Encoding, object, object)"/>
    public static string Md5By32(string value, Encoding encoding)
    {
        return Md5(value, encoding, null, null);
    }

    #endregion

    #region Cifrado DES

    public static string DesKey = "#s^un2ye21fcv%|f0XpR,+vh";

    /// <summary>
    /// Realiza la encriptación y desencriptación de un valor utilizando el algoritmo DES.
    /// </summary>
    /// <param name="value">El objeto que se desea encriptar o desencriptar.</param>
    /// <returns>
    /// Una cadena que representa el valor encriptado o desencriptado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza una clave DES predefinida para realizar la operación.
    /// Asegúrese de que el valor proporcionado sea compatible con el tipo de datos esperado.
    /// </remarks>
    /// <seealso cref="DesEncrypt(object, string)"/>
    public static string DesEncrypt(object value)
    {
        return DesEncrypt(value, DesKey);
    }

    /// <summary>
    /// Realiza la encriptación de un valor utilizando el algoritmo DES.
    /// </summary>
    /// <param name="value">El valor a encriptar. Debe ser un objeto que se pueda convertir a una cadena.</param>
    /// <param name="key">La clave utilizada para la encriptación. Debe cumplir con los requisitos del algoritmo DES.</param>
    /// <param name="encoding">La codificación a utilizar para convertir la cadena en bytes. Si es null, se utilizará la codificación predeterminada.</param>
    /// <param name="cipherMode">El modo de cifrado a utilizar. Por defecto es <see cref="CipherMode.ECB"/>.</param>
    /// <param name="paddingMode">El modo de relleno a utilizar. Por defecto es <see cref="PaddingMode.PKCS7"/>.</param>
    /// <returns>Una cadena que representa el valor encriptado, o una cadena vacía si la validación falla.</returns>
    /// <remarks>
    /// Este método utiliza el algoritmo DES para encriptar el valor proporcionado. 
    /// Asegúrese de que la clave cumpla con los requisitos del algoritmo DES para evitar errores de encriptación.
    /// </remarks>
    /// <seealso cref="CreateDesProvider(string, CipherMode, PaddingMode)"/>
    /// <seealso cref="GetEncryptResult(string, Encoding, ICryptoTransform)"/>
    /// <seealso cref="ValidateDes(string, string)"/>
    public static string DesEncrypt(object value, string key, Encoding encoding = null, CipherMode cipherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.PKCS7)
    {
        string text = value.SafeString();
        if (ValidateDes(text, key) == false)
            return string.Empty;
        using var transform = CreateDesProvider(key, cipherMode, paddingMode).CreateEncryptor();
        return GetEncryptResult(text, encoding, transform);
    }

    /// <summary>
    /// Valida si el texto y la clave proporcionados son válidos para el proceso de encriptación DES.
    /// </summary>
    /// <param name="text">El texto que se desea validar. No debe estar vacío.</param>
    /// <param name="key">La clave que se utilizará para la encriptación. Debe tener una longitud de 24 caracteres.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el texto y la clave son válidos; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Esta función comprueba si el texto y la clave no están vacíos y si la longitud de la clave es exactamente 24 caracteres,
    /// que es un requisito para el algoritmo DES.
    /// </remarks>
    private static bool ValidateDes(string text, string key)
    {
        if (text.IsEmpty() || key.IsEmpty())
            return false;
        return key.Length == 24;
    }

    /// <summary>
    /// Crea una instancia de un proveedor TripleDES configurado con la clave, modo de cifrado y modo de relleno especificados.
    /// </summary>
    /// <param name="key">La clave utilizada para la encriptación, que debe tener una longitud compatible con TripleDES.</param>
    /// <param name="cipherMode">El modo de cifrado a utilizar, que determina cómo se procesan los bloques de datos.</param>
    /// <param name="paddingMode">El modo de relleno a utilizar, que especifica cómo se manejan los datos que no llenan un bloque completo.</param>
    /// <returns>Una instancia de <see cref="TripleDES"/> configurada según los parámetros proporcionados.</returns>
    /// <remarks>
    /// Asegúrese de que la longitud de la clave sea adecuada para el algoritmo TripleDES. 
    /// La longitud de la clave debe ser de 16 o 24 bytes para un funcionamiento correcto.
    /// </remarks>
    private static TripleDES CreateDesProvider(string key, CipherMode cipherMode, PaddingMode paddingMode)
    {
        var result = TripleDES.Create();
        result.Key = Encoding.ASCII.GetBytes(key);
        result.Mode = cipherMode;
        result.Padding = paddingMode;
        return result;
    }

    /// <summary>
    /// Obtiene el resultado de la encriptación de una cadena de texto utilizando un transformador criptográfico especificado.
    /// </summary>
    /// <param name="value">La cadena de texto que se desea encriptar.</param>
    /// <param name="encoding">La codificación que se utilizará para convertir la cadena en un arreglo de bytes. Si es nula, se utilizará UTF-8 por defecto.</param>
    /// <param name="transform">El objeto que realiza la transformación criptográfica sobre los datos.</param>
    /// <returns>
    /// Una representación en base64 de los datos encriptados.
    /// </returns>
    /// <remarks>
    /// Este método convierte la cadena de entrada en un arreglo de bytes utilizando la codificación especificada,
    /// aplica la transformación criptográfica y devuelve el resultado en formato base64.
    /// </remarks>
    private static string GetEncryptResult(string value, Encoding encoding, ICryptoTransform transform)
    {
        encoding ??= Encoding.UTF8;
        var bytes = encoding.GetBytes(value);
        var result = transform.TransformFinalBlock(bytes, 0, bytes.Length);
        return System.Convert.ToBase64String(result);
    }

    /// <summary>
    /// Desencripta un valor utilizando una clave DES predeterminada.
    /// </summary>
    /// <param name="value">El objeto que se desea desencriptar.</param>
    /// <returns>Una cadena que representa el valor desencriptado.</returns>
    /// <remarks>
    /// Este método utiliza una clave DES predefinida para realizar la desencriptación.
    /// Asegúrese de que el valor proporcionado sea compatible con el formato esperado.
    /// </remarks>
    /// <seealso cref="DesDecrypt(object, string)"/>
    public static string DesDecrypt(object value)
    {
        return DesDecrypt(value, DesKey);
    }

    /// <summary>
    /// Desencripta un texto utilizando el algoritmo DES (Data Encryption Standard).
    /// </summary>
    /// <param name="value">El valor a desencriptar, que debe ser una cadena segura.</param>
    /// <param name="key">La clave utilizada para la desencriptación. Debe cumplir con los requisitos del algoritmo DES.</param>
    /// <param name="encoding">La codificación utilizada para convertir el texto desencriptado. Si es null, se utilizará la codificación predeterminada.</param>
    /// <param name="cipherMode">El modo de cifrado a utilizar. Por defecto es ECB (Electronic Codebook).</param>
    /// <param name="paddingMode">El modo de relleno a utilizar. Por defecto es PKCS7.</param>
    /// <returns>El texto desencriptado. Si la validación falla, se devuelve una cadena vacía.</returns>
    /// <remarks>
    /// Este método requiere que la clave y el texto sean válidos para el algoritmo DES. 
    /// Si la validación de la clave o el texto falla, se devolverá una cadena vacía.
    /// </remarks>
    /// <seealso cref="CreateDesProvider(string, CipherMode, PaddingMode)"/>
    /// <seealso cref="GetDecryptResult(string, Encoding, ICryptoTransform)"/>
    /// <seealso cref="ValidateDes(string, string)"/>
    public static string DesDecrypt(object value, string key, Encoding encoding = null, CipherMode cipherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.PKCS7)
    {
        string text = value.SafeString();
        if (!ValidateDes(text, key))
            return string.Empty;
        using var transform = CreateDesProvider(key, cipherMode, paddingMode).CreateDecryptor();
        return GetDecryptResult(text, encoding, transform);
    }

    /// <summary>
    /// Desencripta una cadena codificada en Base64 utilizando el transformador criptográfico especificado.
    /// </summary>
    /// <param name="value">La cadena codificada en Base64 que se desea desencriptar.</param>
    /// <param name="encoding">La codificación que se utilizará para convertir el resultado desencriptado en una cadena. Si es null, se utilizará UTF-8 por defecto.</param>
    /// <param name="transform">El objeto que realiza la transformación criptográfica para desencriptar los datos.</param>
    /// <returns>La cadena desencriptada resultante.</returns>
    /// <remarks>
    /// Este método convierte la cadena Base64 en un arreglo de bytes, aplica la transformación criptográfica y luego convierte el resultado en una cadena utilizando la codificación especificada.
    /// </remarks>
    /// <exception cref="FormatException">Se produce si el valor proporcionado no es una cadena Base64 válida.</exception>
    /// <exception cref="CryptographicException">Se produce si hay un error durante el proceso de desencriptación.</exception>
    private static string GetDecryptResult(string value, Encoding encoding, ICryptoTransform transform)
    {
        encoding ??= Encoding.UTF8;
        var bytes = System.Convert.FromBase64String(value);
        var result = transform.TransformFinalBlock(bytes, 0, bytes.Length);
        return encoding.GetString(result);
    }

    #endregion

    #region Cifrado AES

    private static byte[] _iv;
    /// <summary>
    /// Obtiene el vector de inicialización (IV) utilizado en la criptografía.
    /// </summary>
    /// <remarks>
    /// El vector de inicialización es un arreglo de bytes que se utiliza para asegurar que el mismo texto plano encriptado con la misma clave produzca diferentes resultados. 
    /// Este método garantiza que el IV se inicialice una sola vez y se reutilice en futuras llamadas.
    /// </remarks>
    /// <returns>
    /// Un arreglo de bytes que representa el vector de inicialización.
    /// </returns>
    private static byte[] Iv
    {
        get
        {
            if (_iv == null)
            {
                var size = 16;
                _iv = new byte[size];
                for (int i = 0; i < size; i++)
                    _iv[i] = 0;
            }
            return _iv;
        }
    }

    public static string AesKey = "QaP1AF8utIarcBqdhYTZpVGbiNQ9M6IL";

    /// <summary>
    /// Encripta un valor utilizando el algoritmo AES con una clave predefinida.
    /// </summary>
    /// <param name="value">El valor que se desea encriptar.</param>
    /// <returns>El valor encriptado en formato de cadena.</returns>
    /// <remarks>
    /// Este método utiliza una clave AES predefinida para realizar la encriptación.
    /// Asegúrese de que el valor proporcionado no sea nulo o vacío para evitar excepciones.
    /// </remarks>
    /// <seealso cref="AesKey"/>
    public static string AesEncrypt(string value)
    {
        return AesEncrypt(value, AesKey);
    }

    /// <summary>
    /// Cifra un valor utilizando el algoritmo AES con los parámetros especificados.
    /// </summary>
    /// <param name="value">El texto que se desea cifrar.</param>
    /// <param name="key">La clave utilizada para el cifrado. Debe ser una cadena válida.</param>
    /// <param name="encoding">La codificación a utilizar para convertir el texto en bytes. Si es <c>null</c>, se utilizará la codificación predeterminada.</param>
    /// <param name="cipherMode">El modo de cifrado a utilizar. Por defecto es <see cref="CipherMode.CBC"/>.</param>
    /// <param name="paddingMode">El modo de relleno a utilizar. Por defecto es <see cref="PaddingMode.PKCS7"/>.</param>
    /// <param name="iv">Vector de inicialización a utilizar. Si es <c>null</c>, se utilizará el valor predeterminado.</param>
    /// <returns>El texto cifrado en formato de cadena.</returns>
    /// <remarks>
    /// Este método requiere que la clave y el valor a cifrar no estén vacíos. 
    /// Si alguno de ellos está vacío, se devolverá una cadena vacía.
    /// </remarks>
    /// <seealso cref="CreateAes(string, CipherMode, PaddingMode, byte[])"/>
    /// <seealso cref="GetEncryptResult(string, Encoding, ICryptoTransform)"/>
    public static string AesEncrypt(string value, string key, Encoding encoding = null, CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.PKCS7, byte[] iv = null)
    {
        if (value.IsEmpty() || key.IsEmpty())
            return string.Empty;
        iv ??= Iv;
        var aes = CreateAes(key, cipherMode, paddingMode, iv);
        using var transform = aes.CreateEncryptor(aes.Key, aes.IV);
        return GetEncryptResult(value, encoding, transform);
    }

    /// <summary>
    /// Crea una instancia de un objeto Aes configurado con la clave, modo de cifrado, modo de relleno y vector de inicialización especificados.
    /// </summary>
    /// <param name="key">La clave utilizada para el cifrado, representada como una cadena de texto.</param>
    /// <param name="cipherMode">El modo de cifrado que se utilizará (por ejemplo, CBC, ECB).</param>
    /// <param name="paddingMode">El modo de relleno que se utilizará para el cifrado.</param>
    /// <param name="iv">El vector de inicialización utilizado en el proceso de cifrado, representado como un arreglo de bytes.</param>
    /// <returns>Una instancia de <see cref="Aes"/> configurada con los parámetros proporcionados.</returns>
    /// <remarks>
    /// Asegúrese de que la longitud de la clave sea adecuada para el algoritmo Aes (128, 192 o 256 bits).
    /// El vector de inicialización debe tener una longitud de 16 bytes para el Aes.
    /// </remarks>
    private static Aes CreateAes(string key, CipherMode cipherMode, PaddingMode paddingMode, byte[] iv)
    {
        var result = Aes.Create();
        result.Key = Encoding.ASCII.GetBytes(key);
        result.Mode = cipherMode;
        result.Padding = paddingMode;
        result.IV = iv;
        return result;
    }

    /// <summary>
    /// Desencripta un valor encriptado utilizando el algoritmo AES.
    /// </summary>
    /// <param name="value">El valor encriptado que se desea desencriptar.</param>
    /// <returns>El valor desencriptado en formato de cadena.</returns>
    /// <seealso cref="AesKey"/>
    public static string AesDecrypt(string value)
    {
        return AesDecrypt(value, AesKey);
    }

    /// <summary>
    /// Desencripta una cadena de texto utilizando el algoritmo AES.
    /// </summary>
    /// <param name="value">La cadena de texto cifrada que se desea desencriptar.</param>
    /// <param name="key">La clave utilizada para el proceso de desencriptación.</param>
    /// <param name="encoding">La codificación a utilizar para convertir la cadena desencriptada. Si es <c>null</c>, se utilizará la codificación por defecto.</param>
    /// <param name="cipherMode">El modo de cifrado a utilizar. Por defecto es <see cref="CipherMode.CBC"/>.</param>
    /// <param name="paddingMode">El modo de relleno a utilizar. Por defecto es <see cref="PaddingMode.PKCS7"/>.</param>
    /// <param name="iv">El vector de inicialización a utilizar. Si es <c>null</c>, se utilizará el valor predeterminado.</param>
    /// <returns>La cadena de texto desencriptada. Si la cadena de entrada o la clave están vacías, se devuelve una cadena vacía.</returns>
    /// <remarks>
    /// Este método utiliza el algoritmo AES para desencriptar datos. Asegúrese de que la clave y el vector de inicialización sean correctos para evitar errores de desencriptación.
    /// </remarks>
    /// <seealso cref="CreateAes(string, CipherMode, PaddingMode, byte[])"/>
    /// <seealso cref="GetDecryptResult(string, Encoding, ICryptoTransform)"/>
    public static string AesDecrypt(string value, string key, Encoding encoding = null, CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingMode = PaddingMode.PKCS7, byte[] iv = null)
    {
        if (value.IsEmpty() || key.IsEmpty())
            return string.Empty;
        iv ??= Iv;
        var aes = CreateAes(key, cipherMode, paddingMode, iv);
        using var transform = aes.CreateDecryptor(aes.Key, aes.IV);
        return GetDecryptResult(value, encoding, transform);
    }

    #endregion

    #region HmacSha256 encriptación

    /// <summary>
    /// Calcula el hash HMAC-SHA256 de un valor dado utilizando una clave especificada.
    /// </summary>
    /// <param name="value">El valor que se desea hashear.</param>
    /// <param name="key">La clave utilizada para el cálculo del HMAC.</param>
    /// <param name="encoding">La codificación que se utilizará para convertir el valor y la clave a bytes. Si es null, se utilizará UTF8 por defecto.</param>
    /// <param name="allowEmptyValue">Indica si se permite que el valor o la clave estén vacíos. Si es false y alguno de ellos está vacío, se devolverá una cadena vacía.</param>
    /// <returns>Una representación hexadecimal del hash HMAC-SHA256 calculado.</returns>
    /// <remarks>
    /// Este método utiliza el algoritmo HMAC-SHA256 para generar un hash seguro de un valor dado.
    /// Si el valor o la clave están vacíos y <paramref name="allowEmptyValue"/> es false, se devolverá una cadena vacía.
    /// </remarks>
    public static string HmacSha256(string value, string key, Encoding encoding = null, bool allowEmptyValue = false)
    {
        if (value.IsEmpty() || key.IsEmpty())
        {
            if (allowEmptyValue == false)
                return string.Empty;
            value = string.Empty;
        }
        encoding ??= Encoding.UTF8;
        var sha256 = new HMACSHA256(Encoding.ASCII.GetBytes(key));
        var hash = sha256.ComputeHash(encoding.GetBytes(value));
        return string.Join("", hash.ToList().Select(t => t.ToString("x2")).ToArray());
    }

    #endregion

    #region Cifrado RSA

    /// <summary>
    /// Firma digitalmente un valor utilizando una clave privada RSA.
    /// </summary>
    /// <param name="value">El valor que se desea firmar.</param>
    /// <param name="
    public static string RsaSign(string value, string privateKey, Encoding encoding = null, HashAlgorithmName? hashAlgorithm = null, RSAKeyType rsaKeyType = RSAKeyType.Pkcs1)
    {
        if (value.IsEmpty() || privateKey.IsEmpty())
            return string.Empty;
        var rsa = RSA.Create();
        ImportPrivateKey(rsa, privateKey, rsaKeyType);
        encoding ??= Encoding.UTF8;
        hashAlgorithm ??= HashAlgorithmName.SHA1;
        var result = rsa.SignData(encoding.GetBytes(value), hashAlgorithm.Value, RSASignaturePadding.Pkcs1);
        return System.Convert.ToBase64String(result);
    }

    /// <summary>
    /// Importa una clave privada en un objeto RSA.
    /// </summary>
    /// <param name="rsa">El objeto RSA en el que se importará la clave privada.</param>
    /// <param name="
    private static void ImportPrivateKey(RSA rsa, string privateKey, RSAKeyType rsaKeyType)
    {
        rsa.ImportPrivateKey(rsaKeyType, privateKey);
    }

    /// <summary>
    /// Verifica una firma digital utilizando una clave pública RSA.
    /// </summary>
    /// <param name="value">El valor original que se firmó.</param>
    /// <param name="
    public static bool RsaVerify(string value, string publicKey, string sign, Encoding encoding = null, HashAlgorithmName? hashAlgorithm = null)
    {
        if (value.IsEmpty() || publicKey.IsEmpty() || sign.IsEmpty())
            return false;
        var rsa = RSA.Create();
        ImportPublicKey(rsa, publicKey);
        encoding ??= Encoding.UTF8;
        var signData = System.Convert.FromBase64String(sign);
        hashAlgorithm ??= HashAlgorithmName.SHA1;
        return rsa.VerifyData(encoding.GetBytes(value), signData, hashAlgorithm.Value, RSASignaturePadding.Pkcs1);
    }

    /// <summary>
    /// Importa una clave pública en formato base64 a un objeto RSA.
    /// </summary>
    /// <param name="rsa">El objeto RSA en el que se importará la clave pública.</param>
    /// <param name="
    private static void ImportPublicKey(RSA rsa, string publicKey)
    {
        var key = System.Convert.FromBase64String(publicKey);
        rsa.ImportSubjectPublicKeyInfo(key, out _);
    }

    /// <summary>
    /// Encripta un valor utilizando una clave pública RSA.
    /// </summary>
    /// <param name="value">El valor que se desea encriptar.</param>
    /// <param name="
    public static string RsaEncrypt(string value, string publicKey)
    {
        if (value.IsEmpty() || publicKey.IsEmpty())
            return string.Empty;
        var rsa = RSA.Create();
        ImportPublicKey(rsa, publicKey);
        return rsa.EncryptBigData(value, RSAEncryptionPadding.Pkcs1);
    }

    /// <summary>
    /// Desencripta un valor cifrado utilizando una clave privada RSA.
    /// </summary>
    /// <param name="value">El valor cifrado que se desea desencriptar.</param>
    /// <param name="
    public static string RsaDecrypt(string value, string privateKey)
    {
        if (value.IsEmpty() || privateKey.IsEmpty())
            return string.Empty;
        var rsa = RSA.Create();
        ImportPrivateKey(rsa, privateKey, RSAKeyType.Pkcs1);
        return rsa.DecryptBigData(value, RSAEncryptionPadding.Pkcs1);
    }

    #endregion

    #region Generar un par de claves públicas y privadas RSA.

    /// <summary>
    /// Crea un par de claves RSA, incluyendo una clave pública y una clave privada.
    /// </summary>
    /// <returns>
    /// Una tupla que contiene la clave pública en formato PEM y la clave privada en formato PKCS#1.
    /// </returns>
    public static (string, string) CreateRsaKey()
    {
        var rsa = RSA.Create();
        var publicKey = FormatPublicKey(rsa.ExportSubjectPublicKeyInfoPem());
        var privateKey = rsa.ExportPrivateKey(RSAKeyType.Pkcs1);
        return (publicKey, privateKey);
    }

    /// <summary>
    /// Formatea una clave pública o privada eliminando las cabeceras y pies de las claves.
    /// </summary>
    /// <param name="key">La clave en formato PEM que se desea formatear.</param>
    /// <returns>La clave formateada sin las cabeceras y pies.</returns>
    /// <remarks>
    /// Este método es útil para limpiar las claves antes de su uso en operaciones criptográficas.
    /// Se eliminan las líneas que indican el inicio y el fin de las claves, así como los saltos de línea.
    /// </remarks>
    private static string FormatPublicKey(string key)
    {
        return key.Replace("-----BEGIN RSA PRIVATE KEY-----", "")
            .Replace("-----END RSA PRIVATE KEY-----", "")
            .Replace("-----BEGIN RSA PUBLIC KEY-----", "")
            .Replace("-----END RSA PUBLIC KEY-----", "")
            .Replace("-----BEGIN PRIVATE KEY-----", "")
            .Replace("-----END PRIVATE KEY-----", "")
            .Replace("-----BEGIN PUBLIC KEY-----", "")
            .Replace("-----END PUBLIC KEY-----", "")
            .Replace(Environment.NewLine, "");
    }

    #endregion
}