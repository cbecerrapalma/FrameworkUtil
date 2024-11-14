namespace Util.Images; 

/// <summary>
/// Clase que gestiona las operaciones relacionadas con imágenes.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IImageManager"/> y proporciona métodos para cargar, procesar y guardar imágenes.
/// </remarks>
public class ImageManager : IImageManager {
    private static readonly ConcurrentDictionary<string, FontFamily> _fonts = new();

    /// <inheritdoc />
    /// <summary>
    /// Crea una nueva instancia de un envoltorio de imagen con las dimensiones y el color de fondo especificados.
    /// </summary>
    /// <param name="width">El ancho de la imagen en píxeles.</param>
    /// <param name="height">La altura de la imagen en píxeles.</param>
    /// <param name="backgroundColor">El color de fondo de la imagen en formato hexadecimal. Si no se especifica, se utilizará un color de fondo predeterminado.</param>
    /// <returns>Una instancia de <see cref="IImageWrapper"/> que representa la imagen creada.</returns>
    /// <remarks>
    /// Este método permite crear imágenes con un tamaño específico y un color de fondo opcional. 
    /// Si el color de fondo no se proporciona, se aplicará un color predeterminado.
    /// </remarks>
    public IImageWrapper CreateImage( int width, int height, string backgroundColor = null ) {
        return new ImageWrapper( width, height, backgroundColor );
    }

    /// <inheritdoc />
    /// <summary>
    /// Carga una imagen desde la ruta especificada.
    /// </summary>
    /// <param name="path">La ruta del archivo de imagen que se desea cargar.</param>
    /// <returns>Un objeto que implementa <see cref="IImageWrapper"/> que representa la imagen cargada.</returns>
    /// <remarks>
    /// Este método crea una nueva instancia de <see cref="ImageWrapper"/> utilizando la ruta proporcionada.
    /// Asegúrese de que la ruta sea válida y que el archivo exista para evitar excepciones.
    /// </remarks>
    public IImageWrapper LoadImage( string path ) {
        return new ImageWrapper( path );
    }

    /// <summary>
    /// Carga las fuentes TrueType (TTF) desde el directorio especificado.
    /// </summary>
    /// <param name="path">La ruta del directorio que contiene las fuentes TTF.</param>
    /// <remarks>
    /// Este método verifica si el directorio especificado existe antes de intentar cargar las fuentes.
    /// Si el directorio no existe, el método no realiza ninguna acción.
    /// </remarks>
    /// <seealso cref="LoadTtfFonts(string)"/>
    public static void LoadFonts( string path ) {
        if ( Util.Helpers.File.DirectoryExists( path ) == false )
            return;
        LoadTtfFonts( path );
    }

    /// <summary>
    /// Carga fuentes TrueType (.ttf) desde un directorio especificado.
    /// </summary>
    /// <param name="path">La ruta del directorio desde el cual se cargarán las fuentes.</param>
    /// <remarks>
    /// Este método busca todos los archivos con la extensión .ttf en el directorio proporcionado
    /// y llama al método <see cref="LoadFont(string, string)"/> para cada fuente encontrada.
    /// </remarks>
    private static void LoadTtfFonts( string path ) {
        var files = Util.Helpers.File.GetAllFiles( path, "*.ttf" );
        foreach ( var file in files ) {
            var name = System.IO.Path.GetFileNameWithoutExtension( file.Name );
            LoadFont( name, file.FullName );
        }
    }

    /// <summary>
    /// Carga una fuente desde la ruta especificada y la asocia con un nombre dado.
    /// </summary>
    /// <param name="name">El nombre con el que se asociará la fuente cargada.</param>
    /// <param name="path">La ruta del archivo de la fuente que se desea cargar.</param>
    /// <remarks>
    /// Este método utiliza una colección de fuentes para agregar la fuente especificada
    /// y la almacena en un diccionario para su uso posterior.
    /// </remarks>
    /// <seealso cref="FontCollection"/>
    public static void LoadFont( string name, string path ) {
        var fonts = new FontCollection();
        var fontFamily = fonts.Add( path );
        _fonts.TryAdd( name, fontFamily );
    }

    /// <summary>
    /// Obtiene una familia de fuentes basada en el nombre proporcionado.
    /// </summary>
    /// <param name="name">El nombre de la familia de fuentes que se desea obtener.</param>
    /// <returns>
    /// La familia de fuentes correspondiente al nombre proporcionado. 
    /// Si no se encuentra una familia de fuentes válida, se devuelve la primera familia de fuentes del sistema.
    /// </returns>
    /// <remarks>
    /// Este método busca una familia de fuentes utilizando el nombre especificado. 
    /// Si el nombre no corresponde a ninguna familia de fuentes, se retorna la primera familia de fuentes disponible en el sistema.
    /// </remarks>
    public static FontFamily GetFont( string name ) {
        var result = GetFontFamily( name );
        return result.Name.IsEmpty() ? SystemFonts.Families.FirstOrDefault() : result;
    }

    /// <summary>
    /// Obtiene una instancia de <see cref="FontFamily"/> basada en el nombre proporcionado.
    /// </summary>
    /// <param name="name">El nombre de la familia de fuentes que se desea obtener.</param>
    /// <returns>
    /// Una instancia de <see cref="FontFamily"/> correspondiente al nombre proporcionado.
    /// Si el nombre está vacío, se devuelve una nueva instancia de <see cref="FontFamily"/> vacía.
    /// Si no se encuentra la familia de fuentes en el diccionario, se busca en las fuentes del sistema.
    /// </returns>
    /// <remarks>
    /// Este método verifica primero si el nombre de la familia de fuentes está vacío.
    /// Si no lo está, intenta recuperar la familia de fuentes del diccionario interno.
    /// Si no se encuentra, busca en las familias de fuentes del sistema.
    /// </remarks>
    private static FontFamily GetFontFamily( string name ) {
        if ( name.IsEmpty() )
            return new FontFamily();
        if ( _fonts.TryGetValue( name, out var result ) )
            return result;
        return SystemFonts.Families.FirstOrDefault( t => t.Name == name );
    }

    /// <summary>
    /// Obtiene una lista de nombres de fuentes soportadas por el sistema.
    /// </summary>
    /// <returns>
    /// Una lista de cadenas que contiene los nombres de las fuentes soportadas.
    /// </returns>
    /// <remarks>
    /// Este método combina las fuentes definidas en la colección interna _fonts 
    /// y las fuentes del sistema, utilizando la propiedad SystemFonts.Families.
    /// </remarks>
    public static List<string> GetSupportedFonts() {
        var result = new List<string>();
        foreach ( var fontName in _fonts.Keys ) 
            result.Add( fontName );
        result.AddRange( SystemFonts.Families.Select( t => t.Name ) );
        return result;
    }
}