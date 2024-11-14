namespace Util.Images.Commands; 

/// <summary>
/// Representa un comando de texto que implementa la interfaz <see cref="ICommand"/>.
/// </summary>
public class TextCommand : ICommand {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TextCommand"/>.
    /// </summary>
    /// <param name="font">La fuente que se utilizará para el texto.</param>
    /// <param name="text">El texto que se mostrará.</param>
    /// <param name="color">El color del texto en formato de cadena.</param>
    /// <param name="x">La posición en el eje X donde se dibujará el texto.</param>
    /// <param name="y">La posición en el eje Y donde se dibujará el texto.</param>
    public TextCommand( Font font, string text, string color, float x, float y ) {
        Font = font;
        Text = text;
        Color = color;
        X = x;
        Y = y;
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TextCommand"/>.
    /// </summary>
    /// <param name="options">Las opciones de formato de texto que se aplicarán al comando.</param>
    /// <param name="text">El texto que se va a procesar o mostrar.</param>
    /// <param name="color">El color que se aplicará al texto.</param>
    public TextCommand( RichTextOptions options, string text, string color ) {
        Options = options;
        Text = text;
        Color = color;
    }

    /// <summary>
    /// Obtiene la fuente utilizada para el texto.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a la instancia de la fuente que se utiliza para renderizar el texto. 
    /// La fuente puede ser utilizada para personalizar la apariencia del texto en la interfaz de usuario.
    /// </remarks>
    /// <returns>
    /// Un objeto <see cref="Font"/> que representa la fuente actual.
    /// </returns>
    public Font Font { get; }
    /// <summary>
    /// Obtiene el texto asociado a esta instancia.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y devuelve el valor del texto.
    /// </remarks>
    /// <returns>
    /// Una cadena que representa el texto.
    /// </returns>
    public string Text { get; }
    /// <summary>
    /// Obtiene el color asociado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y devuelve el valor del color.
    /// </remarks>
    /// <value>
    /// Un string que representa el color.
    /// </value>
    public string Color { get; }
    /// <summary>
    /// Obtiene el valor de la coordenada X.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y devuelve un valor de tipo float que representa
    /// la posición en el eje X.
    /// </remarks>
    /// <value>
    /// Un número de punto flotante que indica la posición en el eje X.
    /// </value>
    public float X { get; }
    /// <summary>
    /// Representa la coordenada Y de un punto en un espacio bidimensional.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y se utiliza para obtener el valor de la coordenada Y.
    /// </remarks>
    /// <value>
    /// Un valor de tipo <see cref="float"/> que representa la posición en el eje Y.
    /// </value>
    public float Y { get; }
    /// <summary>
    /// Obtiene las opciones de formato de texto enriquecido.
    /// </summary>
    /// <value>
    /// Un objeto <see cref="RichTextOptions"/> que contiene las configuraciones de formato.
    /// </value>
    public RichTextOptions Options { get; }

    /// <inheritdoc />
    /// <summary>
    /// Invoca la operación de dibujo de texto sobre la imagen proporcionada.
    /// </summary>
    /// <param name="image">La imagen sobre la cual se dibujará el texto.</param>
    /// <remarks>
    /// Este método verifica si la imagen es nula y lanza una excepción si es así.
    /// Si las opciones son nulas, se utiliza un método de dibujo de texto básico.
    /// De lo contrario, se aplican las opciones especificadas para el dibujo.
    /// </remarks>
    public void Invoke( Image image ) {
        image.CheckNull( nameof( image ) );
        if ( Options == null ) {
            image.Mutate( x => x.DrawText( Text, Font, SixLabors.ImageSharp.Color.ParseHex( Color ), new PointF( X, Y ) ) );
            return;
        }
        image.Mutate( x => x.DrawText( Options, Text, SixLabors.ImageSharp.Color.ParseHex( Color ) ) );
    }
}