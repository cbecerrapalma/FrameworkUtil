namespace Util.FileStorage; 

/// <summary>
/// Enumera las unidades de tamaño de archivo.
/// </summary>
public enum FileSizeUnit {
    [Description( "B" )]
    Byte,
    [Description( "KB" )]
    K,
    [Description( "MB" )]
    M,
    [Description( "GB" )]
    G
}