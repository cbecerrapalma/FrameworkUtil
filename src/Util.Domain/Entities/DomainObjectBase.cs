using Util.Domain.Compare;
using Util.Validation;

namespace Util.Domain.Entities;

/// <summary>
/// Clase base abstracta para objetos de dominio.
/// </summary>
/// <typeparam name="T">El tipo de objeto de dominio que hereda de <see cref="IDomainObject"/>.</typeparam>
/// <remarks>
/// Esta clase proporciona una implementación base para objetos de dominio, 
/// permitiendo la comparación de cambios entre instancias del mismo tipo.
/// </remarks>
public abstract class DomainObjectBase<T> : IDomainObject, ICompareChange<T> where T : IDomainObject
{

    #region Campo

    private readonly List<IValidationRule> _rules;
    private IValidationHandler _handler;
    private ChangeValueCollection _changeValues;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DomainObjectBase"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor establece una lista vacía de reglas de validación y un manejador de excepciones por defecto.
    /// </remarks>
    protected DomainObjectBase()
    {
        _rules = new List<IValidationRule>();
        _handler = new ThrowHandler();
    }

    #endregion

    #region Validate(Verificación)

    /// <summary>
    /// Establece el manejador de validación.
    /// </summary>
    /// <param name="handler">El manejador de validación a establecer. Si es <c>null</c>, no se realiza ninguna acción.</param>
    public void SetValidationHandler(IValidationHandler handler)
    {
        if (handler == null)
            return;
        _handler = handler;
    }

    /// <summary>
    /// Agrega un conjunto de reglas de validación.
    /// </summary>
    /// <param name="rules">Una colección de reglas de validación que se agregarán.</param>
    /// <remarks>
    /// Si la colección de reglas es nula, el método no realiza ninguna acción.
    /// Cada regla en la colección se agrega mediante el método <see cref="AddValidationRule(IValidationRule)"/>.
    /// </remarks>
    public void AddValidationRules(IEnumerable<IValidationRule> rules)
    {
        if (rules == null)
            return;
        foreach (var rule in rules)
            AddValidationRule(rule);
    }

    /// <summary>
    /// Agrega una regla de validación a la colección de reglas.
    /// </summary>
    /// <param name="rule">La regla de validación que se desea agregar. Si es <c>null</c>, no se realiza ninguna acción.</param>
    /// <remarks>
    /// Este método verifica si la regla proporcionada es <c>null</c> antes de intentar agregarla a la colección.
    /// Si la regla es válida, se añade a la lista de reglas de validación.
    /// </remarks>
    public void AddValidationRule(IValidationRule rule)
    {
        if (rule == null)
            return;
        _rules.Add(rule);
    }

    /// <summary>
    /// Valida el objeto actual y devuelve una colección de resultados de validación.
    /// </summary>
    /// <returns>
    /// Una colección de resultados de validación que contiene los errores encontrados durante el proceso de validación.
    /// </returns>
    /// <remarks>
    /// Este método llama a <see cref="GetValidationResults"/> para obtener los resultados de validación y luego 
    /// maneja esos resultados utilizando el método <see cref="HandleValidationResults"/>.
    /// </remarks>
    public virtual ValidationResultCollection Validate()
    {
        var result = GetValidationResults();
        HandleValidationResults(result);
        return result;
    }

    /// <summary>
    /// Obtiene los resultados de validación de los datos actuales.
    /// </summary>
    /// <returns>
    /// Una colección de resultados de validación que contiene los resultados de las validaciones realizadas.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la validación de anotaciones de datos y aplica reglas adicionales definidas en la colección _rules.
    /// </remarks>
    /// <exception cref="ValidationException">
    /// Se lanza si alguna de las validaciones falla.
    /// </exception>
    private ValidationResultCollection GetValidationResults()
    {
        var result = DataAnnotationValidation.Validate(this);
        Validate(result);
        foreach (var rule in _rules)
            result.Add(rule.Validate());
        return result;
    }

    /// <summary>
    /// Valida una colección de resultados de validación.
    /// </summary>
    /// <param name="results">La colección de resultados de validación que se va a validar.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una lógica de validación específica.
    /// </remarks>
    protected virtual void Validate(ValidationResultCollection results)
    {
    }

