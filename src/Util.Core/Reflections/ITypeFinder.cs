namespace Util.Reflections; 

/// <summary>
/// Define un contrato para encontrar tipos en un ensamblado.
/// </summary>
public interface ITypeFinder {
    /// <summary>
    /// Busca y devuelve una lista de tipos de datos que coinciden con el tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo de datos que se desea buscar.</typeparam>
    /// <returns>Una lista que contiene todos los tipos que coinciden con el tipo especificado.</returns>
    /// <remarks>
    /// Este método es genérico y permite buscar tipos en un contexto específico. 
    /// Asegúrese de que el tipo T esté definido en el ámbito donde se invoca este método.
    /// </remarks>
    /// <seealso cref="List{T}"/>
    List<Type> Find<T>();
    /// <summary>
    /// Busca y devuelve una lista de tipos que coinciden con el tipo especificado.
    /// </summary>
    /// <param name="findType">El tipo que se desea buscar.</param>
    /// <returns>Una lista de tipos que coinciden con el tipo especificado.</returns>
    /// <remarks>
    /// Este método puede ser útil para realizar búsquedas de tipos en un ensamblado
    /// o para filtrar tipos según ciertos criterios.
    /// </remarks>
    /// <seealso cref="Type"/>
    List<Type> Find( Type findType );
}