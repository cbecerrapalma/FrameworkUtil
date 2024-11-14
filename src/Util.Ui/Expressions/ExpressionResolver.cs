namespace Util.Ui.Expressions;

/// <summary>
/// Clase que implementa la interfaz <see cref="IExpressionResolver"/>.
/// Proporciona métodos para resolver expresiones matemáticas y lógicas.
/// </summary>
public class ExpressionResolver : IExpressionResolver
{
    /// <inheritdoc />
    /// <summary>
    /// Resuelve la información del modelo a partir de una expresión de modelo dada.
    /// </summary>
    /// <param name="expression">La expresión de modelo que se va a resolver.</param>
    /// <returns>
    /// Un objeto <see cref="ModelExpressionInfo"/> que contiene la información resuelta del modelo.
    /// Si la expresión es nula, se devuelve un objeto vacío.
    /// </returns>
    /// <remarks>
    /// Este método extrae y valida diversas propiedades del modelo, incluyendo el tipo de modelo,
    /// el nombre de la propiedad original, y varias validaciones como requerimientos, longitud,
    /// rango, formato de correo electrónico, entre otros.
    /// </remarks>
    /// <seealso cref="ModelExpression"/>
    /// <seealso cref="ModelExpressionInfo"/>
    public ModelExpressionInfo Resolve(ModelExpression expression)
    {
        var result = new ModelExpressionInfo();
        if (expression == null)
            return result;
        result.ModelType = expression.Metadata.ModelType;
        result.OriginalPropertyName = expression.Name;
        result.PropertyName = GetPropertyName(expression.Name);
        result.ModelName = GetModelName(expression.ModelExplorer);
        var property = GetProperty(expression.Metadata);
        if (property == null)
            return result;
        result.Property = property;
        result.LastPropertyName = GetLastPropertyName(property);
        result.DisplayName = GetDisplayName(property);
        result.IsPassword = GetIsPassword(property);
        result.IsBool = GetIsBool(property);
        result.IsEnum = GetIsEnum(property);
        result.IsDate = GetIsDate(property);
        result.IsInt = GetIsInt(property);
        result.IsNumber = GetIsNumber(property);
        ValidateRequired(result, property);
        ValidateLength(result, property);
        ValidateRange(result, property);
        ValidateEmail(result, property);
        ValidatePhone(result, property);
        ValidateIdCard(result, property);
        ValidateUrl(result, property);
        ValidateRegularExpression(result, property);
        return result;
    }

    /// <summary>
    /// Obtiene la información del miembro correspondiente a la propiedad especificada en el metadato.
    /// </summary>
    /// <param name="metadata">El metadato que contiene la información de la propiedad.</param>
    /// <returns>
    /// Un objeto <see cref="MemberInfo"/> que representa la propiedad, o <c>null</c> si no se encuentra la propiedad 
    /// o si el metadato es <c>null</c> o no tiene un tipo de contenedor definido.
    /// </returns>
    /// <remarks>
    /// Este método busca en el tipo de contenedor del metadato la propiedad cuyo nombre se especifica en el 
    /// metadato. Si no se encuentra ningún miembro, se devuelve <c>null</c>.
    /// </remarks>
    protected virtual MemberInfo GetProperty(ModelMetadata metadata)
    {
        if (metadata == null)
            return null;
        if (metadata.ContainerType == null)
            return null;
        var members = metadata.ContainerType.GetMember(metadata.PropertyName!);
        return members.Length == 0 ? null : members[0];
    }

    /// <summary>
    /// Obtiene el nombre de la propiedad a partir del nombre de propiedad original.
    /// </summary>
    /// <param name="originalPropertyName">El nombre de la propiedad original que se desea convertir.</param>
    /// <returns>
    /// El nombre de la propiedad convertido, o null si el nombre de propiedad original está vacío.
    /// </returns>
    /// <remarks>
    /// Este método divide el nombre de la propiedad original en partes utilizando el carácter '.' como separador,
    /// y luego convierte cada parte utilizando el método <see cref="ModelExpressionHelper.ConvertName"/>.
    /// Finalmente, une las partes convertidas en un solo nombre de propiedad.
    /// </remarks>
    protected virtual string GetPropertyName(string originalPropertyName)
    {
        if (originalPropertyName.IsEmpty())
            return null;
        var array = originalPropertyName.Split('.');
        return array.Select(ModelExpressionHelper.ConvertName).Join(separator: ".");
    }