    /// <summary>
    /// Maneja los resultados de validación.
    /// </summary>
    /// <param name="results">La colección de resultados de validación que se va a manejar.</param>
    /// <remarks>
    /// Si la colección de resultados de validación es válida, el método no realiza ninguna acción.
    /// Si la colección contiene errores de validación, se invoca al manejador para procesar dichos resultados.
    /// </remarks>
    protected virtual void HandleValidationResults(ValidationResultCollection results)
    {
        if (results.IsValid)
            return;
        _handler.Handle(results);
    }

    #endregion

    #region GetChanges(Obtener atributos de cambio)

    /// <summary>
    /// Obtiene una colección de cambios para una nueva entidad.
    /// </summary>
    /// <typeparam name="T">El tipo de la entidad que se está evaluando.</typeparam>
    /// <param name="newEntity">La nueva entidad para la cual se desean obtener los cambios.</param>
    /// <returns>
    /// Una colección de cambios que representa las diferencias entre la nueva entidad y el estado anterior.
    /// Si la nueva entidad es nula, se devuelve una colección vacía.
    /// </returns>
    /// <remarks>
    /// Este método crea una colección de cambios utilizando la nueva entidad proporcionada,
    /// y agrega los cambios detectados a la colección.
    /// </remarks>
    public ChangeValueCollection GetChanges(T newEntity)
    {
        if (Equals(newEntity, null))
            return new ChangeValueCollection();
        _changeValues = CreateChangeValueCollection(newEntity);
        AddChanges(newEntity);
        return _changeValues;
    }

    /// <summary>
    /// Crea una colección de valores de cambio para una nueva entidad.
    /// </summary>
    /// <param name="newEntity">La nueva entidad para la cual se crea la colección de valores de cambio.</param>
    /// <returns>Una instancia de <see cref="ChangeValueCollection"/> que contiene información sobre la entidad.</returns>
    /// <remarks>
    /// Este método utiliza la reflexión para obtener el nombre o la descripción de la entidad
    /// y lo utiliza para inicializar la colección de valores de cambio.
    /// </remarks>
    /// <typeparam name="T">El tipo de la entidad que se está procesando.</typeparam>
    protected virtual ChangeValueCollection CreateChangeValueCollection(T newEntity)
    {
        var description = Util.Helpers.Reflection.GetDisplayNameOrDescription(newEntity.GetType());
        return new ChangeValueCollection(newEntity.GetType().ToString(), description);
    }

    /// <summary>
    /// Agrega los cambios de una nueva entidad al sistema.
    /// </summary>
    /// <param name="newEntity">La nueva entidad que se va a agregar.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar
    /// una implementación específica de cómo se deben agregar los cambios de la nueva entidad.
    /// </remarks>
    /// <typeparam name="T">El tipo de la entidad que se está agregando.</typeparam>
    protected virtual void AddChanges(T newEntity)
    {
    }

    /// <summary>
    /// Agrega un cambio a una propiedad específica de un objeto.
    /// </summary>
    /// <typeparam name="TProperty">El tipo de la propiedad que se está modificando.</typeparam>
    /// <typeparam name="TValue">El tipo del nuevo valor que se asignará a la propiedad.</typeparam>
    /// <param name="expression">Una expresión lambda que representa la propiedad que se va a cambiar.</param>
    /// <param name="newValue">El nuevo valor que se asignará a la propiedad.</param>
    /// <remarks>
    /// Este método utiliza reflexión para obtener el nombre y la descripción de la propiedad 
    /// a partir de la expresión proporcionada. Luego, se registra el cambio con el valor 
    /// actual y el nuevo valor.
    /// </remarks>
    /// <seealso cref="Util.Helpers.Lambda"/>
    /// <seealso cref="Util.Helpers.Reflection"/>
    /// <seealso cref="Util.Helpers.Convert"/>
    protected void AddChange<TProperty, TValue>(Expression<Func<T, TProperty>> expression, TValue newValue)
    {
        var member = Util.Helpers.Lambda.GetMemberExpression(expression);
        var name = Util.Helpers.Lambda.GetMemberName(member);
        var description = Util.Helpers.Reflection.GetDisplayNameOrDescription(member.Member);
        var value = member.Member.GetPropertyValue(this);
        AddChange(name, description, Util.Helpers.Convert.To<TValue>(value), newValue);
    }

