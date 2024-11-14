namespace Util.Data;

/// <summary>
/// Representa los operadores utilizados en comparaciones.
/// </summary>
public enum Operator
{
    [Description("igual a")]
    Equal,
    [Description("no igual a")]
    NotEqual,
    [Description("mayor que")]
    Greater,
    [Description("mayor o igual a")]
    GreaterEqual,
    [Description("menor que")]
    Less,
    [Description("menor o igual que")]
    LessEqual,
    [Description("coincidencia de encabezado")]
    Starts,
    [Description("partido de cola")]
    Ends,
    [Description("coincidencia difusa")]
    Contains,
    [Description("In")]
    In,
    [Description("Not In")]
    NotIn,
}