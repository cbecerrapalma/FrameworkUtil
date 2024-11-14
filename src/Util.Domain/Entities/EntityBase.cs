using Util.Domain.Compare;
using Util.Domain.Extending;
using Util.Properties;
using Util.Validation;

namespace Util.Domain.Entities; 

/// <summary>
/// Clase base abstracta para entidades que utilizan un identificador de tipo <see cref="Guid"/>.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que hereda de <see cref="IEntity{TEntity, TKey}"/>.</typeparam>
/// <remarks>
/// Esta clase proporciona una implementación común para las entidades que requieren un identificador único
/// de tipo <see cref="Guid"/>. Se espera que las clases que hereden de esta clase implementen la interfaz
/// <see cref="IEntity{TEntity, TKey}"/> para garantizar una estructura coherente.
/// </remarks>
/// <seealso cref="IEntity{TEntity, TKey}"/>
public abstract class EntityBase<TEntity> : EntityBase<TEntity, Guid> where TEntity : IEntity<TEntity, Guid> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EntityBase"/>.
    /// </summary>
    /// <param name="id">El identificador único de la entidad.</param>
    protected EntityBase(Guid id) : base(id) 
    { 
    }
}

/// <summary>
/// Clase base abstracta que representa una entidad en el dominio.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad que hereda de <see cref="IEntity{TEntity, TKey}"/>.</typeparam>
/// <typeparam name="TKey">El tipo de la clave primaria de la entidad.</typeparam>
/// <remarks>
/// Esta clase proporciona una estructura básica para las entidades en el dominio,
/// asegurando que todas las entidades tengan un identificador único y cumplan con
/// las características definidas por <see cref="IEntity{TEntity, TKey}"/>.
/// </remarks>
public abstract class EntityBase<TEntity, TKey> : DomainObjectBase<TEntity>, IEntity<TEntity, TKey> where TEntity : IEntity<TEntity, TKey> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EntityBase"/>.
    /// </summary>
    /// <param name="id">El identificador único de la entidad.</param>
    /// <remarks>
    /// Este constructor establece el identificador de la entidad y crea un nuevo diccionario para las propiedades adicionales.
    /// </remarks>
    protected EntityBase(TKey id) 
    { 
        Id = id; 
        ExtraProperties = new ExtraPropertyDictionary(); 
    }

    /// <summary>
    /// Representa la clave única del objeto.
    /// </summary>
    /// <typeparam name="TKey">El tipo de la clave única.</typeparam>
    /// <remarks>
    /// Esta propiedad es de solo lectura desde fuera de la clase, lo que significa que su valor solo puede ser establecido dentro de la clase.
    /// </remarks>

    [Key]
    public TKey Id { get; private set; }

    /// <summary>
    /// Obtiene o establece un diccionario de propiedades adicionales.
    /// </summary>
    /// <remarks>
    /// Este diccionario permite almacenar propiedades personalizadas que no están definidas en la clase.
    /// Las propiedades pueden ser de cualquier tipo y se accede a ellas mediante claves.
    /// </remarks>
    /// <value>
    /// Un objeto de tipo <see cref="ExtraPropertyDictionary"/> que contiene las propiedades adicionales.
    /// </value>
    protected ExtraPropertyDictionary ExtraProperties { get; set; }

    /// <summary>
    /// Determina si el objeto actual es igual a otro objeto del mismo tipo.
    /// </summary>
    /// <param name="other">El objeto con el que comparar el objeto actual.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el objeto actual es igual al objeto especificado; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método sobrescribe el método <see cref="object.Equals(object)"/> 
    /// y compara el objeto actual con otro objeto de tipo <see cref="EntityBase{TEntity, TKey}"/>.
    /// </remarks>
    public override bool Equals( object other ) {
        return this == ( other as EntityBase<TEntity, TKey> );
    }

    /// <summary>
    /// Devuelve un código hash para el objeto actual.
    /// </summary>
    /// <returns>
    /// Un entero que representa el código hash del identificador (Id) del objeto actual.
    /// Si el identificador es nulo, se devuelve 0.
    /// </returns>
    public override int GetHashCode() {
        return ReferenceEquals( Id, null ) ? 0 : Id.GetHashCode();
    }

    public static bool operator ==( EntityBase<TEntity, TKey> left, EntityBase<TEntity, TKey> right ) {
        if( (object)left == null && (object)right == null )
            return true;
        if( !( left is TEntity ) || !( right is TEntity ) )
            return false;
        if( Equals( left.Id, null ) )
            return false;
        if( left.Id.Equals( default( TKey ) ) )
            return false;
        return left.Id.Equals( right.Id );
    }

    public static bool operator !=( EntityBase<TEntity, TKey> left, EntityBase<TEntity, TKey> right ) {
        return !( left == right );
    }

    /// <summary>
    /// Crea una colección de valores de cambio para una nueva entidad.
    /// </summary>
    /// <param name="newEntity">La nueva entidad para la cual se crea la colección de valores de cambio.</param>
    /// <returns>Una instancia de <see cref="ChangeValueCollection"/> que contiene la información de la entidad.</returns>
    /// <remarks>
    /// Este método utiliza reflexión para obtener el nombre o la descripción de la entidad
    /// y crea una colección de valores de cambio que incluye el tipo de la entidad, su descripción
    /// y su identificador en formato de cadena segura.
    /// </remarks>
    protected override ChangeValueCollection CreateChangeValueCollection(TEntity newEntity) {
        var description = Util.Helpers.Reflection.GetDisplayNameOrDescription(newEntity.GetType());
        return new ChangeValueCollection(newEntity.GetType().ToString(), description, newEntity.Id.SafeString());
    }

    /// <summary>
    /// Inicializa el objeto, configurando los valores necesarios.
    /// </summary>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito por clases derivadas para proporcionar
    /// una inicialización personalizada. Llama al método <see cref="InitId"/> para establecer
    /// el identificador inicial.
    /// </remarks>
    public virtual void Init() {
        InitId();
    }

    /// <summary>
    /// Inicializa el identificador (Id) de la entidad si no ha sido establecido previamente.
    /// </summary>
    /// <remarks>
    /// Este método verifica si el tipo de TKey es un tipo numérico (int o long).
    /// Si el Id ya tiene un valor válido, no se realiza ninguna acción. 
    /// En caso contrario, se genera un nuevo Id utilizando el método <see cref="CreateId"/>.
    /// </remarks>
    /// <typeparam name="TKey">El tipo del identificador de la entidad.</typeparam>
    protected virtual void InitId() {
        if (typeof(TKey) == typeof(int) || typeof(TKey) == typeof(long))
            return;
        if (string.IsNullOrWhiteSpace(Id.SafeString()) || Id.Equals(default(TKey)))
            Id = CreateId();
    }

    /// <summary>
    /// Crea un nuevo identificador único.
    /// </summary>
    /// <returns>
    /// Un nuevo identificador de tipo <typeparamref name="TKey"/> generado a partir de un GUID.
    /// </returns>
    /// <typeparam name="TKey">
    /// El tipo del identificador que se va a crear.
    /// </typeparam>
    protected virtual TKey CreateId() {
        return Util.Helpers.Convert.To<TKey>(Guid.NewGuid());
    }

    /// <summary>
    /// Valida la colección de resultados de validación.
    /// </summary>
    /// <param name="results">La colección de resultados de validación donde se almacenarán los errores de validación.</param>
    /// <remarks>
    /// Este método se sobrescribe para proporcionar una validación específica de la clase.
    /// Se llama al método <see cref="ValidateId"/> para realizar la validación del identificador.
    /// </remarks>
    protected override void Validate(ValidationResultCollection results) 
    {
        ValidateId(results);
    }

    /// <summary>
    /// Valida el identificador (Id) de un objeto genérico.
    /// </summary>
    /// <param name="results">Colección de resultados de validación donde se agregarán los errores encontrados.</param>
    /// <remarks>
    /// Este método verifica si el tipo de TKey es un tipo numérico (int o long).
    /// Si no lo es, se valida que el Id no esté vacío o sea igual al valor predeterminado de TKey.
    /// En caso de que el Id no cumpla con estas condiciones, se agrega un resultado de validación a la colección.
    /// </remarks>
    protected virtual void ValidateId(ValidationResultCollection results)
    {
        if (typeof(TKey) == typeof(int) || typeof(TKey) == typeof(long))
            return;
        if (string.IsNullOrWhiteSpace(Id.SafeString()) || Id.Equals(default(TKey)))
            results.Add(new ValidationResult(R.IdIsEmpty));
    }

    /// <summary>
    /// Clona la instancia actual de la entidad.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de la entidad del tipo <typeparamref name="TEntity"/> que es una copia de la instancia actual.
    /// </returns>
    /// <typeparam name="TEntity">
    /// El tipo de la entidad que se está clonando.
    /// </typeparam>
    /// <remarks>
    /// Este método utiliza un mapeo para crear una copia de la entidad actual.
    /// Asegúrese de que el método MapTo esté correctamente implementado para evitar problemas de clonación.
    /// </remarks>
    public virtual TEntity Clone() {
        return this.MapTo<TEntity>();
    }
}