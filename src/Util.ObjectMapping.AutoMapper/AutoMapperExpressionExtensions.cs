namespace Util.ObjectMapping; 

/// <summary>
/// Proporciona métodos de extensión para facilitar la creación de expresiones de mapeo utilizando AutoMapper.
/// </summary>
public static class AutoMapperExpressionExtensions {
    /// <summary>
    /// Ignora el miembro de destino especificado en la expresión de mapeo.
    /// </summary>
    /// <typeparam name="TDestination">El tipo de destino del mapeo.</typeparam>
    /// <typeparam name="TMember">El tipo del miembro que se está mapeando.</typeparam>
    /// <typeparam name="TResult">El tipo del resultado que se ignora.</typeparam>
    /// <param name="mappingExpression">La expresión de mapeo que se está configurando.</param>
    /// <param name="destinationMember">Una expresión que representa el miembro de destino a ignorar.</param>
    /// <returns>La expresión de mapeo actualizada con el miembro de destino ignorado.</returns>
    /// <remarks>
    /// Este método se utiliza para evitar que un miembro específico sea mapeado durante la operación de mapeo.
    /// </remarks>
    /// <seealso cref="IMappingExpression{TDestination, TMember}"/>
    public static IMappingExpression<TDestination, TMember> Ignore<TDestination, TMember, TResult>( this IMappingExpression<TDestination, TMember> mappingExpression, Expression<Func<TMember, TResult>> destinationMember ) {
        return mappingExpression.ForMember( destinationMember, options => options.Ignore() );
    }
}