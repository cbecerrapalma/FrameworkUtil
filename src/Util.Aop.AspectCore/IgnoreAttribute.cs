namespace Util.Aop; 

/// <summary>
/// Atributo que indica que un elemento debe ser ignorado por el sistema de aspectos.
/// </summary>
/// <remarks>
/// Este atributo se utiliza para marcar métodos o clases que no deben ser procesados 
/// por la lógica de aspectos, permitiendo así excluir ciertas funcionalidades 
/// del comportamiento general del sistema.
/// </remarks>
public class IgnoreAttribute : NonAspectAttribute {
}