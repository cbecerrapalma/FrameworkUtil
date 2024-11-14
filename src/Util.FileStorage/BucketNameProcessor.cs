namespace Util.FileStorage; 

/// <summary>
/// Clase que implementa la interfaz <see cref="IBucketNameProcessor"/>.
/// Procesa nombres de buckets para realizar diversas operaciones.
/// </summary>
public class BucketNameProcessor : IBucketNameProcessor {
    /// <inheritdoc />
    /// <summary>
    /// Procesa el nombre del bucket, transformándolo a un formato específico.
    /// </summary>
    /// <param name="bucketName">El nombre del bucket a procesar.</param>
    /// <returns>Un objeto <see cref="ProcessedName"/> que contiene el nombre procesado y el nombre original.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="bucketName"/> está vacío.</exception>
    /// <remarks>
    /// Este método convierte el nombre del bucket a minúsculas, reemplaza los guiones bajos y puntos por guiones, 
    /// y elimina los guiones al principio y al final del nombre.
    /// </remarks>
    public ProcessedName Process( string bucketName ) {
        if ( bucketName.IsEmpty() )
            throw new ArgumentNullException( nameof( bucketName ) );
        var result = bucketName.ToLowerInvariant().Replace( "_","-" ).Replace( ".", "-" ).Trim( '-' );
        return new ProcessedName( result, bucketName );
    }
}