using Util.Images;
using SixLabors.ImageSharp;
using CorrectionLevel = ZXing.QrCode.Internal.ErrorCorrectionLevel;

namespace Util.QrCode.ZXing;

/// <summary>
/// Proporciona servicios para la generación y manipulación de códigos QR utilizando la biblioteca ZXing.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IQrCodeService"/> y ofrece métodos para crear códigos QR
/// a partir de datos de entrada, así como para personalizar su apariencia.
/// </remarks>
public class ZXingQrCodeService : IQrCodeService
{
    private string _content;
    private int _size;
    private CorrectionLevel _level;
    private ImageType _imageType;
    private System.Drawing.Color _foreground;
    private System.Drawing.Color _background;
    private int _margin;
    private string _iconPath;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ZXingQrCodeService"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor establece los valores predeterminados para el nivel de corrección,
    /// el tipo de imagen, los colores de primer plano y fondo, el margen y el tamaño de la imagen.
    /// </remarks>
    /// <returns>
    /// Una instancia de <see cref="ZXingQrCodeService"/> con configuraciones predeterminadas.
    /// </returns>
    public ZXingQrCodeService()
    {
        _level = CorrectionLevel.L;
        _imageType = Images.ImageType.Png;
        _foreground = System.Drawing.Color.Black;
        _background = System.Drawing.Color.White;
        _margin = 0;
        Size(160);
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el contenido del código QR.
    /// </summary>
    /// <param name="content">El contenido que se desea codificar en el código QR.</param>
    /// <returns>Una instancia de <see cref="IQrCodeService"/> para permitir la encadenación de métodos.</returns>
    public IQrCodeService Content(string content)
    {
        _content = content;
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el tamaño del código QR.
    /// </summary>
    /// <param name="size">El tamaño del código QR a establecer.</param>
    /// <returns>Una instancia de <see cref="IQrCodeService"/> que permite encadenar llamadas.</returns>
    /// <remarks>
    /// Este método utiliza el valor seguro del tamaño proporcionado para configurar el tamaño del código QR.
    /// </remarks>
    public IQrCodeService Size(QrSize size)
    {
        return Size(size.Value().SafeValue());
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el tamaño del código QR.
    /// </summary>
    /// <param name="size">El tamaño que se desea establecer para el código QR.</param>
    /// <returns>Una instancia de <see cref="IQrCodeService"/> con el tamaño actualizado.</returns>
    /// <remarks>
    /// Este método permite configurar el tamaño del código QR que se generará.
    /// Asegúrese de que el tamaño sea un valor válido para evitar errores en la generación del código.
    /// </remarks>
    public IQrCodeService Size(int size)
    {
        _size = size;
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el nivel de corrección de errores para el servicio de código QR.
    /// </summary>
    /// <param name="level">El nivel de corrección de errores a establecer. 
    /// Puede ser uno de los siguientes: L, M, Q, H.</param>
    /// <returns>Una instancia del servicio de código QR con el nivel de corrección de errores actualizado.</returns>
    /// <remarks>
    /// Los niveles de corrección de errores determinan la capacidad del código QR para 
    /// recuperar información en caso de que parte del código esté dañado. 
    /// - L: 7% de corrección de errores.
    /// - M: 15% de corrección de errores.
    /// - Q: 25% de corrección de errores.
    /// - H: 30% de corrección de errores.
    /// </remarks>
    public IQrCodeService Correction(ErrorCorrectionLevel level)
    {
        switch (level)
        {
            case ErrorCorrectionLevel.L:
                _level = CorrectionLevel.L;
                break;
            case ErrorCorrectionLevel.M:
                _level = CorrectionLevel.M;
                break;
            case ErrorCorrectionLevel.Q:
                _level = CorrectionLevel.Q;
                break;
            case ErrorCorrectionLevel.H:
                _level = CorrectionLevel.H;
                break;
        }
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el tipo de imagen para el servicio de generación de códigos QR.
    /// </summary>
    /// <param name="type">El tipo de imagen que se utilizará para el código QR.</param>
    /// <returns>El servicio de código QR con el tipo de imagen actualizado.</returns>
    /// <seealso cref="IQrCodeService"/>
    public IQrCodeService ImageType(ImageType type)
    {
        _imageType = type;
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el color de primer plano para el código QR.
    /// </summary>
    /// <param name="color">El color que se utilizará como primer plano en el código QR.</param>
    /// <returns>Una instancia del servicio de código QR para permitir la encadenación de llamadas.</returns>
    /// <seealso cref="IQrCodeService"/>
    public IQrCodeService Color(System.Drawing.Color color)
    {
        _foreground = color;
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el color de fondo para el servicio de generación de códigos QR.
    /// </summary>
    /// <param name="color">El color de fondo que se desea aplicar.</param>
    /// <returns>Una instancia del servicio de códigos QR con el color de fondo actualizado.</returns>
    /// <remarks>
    /// Este método permite personalizar el color de fondo del código QR generado.
    /// Asegúrese de que el color proporcionado sea válido para evitar errores de renderizado.
    /// </remarks>
    public IQrCodeService BgColor(System.Drawing.Color color)
    {
        _background = color;
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el margen para el servicio de generación de códigos QR.
    /// </summary>
    /// <param name="margin">El valor del margen que se aplicará al código QR.</param>
    /// <returns>Una instancia de <see cref="IQrCodeService"/> con el margen actualizado.</returns>
    /// <remarks>
    /// Este método permite ajustar el margen que rodea el código QR generado.
    /// Un margen mayor puede mejorar la legibilidad del código QR en ciertas situaciones.
    /// </remarks>
    public IQrCodeService Margin(int margin)
    {
        _margin = margin;
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece la ruta del icono para el servicio de código QR.
    /// </summary>
    /// <param name="path">La ruta del archivo del icono que se desea establecer.</param>
    /// <returns>Una instancia de <see cref="IQrCodeService"/> para permitir la encadenación de métodos.</returns>
    public IQrCodeService Icon(string path)
    {
        _iconPath = path;
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Convierte el objeto actual en un flujo de datos (Stream).
    /// </summary>
    /// <returns>
    /// Un flujo de memoria (MemoryStream) que contiene los bytes del objeto.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el método <see cref="ToBytes"/> para obtener la representación en bytes del objeto.
    /// </remarks>
    public Stream ToStream()
    {
        var bytes = ToBytes();
        return new MemoryStream(bytes);
    }

    /// <inheritdoc />
    /// <summary>
    /// Convierte la imagen actual en un arreglo de bytes.
    /// </summary>
    /// <returns>
    /// Un arreglo de bytes que representa la imagen.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un flujo de memoria para guardar la imagen en formato de bytes.
    /// Asegúrese de que la imagen se haya inicializado correctamente antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="GetImage"/>
    /// <seealso cref="GetImageEncoder"/>
    public byte[] ToBytes()
    {
        using var image = GetImage();
        using var stream = new MemoryStream();
        image.Save(stream, GetImageEncoder());
        return stream.ToArray();
    }

    /// <inheritdoc />
    /// <summary>
    /// Convierte la imagen obtenida a una cadena en formato Base64.
    /// </summary>
    /// <returns>
    /// Una cadena que representa la imagen en formato Base64.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el método <see cref="GetImage"/> para obtener la imagen
    /// y el método <see cref="GetImageFormat"/> para determinar el formato de la imagen.
    /// </remarks>
    public string ToBase64()
    {
        using var image = GetImage();
        return image.ToBase64String(GetImageFormat());
    }

    /// <summary>
    /// Obtiene el formato de imagen asociado con el tipo de imagen actual.
    /// </summary>
    /// <returns>
    /// Un objeto que representa el formato de imagen.
    /// </returns>
    private IImageFormat GetImageFormat()
    {
        return ImageWrapper.GetImageFormat(_imageType);
    }

    /// <inheritdoc />
    /// <summary>
    /// Guarda la imagen en la ruta especificada.
    /// </summary>
    /// <param name="path">La ruta donde se guardará la imagen. Si la ruta está vacía, no se realizará ninguna acción.</param>
    /// <remarks>
    /// Este método crea el directorio especificado en la ruta si no existe y luego guarda la imagen utilizando el codificador de imagen correspondiente.
    /// </remarks>
    /// <seealso cref="GetImage"/>
    /// <seealso cref="GetImageEncoder"/>
    public void Save(string path)
    {
        if (path.IsEmpty())
            return;
        Util.Helpers.File.CreateDirectory(path);
        using var image = GetImage();
        image.Save(path, GetImageEncoder());
    }

    /// <inheritdoc />
    /// <summary>
    /// Convierte el objeto actual en un flujo de datos asíncrono.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Un <see cref="Stream"/> que representa los datos del objeto actual.
    /// </returns>
    /// <remarks>
    /// Este método primero convierte el objeto en un arreglo de bytes y luego lo envuelve en un <see cref="MemoryStream"/>.
    /// </remarks>
    /// <seealso cref="ToBytesAsync(CancellationToken)"/>
    public async Task<Stream> ToStreamAsync(CancellationToken cancellationToken = default)
    {
        var bytes = await ToBytesAsync(cancellationToken);
        return new MemoryStream(bytes);
    }

    /// <inheritdoc />
    /// <summary>
    /// Convierte la imagen actual a un arreglo de bytes de forma asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Un arreglo de bytes que representa la imagen actual.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un flujo de memoria para guardar la imagen y luego convierte el flujo a un arreglo de bytes.
    /// Asegúrese de manejar adecuadamente el token de cancelación para evitar operaciones innecesarias.
    /// </remarks>
    /// <seealso cref="GetImage"/>
    /// <seealso cref="GetImageEncoder"/>
    public async Task<byte[]> ToBytesAsync(CancellationToken cancellationToken = default)
    {
        using var image = GetImage();
        using var stream = new MemoryStream();
        await image.SaveAsync(stream, GetImageEncoder(), cancellationToken);
        return stream.ToArray();
    }

    /// <summary>
    /// Genera una imagen de código QR a partir del contenido especificado.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="Image"/> que representa el código QR generado.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Se lanza cuando el contenido está vacío.
    /// </exception>
    /// <remarks>
    /// Este método utiliza la biblioteca de generación de códigos de barras para crear un código QR.
    /// Se pueden configurar varias opciones como el conjunto de caracteres, el nivel de corrección de errores,
    /// el margen y el tamaño de la imagen. Además, si se proporciona una ruta de icono, 
    /// se fusionará con la imagen del código QR generado.
    /// </remarks>
    private Image GetImage()
    {
        if (_content.IsEmpty())
            throw new ArgumentException("Debes establecer el contenido.");
        var writer = new BarcodeWriter<Image<Rgba32>>
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                CharacterSet = "UTF-8",
                ErrorCorrection = _level,
                Margin = _margin,
                Width = _size,
                Height = _size
            },
            Renderer = new ImageSharpRenderer<Rgba32>
            {
                Foreground = _foreground.ToImageSharpColor(),
                Background = _background.ToImageSharpColor()
            }
        };
        var result = writer.Write(_content);
        return _iconPath.IsEmpty() ? result : MergeImage(result, Image.Load(_iconPath));
    }

    /// <summary>
    /// Combina una imagen con un icono, aplicando márgenes y redondeando las esquinas.
    /// </summary>
    /// <param name="image">La imagen principal a la que se le añadirá el icono.</param>
    /// <param name="icon">El icono que se superpondrá a la imagen principal.</param>
    /// <returns>La imagen resultante que contiene el icono superpuesto en el centro.</returns>
    /// <remarks>
    /// Este método ajusta el tamaño del icono en función de la imagen principal y aplica un margen.
    /// Además, redondea las esquinas de ambas imágenes antes de combinarlas.
    /// </remarks>
    private Image MergeImage(Image image, Image icon)
    {
        var margin = 10 - _margin;
        if (margin <= 0)
            margin = 5;
        var width = (image.Width * margin - 46 * margin) * 1.0f / 46;
        icon.Zoom(width / icon.Width);
        icon.RoundCorners(7);
        image.RoundCorners(7);
        image.Mutate(x =>
        {
            x.DrawImage(icon, new SixLabors.ImageSharp.Point((image.Width - icon.Width) / 2, (image.Height - icon.Height) / 2), 1);
        });
        return image;
    }

    /// <summary>
    /// Obtiene el codificador de imagen correspondiente al tipo de imagen especificado.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="IImageEncoder"/> que se utiliza para codificar imágenes.
    /// </returns>
    private IImageEncoder GetImageEncoder()
    {
        return ImageWrapper.GetImageEncoder(_imageType);
    }

    /// <inheritdoc />
    /// <summary>
    /// Guarda una imagen de forma asíncrona en la ruta especificada.
    /// </summary>
    /// <param name="path">La ruta donde se guardará la imagen.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <remarks>
    /// Este método verifica si la ruta proporcionada está vacía. Si es así, no realiza ninguna acción.
    /// Si la ruta es válida, se crea el directorio correspondiente y se guarda la imagen utilizando un codificador específico.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de guardar la imagen.
    /// </returns>
    public async Task SaveAsync(string path, CancellationToken cancellationToken = default)
    {
        if (path.IsEmpty())
            return;
        Util.Helpers.File.CreateDirectory(path);
        using var image = GetImage();
        await image.SaveAsync(path, GetImageEncoder(), cancellationToken);
    }
}