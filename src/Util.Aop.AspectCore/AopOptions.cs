namespace Util.Aop;

/// <summary>
/// Representa las opciones de configuración para el AOP (Programación Orientada a Aspectos).
/// </summary>
public class AopOptions {
    /// <summary>
    /// Obtiene o establece un valor que indica si el proxy de AOP (Programación Orientada a Aspectos) está habilitado.
    /// </summary>
    /// <value>
    /// <c>true</c> si el proxy de AOP está habilitado; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsEnableIAopProxy { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si el aspecto del parámetro está habilitado.
    /// </summary>
    /// <value>
    /// <c>true</c> si el aspecto del parámetro está habilitado; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsEnableParameterAspect { get; set; } = true;
}