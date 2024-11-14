namespace Util.Security; 

/// <summary>
/// Proporciona una colección de constantes que representan los tipos de reclamaciones.
/// </summary>
/// <remarks>
/// Esta clase contiene constantes que se utilizan para identificar los tipos de reclamaciones en el contexto de la autenticación y autorización.
/// </remarks>
public static class ClaimTypes {
    /// <summary>
    /// Representa el identificador de usuario.
    /// </summary>
    /// <remarks>
    /// Esta propiedad estática permite acceder y modificar el identificador de usuario a nivel de aplicación.
    /// El valor predeterminado es "sub".
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el identificador de usuario.
    /// </value>
    public static string UserId { get; set; } = "sub";
    /// <summary>
    /// Representa el nombre de la entidad.
    /// </summary>
    /// <remarks>
    /// Esta propiedad estática permite acceder y modificar el nombre de manera global.
    /// El valor predeterminado es "name".
    /// </remarks>
    /// <value>
    /// Una cadena que representa el nombre.
    /// </value>
    public static string Name { get; set; } = "name";
    /// <summary>
    /// Representa una dirección de correo electrónico.
    /// </summary>
    /// <remarks>
    /// Esta propiedad estática permite acceder y modificar la dirección de correo electrónico 
    /// utilizada en la aplicación. Por defecto, se inicializa con el valor "email".
    /// </remarks>
    /// <value>
    /// Una cadena que representa la dirección de correo electrónico.
    /// </value>
    public static string Email { get; set; } = "email";
    /// <summary>
    /// Representa un número de teléfono.
    /// </summary>
    /// <remarks>
    /// Esta propiedad estática almacena un número de teléfono en formato de cadena.
    /// El valor predeterminado es "phone_number".
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el número de teléfono.
    /// </value>
    public static string PhoneNumber { get; set; } = "phone_number";
    /// <summary>
    /// Representa el identificador de la aplicación.
    /// </summary>
    /// <remarks>
    /// Este identificador se utiliza para identificar de manera única la aplicación en el sistema.
    /// </remarks>
    /// <value>
    /// Un string que contiene el identificador de la aplicación, por defecto es "application_id".
    /// </value>
    public static string ApplicationId { get; set; } = "application_id";
    /// <summary>
    /// Representa el código de la aplicación.
    /// </summary>
    /// <remarks>
    /// Esta propiedad estática permite acceder y modificar el código de la aplicación desde cualquier parte del código.
    /// El valor predeterminado es "application_code".
    /// </remarks>
    /// <value>
    /// Una cadena que representa el código de la aplicación.
    /// </value>
    public static string ApplicationCode { get; set; } = "application_code";
    /// <summary>
    /// Obtiene o establece el nombre de la aplicación.
    /// </summary>
    /// <value>
    /// El nombre de la aplicación. El valor predeterminado es "application_name".
    /// </value>
    public static string ApplicationName { get; set; } = "application_name";
    /// <summary>
    /// Representa el identificador del inquilino (tenant).
    /// </summary>
    /// <remarks>
    /// Esta propiedad estática se utiliza para almacenar el ID del inquilino, que puede ser utilizado en diversas operaciones relacionadas con la autenticación y autorización.
    /// </remarks>
    /// <value>
    /// Un string que representa el ID del inquilino. El valor predeterminado es "tenant_id".
    /// </value>
    public static string TenantId { get; set; } = "tenant_id";
    /// <summary>
    /// Representa el rol de un usuario en el sistema.
    /// </summary>
    /// <remarks>
    /// Esta propiedad estática permite obtener o establecer el rol actual. 
    /// El valor predeterminado es "role".
    /// </remarks>
    /// <value>
    /// Una cadena que representa el rol del usuario.
    /// </value>
    public static string Role { get; set; } = "role";
}