    /// <summary>
    /// Obtiene el nombre del modelo a partir del explorador de modelos proporcionado.
    /// </summary>
    /// <param name="explorer">El explorador de modelos que contiene información sobre el modelo.</param>
    /// <returns>
    /// El nombre del modelo si se encuentra; de lo contrario, devuelve un valor predeterminado.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el explorador es nulo y si el contenedor del explorador tiene un tipo de modelo válido.
    /// Si se encuentra un atributo de modelo, se devuelve el nombre del modelo especificado en dicho atributo.
    /// Si no se encuentra un nombre de modelo, se devuelve un valor predeterminado.
    /// </remarks>
    protected virtual string GetModelName(ModelExplorer explorer)
    {
        var result = GetDefaultModel();
        if (explorer == null)
            return result;
        if (explorer.Container == null)
            return result;
        if (explorer.Container.ModelType == null)
            return result;
        var modelAttribute = explorer.Container.ModelType.GetCustomAttribute<ModelAttribute>();
        if (modelAttribute != null)
            return modelAttribute.Model;
        var modelName = ModelName.Get(explorer.Container.ModelType);
        if (modelName.IsEmpty())
            return result;
        return modelName;
    }

    /// <summary>
    /// Obtiene el modelo predeterminado.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el modelo predeterminado.
    /// </returns>
    protected virtual string GetDefaultModel()
    {
        return "model";
    }

    /// <summary>
    /// Obtiene el nombre de la última propiedad a partir de la información del miembro proporcionado.
    /// </summary>
    /// <param name="property">El miembro de información que representa la propiedad de la cual se desea obtener el nombre.</param>
    /// <returns>
    /// El nombre de la propiedad convertido a un formato adecuado.
    /// </returns>
    protected virtual string GetLastPropertyName(MemberInfo property)
    {
        return ModelExpressionHelper.ConvertName(property.Name);
    }

    /// <summary>
    /// Obtiene el nombre para mostrar de una propiedad a partir de su información de miembro.
    /// </summary>
    /// <param name="property">La información del miembro que representa la propiedad.</param>
    /// <returns>El nombre para mostrar de la propiedad, o una descripción si no se encuentra un nombre para mostrar.</returns>
    protected virtual string GetDisplayName(MemberInfo property)
    {
        return Util.Helpers.Reflection.GetDisplayNameOrDescription(property);
    }

    /// <summary>
    /// Determina si la propiedad especificada es un campo de contraseña.
    /// </summary>
    /// <param name="property">La información del miembro que representa la propiedad a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la propiedad es un campo de contraseña; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si la propiedad tiene el atributo <see cref="DataTypeAttribute"/> 
    /// y si el tipo de dato especificado es <see cref="DataType.Password"/>.
    /// </remarks>
    protected virtual bool GetIsPassword(MemberInfo property)
    {
        var attribute = property.GetCustomAttribute<DataTypeAttribute>();
        if (attribute == null)
            return false;
        if (attribute.DataType == DataType.Password)
            return true;
        return false;
    }

    /// <summary>
    /// Determina si el miembro especificado es de tipo booleano.
    /// </summary>
    /// <param name="property">El miembro del que se desea verificar el tipo.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el miembro es de tipo booleano; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="Util.Helpers.Reflection"/> para realizar la verificación.
    /// </remarks>
    protected virtual bool GetIsBool(MemberInfo property)
    {
        return Util.Helpers.Reflection.IsBool(property);
    }

    /// <summary>
    /// Determina si el miembro especificado es un enumerador.
    /// </summary>
    /// <param name="property">El miembro del que se desea verificar si es un enumerador.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el miembro es un enumerador; de lo contrario, <c>false</c>.
    /// </returns>
    protected virtual bool GetIsEnum(MemberInfo property)
    {
        return Util.Helpers.Reflection.IsEnum(property);
    }

