using Util.Images.Commands;

namespace Util.Images;

/// <summary>
/// Clase que implementa la interfaz <see cref="IImageWrapper"/> para envolver imágenes.
/// </summary>
public class ImageWrapper : IImageWrapper
{

    #region atributo

    private readonly List<ICommand> _commands = new();
    private ImageType _imageType;
    private readonly int _width;
    private readonly int _height;
    private readonly string _backgroundColor;
    private readonly string _loadPath;
    private static string _defaultFontName;
    private Font _font;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ImageWrapper"/>.
    /// </summary>
    /// <param name="width">El ancho de la imagen en píxeles.</param>
    /// <param name="height">La altura de la imagen en píxeles.</param>
    /// <param name="backgroundColor">El color de fondo de la imagen en formato de cadena.</param>
    /// <remarks>
    /// Este constructor establece el tipo de imagen como PNG y asigna los valores proporcionados 
    /// para el ancho, la altura y el color de fondo.
    /// </remarks>
    public ImageWrapper(int width, int height, string backgroundColor)
    {
        _imageType = Images.ImageType.Png;
        _width = width;
        _height = height;
        _backgroundColor = backgroundColor;
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ImageWrapper"/>.
    /// </summary>
    /// <param name="path">La ruta del archivo de imagen que se va a cargar.</param>
    public ImageWrapper(string path)
    {
        _imageType = Images.ImageType.Png;
        _loadPath = path;
    }

    #endregion

    #region ImageType

    /// <summary>
    /// Establece el tipo de imagen.
    /// </summary>
    /// <param name="type">El tipo de imagen que se va a establecer.</param>
    /// <returns>Una instancia de <see cref="IImageWrapper"/> que representa el objeto actual.</returns>
    public IImageWrapper ImageType(ImageType type)
    {
        _imageType = type;
        return this;
    }

    #endregion

    #region DefaultFontName

    /// <summary>
    /// Establece el nombre de la fuente predeterminada.
    /// </summary>
    /// <param name="name">El nombre de la fuente que se establecerá como predeterminada.</param>
    /// <returns>La instancia actual de <see cref="IImageWrapper"/> para permitir la encadenación de métodos.</returns>
    public IImageWrapper DefaultFontName(string name)
    {
        _defaultFontName = name;
        return this;
    }

    #endregion

    #region Font

    /// <inheritdoc />
    /// <summary>
    /// Establece la fuente utilizada para el envoltorio de la imagen.
    /// </summary>
    /// <param name="size">El tamaño de la fuente en puntos.</param>
    /// <param name="style">El estilo de la fuente, que puede ser regular, negrita, cursiva, etc. El valor predeterminado es <see cref="FontStyle.Regular"/>.</param>
    /// <param name="fontName">El nombre de la fuente. Si se proporciona un nombre vacío, se utilizará la fuente predeterminada.</param>
    /// <returns>Devuelve el objeto actual <see cref="IImageWrapper"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método busca la fuente especificada por el nombre y la carga con el tamaño y estilo dados.
    /// Si el nombre de la fuente no se proporciona o está vacío, se utilizará el nombre de fuente predeterminado.
    /// </remarks>
    /// <seealso cref="FontStyle"/>
    public IImageWrapper Font(float size, FontStyle style = FontStyle.Regular, string fontName = null)
    {
        if (fontName.IsEmpty())
            fontName = _defaultFontName;
        var fontFamily = ImageManager.GetFont(fontName);
        _font = new Font(fontFamily, size, style);
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece la fuente con un tamaño y un nombre de fuente específicos.
    /// </summary>
    /// <param name="size">El tamaño de la fuente que se va a establecer.</param>
    /// <param name="fontName">El nombre de la fuente que se va a utilizar.</param>
    /// <returns>Un objeto que implementa <see cref="IImageWrapper"/> con la fuente configurada.</returns>
    /// <seealso cref="Font(float, FontStyle, string)"/>
    public IImageWrapper Font(float size, string fontName)
    {
        return Font(size, FontStyle.Regular, fontName);
    }

    #endregion

    #region Text

    /// <inheritdoc />
    /// <summary>
    /// Agrega un comando de texto a la colección de comandos.
    /// </summary>
    /// <param name="text">El texto que se desea agregar.</param>
    /// <param name="color">El color del texto en formato de cadena.</param>
    /// <param name="x">La posición en el eje X donde se dibujará el texto.</param>
    /// <param name="y">La posición en el eje Y donde se dibujará el texto.</param>
    /// <returns>Una instancia de <see cref="IImageWrapper"/> para permitir la encadenación de métodos.</returns>
    public IImageWrapper Text(string text, string color, float x, float y)
    {
        _commands.Add(new TextCommand(_font, text, color, x, y));
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Agrega un comando de texto a la lista de comandos.
    /// </summary>
    /// <param name="text">El texto que se va a agregar.</param>
    /// <param name="color">El color del texto en formato de cadena.</param>
    /// <param name="options">Opciones adicionales para el formato del texto.</param>
    /// <returns>Una instancia de <see cref="IImageWrapper"/> para permitir el encadenamiento de métodos.</returns>
    /// <remarks>
    /// Este método permite añadir texto a una imagen, configurando su color y opciones de formato.
    /// </remarks>
    public IImageWrapper Text(string text, string color, RichTextOptions options)
    {
        _commands.Add(new TextCommand(options, text, color));
        return this;
    }

    #endregion

    #region TextCenter

    /// <inheritdoc />
    /// <summary>
    /// Centra el texto en el área definida por el ancho y alto especificados.
    /// </summary>
    /// <param name="text">El texto que se desea centrar.</param>
    /// <param name="color">El color del texto en formato de cadena.</param>
    /// <returns>Un objeto que implementa <see cref="IImageWrapper"/> que representa el texto centrado.</returns>
    /// <remarks>
    /// Este método utiliza opciones de texto enriquecido para establecer la alineación vertical y horizontal en el centro.
    /// </remarks>
    public IImageWrapper TextCenter(string text, string color)
    {
        var options = new RichTextOptions(_font)
        {
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            Origin = new Vector2(_width / 2, _height / 2)
        };
        return Text(text, color, options);
    }

    #endregion

    #region SaveAsync

    /// <inheritdoc />
    /// <summary>
    /// Guarda la imagen de forma asíncrona en la ruta especificada.
    /// </summary>
    /// <param name="path">La ruta donde se guardará la imagen.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de guardado de la imagen.</returns>
    /// <remarks>
    /// Este método obtiene una imagen, aplica una serie de comandos a la misma y luego la guarda en el directorio especificado.
    /// Si el directorio no existe, se creará automáticamente.
    /// </remarks>
    /// <seealso cref="GetImage"/>
    /// <seealso cref="GetImageEncoder"/>
    public async Task SaveAsync(string path, CancellationToken cancellationToken = default)
    {
        using var image = GetImage();
        _commands.ForEach(command => command.Invoke(image));
        Util.Helpers.File.CreateDirectory(path);
        await image.SaveAsync(path, GetImageEncoder(), cancellationToken);
    }

    /// <summary>
    /// Obtiene una imagen basada en la ruta de carga especificada.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="Image{T}"/> que representa la imagen cargada 
    /// desde la ruta especificada o una nueva imagen con un color de fondo 
    /// si la ruta de carga está vacía.
    /// </returns>
    /// <remarks>
    /// Si la ruta de carga (_loadPath) está vacía, se crea una nueva imagen 
    /// con el ancho y alto especificados, utilizando el color de fondo definido.
    /// De lo contrario, se carga la imagen desde la ruta especificada.
    /// </remarks>
    protected Image GetImage()
    {
        if (_loadPath.IsEmpty())
            return new Image<Rgba32>(_width, _height, GetColor(_backgroundColor));
        return Image.Load(_loadPath);
    }

    /// <summary>
    /// Obtiene un objeto <see cref="Color"/> a partir de una representación en formato hexadecimal.
    /// </summary>
    /// <param name="color">Una cadena que representa el color en formato hexadecimal.</param>
    /// <returns>
    /// Un objeto <see cref="Color"/> que representa el color especificado.
    /// </returns>
    /// <exception cref="FormatException">Se produce si la cadena <paramref name="color"/> no tiene un formato hexadecimal válido.</exception>
    protected Color GetColor(string color)
    {
        return Color.ParseHex(color);
    }

    /// <summary>
    /// Obtiene el codificador de imágenes correspondiente al tipo de imagen especificado.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="IImageEncoder"/> que se utiliza para codificar imágenes.
    /// </returns>
    private IImageEncoder GetImageEncoder()
    {
        return GetImageEncoder(_imageType);
    }

    #endregion

    #region ToStreamAsync

    /// <inheritdoc />
    /// <summary>
    /// Convierte la imagen a un arreglo de bytes de manera asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>
    /// Un arreglo de bytes que representa la imagen convertida.
    /// </returns>
    /// <remarks>
    /// Este método utiliza una lista de comandos para modificar la imagen antes de guardarla en un flujo de memoria.
    /// La imagen se guarda utilizando un codificador específico y se convierte a un arreglo de bytes.
    /// </remarks>
    /// <seealso cref="GetImage"/>
    /// <seealso cref="GetImageEncoder"/>
    /// <seealso cref="Util.Helpers.File.ToBytesAsync(Stream, CancellationToken)"/>
    public async Task<byte[]> ToStreamAsync(CancellationToken cancellationToken = default)
    {
        using var image = GetImage();
        using var stream = new MemoryStream();
        _commands.ForEach(command => command.Invoke(image));
        await image.SaveAsync(stream, GetImageEncoder(), cancellationToken);
        return await Util.Helpers.File.ToBytesAsync(stream, cancellationToken);
    }

    #endregion

    #region ToBase64

    /// <inheritdoc />
    /// <summary>
    /// Convierte la imagen actual a una representación en formato Base64.
    /// </summary>
    /// <returns>
    /// Una cadena que representa la imagen en formato Base64.
    /// </returns>
    /// <remarks>
    /// Este método obtiene la imagen actual, aplica una serie de comandos sobre ella 
    /// y finalmente convierte la imagen resultante a una cadena en formato Base64.
    /// </remarks>
    /// <seealso cref="GetImage"/>
    /// <seealso cref="GetImageFormat"/>
    public string ToBase64()
    {
        using var image = GetImage();
        _commands.ForEach(command => command.Invoke(image));
        return image.ToBase64String(GetImageFormat());
    }

    /// <summary>
    /// Obtiene el formato de imagen basado en el tipo de imagen actual.
    /// </summary>
    /// <returns>
    /// Un objeto que representa el formato de imagen correspondiente al tipo de imagen.
    /// </returns>
    private IImageFormat GetImageFormat()
    {
        return GetImageFormat(_imageType);
    }

    #endregion

    #region GetImageEncoder

    /// <summary>
    /// Obtiene un codificador de imágenes basado en el tipo de imagen especificado.
    /// </summary>
    /// <param name="type">El tipo de imagen para el cual se desea obtener el codificador.</param>
    /// <returns>Un objeto que implementa la interfaz <see cref="IImageEncoder"/> correspondiente al tipo de imagen.</returns>
    /// <remarks>
    /// Si el tipo de imagen no coincide con ninguno de los tipos definidos, se devuelve un codificador PNG por defecto.
    /// </remarks>
    public static IImageEncoder GetImageEncoder(ImageType type)
    {
        switch (type)
        {
            case Images.ImageType.Png:
                return new PngEncoder();
            case Images.ImageType.Gif:
                return new GifEncoder();
            case Images.ImageType.Bmp:
                return new BmpEncoder();
            case Images.ImageType.Jpg:
                return new JpegEncoder();
            default:
                return new PngEncoder();
        }
    }

    #endregion

    #region GetImageFormat

    /// <summary>
    /// Obtiene el formato de imagen correspondiente al tipo de imagen especificado.
    /// </summary>
    /// <param name="type">El tipo de imagen para el cual se desea obtener el formato.</param>
    /// <returns>
    /// Un objeto que implementa <see cref="IImageFormat"/> que representa el formato de imagen correspondiente al tipo especificado.
    /// Si el tipo no coincide con ninguno de los tipos conocidos, se devuelve el formato PNG por defecto.
    /// </returns>
    /// <remarks>
    /// Este método es útil para determinar el formato de imagen que se debe utilizar al procesar o guardar imágenes
    /// basadas en el tipo de imagen proporcionado.
    /// </remarks>
    /// <seealso cref="ImageType"/>
    /// <seealso cref="IImageFormat"/>
    public static IImageFormat GetImageFormat(ImageType type)
    {
        switch (type)
        {
            case Images.ImageType.Png:
                return PngFormat.Instance;
            case Images.ImageType.Gif:
                return GifFormat.Instance;
            case Images.ImageType.Bmp:
                return BmpFormat.Instance;
            case Images.ImageType.Jpg:
                return JpegFormat.Instance;
            default:
                return PngFormat.Instance;
        }
    }

    #endregion
}