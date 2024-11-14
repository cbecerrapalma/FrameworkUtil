namespace Util.Images;

/// <summary>
/// Clase que gestiona los avatares en la aplicación.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IAvatarManager"/> y proporciona métodos para crear, 
/// actualizar y eliminar avatares, así como para obtener información sobre ellos.
/// </remarks>
public class AvatarManager : IAvatarManager
{

    #region Campo

    private string _backgroundColor;
    private int _size;
    private string _fontName;
    private double _fontSize;
    private string _fontColor;
    private bool _isBold;
    private bool _isItalic;
    private bool _isUppercase;
    private string _text;
    private int _length;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="AvatarManager"/>.
    /// </summary>
    /// <param name="imageManager">Una instancia de <see cref="IImageManager"/> que se utilizará para gestionar imágenes.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="imageManager"/> es <c>null</c>.</exception>
    public AvatarManager(IImageManager imageManager)
    {
        ImageManager = imageManager ?? throw new ArgumentNullException(nameof(imageManager));
        _backgroundColor = "5d005d";
        _size = 64;
        _fontSize = 0.5;
        _fontColor = "ffffff";
        _isUppercase = true;
    }

    #endregion

    #region atributo

    /// <summary>
    /// Representa un gestor de imágenes.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a la instancia del gestor de imágenes,
    /// permitiendo realizar operaciones relacionadas con la manipulación y gestión
    /// de imágenes en la aplicación.
    /// </remarks>
    /// <value>
    /// Una instancia de <see cref="IImageManager"/> que maneja las operaciones de imagen.
    /// </value>
    protected IImageManager ImageManager { get; }

    #endregion

    #region BackgroundColor

    /// <inheritdoc />
    /// <summary>
    /// Establece el color de fondo para el administrador de avatares.
    /// </summary>
    /// <param name="color">El color de fondo en formato de cadena.</param>
    /// <returns>Una instancia del <see cref="IAvatarManager"/> con el color de fondo actualizado.</returns>
    /// <remarks>
    /// Este método permite personalizar el color de fondo que se utilizará en la representación del avatar.
    /// </remarks>
    public IAvatarManager BackgroundColor(string color)
    {
        _backgroundColor = color;
        return this;
    }

    #endregion

    #region Size

    /// <inheritdoc />
    /// <summary>
    /// Establece el tamaño del avatar.
    /// </summary>
    /// <param name="size">El tamaño que se asignará al avatar.</param>
    /// <returns>Una instancia de <see cref="IAvatarManager"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método permite configurar el tamaño del avatar, lo que puede afectar su representación visual en la interfaz de usuario.
    /// </remarks>
    public IAvatarManager Size(int size)
    {
        _size = size;
        return this;
    }

    #endregion

    #region Font

    /// <inheritdoc />
    /// <summary>
    /// Establece el nombre de la fuente para el administrador de avatares.
    /// </summary>
    /// <param name="fontName">El nombre de la fuente que se desea establecer.</param>
    /// <returns>Una instancia de <see cref="IAvatarManager"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método permite configurar la fuente que se utilizará en los avatares. 
    /// Asegúrese de que el nombre de la fuente sea válido y esté disponible en el sistema.
    /// </remarks>
    public IAvatarManager Font(string fontName)
    {
        _fontName = fontName;
        return this;
    }

    #endregion

    #region FontSize

    /// <inheritdoc />
    /// <summary>
    /// Establece el tamaño de fuente para el administrador de avatares.
    /// </summary>
    /// <param name="size">El tamaño de fuente que se desea establecer.</param>
    /// <returns>Una instancia del administrador de avatares con el tamaño de fuente actualizado.</returns>
    /// <remarks>
    /// Este método permite ajustar el tamaño de la fuente utilizada en la representación del avatar.
    /// </remarks>
    public IAvatarManager FontSize(double size)
    {
        _fontSize = size;
        return this;
    }

    #endregion

    #region FontColor

    /// <inheritdoc />
    /// <summary>
    /// Establece el color de la fuente para el administrador de avatares.
    /// </summary>
    /// <param name="color">El color de la fuente que se desea establecer.</param>
    /// <returns>Una instancia de <see cref="IAvatarManager"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método permite personalizar el color de la fuente utilizada en el avatar.
    /// Asegúrese de que el color proporcionado sea un valor válido.
    /// </remarks>
    public IAvatarManager FontColor(string color)
    {
        _fontColor = color;
        return this;
    }

    #endregion

    #region Bold

    /// <inheritdoc />
    /// <summary>
    /// Establece el estado de negrita para el gestor de avatares.
    /// </summary>
    /// <param name="isBold">Indica si el texto debe mostrarse en negrita. El valor predeterminado es verdadero.</param>
    /// <returns>Devuelve la instancia actual de <see cref="IAvatarManager"/>.</returns>
    /// <remarks>
    /// Este método permite alternar el formato de texto en negrita para el avatar.
    /// </remarks>
    public IAvatarManager Bold(bool isBold = true)
    {
        _isBold = isBold;
        return this;
    }