    /// <summary>
    /// Determina si la propiedad especificada es de tipo fecha.
    /// </summary>
    /// <param name="property">La información del miembro que representa la propiedad a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la propiedad es de tipo fecha; de lo contrario, <c>false</c>.
    /// </returns>
    protected virtual bool GetIsDate(MemberInfo property)
    {
        return Util.Helpers.Reflection.IsDate(property);
    }

    /// <summary>
    /// Determina si el miembro especificado es de tipo entero.
    /// </summary>
    /// <param name="property">El miembro del que se desea verificar el tipo.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el miembro es de tipo entero; de lo contrario, <c>false</c>.
    /// </returns>
    protected virtual bool GetIsInt(MemberInfo property)
    {
        return Util.Helpers.Reflection.IsInt(property);
    }

    /// <summary>
    /// Determina si el miembro especificado es un número.
    /// </summary>
    /// <param name="property">El miembro que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el miembro es un número; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="Util.Helpers.Reflection"/> para realizar la verificación.
    /// </remarks>
    protected virtual bool GetIsNumber(MemberInfo property)
    {
        return Util.Helpers.Reflection.IsNumber(property);
    }

    /// <summary>
    /// Valida si una propiedad está marcada como requerida mediante el atributo <see cref="RequiredAttribute"/>.
    /// </summary>
    /// <param name="result">El objeto <see cref="ModelExpressionInfo"/> que contiene la información del modelo.</param>
    /// <param name="property">La información del miembro que se va a validar.</param>
    /// <remarks>
    /// Si la propiedad no tiene el atributo <see cref="RequiredAttribute"/>, no se realiza ninguna acción.
    /// Si el atributo está presente, se establece la propiedad <see cref="ModelExpressionInfo.IsRequired"/> en verdadero
    /// y se asigna el mensaje de error correspondiente a <see cref="ModelExpressionInfo.RequiredMessage"/>.
    /// </remarks>
    protected virtual void ValidateRequired(ModelExpressionInfo result, MemberInfo property)
    {
        var attribute = property.GetCustomAttribute<RequiredAttribute>();
        if (attribute == null)
            return;
        result.IsRequired = true;
        result.RequiredMessage = attribute.ErrorMessage;
    }

    /// <summary>
    /// Valida la longitud de una propiedad del modelo.
    /// </summary>
    /// <param name="result">Información de la expresión del modelo que contiene los datos a validar.</param>
    /// <param name="property">La información del miembro (propiedad) que se está validando.</param>
    /// <remarks>
    /// Este método llama a otros métodos para validar la longitud mínima y máxima
    /// de la cadena, así como la longitud total de la propiedad especificada.
    /// </remarks>
    protected virtual void ValidateLength(ModelExpressionInfo result, MemberInfo property)
    {
        ValidateStringLength(result, property);
        ValidateMaxLength(result, property);
        ValidateMinLength(result, property);
    }

    /// <summary>
    /// Valida la longitud de una cadena según los atributos de longitud máxima y mínima 
    /// definidos en la propiedad especificada.
    /// </summary>
    /// <param name="result">El objeto que contiene la información del resultado de la validación.</param>
    /// <param name="property">La información del miembro que se está validando.</param>
    /// <remarks>
    /// Este método verifica si la propiedad tiene un atributo <see cref="StringLengthAttribute"/> 
    /// y, si es así, establece las longitudes máxima y mínima en el objeto de resultado 
    /// junto con los mensajes de error correspondientes.
    /// </remarks>
    protected virtual void ValidateStringLength(ModelExpressionInfo result, MemberInfo property)
    {
        var attribute = property.GetCustomAttribute<StringLengthAttribute>();
        if (attribute == null)
            return;
        if (attribute.MaximumLength > 0)
        {
            result.MaxLength = attribute.MaximumLength;
            result.MaxLengthMessage = attribute.ErrorMessage;
        }
        if (attribute.MinimumLength > 0)
        {
            result.MinLength = attribute.MinimumLength;
            result.MinLengthMessage = attribute.ErrorMessage;
        }
    }

