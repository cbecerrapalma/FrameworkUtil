namespace Util.Microservices.Dapr.StateManagements.Queries;

/// <summary>
/// Representa una colección de elementos de ordenación que hereda de la lista genérica.
/// </summary>
/// <remarks>
/// Esta clase permite almacenar y gestionar una lista de elementos de tipo <see cref="OrderByItem"/> 
/// que se utilizan para definir el orden en que se deben presentar los datos.
/// </remarks>
public class StateSort : List<OrderByItem> {
    /// <summary>
    /// Ordena los elementos según la propiedad especificada.
    /// </summary>
    /// <param name="property">El nombre de la propiedad por la cual se ordenarán los elementos.</param>
    public void OrderBy( string property ) {
        Add( property );
    }

    /// <summary>
    /// Ordena los elementos en orden descendente según la propiedad especificada.
    /// </summary>
    /// <param name="property">El nombre de la propiedad por la cual se ordenarán los elementos.</param>
    public void OrderByDescending( string property ) {
        Add( $"{property} DESC" );
    }

    /// <summary>
    /// Agrega elementos de ordenación a la colección a partir de una cadena de texto.
    /// </summary>
    /// <param name="orderBy">Una cadena que contiene los elementos de ordenación separados por comas.</param>
    /// <remarks>
    /// Si la cadena <paramref name="orderBy"/> está vacía, el método no realiza ninguna acción.
    /// Cada elemento de la cadena se divide por comas y se agrega como un nuevo <see cref="OrderByItem"/>.
    /// </remarks>
    public void Add( string orderBy ) {
        if ( orderBy.IsEmpty() )
            return;
        var items = orderBy.Split( "," );
        foreach ( var item in items ) {
            Add( new OrderByItem( item ) );
        }
    }

    /// <summary>
    /// Ordena los elementos de una secuencia en función de una clave especificada por una expresión.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos en la secuencia.</typeparam>
    /// <param name="expression">Una expresión que especifica la clave por la que se ordenarán los elementos.</param>
    /// <remarks>
    /// Este método utiliza el método <see cref="StateQueryHelper.GetProperty"/> para obtener la propiedad
    /// a partir de la expresión proporcionada y luego realiza la ordenación.
    /// </remarks>
    public void OrderBy<T>( Expression<Func<T, object>> expression ) {
        OrderBy( StateQueryHelper.GetProperty( expression ) );
    }

    /// <summary>
    /// Ordena los elementos en orden descendente según la expresión proporcionada.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se están ordenando.</typeparam>
    /// <param name="expression">La expresión que especifica la propiedad por la cual se ordenarán los elementos.</param>
    /// <remarks>
    /// Este método utiliza la expresión proporcionada para obtener la propiedad correspondiente
    /// y luego aplica el ordenamiento descendente sobre la consulta actual.
    /// </remarks>
    /// <seealso cref="OrderByDescending{T}(Expression{Func{T, object}})"/>
    public void OrderByDescending<T>( Expression<Func<T, object>> expression ) {
        OrderByDescending( StateQueryHelper.GetProperty( expression ) );
    }
}