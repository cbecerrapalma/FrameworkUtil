namespace Util.Security;

/// <summary>
/// Atributo que se utiliza para autorizar el acceso a recursos basados en el control de acceso a nivel de aplicación.
/// Hereda de <see cref="AuthorizeAttribute"/>.
/// </summary>
public class AclAttribute : AuthorizeAttribute {
    private bool _ignore;
    private string _resourceUri;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="AclAttribute"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor establece la propiedad <see cref="Policy"/> utilizando el método 
    /// <see cref="AclPolicyHelper.GetPolicy"/> con parámetros nulos y falso.
    /// </remarks>
    public AclAttribute() {
        Policy = AclPolicyHelper.GetPolicy( null, false );
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="AclAttribute"/>.
    /// </summary>
    /// <param name="resourceUri">La URI del recurso asociado a este atributo de control de acceso.</param>
    public AclAttribute( string resourceUri ) {
        ResourceUri = resourceUri;
    }

    /// <summary>
    /// Obtiene o establece un valor que indica si se debe ignorar la política de acceso.
    /// </summary>
    /// <value>
    /// <c>true</c> si se debe ignorar la política; de lo contrario, <c>false</c>.
    /// </value>
    /// <remarks>
    /// Este propiedad utiliza el método <see cref="AclPolicyHelper.GetRequirement(string)"/> 
    /// para obtener el requisito de la política actual y determina si debe ser ignorado.
    /// Al establecer el valor, también actualiza la política correspondiente utilizando 
    /// <see cref="AclPolicyHelper.GetPolicy(string, bool)"/>.
    /// </remarks>
    public bool Ignore {
        get {
            var requirement = AclPolicyHelper.GetRequirement(Policy);
            return requirement.Ignore;
        }
        set {
            _ignore = value;
            Policy = AclPolicyHelper.GetPolicy( _resourceUri , _ignore );
        }
    }

    /// <summary>
    /// Obtiene o establece la URI del recurso asociado con la política de acceso.
    /// </summary>
    /// <value>
    /// La URI del recurso.
    /// </value>
    /// <remarks>
    /// Al obtener la URI, se recupera el requisito de la política actual.
    /// Al establecer la URI, se actualiza la política de acceso correspondiente.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Se lanza si la URI del recurso es nula al establecerla.
    /// </exception>
    public string ResourceUri {
        get {
            var requirement = AclPolicyHelper.GetRequirement( Policy );
            return requirement.Uri;
        }
        set {
            _resourceUri = value;
            Policy = AclPolicyHelper.GetPolicy( _resourceUri, _ignore );
        }
    }
}