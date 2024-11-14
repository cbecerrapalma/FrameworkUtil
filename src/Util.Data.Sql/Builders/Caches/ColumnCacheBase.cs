using Util.Data.Sql.Builders.Core;

namespace Util.Data.Sql.Builders.Caches; 

/// <summary>
/// Clase abstracta que define la base para el almacenamiento en caché de columnas.
/// Implementa la interfaz <see cref="IColumnCache"/>.
/// </summary>
/// <remarks>
/// Esta clase proporciona una estructura básica para las implementaciones específicas de caché de columnas,
/// permitiendo a las subclases definir su propio comportamiento de almacenamiento y recuperación de datos.
/// </remarks>
public abstract class ColumnCacheBase : IColumnCache {
    protected readonly IDialect Dialect;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ColumnCacheBase"/>.
    /// </summary>
    /// <param name="dialect">El dialecto de la base de datos que se utilizará.</param>
    protected ColumnCacheBase( IDialect dialect ) {
        Dialect = dialect;
    }

    /// <summary>
    /// Obtiene una representación segura de las columnas especificadas.
    /// </summary>
    /// <param name="columns">Una cadena que representa las columnas a procesar.</param>
    /// <returns>Una cadena que contiene las columnas de forma segura.</returns>
    /// <remarks>
    /// Este método es abstracto y debe ser implementado en una clase derivada.
    /// La implementación debe asegurar que las columnas devueltas no contengan
    /// caracteres o formatos que puedan comprometer la seguridad de la aplicación.
    /// </remarks>
    public abstract string GetSafeColumns( string columns );

    /// <summary>
    /// Normaliza una cadena de columnas separadas por comas.
    /// </summary>
    /// <param name="columns">Una cadena que contiene los nombres de las columnas separados por comas.</param>
    /// <returns>
    /// Una cadena que representa las columnas normalizadas. 
    /// Devuelve <c>null</c> si la cadena de entrada está vacía.
    /// </returns>
    /// <remarks>
    /// Este método elimina las columnas vacías y formatea cada columna utilizando la clase <see cref="ColumnItem"/>.
    /// Al final, se asegura de que no haya una coma adicional al final de la cadena resultante.
    /// </remarks>
    protected string NormalizeColumns(string columns) 
    {
        if (columns.IsEmpty())
            return null;
        var result = new StringBuilder();
        var items = columns.Split(',').Where(column => column.IsEmpty() == false).Select(column => new ColumnItem(Dialect, column));
        foreach (var item in items) 
        {
            item.AppendTo(result);
            result.Append(",");
        }
        result.RemoveEnd(",");
        return result.ToString();
    }

    /// <summary>
    /// Obtiene el nombre de una columna de forma segura, asegurando que se cumplan las 
    /// condiciones necesarias para su uso en consultas o manipulación de datos.
    /// </summary>
    /// <param name="column">El nombre de la columna que se desea obtener de forma segura.</param>
    /// <returns>El nombre de la columna de forma segura.</returns>
    /// <remarks>
    /// Este método es abstracto y debe ser implementado por las clases derivadas. 
    /// La implementación debe garantizar que el nombre de la columna devuelto sea 
    /// válido y seguro para su uso.
    /// </remarks>
    public abstract string GetSafeColumn( string column );

    /// <summary>
    /// Normaliza el nombre de una columna.
    /// </summary>
    /// <param name="column">El nombre de la columna a normalizar.</param>
    /// <returns>
    /// Devuelve el resultado de la normalización de la columna. 
    /// Si el nombre de la columna está vacío, se devuelve null.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="ColumnItem"/> para realizar la normalización 
    /// del nombre de la columna según el dialecto especificado.
    /// </remarks>
    protected string NormalizeColumn(string column) 
    {
        if (column.IsEmpty())
            return null;
        var item = new ColumnItem(Dialect, column);
        return item.ToResult();
    }
}