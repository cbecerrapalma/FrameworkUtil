namespace Util.Ui.Expressions; 

/// <summary>
/// Proporciona métodos de ayuda para trabajar con expresiones de modelo.
/// </summary>
public static class ModelExpressionHelper {
    /// <summary>
    /// Crea una instancia de <see cref="ModelExpression"/> utilizando el nombre y una expresión lambda que representa una propiedad del modelo.
    /// </summary>
    /// <typeparam name="TModel">El tipo del modelo que contiene la propiedad.</typeparam>
    /// <typeparam name="TProperty">El tipo de la propiedad que se está expresando.</typeparam>
    /// <param name="name">El nombre de la expresión del modelo.</param>
    /// <param name="expression">Una expresión lambda que representa la propiedad del modelo.</param>
    /// <returns>
    /// Una instancia de <see cref="ModelExpression"/> que contiene la información de la propiedad especificada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un proveedor de metadatos del modelo para obtener la información de la propiedad y crear un explorador de modelos.
    /// Asegúrese de que la expresión proporcionada sea válida y que la propiedad exista en el modelo especificado.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="expression"/> es nulo o si la propiedad no se puede obtener.</exception>
    /// <seealso cref="ModelExpression"/>
    /// <seealso cref="ModelExplorer"/>
    public static ModelExpression Create<TModel,TProperty>( string name, Expression<Func<TModel, TProperty>> expression ) {
        var property = Util.Helpers.Lambda.GetMember( expression ) as PropertyInfo;
        property!.CheckNull( nameof(property) );
        var provider = CreateModelMetadataProvider();
        var metadata = provider.GetMetadataForProperty( property, typeof( TProperty ) );
        var modelExplorer = new ModelExplorer( provider, metadata, null );
        return new ModelExpression( name, modelExplorer );
    }

    /// <summary>
    /// Crea una instancia de <see cref="ModelExpression"/> utilizando el nombre y el tipo del modelo especificado.
    /// </summary>
    /// <typeparam name="TModel">El tipo del modelo para el cual se está creando la expresión.</typeparam>
    /// <param name="name">El nombre que se asignará a la expresión del modelo.</param>
    /// <returns>
    /// Una instancia de <see cref="ModelExpression"/> que representa el modelo especificado.
    /// </returns>
    /// <seealso cref="ModelExpression"/>
    public static ModelExpression Create<TModel>( string name ) {
        return Create( name, typeof( TModel ), typeof( TModel ) );
    }

    /// <summary>
    /// Crea una instancia de <see cref="ModelMetadataProvider"/>.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="EmptyModelMetadataProvider"/> que implementa <see cref="ModelMetadataProvider"/>.
    /// </returns>
    private static ModelMetadataProvider CreateModelMetadataProvider() {
        return new EmptyModelMetadataProvider();
    }

    /// <summary>
    /// Crea una instancia de <see cref="ModelExpression"/> utilizando el nombre proporcionado y los tipos de modelo y contenedor especificados.
    /// </summary>
    /// <typeparam name="TModel">El tipo del modelo que se utilizará.</typeparam>
    /// <typeparam name="TContainer">El tipo del contenedor que se utilizará.</typeparam>
    /// <param name="name">El nombre que se asignará a la expresión del modelo.</param>
    /// <returns>Una instancia de <see cref="ModelExpression"/> que representa la expresión del modelo creada.</returns>
    /// <seealso cref="ModelExpression"/>
    public static ModelExpression Create<TModel, TContainer>( string name ) {
        return Create( name, typeof( TModel ), typeof( TContainer ) );
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="ModelExpression"/> utilizando el nombre, el tipo del modelo y el tipo del contenedor especificados.
    /// </summary>
    /// <param name="name">El nombre que se asignará a la expresión del modelo.</param>
    /// <param name="modelType">El tipo del modelo para el cual se está creando la expresión.</param>
    /// <param name="containerType">El tipo del contenedor que contiene el modelo.</param>
    /// <returns>
    /// Una instancia de <see cref="ModelExpression"/> que representa la expresión del modelo creada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un proveedor de metadatos del modelo para obtener información sobre el modelo y el contenedor,
    /// y luego crea un <see cref="ModelExplorer"/> que se utiliza para construir la <see cref="ModelExpression"/>.
    /// </remarks>
    private static ModelExpression Create( string name, Type modelType, Type containerType ) {
        var provider = CreateModelMetadataProvider();
        var containerModelExplorer = provider.GetModelExplorerForType( containerType, null );
        var metadata = provider.GetMetadataForType( modelType );
        var modelExplorer = new ModelExplorer( provider, containerModelExplorer, metadata, null );
        return new ModelExpression( name, modelExplorer );
    }

    /// <summary>
    /// Convierte el nombre proporcionado a un formato específico, asegurando que la
    /// primera letra esté en mayúscula y el resto en minúscula. Si el nombre es nulo
    /// o vacío, o si la primera letra ya no está en mayúscula, se devuelve tal cual.
    /// </summary>
    /// <param name="name">El nombre que se desea convertir.</param>
    /// <returns>El nombre convertido, o el mismo nombre si no se requiere conversión.</returns>
    /// <remarks>
    /// Este método utiliza la función <c>string.Create</c> para crear una nueva cadena
    /// con el formato adecuado. Si el nombre ya cumple con las condiciones, se devuelve
    /// sin modificaciones.
    /// </remarks>
    /// <seealso cref="char.IsUpper(char)"/>
    /// <seealso cref="string.IsNullOrEmpty(string)"/>
    public static string ConvertName( string name ) {
        if ( string.IsNullOrEmpty( name ) || !char.IsUpper( name[0] ) ) {
            return name;
        }
        return string.Create( name.Length, name, ( chars, name2 ) => {
            name2.AsSpan().CopyTo( chars );
            FixCasing( chars );
        } );
    }

    /// <summary>
    /// Corrige el casing de los caracteres en un <see cref="Span{T}"/> de caracteres.
    /// </summary>
    /// <param name="chars">Un <see cref="Span{T}"/> de caracteres que se va a modificar para corregir el casing.</param>
    /// <remarks>
    /// Este método recorre cada carácter en el span y lo convierte a minúsculas, 
    /// con ciertas excepciones basadas en la posición del carácter y el estado de los caracteres adyacentes.
    /// Si el segundo carácter no es mayúscula, se detiene la conversión.
    /// Si un carácter es seguido por un espacio y no es el primer carácter, 
    /// también se convierte a minúscula.
    /// </remarks>
    private static void FixCasing( Span<char> chars ) {
        for ( int i = 0; i < chars.Length; i++ ) {
            if ( i == 1 && !char.IsUpper( chars[i] ) )
                break;
            bool hasNext = ( i + 1 < chars.Length );
            if ( i > 0 && hasNext && !char.IsUpper( chars[i + 1] ) ) {
                if ( chars[i + 1] == ' ' ) {
                    chars[i] = char.ToLowerInvariant( chars[i] );
                }
                break;
            }
            chars[i] = char.ToLowerInvariant( chars[i] );
        }
    }
}