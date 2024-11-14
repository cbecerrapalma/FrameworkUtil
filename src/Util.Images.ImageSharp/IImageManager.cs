using Util.Dependency;

namespace Util.Images; 

/// <summary>
/// Interfaz que define las operaciones para la gestión de imágenes.
/// Esta interfaz hereda de <see cref="ISingletonDependency"/> para asegurar que 
/// su implementación sea un singleton en el contenedor de dependencias.
/// </summary>
public interface IImageManager : ISingletonDependency {
    /// <summary>
    /// Crea una imagen con las dimensiones y el color de fondo especificados.
    /// </summary>
    /// <param name="width">El ancho de la imagen en píxeles.</param>
    /// <param name="height">La altura de la imagen en píxeles.</param>
    /// <param name="backgroundColor">El color de fondo de la imagen en formato hexadecimal (opcional). Si no se especifica, se utilizará un color de fondo predeterminado.</param>
    /// <returns>Una instancia de <see cref="IImageWrapper"/> que representa la imagen creada.</returns>
    /// <remarks>
    /// Este método permite crear imágenes personalizadas con un tamaño y color de fondo específicos. 
    /// Si el color de fondo no se proporciona, se aplicará un color predeterminado.
    /// </remarks>
    IImageWrapper CreateImage(int width, int height, string backgroundColor = null);
    /// <summary>
    /// Carga una imagen desde la ruta especificada.
    /// </summary>
    /// <param name="path">La ruta del archivo de imagen que se desea cargar.</param>
    /// <returns>Un objeto que implementa la interfaz <see cref="IImageWrapper"/> que representa la imagen cargada.</returns>
    /// <remarks>
    /// Este método puede lanzar excepciones si la ruta es inválida o si el archivo no se puede abrir.
    /// Asegúrese de que el archivo de imagen exista y que la ruta sea accesible.
    /// </remarks>
    IImageWrapper LoadImage(string path);
}