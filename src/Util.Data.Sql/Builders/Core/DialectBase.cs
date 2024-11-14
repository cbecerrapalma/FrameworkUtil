namespace Util.Data.Sql.Builders.Core; 

/// <summary>
/// Clase base abstracta que define la estructura y comportamiento común para los dialectos.
/// Implementa la interfaz <see cref="IDialect"/>.
/// </summary>
public abstract class DialectBase : IDialect {
    /// <summary>
    /// Obtiene el identificador de apertura.
    /// </summary>
    /// <returns>
    /// Un <see cref="string"/> que representa el identificador de apertura.
    /// </returns>
    /// <remarks>
    /// Este método es abstracto y debe ser implementado por las clases derivadas.
    /// </remarks>
    public abstract string GetOpeningIdentifier();

    /// <summary>
    /// Obtiene el identificador de cierre asociado.
    /// </summary>
    /// <returns>
    /// Un <see cref="string"/> que representa el identificador de cierre.
    /// </returns>
    /// <remarks>
    /// Este método debe ser implementado por las clases derivadas.
    /// </remarks>
    public abstract string GetClosingIdentifier();

    /// <summary>
    /// Obtiene un nombre seguro a partir del nombre proporcionado.
    /// </summary>
    /// <param name="name">El nombre que se desea filtrar y convertir en un nombre seguro.</param>
    /// <returns>
    /// Un nombre seguro. Si el nombre proporcionado es nulo, vacío o solo contiene espacios en blanco, 
    /// se devolverá una cadena vacía. Si el nombre es un asterisco ("*"), se devolverá tal cual. 
    /// En otros casos, se devolverá el nombre filtrado y rodeado por identificadores de apertura y cierre.
    /// </returns>
    public virtual string GetSafeName( string name ) {
        if( string.IsNullOrWhiteSpace( name ) )
            return string.Empty;
        if( name == "*" )
            return name;
        return $"{GetOpeningIdentifier()}{FilterName( name )}{GetClosingIdentifier()}";
    }

    /// <summary>
    /// Filtra el nombre proporcionado eliminando los identificadores de apertura y cierre.
    /// </summary>
    /// <param name="name">El nombre que se desea filtrar.</param>
    /// <returns>El nombre filtrado sin los identificadores de apertura y cierre.</returns>
    /// <remarks>
    /// Este método utiliza los métodos <c>Trim</c>, <c>RemoveStart</c> y <c>RemoveEnd</c> 
    /// para realizar la limpieza del nombre. Asegúrese de que los identificadores de apertura 
    /// y cierre estén correctamente definidos en los métodos <c>GetOpeningIdentifier</c> 
    /// y <c>GetClosingIdentifier</c>.
    /// </remarks>
    private string FilterName( string name ) {
        return name.Trim().RemoveStart( GetOpeningIdentifier() ).RemoveEnd( GetClosingIdentifier() );
    }

    /// <summary>
    /// Obtiene el prefijo asociado con la implementación concreta de la clase.
    /// </summary>
    /// <returns>
    /// Un <see cref="string"/> que representa el prefijo.
    /// </returns>
    /// <remarks>
    /// Este método debe ser implementado por las clases derivadas para proporcionar el prefijo específico.
    /// </remarks>
    public abstract string GetPrefix();

    /// <summary>
    /// Indica si se admite la selección utilizando la cláusula AS.
    /// </summary>
    /// <returns>
    /// Devuelve true si se admite la selección con la cláusula AS; de lo contrario, devuelve false.
    /// </returns>
    public virtual bool SupportSelectAs() {
        return true;
    }

    /// <summary>
    /// Reemplaza los identificadores de SQL en la cadena proporcionada.
    /// </summary>
    /// <param name="sql">La cadena SQL que contiene los identificadores a reemplazar.</param>
    /// <returns>
    /// Una nueva cadena con los identificadores de SQL reemplazados según las reglas definidas.
    /// Si la cadena de entrada es <c>null</c>, se devuelve <c>null</c>.</returns>
    /// <remarks>
    /// Este método realiza múltiples reemplazos en la cadena SQL para transformar los
    /// identificadores de apertura y cierre de corchetes en un formato alternativo y
    /// luego los convierte de nuevo a su forma original después de haber sido
    /// procesados. Esto es útil para manejar identificadores que pueden ser
    /// interpretados de manera diferente en un contexto SQL.
    /// </remarks>
    public virtual string ReplaceSql( string sql ) {
        return sql?
            .Replace( "[[", "|&<&|", StringComparison.Ordinal )
            .Replace( "]]", "|&>&|", StringComparison.Ordinal )
            .Replace( "[", GetOpeningIdentifier(), StringComparison.Ordinal )
            .Replace( "]", GetClosingIdentifier(), StringComparison.Ordinal )
            .Replace( "|&<&|", "[", StringComparison.Ordinal )
            .Replace( "|&>&|", "]", StringComparison.Ordinal );
    }
}