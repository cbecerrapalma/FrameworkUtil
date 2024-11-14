using Util.Helpers;

namespace Util.Domain.Auditing;

/// <summary>
/// Clase que proporciona funcionalidad para establecer información de auditoría de creación.
/// </summary>
public class CreationAuditedSetter {
    private readonly object _entity;
    private readonly string _userId;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CreationAuditedSetter"/>.
    /// </summary>
    /// <param name="entity">El objeto que se va a auditar.</param>
    /// <param name="userId">El identificador del usuario que realiza la creación.</param>
    private CreationAuditedSetter( object entity, string userId ) {
        _entity = entity;
        _userId = userId;
    }

    /// <summary>
    /// Establece la información de auditoría de creación para una entidad especificada.
    /// </summary>
    /// <param name="entity">La entidad a la que se le establecerá la información de auditoría.</param>
    /// <param name="userId">El identificador del usuario que realiza la creación de la entidad.</param>
    /// <remarks>
    /// Este método inicializa un objeto <see cref="CreationAuditedSetter"/> 
    /// que se encarga de establecer los datos de auditoría en la entidad.
    /// </remarks>
    /// <seealso cref="CreationAuditedSetter"/>
    public static void Set( object entity, string userId ) {
        new CreationAuditedSetter( entity, userId ).Init();
    }

    /// <summary>
    /// Inicializa la entidad estableciendo el creador si es necesario.
    /// </summary>
    /// <remarks>
    /// Este método verifica si la entidad está inicializada y si el identificador del usuario está disponible.
    /// Dependiendo del tipo de la entidad, se asigna el identificador del creador si aún no está establecido.
    /// </remarks>
    /// <param name="_entity">La entidad que se va a inicializar.</param>
    /// <param name="_userId">El identificador del usuario que está creando la entidad.</param>
    /// <exception cref="ArgumentNullException">Se lanza si la entidad es nula.</exception>
    public void Init() {
        if ( _entity == null )
            return;
        InitCreationTime();
        if ( _userId.IsEmpty() )
            return;
        if ( _entity is ICreationAudited<Guid> entity ) {
            if ( IsEmpty( entity.CreatorId ) )
                entity.CreatorId = _userId.ToGuid();
            return;
        }
        if ( _entity is ICreationAudited<Guid?> entity2 ) {
            if ( IsEmpty( entity2.CreatorId ) )
                entity2.CreatorId = _userId.ToGuidOrNull();
            return;
        }
        if ( _entity is ICreationAudited<int> entity3 ) {
            if ( IsEmpty( entity3.CreatorId ) )
                entity3.CreatorId = _userId.ToInt();
            return;
        }
        if ( _entity is ICreationAudited<int?> entity4 ) {
            if ( IsEmpty( entity4.CreatorId ) )
                entity4.CreatorId = _userId.ToIntOrNull();
            return;
        }
        if ( _entity is ICreationAudited<string> entity5 ) {
            if ( IsEmpty( entity5.CreatorId ) )
                entity5.CreatorId = _userId.SafeString();
            return;
        }
        if ( _entity is ICreationAudited<long> entity6 ) {
            if ( IsEmpty( entity6.CreatorId ) )
                entity6.CreatorId = _userId.ToLong();
            return;
        }
        if ( _entity is ICreationAudited<long?> entity7 ) {
            if ( IsEmpty( entity7.CreatorId ) )
                entity7.CreatorId = _userId.ToLongOrNull();
            return;
        }
    }

    /// <summary>
    /// Inicializa el tiempo de creación de la entidad si la entidad implementa la interfaz <see cref="ICreationTime"/>.
    /// </summary>
    /// <remarks>
    /// Este método verifica si la entidad actual es de un tipo que implementa la interfaz <see cref="ICreationTime"/>.
    /// Si es así, se establece el tiempo de creación en el momento actual si aún no ha sido definido.
    /// </remarks>
    private void InitCreationTime() {
        if ( _entity is ICreationTime entity ) 
            entity.CreationTime ??= Time.Now;
    }

    /// <summary>
    /// Determina si el identificador proporcionado está vacío.
    /// </summary>
    /// <typeparam name="T">El tipo del identificador que se está evaluando.</typeparam>
    /// <param name="creatorId">El identificador a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el identificador es <c>null</c> o su valor por defecto; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método es genérico y puede ser utilizado con cualquier tipo de datos.
    /// </remarks>
    private bool IsEmpty<T>( T creatorId ) {
        if ( creatorId == null )
            return true;
        if ( creatorId.Equals( default(T) ) )
            return true;
        return false;
    }
}