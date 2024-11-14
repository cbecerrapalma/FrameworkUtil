using SixLabors.ImageSharp.Drawing;

namespace Util.Images;

/// <summary>
/// Proporciona métodos de extensión para trabajar con imágenes utilizando la biblioteca ImageSharp.
/// </summary>
public static class ImageSharpExtensions {
    /// <summary>
    /// Convierte un color de System.Drawing a un color de ImageSharp.
    /// </summary>
    /// <param name="color">El color de tipo System.Drawing.Color que se desea convertir.</param>
    /// <returns>Un color de tipo ImageSharp.Color representando el mismo color que el original.</returns>
    public static Color ToImageSharpColor( this System.Drawing.Color color ) {
        return Color.FromRgba( color.R, color.G, color.B, color.A );
    }

    /// <summary>
    /// Escala una imagen a un tamaño especificado por un factor de escala.
    /// </summary>
    /// <param name="image">La imagen que se va a escalar.</param>
    /// <param name="scale">El factor de escala que se aplicará a la imagen.</param>
    /// <returns>La imagen escalada.</returns>
    /// <remarks>
    /// Este método utiliza la biblioteca de manipulación de imágenes para cambiar el tamaño de la imagen.
    /// El nuevo tamaño se calcula multiplicando el ancho y el alto de la imagen original por el factor de escala.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si la imagen proporcionada es nula.</exception>
    public static Image Zoom( this Image image, double scale ) {
        var width = Util.Helpers.Convert.ToInt( image.Width * scale );
        var height = Util.Helpers.Convert.ToInt( image.Height * scale );
        image.Mutate( t => {
            t.Resize( new Size( width, height ) );
        } );
        return image;
    }

    /// <summary>
    /// Redondea las esquinas de una imagen con un radio de esquina especificado.
    /// </summary>
    /// <param name="image">La imagen a la que se le redondearán las esquinas.</param>
    /// <param name="cornerRadius">El radio de las esquinas redondeadas.</param>
    /// <returns>La imagen con las esquinas redondeadas.</returns>
    /// <remarks>
    /// Este método utiliza la biblioteca SixLabors.ImageSharp para modificar la imagen.
    /// Asegúrese de que la imagen no sea nula antes de llamar a este método.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si la imagen es nula.</exception>
    public static Image RoundCorners( this Image image, int cornerRadius ) {
        image.Mutate( context => RoundCorners( context, cornerRadius ) );
        return image;
    }

    /// <summary>
    /// Redondea las esquinas de una imagen en el contexto de procesamiento de imágenes especificado.
    /// </summary>
    /// <param name="context">El contexto de procesamiento de imágenes donde se aplicarán los cambios.</param>
    /// <param name="cornerRadius">El radio de las esquinas redondeadas.</param>
    /// <remarks>
    /// Este método utiliza el contexto proporcionado para obtener el tamaño actual de la imagen
    /// y construir una forma que representa las esquinas redondeadas. Luego, establece las opciones
    /// gráficas necesarias para aplicar el efecto de redondeo y llena las esquinas con un color específico.
    /// </remarks>
    private static void RoundCorners( IImageProcessingContext context, int cornerRadius ) {
        var size = context.GetCurrentSize();
        var corners = BuildCorners( size.Width, size.Height, cornerRadius );
        context.SetGraphicsOptions( new GraphicsOptions { AlphaCompositionMode = PixelAlphaCompositionMode.DestOut } );
        context.Fill( Color.Red, corners );
    }

    /// <summary>
    /// Construye una colección de caminos que representan las esquinas redondeadas de un rectángulo.
    /// </summary>
    /// <param name="imageWidth">El ancho de la imagen en píxeles.</param>
    /// <param name="imageHeight">La altura de la imagen en píxeles.</param>
    /// <param name="cornerRadius">El radio de las esquinas redondeadas.</param>
    /// <returns>
    /// Una colección de caminos que representan las cuatro esquinas redondeadas del rectángulo,
    /// posicionadas de acuerdo a las dimensiones de la imagen.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un polígono rectangular y un polígono elíptico para crear las esquinas redondeadas.
    /// Las esquinas se rotan y se traducen para posicionarlas correctamente en la imagen.
    /// </remarks>
    /// <seealso cref="RectangularPolygon"/>
    /// <seealso cref="EllipsePolygon"/>
    /// <seealso cref="PathCollection"/>
    private static IPathCollection BuildCorners( int imageWidth, int imageHeight, float cornerRadius ) {
        var rect = new RectangularPolygon( -0.5f, -0.5f, cornerRadius, cornerRadius );
        var cornerTopLeft = rect.Clip( new EllipsePolygon( cornerRadius - 0.5f, cornerRadius - 0.5f, cornerRadius ) );
        var rightPos = imageWidth - cornerTopLeft.Bounds.Width + 1;
        var bottomPos = imageHeight - cornerTopLeft.Bounds.Height + 1;
        var cornerTopRight = cornerTopLeft.RotateDegree( 90 ).Translate( rightPos, 0 );
        var cornerBottomLeft = cornerTopLeft.RotateDegree( -90 ).Translate( 0, bottomPos );
        var cornerBottomRight = cornerTopLeft.RotateDegree( 180 ).Translate( rightPos, bottomPos );
        return new PathCollection( cornerTopLeft, cornerBottomLeft, cornerTopRight, cornerBottomRight );
    }
}