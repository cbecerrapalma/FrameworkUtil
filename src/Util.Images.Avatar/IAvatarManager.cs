using Util.Dependency;

namespace Util.Images; 

/// <summary>
/// Define un contrato para la gestión de avatares.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ITransientDependency"/> lo que indica que su implementación
/// debe ser registrada como una dependencia transitoria en el contenedor de inyección de dependencias.
/// </remarks>
public interface IAvatarManager : ITransientDependency{
    /// <summary>
    /// Establece el color de fondo del avatar.
    /// </summary>
    /// <param name="color">El color de fondo en formato de cadena, que puede ser un nombre de color o un código hexadecimal.</param>
    /// <returns>Una instancia de <see cref="IAvatarManager"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método permite personalizar el color de fondo del avatar, lo que puede ser útil para mejorar la apariencia visual 
    /// o para adaptarse a un tema específico de la aplicación.
    /// </remarks>
    IAvatarManager BackgroundColor(string color);
    /// <summary>
    /// Establece el tamaño del avatar.
    /// </summary>
    /// <param name="size">El tamaño deseado del avatar en unidades específicas.</param>
    /// <returns>Una instancia de <see cref="IAvatarManager"/> con el tamaño actualizado.</returns>
    /// <remarks>
    /// Este método permite ajustar el tamaño del avatar según las necesidades del usuario. 
    /// Asegúrese de que el tamaño proporcionado sea un valor válido.
    /// </remarks>
    IAvatarManager Size(int size);
    /// <summary>
    /// Establece la fuente especificada para el gestor de avatares.
    /// </summary>
    /// <param name="fontName">El nombre de la fuente que se desea establecer.</param>
    /// <returns>Una instancia de <see cref="IAvatarManager"/> con la fuente actualizada.</returns>
    /// <remarks>
    /// Este método permite cambiar la fuente utilizada por el gestor de avatares. 
    /// Asegúrese de que la fuente especificada esté disponible en el sistema.
    /// </remarks>
    IAvatarManager Font(string fontName);
    /// <summary>
    /// Establece el tamaño de fuente para el administrador de avatares.
    /// </summary>
    /// <param name="size">El tamaño de fuente que se desea establecer, en puntos.</param>
    /// <returns>Una instancia de <see cref="IAvatarManager"/> con el tamaño de fuente actualizado.</returns>
    /// <remarks>
    /// Este método permite personalizar el tamaño de la fuente utilizada en la representación de los avatares.
    /// Asegúrese de que el tamaño proporcionado sea un valor positivo.
    /// </remarks>
    IAvatarManager FontSize(double size);
    /// <summary>
    /// Establece el color de fuente para el avatar.
    /// </summary>
    /// <param name="color">El color de la fuente en formato de cadena. Puede ser un nombre de color o un código hexadecimal.</param>
    /// <returns>Una instancia de <see cref="IAvatarManager"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método permite personalizar el color de la fuente del avatar, lo que puede ser útil para mejorar la visibilidad o para adaptarse a un tema específico.
    /// </remarks>
    IAvatarManager FontColor(string color);
    /// <summary>
    /// Establece el estilo de texto en negrita.
    /// </summary>
    /// <param name="isBold">Indica si el texto debe estar en negrita. El valor predeterminado es <c>true</c>.</param>
    /// <returns>Una instancia de <see cref="IAvatarManager"/> que permite encadenar llamadas.</returns>
    IAvatarManager Bold(bool isBold = true);
    /// <summary>
    /// Configura el estilo de texto en cursiva.
    /// </summary>
    /// <param name="isItalic">Indica si el texto debe mostrarse en cursiva. El valor predeterminado es <c>true</c>.</param>
    /// <returns>Una instancia de <see cref="IAvatarManager"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método permite modificar el estilo de texto de los avatares. 
    /// Se puede utilizar para resaltar texto o para cumplir con requisitos de diseño específicos.
    /// </remarks>
    IAvatarManager Italic(bool isItalic = true);
    /// <summary>
    /// Cambia el formato del texto a mayúsculas o minúsculas según el valor del parámetro.
    /// </summary>
    /// <param name="isUppercase">Indica si el texto debe ser convertido a mayúsculas. Por defecto es <c>true</c>.</param>
    /// <returns>Una instancia de <see cref="IAvatarManager"/> con el formato de texto actualizado.</returns>
    /// <remarks>
    /// Este método permite alternar entre mayúsculas y minúsculas, facilitando la personalización del texto según las necesidades del usuario.
    /// </remarks>
    IAvatarManager Uppercase(bool isUppercase = true);
    /// <summary>
    /// Establece el texto para el avatar con una longitud opcional.
    /// </summary>
    /// <param name="text">El texto que se desea establecer en el avatar.</param>
    /// <param name="length">La longitud máxima del texto. Por defecto es 1.</param>
    /// <returns>Un objeto que representa el estado actualizado del avatar.</returns>
    /// <remarks>
    /// Este método permite personalizar el texto que se mostrará en el avatar,
    /// asegurando que no exceda la longitud especificada.
    /// </remarks>
    /// <seealso cref="IAvatarManager"/>
    IAvatarManager Text( string text,int length = 1 );
    /// <summary>
    /// Guarda de manera asíncrona los datos en la ubicación especificada.
    /// </summary>
    /// <param name="path">La ruta donde se guardarán los datos.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de guardado.</returns>
    /// <remarks>
    /// Este método permite guardar datos de forma asíncrona, lo que puede ser útil para evitar bloqueos en la interfaz de usuario
    /// o en aplicaciones que requieren alta disponibilidad. Asegúrese de manejar adecuadamente el <paramref name="cancellationToken"/>
    /// para permitir la cancelación de la operación si es necesario.
    /// </remarks>
    /// <seealso cref="LoadAsync(string, CancellationToken)"/>
    Task SaveAsync( string path, CancellationToken cancellationToken = default );
    /// <summary>
    /// Convierte el contenido en un arreglo de bytes de forma asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que es un arreglo de bytes.</returns>
    /// <remarks>
    /// Este método permite obtener el contenido en formato de bytes, lo cual puede ser útil para operaciones de 
    /// entrada/salida, como la escritura en un archivo o el envío a través de una red.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task<byte[]> ToStreamAsync( CancellationToken cancellationToken = default );
    /// <summary>
    /// Convierte la cadena actual en su representación en Base64.
    /// </summary>
    /// <returns>
    /// Una cadena que representa la cadena actual codificada en Base64.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la codificación UTF8 para convertir la cadena en un arreglo de bytes,
    /// que luego se codifica en Base64. Es útil para la transmisión de datos en formatos que
    /// requieren una representación textual, como en el caso de datos binarios en JSON o XML.
    /// </remarks>
    string ToBase64();
}