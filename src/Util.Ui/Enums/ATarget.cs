namespace Util.Ui.Enums; 

/// <summary>
/// Enum que representa los posibles destinos de un enlace HTML.
/// </summary>
public enum ATarget {
    [Description( "_self" )]
    Self,
    [Description( "_blank" )]
    Blank,
    [Description( "_parent" )]
    Parent,
    [Description( "_top" )]
    Top
}