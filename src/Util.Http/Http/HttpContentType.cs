namespace Util.Http; 

/// <summary>
/// Enumera los tipos de contenido HTTP que se pueden utilizar en las solicitudes.
/// </summary>
public enum HttpContentType {
    [Description( "application/x-www-form-urlencoded" )]
    FormUrlEncoded,
    [Description( "application/json" )]
    Json,
    [Description( "text/xml" )]
    Xml,
    [Description( "multipart/form-data" )]
    FormData
}