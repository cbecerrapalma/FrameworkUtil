namespace Util.Images; 

/// <summary>
/// Define una interfaz para envolver imágenes y proporcionar acceso a sus propiedades y métodos.
/// </summary>
public interface IImageWrapper {
    /// <summary>
    /// Obtiene una instancia de <see cref="IImageWrapper"/> basada en el tipo de imagen especificado.
    /// </summary>
    /// <param name="type">El tipo de imagen que se desea envolver.</param>
    /// <returns>Una instancia de <see cref="IImageWrapper"/> que representa el tipo de imagen especificado.</returns>
    /// <remarks>
    /// Este método permite crear diferentes implementaciones de <see cref="IImageWrapper"/> 
    /// según el tipo de imagen que se necesite manejar. Asegúrese de pasar un tipo de imagen válido.
    /// </remarks>
    /// <seealso cref="IImageWrapper"/>
    /// <seealso cref="ImageType"/>
    IImageWrapper ImageType( ImageType type );

    /// <summary>
    /// Establece el nombre de la fuente predeterminada.
    /// </summary>
    /// <param name="name">El nombre de la fuente que se desea establecer como predeterminada.</param>
    /// <returns>Una instancia de <see cref="IImageWrapper"/> que representa la fuente configurada.</returns>
    /// <remarks>
    /// Este método permite cambiar la fuente utilizada por defecto en el sistema. 
    /// Asegúrese de que el nombre de la fuente proporcionado sea válido y esté disponible en el sistema.
    /// </remarks>
    IImageWrapper DefaultFontName(string name);

    /// <summary>
    /// Crea un envoltorio de imagen con la fuente especificada.
    /// </summary>
    /// <param name="size">El tamaño de la fuente en puntos.</param>
    /// <param name="style">El estilo de la fuente. Por defecto es <see cref="FontStyle.Regular"/>.</param>
    /// <param name="fontName">El nombre de la fuente. Si es <c>null</c>, se utilizará la fuente predeterminada.</param>
    /// <returns>Un objeto que implementa <see cref="IImageWrapper"/> con la fuente configurada.</returns>
    /// <remarks>
    /// Este método permite personalizar la apariencia del texto en la imagen.
    /// Asegúrese de que la fuente especificada esté disponible en el sistema.
    /// </remarks>
    /// <seealso cref="FontStyle"/>
    IImageWrapper Font( float size, FontStyle style = FontStyle.Regular, string fontName = null );

    /// <summary>
    /// Crea una instancia de un envoltorio de imagen con la fuente especificada y el tamaño dado.
    /// </summary>
    /// <param name="size">El tamaño de la fuente en puntos.</param>
    /// <param name="fontName">El nombre de la fuente que se utilizará.</param>
    /// <returns>Una instancia de <see cref="IImageWrapper"/> que representa la imagen con la fuente aplicada.</returns>
    /// <remarks>
    /// Este método permite personalizar la apariencia de la imagen mediante la selección de diferentes fuentes y tamaños.
    /// Asegúrese de que la fuente especificada esté disponible en el sistema.
    /// </remarks>
    IImageWrapper Font( float size, string fontName );

    /// <summary>
    /// Crea una imagen de texto con las propiedades especificadas.
    /// </summary>
    /// <param name="text">El texto que se mostrará en la imagen.</param>
    /// <param name="color">El color del texto, especificado en formato de cadena.</param>
    /// <param name="x">La posición X en la que se dibujará el texto.</param>
    /// <param name="y">La posición Y en la que se dibujará el texto.</param>
    /// <returns>Una instancia de <see cref="IImageWrapper"/> que representa la imagen de texto creada.</returns>
    /// <remarks>
    /// Este método permite personalizar la apariencia del texto mediante la selección de un color y la posición en la que se dibuja.
    /// Asegúrese de que el color especificado sea válido para evitar errores en la representación.
    /// </remarks>
    IImageWrapper Text( string text, string color, float x, float y );

    /// <summary>
    /// Crea una representación de texto con las opciones especificadas.
    /// </summary>
    /// <param name="text">El texto que se desea mostrar.</param>
    /// <param name="color">El color del texto en formato hexadecimal o nombre de color.</param>
    /// <param name="options">Opciones adicionales para el formato del texto.</param>
    /// <returns>Una instancia de <see cref="IImageWrapper"/> que representa el texto formateado.</returns>
    /// <remarks>
    /// Este método permite personalizar la apariencia del texto mediante el uso de opciones ricas.
    /// Asegúrese de que el color proporcionado sea válido para evitar errores de renderizado.
    /// </remarks>
    /// <seealso cref="IImageWrapper"/>
    /// <seealso cref="RichTextOptions"/>
    IImageWrapper Text( string text, string color, RichTextOptions options );

    /// <summary>
    /// Centra el texto en un contenedor de imagen con un color específico.
    /// </summary>
    /// <param name="text">El texto que se desea centrar.</param>
    /// <param name="color">El color en el que se mostrará el texto.</param>
    /// <returns>Un objeto que implementa la interfaz <see cref="IImageWrapper"/> que contiene el texto centrado.</returns>
    /// <remarks>
    /// Este método es útil para crear imágenes que contengan texto centrado, permitiendo personalizar el color del texto.
    /// Asegúrese de que el color proporcionado sea un valor válido para evitar errores de formato.
    /// </remarks>
    IImageWrapper TextCenter(string text, string color);

    /// <summary>
    /// Guarda de manera asíncrona los datos en la ruta especificada.
    /// </summary>
    /// <param name="path">La ruta donde se guardarán los datos.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de guardar.</returns>
    /// <remarks>
    /// Este método permite guardar datos de forma no bloqueante, lo que es útil en aplicaciones que requieren una interfaz de usuario responsiva.
    /// Asegúrese de manejar adecuadamente el token de cancelación para permitir que la operación se cancele si es necesario.
    /// </remarks>
    /// <seealso cref="LoadAsync(string, CancellationToken)"/>
    Task SaveAsync( string path, CancellationToken cancellationToken = default );

    /// <summary>
    /// Convierte el contenido en un arreglo de bytes de forma asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que es un arreglo de bytes.</returns>
    /// <remarks>
    /// Este método permite obtener el contenido en formato de bytes, lo que puede ser útil para operaciones de 
    /// entrada/salida, como la escritura en un archivo o la transmisión de datos a través de una red.
    /// </remarks>
    /// <seealso cref="System.Threading.CancellationToken"/>
    Task<byte[]> ToStreamAsync( CancellationToken cancellationToken = default );

    /// <summary>
    /// Convierte una cadena en su representación en Base64.
    /// </summary>
    /// <returns>
    /// Una cadena que representa la entrada codificada en Base64.
    /// </returns>
    /// <remarks>
    /// Este método toma una cadena de texto y la convierte en su equivalente en Base64,
    /// lo que es útil para la transmisión de datos en formatos que requieren codificación
    /// de texto, como en la transmisión de datos a través de redes o en la inclusión de
    /// datos binarios en formatos de texto como JSON o XML.
    /// </remarks>
    string ToBase64();
}