    /// <summary>
    /// Agrega un cambio a la lista de cambios si el valor antiguo y el nuevo son diferentes.
    /// </summary>
    /// <typeparam name="TValue">El tipo de los valores antiguos y nuevos.</typeparam>
    /// <param name="propertyName">El nombre de la propiedad que ha cambiado.</param>
    /// <param name="description">Una descripción del cambio realizado.</param>
    /// <param name="oldValue">El valor antiguo de la propiedad.</param>
    /// <param name="newValue">El nuevo valor de la propiedad.</param>
    /// <remarks>
    /// Este método compara el valor antiguo con el nuevo y, si son diferentes, 
    /// los agrega a la colección de cambios. Se utiliza un método de extensión 
    /// <c>SafeString</c> para convertir los valores a cadenas de texto, 
    /// y se ignoran las diferencias de mayúsculas y minúsculas así como los espacios en blanco al principio y al final.
    /// </remarks>
    /// <seealso cref="SafeString"/>
    protected void AddChange<TValue>(string propertyName, string description, TValue oldValue, TValue newValue)
    {
        if (Equals(oldValue, newValue))
            return;
        string oldValueString = oldValue.SafeString().ToLower().Trim();
        string newValueString = newValue.SafeString().ToLower().Trim();
        if (oldValueString == newValueString)
            return;
        _changeValues.Add(propertyName, description, oldValueString, newValueString);
    }

    /// <summary>
    /// Agrega los cambios entre un objeto antiguo y un nuevo objeto.
    /// </summary>
    /// <typeparam name="TDomainObject">El tipo del objeto de dominio que se está comparando.</typeparam>
    /// <param name="oldObject">El objeto antiguo que se utilizará para comparar los cambios.</param>
    /// <param name="newObject">El nuevo objeto que se comparará con el objeto antiguo.</param>
    /// <remarks>
    /// Este método verifica si alguno de los objetos es nulo antes de intentar obtener los cambios.
    /// Si ambos objetos son válidos, se agregan los cambios detectados a la colección de cambios.
    /// </remarks>
    protected void AddChange<TDomainObject>(ICompareChange<TDomainObject> oldObject, TDomainObject newObject) where TDomainObject : IDomainObject
    {
        if (Equals(oldObject, null))
            return;
        if (Equals(newObject, null))
            return;
        _changeValues.AddRange(oldObject.GetChanges(newObject));
    }

    /// <summary>
    /// Agrega cambios entre dos colecciones de objetos, comparando los objetos antiguos con los nuevos.
    /// </summary>
    /// <typeparam name="TDomainObject">El tipo de objeto de dominio que implementa <see cref="IDomainObject"/>.</typeparam>
    /// <param name="oldObjects">Una colección de objetos antiguos que implementan <see cref="ICompareChange{T}"/>.</param>
    /// <param name="newObjects">Una colección de nuevos objetos del tipo <typeparamref name="TDomainObject"/>.</param>
    /// <remarks>
    /// Este método compara los objetos de las colecciones proporcionadas y llama a otro método para agregar los cambios
    /// correspondientes. Si alguna de las colecciones es nula, el método no realiza ninguna acción.
    /// </remarks>
    /// <seealso cref="IDomainObject"/>
    /// <seealso cref="ICompareChange{T}"/>
    protected void AddChange<TDomainObject>(IEnumerable<ICompareChange<TDomainObject>> oldObjects, IEnumerable<TDomainObject> newObjects) where TDomainObject : IDomainObject
    {
        if (Equals(oldObjects, null))
            return;
        if (Equals(newObjects, null))
            return;
        var oldList = oldObjects.ToList();
        var newList = newObjects.ToList();
        for (int i = 0; i < oldList.Count; i++)
        {
            if (newList.Count <= i)
                return;
            AddChange(oldList[i], newList[i]);
        }
    }

    #endregion
}