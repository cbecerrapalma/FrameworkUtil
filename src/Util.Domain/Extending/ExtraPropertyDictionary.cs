namespace Util.Domain.Extending; 

/// <summary>
/// Representa un diccionario que almacena propiedades adicionales como pares clave-valor.
/// Esta clase hereda de <see cref="Dictionary{TKey, TValue}"/> donde la clave es una cadena y el valor es un objeto.
/// </summary>
public class ExtraPropertyDictionary : Dictionary<string, object> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ExtraPropertyDictionary"/>.
    /// </summary>
    /// <remarks>
    /// Esta clase se utiliza para almacenar propiedades adicionales en un diccionario.
    /// </remarks>
    public ExtraPropertyDictionary() {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ExtraPropertyDictionary"/> 
    /// utilizando un diccionario existente.
    /// </summary>
    /// <param name="dictionary">Un diccionario que contiene pares clave-valor 
    /// que se utilizarán para inicializar la instancia.</param>
    public ExtraPropertyDictionary( IDictionary<string, object> dictionary ) : base( dictionary ) {
    }

    /// <summary>
    /// Obtiene o establece un valor que indica si se debe recortar las cadenas.
    /// </summary>
    /// <remarks>
    /// Si el valor es <c>true</c>, las cadenas se recortarán al eliminar los espacios en blanco al inicio y al final.
    /// De lo contrario, se mantendrán los espacios en blanco.
    /// </remarks>
    /// <value>
    /// <c>true</c> si se debe recortar las cadenas; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsTrimString { get; set; } = true;
}