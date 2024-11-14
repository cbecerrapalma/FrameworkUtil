namespace RazorEngineCore; 

/// <summary>
/// Clase base abstracta para plantillas de Razor Engine que permite la creación de plantillas tipadas.
/// </summary>
/// <typeparam name="T">El tipo de modelo que se utilizará en la plantilla.</typeparam>
/// <remarks>
/// Esta clase proporciona la funcionalidad básica para trabajar con plantillas Razor, 
/// permitiendo la inyección de un modelo de tipo <typeparamref name="T"/> en las plantillas.
/// </remarks>
public abstract class RazorEngineTemplateBase<T> : RazorEngineTemplateBase
{
    /// <summary>
    /// Obtiene o establece el modelo de tipo <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite acceder y modificar el modelo asociado a la clase.
    /// </remarks>
    /// <typeparam name="T">El tipo del modelo que se está utilizando.</typeparam>
    public new T Model { get; set; }
}