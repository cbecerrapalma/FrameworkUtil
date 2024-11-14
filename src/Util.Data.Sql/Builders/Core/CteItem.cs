namespace Util.Data.Sql.Builders.Core; 

/// <summary>
/// Representa un elemento de un contrato o documento.
/// </summary>
public class CteItem {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CteItem"/>.
    /// </summary>
    /// <param name="name">El nombre del elemento CTE.</param>
    /// <param name="builder">Una instancia de <see cref="ISqlBuilder"/> que se utilizará para construir consultas SQL.</param>
    public CteItem( string name, ISqlBuilder builder ) {
        Name = name;
        Builder = builder;
    }

    /// <summary>
    /// Obtiene el nombre asociado a la instancia actual.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y devuelve un valor de tipo <see cref="string"/>.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el nombre.
    /// </value>
    public string Name { get; }

    /// <summary>
    /// Obtiene el constructor de consultas SQL asociado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso al objeto que implementa la interfaz <see cref="ISqlBuilder"/>.
    /// Utiliza este constructor para construir consultas SQL de manera programática.
    /// </remarks>
    /// <value>
    /// Un objeto que implementa <see cref="ISqlBuilder"/>.
    /// </value>
    public ISqlBuilder Builder { get; }

    /// <summary>
    /// Crea una copia del objeto actual <see cref="CteItem"/>.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="CteItem"/> que es una copia del objeto actual.
    /// </returns>
    public CteItem Clone() {
        return new CteItem( Name, Builder.Clone() );
    }
}