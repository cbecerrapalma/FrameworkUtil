namespace Util.Aop; 

/// <summary>
/// Clase base abstracta para interceptores de parámetros.
/// </summary>
/// <remarks>
/// Esta clase proporciona una base para implementar interceptores que pueden modificar o 
/// manejar parámetros en métodos decorados con atributos de tipo <see cref="ParameterInterceptorAttribute"/>.
/// </remarks>
public abstract class ParameterInterceptorBase : ParameterInterceptorAttribute {
}