using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace RazorEngineCore; 

/// <summary>
/// Representa un envoltorio para tipos anónimos, permitiendo el acceso dinámico a sus propiedades.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="DynamicObject"/> y permite que los tipos anónimos sean utilizados
/// en contextos donde se requiere un comportamiento dinámico, como en la reflexión o en la 
/// manipulación de objetos en tiempo de ejecución.
/// </remarks>
public class AnonymousTypeWrapper : DynamicObject
{
    private readonly object model;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="AnonymousTypeWrapper"/>.
    /// </summary>
    /// <param name="model">El modelo que se encapsulará en la instancia de <see cref="AnonymousTypeWrapper"/>.</param>
    public AnonymousTypeWrapper(object model)
    {
        this.model = model;
    }

    /// <summary>
    /// Intenta obtener un miembro del modelo basado en el nombre del binder.
    /// </summary>
    /// <param name="binder">El objeto que contiene información sobre el miembro que se está intentando obtener.</param>
    /// <param name="result">El resultado que contendrá el valor del miembro si se encuentra, de lo contrario será null.</param>
    /// <returns>
    /// Devuelve true si el miembro se encuentra y se puede obtener; de lo contrario, devuelve false.
    /// </returns>
    /// <remarks>
    /// Este método busca una propiedad en el modelo utilizando el nombre proporcionado por el binder. 
    /// Si la propiedad es anónima, se envuelve en un <see cref="AnonymousTypeWrapper"/>. 
    /// Si el resultado es una colección, cada elemento también se envuelve si es anónimo.
    /// </remarks>
    /// <seealso cref="AnonymousTypeWrapper"/>
    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        PropertyInfo propertyInfo = this.model.GetType().GetProperty(binder.Name);

        if (propertyInfo == null)
        {
            result = null;
            return false;
        }

        result = propertyInfo.GetValue(this.model, null);

        if (result == null)
        {
            return true;
        }

        var type = result.GetType();

        if (result.IsAnonymous())
        {
            result = new AnonymousTypeWrapper(result);
        }

        bool isEnumerable = typeof(IEnumerable).IsAssignableFrom(type);

        if (isEnumerable && !(result is string))
        {
            result = ((IEnumerable<object>) result)
                .Select(e =>
                {
                    if (e.IsAnonymous())
                    {
                        return new AnonymousTypeWrapper(e);
                    }

                    return e;
                })
                .ToList();
        }
        

        return true;
    }
}