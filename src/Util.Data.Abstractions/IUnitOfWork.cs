using Util.Data.Filters;

namespace Util.Data; 

/// <summary>
/// Atributo que indica que un método o clase debe ser ignorado por el sistema de AOP (Programación Orientada a Aspectos).
/// </summary>
/// <remarks>
/// Este atributo se utiliza para excluir métodos o clases específicas de la interceptación o modificación por parte de aspectos.
/// Es útil en situaciones donde se desea mantener el comportamiento original de un método o clase sin interferencias.
/// </remarks>
[Util.Aop.Ignore]
public interface IUnitOfWork : IDisposable, IFilterOperation {
    /// <summary>
    /// Realiza un commit de las operaciones pendientes de forma asíncrona.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El valor devuelto es un entero que indica el resultado del commit.
    /// </returns>
    /// <remarks>
    /// Este método debe ser llamado cuando se deseen guardar los cambios realizados en el contexto actual.
    /// Asegúrese de manejar las excepciones que puedan surgir durante la operación.
    /// </remarks>
    /// <seealso cref="RollbackAsync"/>
    Task<int> CommitAsync();
}