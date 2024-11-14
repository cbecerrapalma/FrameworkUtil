namespace Util.Images; 

/// <summary>
/// Enumera los diferentes tipos de imágenes soportados.
/// </summary>
public enum ImageType {
    [Description( ".jpg" )]
    Jpg,
    [Description( ".jpeg" )]
    Jpeg,
    [Description( ".png" )]
    Png,
    [Description( ".gif" )]
    Gif,
    [Description( ".svg" )]
    Svg,
    [Description( ".bmp" )]
    Bmp,
    [Description( ".webp" )]
    Webp
}