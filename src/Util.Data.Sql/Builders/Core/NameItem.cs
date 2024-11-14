namespace Util.Data.Sql.Builders.Core; 

/// <summary>
/// Representa un elemento que contiene un nombre.
/// </summary>
public class NameItem {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="NameItem"/>.
    /// </summary>
    /// <param name="name">El nombre que se utilizará para resolver el objeto.</param>
    public NameItem( string name ) {
        Resolve( name );
    }

    /// <summary>
    /// Resuelve un nombre dividiéndolo en un prefijo y un nombre.
    /// </summary>
    /// <param name="name">El nombre a resolver, que será filtrado y dividido.</param>
    /// <remarks>
    /// Este método utiliza la clase <see cref="SplitItem"/> para dividir el nombre en
    /// un prefijo y un nombre. Si el nombre no contiene un punto, se asigna el valor
    /// a la propiedad <see cref="Name"/>. Si contiene un punto, el valor a la izquierda
    /// del punto se asigna a <see cref="Prefix"/> y el valor a la derecha se asigna a
    /// <see cref="Name"/>.
    /// </remarks>
    private void Resolve( string name ) {
        name = Filter( name );
        var asItem = new SplitItem( name );
        Alias = asItem.Right;
        var dotItem = new SplitItem( asItem.Left, "." );
        if( dotItem.Right.IsEmpty() ) {
            Name = dotItem.Left;
            return;
        }
        Prefix = dotItem.Left;
        Name = dotItem.Right;
    }

    /// <summary>
    /// Filtra el nombre proporcionado aplicando dos métodos de filtrado.
    /// </summary>
    /// <param name="name">El nombre que se va a filtrar.</param>
    /// <returns>El nombre filtrado después de aplicar los métodos de filtrado.</returns>
    private string Filter( string name ) {
        name = FilterAs( name );
        return FilterDotSpace( name );
    }

    /// <summary>
    /// Filtra la cadena de texto reemplazando la subcadena " as " por un espacio en blanco.
    /// </summary>
    /// <param name="name">La cadena de texto que se va a filtrar.</param>
    /// <returns>La cadena de texto resultante después de realizar el reemplazo.</returns>
    private string FilterAs( string name ) {
        return name.Replace( " as ", " ",StringComparison.OrdinalIgnoreCase );
    }

    /// <summary>
    /// Filtra los espacios antes y después de un punto en una cadena.
    /// </summary>
    /// <param name="name">La cadena de texto que se va a filtrar.</param>
    /// <returns>Una nueva cadena con los espacios eliminados antes y después de los puntos.</returns>
    private string FilterDotSpace( string name ) {
        return name.Replace( @" .", "." ).Replace( @". ", "." );
    }

    /// <summary>
    /// Obtiene el prefijo asociado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y se establece internamente.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el prefijo.
    /// </value>
    public string Prefix { get; private set; }

    /// <summary>
    /// Obtiene el nombre.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura desde fuera de la clase,
    /// ya que su setter es privado.
    /// </remarks>
    /// <value>
    /// Un string que representa el nombre.
    /// </value>
    public string Name { get; private set; }

    /// <summary>
    /// Obtiene el alias asociado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite acceder al alias, pero no permite modificarlo desde fuera de la clase.
    /// </remarks>
    /// <value>
    /// Un string que representa el alias.
    /// </value>
    public string Alias { get; private set; }
}