namespace Util.FileStorage; 

/// <summary>
/// Representa los argumentos necesarios para la operación de almacenamiento de archivos.
/// </summary>
public class FileStorageArgs {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="FileStorageArgs"/>.
    /// </summary>
    /// <param name="fileName">El nombre del archivo que se almacenará.</param>
    public FileStorageArgs( string fileName ) {
        FileName = fileName;
    }

    /// <summary>
    /// Obtiene el nombre del archivo.
    /// </summary>
    /// <remarks>
    /// Esta propiedad solo tiene un getter, lo que significa que el nombre del archivo es de solo lectura.
    /// </remarks>
    /// <returns>
    /// Un <see cref="string"/> que representa el nombre del archivo.
    /// </returns>
    public string FileName { get; }

    /// <summary>
    /// Obtiene o establece el nombre del archivo según la política definida.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar el nombre de un archivo que cumple con las políticas establecidas 
    /// en la aplicación. Asegúrese de que el nombre del archivo sea válido y cumpla con los requisitos necesarios 
    /// antes de asignarlo a esta propiedad.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el nombre del archivo.
    /// </value>
    public string FileNamePolicy { get; set; }

    /// <summary>
    /// Obtiene o establece el nombre del bucket.
    /// </summary>
    /// <remarks>
    /// Un bucket es un contenedor en la nube donde se almacenan los objetos. 
    /// Asegúrese de que el nombre del bucket cumpla con las normas de nomenclatura 
    /// de su proveedor de servicios en la nube.
    /// </remarks>
    /// <value>
    /// El nombre del bucket como una cadena.
    /// </value>
    public string BucketName { get; set; }

    /// <summary>
    /// Obtiene o establece el nombre de la política del bucket.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para definir la política asociada a un bucket específico,
    /// permitiendo gestionar los permisos y accesos a los recursos almacenados en el mismo.
    /// </remarks>
    /// <value>
    /// Un string que representa el nombre de la política del bucket.
    /// </value>
    public string BucketNamePolicy { get; set; }
}