    #endregion

    #region Italic

    /// <inheritdoc />
    /// <summary>
    /// Establece el estilo de texto en cursiva.
    /// </summary>
    /// <param name="isItalic">Indica si el texto debe estar en cursiva. El valor predeterminado es <c>true</c>.</param>
    /// <returns>Una instancia de <see cref="IAvatarManager"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método permite modificar el estilo de texto de un avatar.
    /// </remarks>
    public IAvatarManager Italic(bool isItalic = true)
    {
        _isItalic = isItalic;
        return this;
    }

    #endregion

    #region Uppercase

    /// <inheritdoc />
    /// <summary>
    /// Establece si el texto debe ser convertido a mayúsculas.
    /// </summary>
    /// <param name="isUppercase">Indica si el texto debe ser convertido a mayúsculas. El valor predeterminado es <c>true</c>.</param>
    /// <returns>La instancia actual de <see cref="IAvatarManager"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método permite configurar el comportamiento de conversión de texto a mayúsculas
    /// en el contexto del gestor de avatares.
    /// </remarks>
    public IAvatarManager Uppercase(bool isUppercase = true)
    {
        _isUppercase = isUppercase;
        return this;
    }

    #endregion

    #region Text

    /// <inheritdoc />
    /// <summary>
    /// Establece el texto y la longitud del texto para el gestor de avatares.
    /// </summary>
    /// <param name="text">El texto que se asignará al gestor de avatares.</param>
    /// <param name="length">La longitud del texto, por defecto es 1.</param>
    /// <returns>Una instancia del gestor de avatares con el texto y la longitud establecidos.</returns>
    /// <remarks>
    /// Este método permite personalizar el texto que se mostrará en el avatar,
    /// así como definir su longitud máxima.
    /// </remarks>
    public IAvatarManager Text(string text, int length = 1)
    {
        _text = text;
        _length = length;
        return this;
    }

    #endregion

    #region SaveAsync

    /// <inheritdoc />
    /// <summary>
    /// Guarda de forma asíncrona el contenido en la ruta especificada.
    /// </summary>
    /// <param name="path">La ruta donde se guardará el contenido.</param>
    /// <param name="cancellationToken">Un token para cancelar la operación, si es necesario.</param>
    /// <remarks>
    /// Este método verifica si el contenido está vacío antes de intentar guardarlo.
    /// Si el contenido está vacío, el método no realiza ninguna acción.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación de guardado asíncrona.
    /// </returns>
    /// <seealso cref="GetImageWrapper"/>
    public virtual async Task SaveAsync(string path, CancellationToken cancellationToken = default)
    {
        if (_text.IsEmpty())
            return;
        var imageWrapper = GetImageWrapper();
        await imageWrapper.SaveAsync(path, cancellationToken);
    }

    /// <summary>
    /// Obtiene un envoltorio de imagen configurado con el tamaño, color de fondo, 
    /// fuente y texto especificados.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="IImageWrapper"/> que representa 
    /// la imagen configurada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el <see cref="ImageManager"/> para crear una imagen con 
    /// las propiedades definidas por el tamaño, color de fondo, tamaño de fuente, 
    /// estilo de fuente, nombre de fuente y color de texto.
    /// </remarks>
    private IImageWrapper GetImageWrapper()
    {
        return ImageManager.CreateImage(GetSize(), GetSize(), _backgroundColor)
            .Font(GetFontSize(), GetFontStyle(), _fontName)
            .TextCenter(GetText(), _fontColor);
    }

    /// <summary>
    /// Obtiene el tamaño ajustado de un recurso.
    /// </summary>
    /// <remarks>
    /// Este método asegura que el tamaño devuelto esté dentro de un rango específico,
    /// donde el tamaño mínimo es 16 y el tamaño máximo es 512. Si el tamaño actual
    /// es menor que 16, se devuelve 16. Si es mayor que 512, se devuelve 512.
    /// En caso contrario, se devuelve el tamaño actual.
    /// </remarks>
    /// <returns>
    /// Un entero que representa el tamaño ajustado, que será al menos 16 y
    /// como máximo 512.
    /// </returns>
    private int GetSize()
    {
        if (_size < 16)
            return 16;
        if (_size > 512)
            return 512;
        return _size;
    }

