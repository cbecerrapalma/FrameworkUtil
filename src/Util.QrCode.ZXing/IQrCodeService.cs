using Util.Images;

namespace Util.QrCode;

/// <summary>
/// Interfaz que define los servicios relacionados con la generación y manejo de códigos QR.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ITransientDependency"/> lo que indica que su ciclo de vida es transitorio.
/// Los servicios que implementen esta interfaz deben proporcionar métodos para crear, validar y manipular códigos QR.
/// </remarks>
public interface IQrCodeService : ITransientDependency {
    /// <summary>
    /// Genera un código QR a partir del contenido proporcionado.
    /// </summary>
    /// <param name="content">El contenido que se desea codificar en el código QR.</param>
    /// <returns>Una instancia de <see cref="IQrCodeService"/> que representa el código QR generado.</returns>
    /// <remarks>
    /// Este método toma una cadena de texto y devuelve un objeto que puede ser utilizado para manipular o mostrar el código QR.
    /// Asegúrese de que el contenido no esté vacío o nulo para evitar excepciones.
    /// </remarks>
    IQrCodeService Content(string content);
    /// <summary>
    /// Establece el tamaño del código QR.
    /// </summary>
    /// <param name="size">El tamaño deseado para el código QR.</param>
    /// <returns>Una instancia de <see cref="IQrCodeService"/> con el tamaño configurado.</returns>
    /// <remarks>
    /// Este método permite personalizar el tamaño del código QR que se generará. 
    /// Asegúrese de seleccionar un tamaño adecuado para la cantidad de datos que se desea codificar.
    /// </remarks>
    IQrCodeService Size(QrSize size);
    /// <summary>
    /// Establece el tamaño del código QR.
    /// </summary>
    /// <param name="size">El tamaño del código QR en píxeles.</param>
    /// <returns>Una instancia de <see cref="IQrCodeService"/> con el tamaño especificado.</returns>
    /// <remarks>
    /// Este método permite ajustar el tamaño del código QR generado. 
    /// Asegúrese de que el tamaño sea un valor positivo para evitar errores en la generación del código.
    /// </remarks>
    IQrCodeService Size(int size);
    /// <summary>
    /// Corrige el nivel de corrección de errores para el servicio de generación de códigos QR.
    /// </summary>
    /// <param name="level">El nivel de corrección de errores a aplicar. Este valor determina la cantidad de datos que se pueden recuperar en caso de que el código QR esté dañado.</param>
    /// <returns>Una instancia de <see cref="IQrCodeService"/> con el nivel de corrección de errores configurado.</returns>
    /// <remarks>
    /// Los niveles de corrección de errores pueden variar desde bajo hasta alto, permitiendo que el código QR sea más o menos resistente a daños.
    /// </remarks>
    /// <seealso cref="ErrorCorrectionLevel"/>
    IQrCodeService Correction( ErrorCorrectionLevel level );
    /// <summary>
    /// Establece el tipo de imagen para el servicio de generación de códigos QR.
    /// </summary>
    /// <param name="type">El tipo de imagen que se utilizará para el código QR.</param>
    /// <returns>Una instancia de <see cref="IQrCodeService"/> con el tipo de imagen configurado.</returns>
    /// <remarks>
    /// Este método permite especificar el formato de imagen deseado, lo que puede incluir tipos como PNG, JPEG, etc.
    /// Asegúrese de seleccionar un tipo de imagen compatible con su aplicación.
    /// </remarks>
    IQrCodeService ImageType( ImageType type );
    /// <summary>
    /// Establece el color del código QR.
    /// </summary>
    /// <param name="color">El color que se aplicará al código QR.</param>
    /// <returns>Una instancia de <see cref="IQrCodeService"/> con el color actualizado.</returns>
    /// <remarks>
    /// Este método permite personalizar el color del código QR generado.
    /// Asegúrese de que el color proporcionado sea compatible con el formato de salida deseado.
    /// </remarks>
    IQrCodeService Color(Color color);
    /// <summary>
    /// Establece el color de fondo para el código QR.
    /// </summary>
    /// <param name="color">El color de fondo que se aplicará al código QR.</param>
    /// <returns>Una instancia de <see cref="IQrCodeService"/> con el color de fondo actualizado.</returns>
    /// <remarks>
    /// Este método permite personalizar el color de fondo del código QR generado.
    /// Asegúrese de que el color proporcionado sea compatible con la visualización del código QR.
    /// </remarks>
    IQrCodeService BgColor(Color color);
    /// <summary>
    /// Establece el margen para el código QR generado.
    /// </summary>
    /// <param name="margin">El valor del margen en píxeles que se aplicará al código QR.</param>
    /// <returns>Una instancia de <see cref="IQrCodeService"/> con el margen configurado.</returns>
    /// <remarks>
    /// Este método permite ajustar el espacio en blanco alrededor del código QR,
    /// lo que puede ser útil para mejorar la legibilidad y la escaneabilidad del código.
    /// </remarks>
    IQrCodeService Margin(int margin);
    /// <summary>
    /// Genera un código QR a partir de la ruta especificada.
    /// </summary>
    /// <param name="path">La ruta donde se generará el código QR.</param>
    /// <returns>Una instancia de <see cref="IQrCodeService"/> que representa el código QR generado.</returns>
    /// <remarks>
    /// Este método permite crear un código QR que puede ser utilizado para diversos propósitos, 
    /// como compartir información o enlaces de manera rápida y eficiente.
    /// </remarks>
    /// <seealso cref="IQrCodeService"/>
    IQrCodeService Icon( string path );
    /// <summary>
    /// Convierte el objeto actual en un flujo de datos (Stream).
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="Stream"/> que representa el flujo de datos del objeto actual.
    /// </returns>
    /// <remarks>
    /// Este método puede ser utilizado para serializar el objeto actual a un flujo,
    /// permitiendo su almacenamiento o transmisión a través de una red.
    /// </remarks>
    Stream ToStream();
    /// <summary>
    /// Convierte el objeto actual en un arreglo de bytes.
    /// </summary>
    /// <returns>
    /// Un arreglo de bytes que representa el objeto actual.
    /// </returns>
    /// <remarks>
    /// Este método es útil para serializar el objeto en un formato que puede ser almacenado o transmitido.
    /// </remarks>
    byte[] ToBytes();
    /// <summary>
    /// Convierte la cadena actual en su representación en Base64.
    /// </summary>
    /// <returns>
    /// Una cadena que representa la cadena actual codificada en Base64.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la codificación UTF-8 para convertir la cadena en un arreglo de bytes,
    /// que luego se convierte a una cadena en Base64.
    /// </remarks>
    string ToBase64();
    /// <summary>
    /// Guarda los datos en la ubicación especificada por la ruta.
    /// </summary>
    /// <param name="path">La ruta donde se guardarán los datos.</param>
    /// <remarks>
    /// Este método sobrescribirá cualquier archivo existente en la ruta especificada.
    /// Asegúrese de que la ruta sea válida y que tenga permisos de escritura.
    /// </remarks>
    void Save(string path);
    /// <summary>
    /// Convierte el objeto actual en un flujo (Stream) de forma asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El valor de la tarea es un flujo (Stream) que representa el contenido del objeto actual.
    /// </returns>
    /// <remarks>
    /// Este método permite obtener una representación en forma de flujo del objeto, lo que puede ser útil para operaciones de entrada/salida, como la lectura o escritura de datos.
    /// </remarks>
    /// <seealso cref="Stream"/>
    Task<Stream> ToStreamAsync( CancellationToken cancellationToken = default );
    /// <summary>
    /// Convierte el objeto actual en un arreglo de bytes de manera asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que contiene el arreglo de bytes.</returns>
    /// <remarks>
    /// Este método permite convertir el objeto en un formato de bytes, lo cual puede ser útil para la serialización,
    /// almacenamiento o transmisión de datos. La operación puede ser cancelada utilizando el <paramref name="cancellationToken"/>.
    /// </remarks>
    /// <seealso cref="FromBytesAsync"/>
    Task<byte[]> ToBytesAsync( CancellationToken cancellationToken = default );
    /// <summary>
    /// Guarda de manera asíncrona los datos en la ruta especificada.
    /// </summary>
    /// <param name="path">La ruta donde se guardarán los datos.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de guardado.</returns>
    /// <remarks>
    /// Este método permite guardar datos de forma asíncrona, lo que es útil para no bloquear el hilo de la interfaz de usuario 
    /// durante operaciones de entrada/salida que pueden tardar un tiempo considerable.
    /// </remarks>
    /// <exception cref="System.IO.IOException">Se produce si hay un error de entrada/salida durante el guardado.</exception>
    /// <exception cref="System.ArgumentException">Se produce si la ruta especificada es inválida.</exception>
    /// <seealso cref="LoadAsync(string, CancellationToken)"/>
    Task SaveAsync( string path, CancellationToken cancellationToken = default );
}