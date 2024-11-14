namespace Util.Domain.Extending; 

/// <summary>
/// Representa una propiedad adicional que puede ser utilizada en diferentes contextos.
/// </summary>
/// <typeparam name="TProperty">El tipo de la propiedad adicional. Debe ser una clase.</typeparam>
public class ExtraProperty<TProperty> where TProperty : class {
    private ExtraPropertyDictionary _properties;
    private TProperty _property;
    private readonly string _propertyName;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ExtraProperty"/>.
    /// </summary>
    /// <param name="propertyName">El nombre de la propiedad adicional.</param>
    public ExtraProperty( string propertyName ) {
        _propertyName = propertyName;
    }

    /// <summary>
    /// Obtiene una propiedad del diccionario de propiedades extras.
    /// </summary>
    /// <param name="properties">El diccionario de propiedades extras desde el cual se obtiene la propiedad.</param>
    /// <returns>La propiedad solicitada de tipo <typeparamref name="TProperty"/> si existe; de lo contrario, null.</returns>
    /// <remarks>
    /// Si el diccionario de propiedades es null, se inicializa con un nuevo <see cref="ExtraPropertyDictionary"/>.
    /// Si la propiedad ya ha sido obtenida previamente, se devuelve la misma instancia.
    /// En caso contrario, se actualiza la propiedad en el diccionario.
    /// </remarks>
    /// <typeparam name="TProperty">El tipo de la propiedad que se desea obtener.</typeparam>
    public TProperty GetProperty( ExtraPropertyDictionary properties ) {
        _properties = properties ?? new ExtraPropertyDictionary();
        var property = _properties.GetProperty<TProperty>( _propertyName );
        if ( property == null )
            return null;
        if( _property == property )
            return _property;
        _property = property;
        _properties.SetProperty( _propertyName, _property );
        return _property;
    }

    /// <summary>
    /// Establece una propiedad en el diccionario de propiedades extra.
    /// </summary>
    /// <param name="properties">El diccionario de propiedades extra donde se establecerá la propiedad.</param>
    /// <param name="property">El valor de la propiedad que se desea establecer.</param>
    public void SetProperty( ExtraPropertyDictionary properties,TProperty property ) {
        _properties = properties;
        _property = property;
        _properties.SetProperty( _propertyName, _property );
    }
}