    /// <summary>
    /// Valida la longitud máxima de un campo basado en el atributo <see cref="MaxLengthAttribute"/>.
    /// </summary>
    /// <param name="result">La información del modelo que se actualizará con la longitud máxima y el mensaje de error.</param>
    /// <param name="property">La propiedad del modelo que se está validando.</param>
    /// <remarks>
    /// Este método busca el atributo <see cref="MaxLengthAttribute"/> en la propiedad especificada.
    /// Si se encuentra, se establece la longitud máxima y el mensaje de error en el objeto <paramref name="result"/>.
    /// </remarks>
    protected virtual void ValidateMaxLength(ModelExpressionInfo result, MemberInfo property)
    {
        var attribute = property.GetCustomAttribute<MaxLengthAttribute>();
        if (attribute == null)
            return;
        result.MaxLength = attribute.Length;
        result.MaxLengthMessage = attribute.ErrorMessage;
    }

    /// <summary>
    /// Valida la longitud mínima de un campo basado en el atributo <see cref="MinLengthAttribute"/>.
    /// </summary>
    /// <param name="result">El objeto <see cref="ModelExpressionInfo"/> que contiene la información del modelo a validar.</param>
    /// <param name="property">El <see cref="MemberInfo"/> que representa la propiedad a validar.</param>
    /// <remarks>
    /// Este método busca el atributo <see cref="MinLengthAttribute"/> en la propiedad especificada.
    /// Si se encuentra, se establece la longitud mínima y el mensaje de error en el objeto <paramref name="result"/>.
    /// </remarks>
    protected virtual void ValidateMinLength(ModelExpressionInfo result, MemberInfo property)
    {
        var attribute = property.GetCustomAttribute<MinLengthAttribute>();
        if (attribute == null)
            return;
        result.MinLength = attribute.Length;
        result.MinLengthMessage = attribute.ErrorMessage;
    }

    /// <summary>
    /// Valida el rango de un valor basado en los atributos de rango definidos en la propiedad.
    /// </summary>
    /// <param name="result">El objeto que contiene la información del resultado de la validación.</param>
    /// <param name="property">La información del miembro que se está validando.</param>
    /// <remarks>
    /// Este método busca un atributo <see cref="RangeAttribute"/> en la propiedad especificada.
    /// Si se encuentra, establece los valores mínimo y máximo en el objeto resultante,
    /// así como los mensajes de error correspondientes.
    /// </remarks>
    protected virtual void ValidateRange(ModelExpressionInfo result, MemberInfo property)
    {
        var attribute = property.GetCustomAttribute<RangeAttribute>();
        if (attribute == null)
            return;
        result.Min = attribute.Minimum;
        result.Max = attribute.Maximum;
        result.MinMessage = attribute.ErrorMessage;
        result.MaxMessage = attribute.ErrorMessage;
    }

    /// <summary>
    /// Valida si el valor de una propiedad es una dirección de correo electrónico válida.
    /// </summary>
    /// <param name="result">El objeto que contiene la información del resultado de la validación.</param>
    /// <param name="property">La información del miembro que se está validando.</param>
    /// <remarks>
    /// Este método verifica si la propiedad está decorada con el atributo <see cref="EmailAddressAttribute"/>.
    /// Si el atributo está presente, se establece la propiedad <c>IsEmail</c> en <c>true</c> y se asigna un mensaje de error
    /// si el mensaje de error del atributo no es nulo y no contiene el texto específico.
    /// </remarks>
    protected virtual void ValidateEmail(ModelExpressionInfo result, MemberInfo property)
    {
        var attribute = property.GetCustomAttribute<EmailAddressAttribute>();
        if (attribute == null)
            return;
        result.IsEmail = true;
        if (attribute.ErrorMessage != null && attribute.ErrorMessage.Contains("el campo no es una dirección de correo electrónico válida"))
            return;
        result.EmailMessage = attribute.ErrorMessage;
    }

