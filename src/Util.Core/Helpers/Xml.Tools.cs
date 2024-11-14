namespace Util.Helpers; 

/// <summary>
/// Representa una clase para manejar operaciones relacionadas con XML.
/// </summary>
/// <remarks>
/// Esta clase proporciona métodos para cargar, manipular y guardar documentos XML.
/// </remarks>
public partial class Xml {
    /// <summary>
    /// Convierte una cadena XML en un objeto <see cref="XDocument"/>.
    /// </summary>
    /// <param name="xml">La cadena que representa el contenido XML a convertir.</param>
    /// <returns>
    /// Un objeto <see cref="XDocument"/> que representa el XML proporcionado.
    /// </returns>
    /// <exception cref="System.Xml.XmlException">
    /// Se lanza si la cadena XML no es válida.
    /// </exception>
    public static XDocument ToDocument( string xml ) {
        return XDocument.Parse( xml );
    }

    /// <summary>
    /// Convierte una cadena XML en una lista de elementos XML.
    /// </summary>
    /// <param name="xml">La cadena que contiene el contenido XML que se desea convertir.</param>
    /// <returns>
    /// Una lista de elementos XML extraídos del documento XML. 
    /// Si el documento no tiene una raíz o si la cadena XML es nula o vacía, se devuelve una lista vacía.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el método <see cref="ToDocument(string)"/> para crear un objeto <see cref="XDocument"/> 
    /// a partir de la cadena XML proporcionada. Luego, extrae los elementos hijos de la raíz del documento.
    /// </remarks>
    public static List<XElement> ToElements( string xml ) {
        var document = ToDocument( xml );
        if( document?.Root == null )
            return new List<XElement>();
        return document.Root.Elements().ToList();
    }

    /// <summary>
    /// Carga un archivo XML desde la ruta especificada y lo convierte en un documento XDocument.
    /// </summary>
    /// <param name="filePath">La ruta del archivo XML que se va a cargar.</param>
    /// <returns>
    /// Un objeto <see cref="XDocument"/> que representa el contenido del archivo XML cargado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza una codificación UTF-8 de forma predeterminada para leer el archivo.
    /// </remarks>
    /// <seealso cref="LoadFileToDocumentAsync(string, Encoding)"/>
    public static async Task<XDocument> LoadFileToDocumentAsync( string filePath ) {
        return await LoadFileToDocumentAsync( filePath, Encoding.UTF8 );
    }

    /// <summary>
    /// Carga un archivo XML desde la ruta especificada y lo convierte en un objeto <see cref="XDocument"/>.
    /// </summary>
    /// <param name="filePath">La ruta del archivo XML que se desea cargar.</param>
    /// <param name="encoding">La codificación que se utilizará para leer el archivo.</param>
    /// <returns>
    /// Un objeto <see cref="XDocument"/> que representa el contenido del archivo XML.
    /// </returns>
    /// <remarks>
    /// Este método es asíncrono y utiliza la codificación proporcionada para leer el archivo. 
    /// Asegúrese de que la ruta del archivo sea válida y que el archivo exista para evitar excepciones.
    /// </remarks>
    /// <seealso cref="XDocument"/>
    /// <seealso cref="Util.Helpers.File.ReadToStringAsync(string, Encoding)"/>
    public static async Task<XDocument> LoadFileToDocumentAsync( string filePath,Encoding encoding ) {
        var xml = await Util.Helpers.File.ReadToStringAsync( filePath, encoding );
        return ToDocument( xml );
    }

    /// <summary>
    /// Carga un archivo XML y lo convierte en una lista de elementos XElement de forma asíncrona.
    /// </summary>
    /// <param name="filePath">La ruta del archivo XML que se desea cargar.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El valor de la tarea es una lista de elementos XElement 
    /// que se han cargado desde el archivo XML especificado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza una codificación UTF-8 para leer el archivo. 
    /// Se recomienda asegurarse de que el archivo esté en el formato correcto para evitar errores de análisis.
    /// </remarks>
    /// <seealso cref="LoadFileToElementsAsync(string, Encoding)"/>
    public static async Task<List<XElement>> LoadFileToElementsAsync( string filePath ) {
        return await LoadFileToElementsAsync( filePath, Encoding.UTF8 );
    }

    /// <summary>
    /// Carga un archivo XML y lo convierte en una lista de elementos XML.
    /// </summary>
    /// <param name="filePath">La ruta del archivo XML que se va a cargar.</param>
    /// <param name="encoding">La codificación que se utilizará para leer el archivo.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con una lista de elementos XML como resultado.
    /// </returns>
    /// <remarks>
    /// Este método lee el contenido de un archivo XML de forma asíncrona y lo convierte en una lista de elementos XML utilizando el método <see cref="ToElements(string)"/>.
    /// </remarks>
    /// <seealso cref="ToElements(string)"/>
    public static async Task<List<XElement>> LoadFileToElementsAsync( string filePath, Encoding encoding ) {
        var xml = await Util.Helpers.File.ReadToStringAsync( filePath, encoding );
        return ToElements( xml );
    }
}