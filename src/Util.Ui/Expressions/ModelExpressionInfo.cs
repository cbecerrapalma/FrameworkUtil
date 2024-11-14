namespace Util.Ui.Expressions; 

/// <summary>
/// Representa la información de una expresión de modelo.
/// </summary>
public class ModelExpressionInfo {
    /// <summary>
    /// Obtiene o establece el nombre de la propiedad de expresión.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para definir el nombre de una propiedad que se puede utilizar en expresiones, 
    /// facilitando la vinculación de datos y la reflexión en el contexto de la programación.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el nombre de la propiedad de expresión.
    /// </value>
    public string ExpressionPropertyName { get; set; }
    /// <summary>
    /// Obtiene o establece el tipo del modelo.
    /// </summary>
    /// <value>
    /// El tipo del modelo.
    /// </value>
    public Type ModelType { get; set; }
    /// <summary>
    /// Obtiene o establece la información del miembro que representa una propiedad.
    /// </summary>
    /// <value>
    /// Un objeto <see cref="MemberInfo"/> que contiene la información de la propiedad.
    /// </value>
    public MemberInfo Property { get; set; }
    /// <summary>
    /// Obtiene o establece el nombre de la propiedad original.
    /// </summary>
    /// <value>
    /// El nombre de la propiedad original como una cadena.
    /// </value>
    public string OriginalPropertyName { get; set; }
    /// <summary>
    /// Obtiene o establece el nombre de la propiedad.
    /// </summary>
    /// <value>
    /// El nombre de la propiedad como una cadena.
    /// </value>
    public string PropertyName { get; set; }
    /// <summary>
    /// Obtiene el nombre de la propiedad de forma segura basado en el nombre del modelo.
    /// </summary>
    /// <value>
    /// Un <see cref="string"/> que representa el nombre de la propiedad segura.
    /// </value>
    /// <remarks>
    /// Este método utiliza el nombre del modelo para generar un nombre de propiedad que es seguro para su uso,
    /// evitando caracteres no válidos o problemas de formato.
    /// </remarks>
    public string SafePropertyName => GetSafePropertyName( ModelName );
    /// <summary>
    /// Obtiene o establece el nombre de la última propiedad.
    /// </summary>
    /// <value>
    /// El nombre de la última propiedad como una cadena.
    /// </value>
    public string LastPropertyName { get; set; }
    /// <summary>
    /// Obtiene o establece el nombre que se mostrará.
    /// </summary>
    /// <value>
    /// Un <see cref="string"/> que representa el nombre a mostrar.
    /// </value>
    public string DisplayName { get; set; }
    /// <summary>
    /// Obtiene o establece el nombre del modelo.
    /// </summary>
    /// <remarks>
    /// El valor predeterminado del nombre del modelo es "model".
    /// </remarks>
    public string ModelName { get; set; } = "model";
    /// <summary>
    /// Obtiene o establece un valor que indica si la propiedad representa una contraseña.
    /// </summary>
    /// <value>
    /// <c>true</c> si la propiedad es una contraseña; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsPassword { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si es un booleano.
    /// </summary>
    /// <value>
    /// <c>true</c> si es un booleano; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsBool { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si el tipo es un enumerado.
    /// </summary>
    /// <value>
    /// <c>true</c> si el tipo es un enumerado; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsEnum { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si la propiedad representa una fecha.
    /// </summary>
    /// <value>
    /// <c>true</c> si la propiedad representa una fecha; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsDate { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si el objeto es un entero.
    /// </summary>
    /// <value>
    /// <c>true</c> si el objeto es un entero; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsInt { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si el objeto es un número.
    /// </summary>
    /// <value>
    /// <c>true</c> si el objeto es un número; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsNumber { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si el elemento es obligatorio.
    /// </summary>
    /// <value>
    /// <c>true</c> si el elemento es obligatorio; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsRequired { get; set; }
    /// <summary>
    /// Obtiene o establece el mensaje que se mostrará cuando un campo sea obligatorio.
    /// </summary>
    /// <remarks>
    /// Este mensaje se utiliza para informar al usuario que debe completar el campo correspondiente.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el mensaje requerido.
    /// </value>
    public string RequiredMessage { get; set; }
    /// <summary>
    /// Obtiene o establece la longitud mínima.
    /// </summary>
    /// <remarks>
    /// Este propiedad permite definir un valor mínimo para la longitud. 
    /// Si no se establece ningún valor, será nulo.
    /// </remarks>
    /// <value>
    /// Un entero que representa la longitud mínima, o null si no se ha definido.
    /// </value>
    public int? MinLength { get; set; }
    /// <summary>
    /// Obtiene o establece el mensaje que se muestra cuando la longitud de una cadena es menor que la mínima permitida.
    /// </summary>
    /// <remarks>
    /// Este mensaje puede ser utilizado para proporcionar retroalimentación al usuario sobre la longitud mínima requerida
    /// para un campo de entrada específico. Es útil en validaciones de formularios donde se necesita asegurar que
    /// el usuario ingrese una cantidad suficiente de caracteres.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el mensaje de longitud mínima.
    /// </value>
    public string MinLengthMessage { get; set; }
    /// <summary>
    /// Obtiene o establece la longitud máxima permitida.
    /// </summary>
    /// <remarks>
    /// Este valor puede ser nulo, lo que indica que no hay un límite de longitud establecido.
    /// </remarks>
    /// <value>
    /// Un entero que representa la longitud máxima, o null si no se ha establecido un límite.
    /// </value>
    public int? MaxLength { get; set; }
    /// <summary>
    /// Obtiene o establece el mensaje que se mostrará cuando se supere la longitud máxima permitida.
    /// </summary>
    /// <value>
    /// Un <see cref="string"/> que representa el mensaje de longitud máxima.
    /// </value>
    public string MaxLengthMessage { get; set; }
    /// <summary>
    /// Obtiene o establece el valor mínimo.
    /// </summary>
    /// <remarks>
    /// Esta propiedad puede ser utilizada para definir un límite inferior en un rango de valores.
    /// </remarks>
    /// <value>
    /// Un objeto que representa el valor mínimo.
    /// </value>
    public object Min { get; set; }
    /// <summary>
    /// Obtiene o establece el mensaje mínimo.
    /// </summary>
    /// <value>
    /// Un string que representa el mensaje mínimo.
    /// </value>
    public string MinMessage { get; set; }
    /// <summary>
    /// Representa el valor máximo permitido.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar el valor máximo que puede ser asignado.
    /// </remarks>
    /// <value>
    /// Un objeto que representa el valor máximo.
    /// </value>
    public object Max { get; set; }
    /// <summary>
    /// Obtiene o establece el mensaje máximo.
    /// </summary>
    /// <value>
    /// Un string que representa el mensaje máximo.
    /// </value>
    public string MaxMessage { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si el objeto representa una dirección de correo electrónico válida.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para validar si el formato de la dirección de correo electrónico es correcto.
    /// </remarks>
    /// <value>
    /// <c>true</c> si el objeto representa una dirección de correo electrónico válida; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsEmail { get; set; }
    /// <summary>
    /// Obtiene o establece el mensaje de correo electrónico.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar el contenido del mensaje que se enviará por correo electrónico.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el mensaje de correo electrónico.
    /// </value>
    public string EmailMessage { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si el número es un teléfono.
    /// </summary>
    /// <value>
    /// <c>true</c> si el número es un teléfono; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsPhone { get; set; }
    /// <summary>
    /// Obtiene o establece el mensaje de teléfono.
    /// </summary>
    /// <value>
    /// Un string que representa el mensaje de teléfono.
    /// </value>
    public string PhoneMessage { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si el objeto representa una tarjeta de identificación.
    /// </summary>
    /// <value>
    /// <c>true</c> si el objeto es una tarjeta de identificación; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsIdCard { get; set; }
    /// <summary>
    /// Obtiene o establece el mensaje de la tarjeta de identificación.
    /// </summary>
    /// <value>
    /// Un <see cref="string"/> que representa el mensaje de la tarjeta de identificación.
    /// </value>
    public string IdCardMessage { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si la propiedad representa una URL.
    /// </summary>
    /// <value>
    /// <c>true</c> si la propiedad es una URL; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsUrl { get; set; }
    /// <summary>
    /// Obtiene o establece el mensaje de URL.
    /// </summary>
    /// <value>
    /// Una cadena que representa el mensaje de URL.
    /// </value>
    public string UrlMessage { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si la expresión es una expresión regular.
    /// </summary>
    /// <value>
    /// <c>true</c> si la expresión es una expresión regular; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsRegularExpression { get; set; }
    /// <summary>
    /// Obtiene o establece el patrón asociado.
    /// </summary>
    /// <remarks>
    /// Este propiedad permite definir un patrón que puede ser utilizado para validaciones o 
    /// configuraciones específicas dentro de la aplicación.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el patrón.
    /// </value>
    public string Pattern { get; set; }
    /// <summary>
    /// Obtiene o establece el mensaje de expresión regular.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar un mensaje que describe la expresión regular
    /// que se está utilizando en la validación de datos. Puede ser útil para mostrar mensajes
    /// de error o advertencia al usuario cuando la entrada no cumple con el formato esperado.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el mensaje de expresión regular.
    /// </value>
    public string RegularExpressionMessage { get; set; }

    /// <summary>
    /// Obtiene un nombre de propiedad seguro a partir del nombre de propiedad original.
    /// </summary>
    /// <param name="model">El nombre del modelo que se utilizará como prefijo. Por defecto es "model".</param>
    /// <returns>
    /// Un string que representa el nombre de propiedad seguro, o null si el nombre de propiedad original está vacío.
    /// </returns>
    /// <remarks>
    /// Este método divide el nombre de propiedad original en partes utilizando el carácter '.' como separador,
    /// convierte cada parte en un nombre seguro y luego los une utilizando '&&' como separador.
    /// </remarks>
    /// <seealso cref="ModelExpressionHelper.ConvertName(string)"/>
    public string GetSafePropertyName( string model = "model" ) {
        if ( OriginalPropertyName.IsEmpty() )
            return null;
        var result = new List<string>();
        var temp = new List<string> { model };
        var array = OriginalPropertyName.Split( '.' );
        foreach ( var item in array ) {
            temp.Add( ModelExpressionHelper.ConvertName( item ) );
            result.Add( temp.Join( separator: "." ) );
        }
        return result.Join( separator: "&&" );
    }
}