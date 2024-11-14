using System.ComponentModel;

namespace Util.Ui.NgZorro.Enums;

/// <summary>
/// Enum que representa el manejo de parámetros de consulta.
/// </summary>
public enum QueryParamsHandling {
    [Description( "merge" )]
    Merge,
    [Description( "preserve" )]
    Preserve
}