    /// <summary>
    /// Valida si el número de teléfono proporcionado en el modelo es válido.
    /// </summary>
    /// <param name="result">La información del resultado que contiene el estado de validación del número de teléfono.</param>
    /// <param name="property">La información del miembro que se está validando, que debe tener un atributo de teléfono.</param>
    /// <remarks>
    /// Este método verifica si el miembro tiene un atributo <see cref="PhoneAttribute"/>. 
    /// Si el atributo está presente, se establece el estado de validación en verdadero.
    /// Además, se asigna un mensaje de error personalizado si existe y no contiene el texto 
    /// "field is not a valid phone number".
    /// </remarks>
    protected virtual void ValidatePhone(ModelExpressionInfo result, MemberInfo property)
    {
        var attribute = property.GetCustomAttribute<PhoneAttribute>();
        if (attribute == null)
            return;
        result.IsPhone = true;
        if (attribute.ErrorMessage != null && attribute.ErrorMessage.Contains("el campo no es un número de teléfono válido"))
            return;
        result.PhoneMessage = attribute.ErrorMessage;
    }

    /// <summary>
    /// Valida el número de identificación de una tarjeta.
    /// </summary>
    /// <param name="result">El objeto que contiene la información del resultado de la validación.</param>
    /// <param name="property">La información del miembro que se está validando.</param>
    /// <remarks>
    /// Este método verifica si el miembro tiene el atributo <see cref="IdCardAttribute"/>. 
    /// Si el atributo está presente, se establece la propiedad <c>IsIdCard</c> en <c>true</c> 
    /// y se asigna un mensaje de error si el mensaje de error del atributo no coincide 
    /// con el valor de <c>R.InvalidIdCard</c>.
    /// </remarks>
    protected virtual void ValidateIdCard(ModelExpressionInfo result, MemberInfo property)
    {
        var attribute = property.GetCustomAttribute<IdCardAttribute>();
        if (attribute == null)
            return;
        result.IsIdCard = true;
        if (attribute.ErrorMessage == R.InvalidIdCard)
            return;
        result.IdCardMessage = attribute.ErrorMessage;
    }

    /// <summary>
    /// Valida si la propiedad especificada es una URL válida según los atributos definidos.
    /// </summary>
    /// <param name="result">La información del modelo que se actualizará con el resultado de la validación.</param>
    /// <param name="property">La información del miembro que se está validando.</param>
    /// <remarks>
    /// Este método verifica si la propiedad tiene un atributo de tipo <see cref="UrlAttribute"/>.
    /// Si el atributo está presente, se establece la propiedad <c>IsUrl</c> en <c>true</c>.
    /// Además, si el mensaje de error del atributo contiene un mensaje específico, se omite la asignación del mensaje de URL.
    /// En caso contrario, se asigna el mensaje de error del atributo a <c>UrlMessage</c>.
    /// </remarks>
    /// <seealso cref="UrlAttribute"/>
    protected virtual void ValidateUrl(ModelExpressionInfo result, MemberInfo property)
    {
        var attribute = property.GetCustomAttribute<UrlAttribute>();
        if (attribute == null)
            return;
        result.IsUrl = true;
        if (attribute.ErrorMessage != null && attribute.ErrorMessage.Contains("el campo no es una URL válida completamente cualificada de http, https o ftp"))
            return;
        result.UrlMessage = attribute.ErrorMessage;
    }

    /// <summary>
    /// Valida si el atributo de expresión regular está presente en la propiedad especificada
    /// y actualiza la información del modelo en consecuencia.
    /// </summary>
    /// <param name="result">La información del modelo que se actualizará con los resultados de la validación.</param>
    /// <param name="property">La propiedad de la que se extraerá el atributo de expresión regular.</param>
    /// <remarks>
    /// Si la propiedad no tiene un atributo de expresión regular, el método no realiza ninguna acción.
    /// Si se encuentra un atributo, se establece el patrón de expresión regular y el mensaje de error
    /// en la información del modelo.
    /// </remarks>
    protected virtual void ValidateRegularExpression(ModelExpressionInfo result, MemberInfo property)
    {
        var attribute = property.GetCustomAttribute<RegularExpressionAttribute>();
        if (attribute == null)
            return;
        result.IsRegularExpression = true;
        result.Pattern = attribute.Pattern;
        result.RegularExpressionMessage = attribute.ErrorMessage;
    }
}