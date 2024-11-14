namespace Util.Domain.Entities; 

/// <summary>
/// Clase base abstracta para representar un objeto de valor en el dominio.
/// </summary>
/// <typeparam name="TValueObject">El tipo del objeto de valor que hereda de <see cref="ValueObjectBase{TValueObject}"/>.</typeparam>
/// <remarks>
/// Un objeto de valor es un objeto que no tiene identidad y se define por sus atributos.
/// Esta clase proporciona la funcionalidad básica para la comparación de objetos de valor.
/// </remarks>
public abstract class ValueObjectBase<TValueObject> : DomainObjectBase<TValueObject>, IEquatable<TValueObject> where TValueObject : ValueObjectBase<TValueObject> {
    /// <summary>
    /// Compara el objeto actual con otro objeto del mismo tipo para determinar si son iguales.
    /// </summary>
    /// <param name="other">El otro objeto de tipo <typeparamref name="TValueObject"/> con el que se va a comparar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el objeto actual es igual al objeto especificado; de lo contrario, <c>false</c>.
    /// </returns>
    public bool Equals( TValueObject other ) {
        return this == other;
    }

    /// <summary>
    /// Determina si el objeto especificado es igual a la instancia actual.
    /// </summary>
    /// <param name="other">El objeto que se va a comparar con la instancia actual.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el objeto especificado es igual a la instancia actual; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método intenta convertir el objeto proporcionado a un tipo específico de valor objeto 
    /// (TValueObject) antes de realizar la comparación.
    /// </remarks>
    public override bool Equals( object other ) {
        return Equals( other as TValueObject );
    }

    public static bool operator ==( ValueObjectBase<TValueObject> left, ValueObjectBase<TValueObject> right ) {
        if( (object)left == null || (object)right == null )
            return false;
        if( !( left is TValueObject ) || !( right is TValueObject ) )
            return false;
        var properties = left.GetType().GetTypeInfo().GetProperties();
        return properties.All( property => property.GetValue( left ) == property.GetValue( right ) );
    }

    public static bool operator !=( ValueObjectBase<TValueObject> left, ValueObjectBase<TValueObject> right ) {
        return !( left == right );
    }

    /// <summary>
    /// Calcula el código hash para la instancia actual de la clase.
    /// </summary>
    /// <returns>
    /// Un entero que representa el código hash de la instancia actual.
    /// </returns>
    /// <remarks>
    /// Este método utiliza las propiedades de la clase para generar un código hash.
    /// Solo se consideran las propiedades cuyo valor no es nulo.
    /// Se utiliza una operación XOR para combinar los códigos hash de las propiedades.
    /// </remarks>
    public override int GetHashCode() {
        var properties = GetType().GetTypeInfo().GetProperties();
        return properties.Select( property => property.GetValue( this ) )
            .Where( value => value != null )
            .Aggregate( 0, ( current, value ) => current ^ value.GetHashCode() );
    }

    /// <summary>
    /// Crea una copia superficial del objeto actual.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <typeparamref name="TValueObject"/> que es una copia superficial del objeto actual.
    /// </returns>
    /// <remarks>
    /// Este método utiliza <see cref="MemberwiseClone"/> para realizar la copia superficial,
    /// y luego convierte el resultado al tipo <typeparamref name="TValueObject"/> utilizando
    /// el método de conversión de <see cref="Util.Helpers.Convert"/>.
    /// </remarks>
    /// <typeparam name="TValueObject">
    /// El tipo del objeto de valor que se está clonando.
    /// </typeparam>
    public virtual TValueObject Clone() {
        return Util.Helpers.Convert.To<TValueObject>( MemberwiseClone() );
    }
}