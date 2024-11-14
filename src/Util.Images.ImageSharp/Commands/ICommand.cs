namespace Util.Images.Commands; 

/// <summary>
/// Define un contrato para comandos que pueden ser ejecutados.
/// </summary>
public interface ICommand {
    /// <summary>
    /// Invoca un proceso utilizando la imagen proporcionada.
    /// </summary>
    /// <param name="image">La imagen que se utilizará en el proceso.</param>
    /// <remarks>
    /// Este método es responsable de realizar las operaciones necesarias con la imagen.
    /// Asegúrese de que la imagen no sea nula antes de invocar este método.
    /// </remarks>
    void Invoke(Image image);
}