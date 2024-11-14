namespace Util; 

/// <summary>
/// Representa un elemento que puede ser comparado con otros elementos.
/// </summary>
/// <remarks>
/// La clase <c>Item</c> implementa la interfaz <c>IComparable</c> para permitir la comparación
/// entre instancias de <c>Item</c>. Esto es útil para ordenar colecciones de elementos.
/// </remarks>
public class Item : IComparable<Item> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Item"/>.
    /// </summary>
    /// <param name="text">El texto que se mostrará para el elemento.</param>
    /// <param name="value">El valor asociado al elemento.</param>
    /// <param name="sortId">El identificador de ordenación del elemento. Puede ser nulo.</param>
    /// <param name="group">El grupo al que pertenece el elemento. Puede ser nulo.</param>
    /// <param name="disabled">Indica si el elemento está deshabilitado. Puede ser nulo.</param>
    /// <param name="selected">Indica si el elemento está seleccionado. Puede ser nulo.</param>
    /// <param name="icon">El icono asociado al elemento. Puede ser nulo.</param>
    /// <remarks>
    /// Esta clase se utiliza para representar un elemento con un texto, un valor y opciones adicionales como ordenación, grupo, estado de habilitación y selección.
    /// </remarks>
    public Item( string text, object value, int? sortId = null, string group = null, bool? disabled = null, bool? selected = null,string icon = null ) {
        Text = text;
        Value = value;
        SortId = sortId;
        Group = group;
        Disabled = disabled;
        Selected = selected;
        Icon = icon;
    }

    /// <summary>
    /// Representa el texto asociado con el objeto.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y se utiliza para almacenar el texto en formato JSON.
    /// </remarks>
    /// <returns>
    /// Devuelve el texto como una cadena de caracteres.
    /// </returns>
    [JsonPropertyName("text")]
    public string Text { get; }

    /// <summary>
    /// Representa el valor asociado a la propiedad.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y se utiliza para almacenar un objeto de tipo dinámico.
    /// </remarks>
    /// <returns>
    /// Un objeto que puede ser de cualquier tipo, representando el valor.
    /// </returns>
    [JsonPropertyName("value")]
    public object Value { get; }

    /// <summary>
    /// Representa el identificador de orden para un elemento.
    /// </summary>
    /// <remarks>
    /// Este campo puede ser nulo, lo que indica que no se ha asignado un identificador de orden.
    /// </remarks>
    /// <returns>
    /// Un entero que representa el identificador de orden, o null si no está asignado.
    /// </returns>
    [JsonPropertyName("sortId")]
    public int? SortId { get; }

    /// <summary>
    /// Representa el grupo asociado a un objeto.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y se utiliza para almacenar el nombre del grupo en formato JSON.
    /// </remarks>
    /// <returns>
    /// Devuelve el nombre del grupo como una cadena de texto.
    /// </returns>
    [JsonPropertyName("group")]
    public string Group { get; }

    /// <summary>
    /// Representa el estado de desactivación de un objeto.
    /// </summary>
    /// <remarks>
    /// Esta propiedad indica si el objeto está deshabilitado o no. 
    /// Puede ser nula, lo que significa que el estado no está definido.
    /// </remarks>
    /// <returns>
    /// Un valor booleano que indica si el objeto está deshabilitado.
    /// </returns>
    [JsonPropertyName("disabled")]
    public bool? Disabled { get; }

    /// <summary>
    /// Representa el estado de selección de un elemento.
    /// </summary>
    /// <remarks>
    /// Esta propiedad indica si el elemento está seleccionado o no.
    /// Puede ser nula, lo que indica que el estado de selección no está definido.
    /// </remarks>
    /// <returns>
    /// Un valor booleano que representa el estado de selección. 
    /// Puede ser verdadero, falso o nulo.
    /// </returns>
    [JsonPropertyName("selected")]
    public bool? Selected { get; }

    /// <summary>
    /// Representa el ícono asociado a un objeto.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y se utiliza para almacenar la representación visual del objeto.
    /// </remarks>
    /// <returns>
    /// Una cadena que contiene el nombre o la ruta del ícono.
    /// </returns>
    [JsonPropertyName("icon")]
    public string Icon { get; }

    /// <summary>
    /// Compara el objeto actual con otro objeto del mismo tipo.
    /// </summary>
    /// <param name="other">El otro objeto <see cref="Item"/> a comparar.</param>
    /// <returns>
    /// Un valor entero que indica la relación de orden entre el objeto actual y el objeto <paramref name="other"/>.
    /// Un valor menor que cero indica que el objeto actual es anterior a <paramref name="other"/>.
    /// Un valor cero indica que son equivalentes.
    /// Un valor mayor que cero indica que el objeto actual es posterior a <paramref name="other"/>.
    /// </returns>
    public int CompareTo( Item other ) {
        return string.Compare( Text, other.Text, StringComparison.CurrentCulture );
    }
}