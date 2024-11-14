using Util.Helpers;

namespace Util.Domain.Auditing; 

/// <summary>
/// Clase que representa un conjunto de propiedades para auditar modificaciones.
/// </summary>
public class ModificationAuditedSetter {
    private readonly object _entity;
    private readonly string _userId;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ModificationAuditedSetter"/>.
    /// </summary>
    /// <param name="entity">El objeto que se va a auditar.</param>
    /// <param name="userId">El identificador del usuario que realiza la modificación.</param>
    private ModificationAuditedSetter( object entity, string userId ) {
        _entity = entity;
        _userId = userId;
    }

    /// <summary>
    /// Establece la información de auditoría de modificación para una entidad.
    /// </summary>
    /// <param name="entity">La entidad a la que se le aplicará la auditoría de modificación.</param>
    /// <param name="userId">El identificador del usuario que realiza la modificación.</param>
    /// <remarks>
    /// Este método crea una instancia de <see cref="ModificationAuditedSetter"/> 
    /// y llama al método <c>Init</c> para inicializar la auditoría de modificación.
    /// </remarks>
    public static void Set( object entity, string userId ) {
        new ModificationAuditedSetter( entity, userId ).Init();
    }

    /// <summary>
    /// Inicializa la información de modificación del objeto actual.
    /// </summary>
    /// <remarks>
    /// Este método verifica si la entidad está inicializada y si el identificador del usuario no está vacío.
    /// Si ambas condiciones se cumplen, se actualiza el identificador del último modificador en función del tipo de la entidad.
    /// </remarks>
    public void Init() {
        if ( _entity == null )
            return;
        InitLastModificationTime();
        if ( _userId.IsEmpty() )
            return;
        if ( _entity is IModificationAudited<Guid> entity ) {
            entity.LastModifierId = _userId.ToGuid();
            return;
        }
        if ( _entity is IModificationAudited<Guid?> entity2 ) {
            entity2.LastModifierId = _userId.ToGuidOrNull();
            return;
        }
        if ( _entity is IModificationAudited<int> entity3 ) {
            entity3.LastModifierId = _userId.ToInt();
            return;
        }
        if ( _entity is IModificationAudited<int?> entity4 ) {
            entity4.LastModifierId = _userId.ToIntOrNull();
            return;
        }
        if ( _entity is IModificationAudited<string> entity5 ) {
            entity5.LastModifierId = _userId.SafeString();
            return;
        }
        if ( _entity is IModificationAudited<long> entity6 ) {
            entity6.LastModifierId = _userId.ToLong();
            return;
        }
        if ( _entity is IModificationAudited<long?> entity7 ) {
            entity7.LastModifierId = _userId.ToLongOrNull();
            return;
        }
    }

    /// <summary>
    /// Inicializa la propiedad LastModificationTime de la entidad si la entidad implementa la interfaz ILastModificationTime.
    /// </summary>
    /// <remarks>
    /// Este método se utiliza para establecer la fecha y hora de la última modificación de la entidad.
    /// Se asegura de que solo se intente establecer el tiempo si la entidad es compatible con la interfaz.
    /// </remarks>
    private void InitLastModificationTime() {
        if ( _entity is ILastModificationTime entity )
            entity.LastModificationTime = Time.Now;
    }
}