using Util.Ui.Builders;

namespace Util.Ui.Extensions; 

/// <summary>
/// Proporciona métodos de extensión para trabajar con etiquetas en el contexto de los Tag Helpers.
/// </summary>
public static class TagHelperExtensions {
    /// <summary>
    /// Determina si el contenido de un <see cref="TagHelperContent"/> está vacío o es nulo.
    /// </summary>
    /// <param name="content">El contenido del <see cref="TagHelperContent"/> que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el contenido es nulo o está vacío; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión de la clase <see cref="TagHelperContent"/> y se puede utilizar para verificar rápidamente
    /// si el contenido es nulo o solo contiene espacios en blanco.
    /// </remarks>
    public static bool IsEmpty( this TagHelperContent content ) {
        return content == null || content.IsEmptyOrWhiteSpace;
    }

    /// <summary>
    /// Agrega el contenido de un <see cref="TagHelperContent"/> a un <see cref="TagBuilder"/>.
    /// </summary>
    /// <param name="content">El contenido que se va a agregar al <see cref="TagBuilder"/>.</param>
    /// <param name="builder">El <see cref="TagBuilder"/> al que se agregará el contenido.</param>
    /// <remarks>
    /// Si el contenido está vacío, no se realizará ninguna acción.
    /// </remarks>
    public static void AppendTo( this TagHelperContent content,TagBuilder builder ) {
        if( content.IsEmpty() )
            return;
        builder.AppendContent( content );
    }

    /// <summary>
    /// Establece el contenido de un <see cref="TagBuilder"/> utilizando el contenido de un <see cref="TagHelperContent"/>.
    /// </summary>
    /// <param name="content">El contenido del <see cref="TagHelperContent"/> que se va a establecer.</param>
    /// <param name="builder">El <see cref="TagBuilder"/> en el que se establecerá el contenido.</param>
    /// <remarks>
    /// Si el contenido proporcionado está vacío, no se realizará ninguna acción.
    /// </remarks>
    /// <seealso cref="TagHelperContent"/>
    /// <seealso cref="TagBuilder"/>
    public static void SetTo( this TagHelperContent content, TagBuilder builder ) {
        if ( content.IsEmpty() )
            return;
        builder.SetContent( content );
    }

    /// <summary>
    /// Obtiene un valor del contexto de TagHelper utilizando una clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se desea obtener.</typeparam>
    /// <param name="context">El contexto de TagHelper desde el cual se extraerá el valor.</param>
    /// <param name="key">La clave utilizada para buscar el valor en el contexto. Si es nula, se utilizará el tipo de T como clave.</param>
    /// <returns>
    /// El valor asociado a la clave especificada, convertido al tipo T, o el valor predeterminado de T si la clave no existe.
    /// </returns>
    /// <remarks>
    /// Este método permite acceder a los elementos almacenados en el contexto de TagHelper de manera segura,
    /// manejando la conversión de tipos y la posibilidad de que la clave no exista.
    /// </remarks>
    /// <seealso cref="TagHelperContext"/>
    public static T GetValueFromItems<T>( this TagHelperContext context, object key = null ) {
        key ??= typeof(T);
        var exists = context.Items.TryGetValue( key, out var value );
        if( exists == false )
            return default;
        if( !( value is TagHelperAttribute tagHelperAttribute ) )
            return Util.Helpers.Convert.To<T>( value );
        return Util.Helpers.Convert.To<T>( tagHelperAttribute.Value );
    }

    /// <summary>
    /// Establece un valor para los elementos en el contexto de TagHelper.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a establecer.</typeparam>
    /// <param name="context">El contexto de TagHelper donde se establecerá el valor.</param>
    /// <param name="value">El valor que se desea establecer en los elementos.</param>
    /// <remarks>
    /// Este método extiende la funcionalidad del TagHelperContext, permitiendo
    /// asignar un valor de tipo genérico a los elementos dentro del contexto.
    /// </remarks>
    /// <seealso cref="TagHelperContext"/>
    public static void SetValueToItems<T>( this TagHelperContext context, T value ) {
        var key = typeof( T );
        SetValueToItems( context, key, value );
    }

    /// <summary>
    /// Establece un valor en el diccionario de elementos del contexto de TagHelper.
    /// </summary>
    /// <param name="context">El contexto de TagHelper donde se almacenará el valor.</param>
    /// <param name="key">La clave asociada al valor que se desea establecer. Si es <c>null</c>, no se realiza ninguna acción.</param>
    /// <param name="value">El valor que se desea almacenar en el contexto de TagHelper.</param>
    /// <remarks>
    /// Este método permite agregar o actualizar un valor en el diccionario de elementos del contexto de TagHelper.
    /// Si la clave es <c>null</c>, el método no realiza ninguna operación.
    /// </remarks>
    public static void SetValueToItems( this TagHelperContext context, object key, object value ) {
        if ( key == null )
            return;
        context.Items[key] = value;
    }

    /// <summary>
    /// Obtiene el valor de un atributo específico del contexto de TagHelper.
    /// </summary>
    /// <param name="context">El contexto de TagHelper del cual se extraerá el valor del atributo.</param>
    /// <param name="key">La clave del atributo cuyo valor se desea obtener.</param>
    /// <returns>
    /// El valor del atributo como una cadena, o null si el atributo no existe.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="context"/> o <paramref name="key"/> es null.</exception>
    public static string GetValueFromAttributes( this TagHelperContext context, string key ) {
        return context.GetValueFromAttributes<string>( key );
    }

    /// <summary>
    /// Verifica si el contexto de TagHelper contiene un atributo con el nombre especificado.
    /// </summary>
    /// <param name="context">El contexto de TagHelper en el que se buscarán los atributos.</param>
    /// <param name="key">El nombre del atributo que se desea buscar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el atributo con el nombre especificado existe en el contexto; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    public static bool ContainsByAttributes( this TagHelperContext context, string key ) {
        return context.AllAttributes.ContainsName( key );
    }

    /// <summary>
    /// Obtiene el valor de un atributo de un contexto de TagHelper.
    /// </summary>
    /// <typeparam name="T">El tipo al que se convertirá el valor del atributo.</typeparam>
    /// <param name="context">El contexto de TagHelper desde el cual se obtendrá el atributo.</param>
    /// <param name="key">La clave del atributo que se desea obtener.</param>
    /// <returns>
    /// El valor del atributo convertido al tipo especificado, o el valor predeterminado de T si el atributo no existe o no se puede convertir.
    /// </returns>
    /// <remarks>
    /// Este método intenta buscar un atributo en el contexto de TagHelper. Si el atributo existe, se convierte a 
    /// tipo T utilizando un método de conversión auxiliar. Si el atributo no existe o no se puede convertir, 
    /// se devuelve el valor predeterminado del tipo T.
    /// </remarks>
    /// <seealso cref="TagHelperContext"/>
    public static T GetValueFromAttributes<T>( this TagHelperContext context, string key ) {
        var exists = context.AllAttributes.TryGetAttribute( key, out var value );
        if( exists == false )
            return default;
        return value is not { } tagHelperAttribute ? default : Util.Helpers.Convert.To<T>( tagHelperAttribute?.Value );
    }
}