    /// <summary>
    /// Obtiene el tamaño de fuente ajustado según las condiciones definidas.
    /// </summary>
    /// <returns>
    /// El tamaño de fuente calculado como un valor de punto flotante.
    /// </returns>
    /// <remarks>
    /// Este método evalúa el valor de la variable privada <c>_fontSize</c> y devuelve un tamaño de fuente basado en varias condiciones:
    /// <list type="bullet">
    /// <item>
    /// Si <c>_fontSize</c> está entre 0.1 y 1, se multiplica por <c>_size</c>.
    /// </item>
    /// <item>
    /// Si <c>_fontSize</c> es menor que 16, se devuelve 16.
    /// </item>
    /// <item>
    /// Si <c>_fontSize</c> es mayor que <c>_size</c>, se devuelve la mitad de <c>_size</c>.
    /// </item>
    /// <item>
    /// En cualquier otro caso, se devuelve el valor de <c>_fontSize</c> como está.
    /// </item>
    /// </list>
    /// </remarks>
    private float GetFontSize()
    {
        if (_fontSize is >= 0.1 and <= 1)
            return _size * (float)_fontSize;
        if (_fontSize < 16)
            return 16;
        if (_fontSize > _size)
            return _size * 0.5f;
        return (float)_fontSize;
    }

    /// <summary>
    /// Obtiene el estilo de fuente actual basado en las propiedades de formato.
    /// </summary>
    /// <returns>
    /// Un valor de la enumeración <see cref="FontStyle"/> que representa el estilo de fuente.
    /// Puede ser <see cref="FontStyle.BoldItalic"/>, <see cref="FontStyle.Bold"/>, 
    /// <see cref="FontStyle.Italic"/> o <see cref="FontStyle.Regular"/>.
    /// </returns>
    private FontStyle GetFontStyle()
    {
        if (_isBold && _isItalic)
            return FontStyle.BoldItalic;
        if (_isBold)
            return FontStyle.Bold;
        if (_isItalic)
            return FontStyle.Italic;
        return FontStyle.Regular;
    }

    /// <summary>
    /// Obtiene un texto procesado basado en ciertas condiciones.
    /// </summary>
    /// <returns>
    /// Devuelve el texto procesado si no está vacío; de lo contrario, devuelve null.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el texto está vacío, lo obtiene en un formato separado por "+" y, 
    /// si se indica, lo convierte a mayúsculas. Si el texto resultante excede una longitud 
    /// predefinida, se devuelve un substring del texto hasta esa longitud.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Se lanza si la longitud especificada es menor que cero.
    /// </exception>
    private string GetText()
    {
        if (_text.IsEmpty())
            return null;
        _text = GetPlusSeparatedText();
        if (_isUppercase)
            _text = _text.ToUpperInvariant();
        if (_text.Length > _length)
            return _text.Substring(0, _length);
        return _text;
    }

    /// <summary>
    /// Obtiene un texto separado por el carácter '+'.
    /// </summary>
    /// <returns>
    /// Una cadena de texto que contiene las primeras letras de cada segmento del texto original, 
    /// separados por el carácter '+', o el texto original si no contiene el carácter '+'.
    /// </returns>
    /// <remarks>
    /// Si el texto original no contiene el carácter '+', se devuelve tal cual. 
    /// En caso contrario, se separa el texto por el carácter '+', se eliminan los segmentos vacíos, 
    /// se recortan los espacios en blanco y se toma la primera letra de cada segmento.
    /// </remarks>
    private string GetPlusSeparatedText()
    {
        if (_text.Contains("+") == false)
            return _text;
        _length = 3;
        return _text.Split("+").Where(t => t.IsEmpty() == false).Select(t => t.Trim().Substring(0, 1)).Join(separator: "");
    }

    #endregion

    #region ToStreamAsync

    /// <summary>
    /// Convierte el contenido en un flujo de bytes de forma asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>
    /// Un arreglo de bytes que representa el contenido, o null si el contenido está vacío.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el contenido de texto está vacío antes de intentar convertirlo en un flujo.
    /// Si el contenido está vacío, se devuelve null.
    /// </remarks>
    /// <seealso cref="GetImageWrapper"/>
    public virtual async Task<byte[]> ToStreamAsync(CancellationToken cancellationToken = default)
    {
        if (_text.IsEmpty())
            return null;
        var imageWrapper = GetImageWrapper();
        return await imageWrapper.ToStreamAsync(cancellationToken);
    }

    #endregion

    #region ToBase64

    /// <inheritdoc />
    /// <summary>
    /// Convierte el contenido de la instancia a una representación en Base64.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el contenido en Base64, o null si el texto está vacío.
    /// </returns>
    /// <remarks>
    /// Este método primero verifica si el texto está vacío. Si es así, devuelve null.
    /// De lo contrario, obtiene un envoltorio de imagen y llama al método ToBase64 
    /// de ese envoltorio para obtener la representación en Base64.
    /// </remarks>
    public string ToBase64()
    {
        if (_text.IsEmpty())
            return null;
        var imageWrapper = GetImageWrapper();
        return imageWrapper.ToBase64();
    }

    #endregion
}