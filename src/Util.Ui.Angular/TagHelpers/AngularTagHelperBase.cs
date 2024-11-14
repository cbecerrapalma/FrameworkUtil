using Util.Ui.TagHelpers;

namespace Util.Ui.Angular.TagHelpers; 

/// <summary>
/// Clase base abstracta para la implementación de Tag Helpers relacionados con Angular.
/// </summary>
/// <remarks>
/// Esta clase proporciona funcionalidades comunes que pueden ser utilizadas por las clases derivadas
/// que implementan Tag Helpers específicos de Angular.
/// </remarks>
public abstract class AngularTagHelperBase : TagHelperBase {
    /// <summary>
    /// Obtiene o establece el identificador en formato sin procesar.
    /// </summary>
    /// <remarks>
    /// Este identificador puede ser utilizado para realizar operaciones que requieren el valor original sin modificaciones.
    /// </remarks>
    public string RawId { get; set; }
    /// <summary>
    /// Representa una propiedad que se utiliza para determinar si un elemento debe ser mostrado o no,
    /// en función de una condición específica.
    /// </summary>
    /// <remarks>
    /// Esta propiedad puede ser utilizada en el contexto de la visualización condicional de elementos
    /// en una interfaz de usuario, permitiendo que el elemento asociado se muestre solo si la
    /// condición evaluada es verdadera.
    /// </remarks>
    /// <value>
    /// Un string que representa la condición que se evaluará para mostrar el elemento.
    /// </value>
    public string NgIf { get; set; }
    /// <summary>
    /// Obtiene o establece el valor de la propiedad NgSwitch.
    /// </summary>
    /// <value>
    /// Un <see cref="string"/> que representa el valor de NgSwitch.
    /// </value>
    public string NgSwitch { get; set; }
    /// <summary>
    /// Obtiene o establece el valor del caso de conmutación (switch case) utilizado en la lógica de la aplicación.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para determinar qué caso debe ser ejecutado en una estructura de conmutación.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el caso actual de la conmutación.
    /// </value>
    public string NgSwitchCase { get; set; }
    /// <summary>
    /// Obtiene o establece el valor que indica si este es el caso predeterminado en una estructura de conmutación.
    /// </summary>
    /// <remarks>
    /// Este valor se utiliza en el contexto de una estructura de conmutación para determinar el comportamiento
    /// cuando no se cumple ninguna de las condiciones especificadas.
    /// </remarks>
    /// <value>
    /// <c>true</c> si este es el caso predeterminado; de lo contrario, <c>false</c>.
    /// </value>
    public bool NgSwitchDefault { get; set; }
    /// <summary>
    /// Representa una propiedad que almacena un valor de tipo cadena.
    /// </summary>
    /// <remarks>
    /// Esta propiedad puede ser utilizada para almacenar información que se desea iterar o procesar en un contexto específico.
    /// </remarks>
    /// <value>
    /// Un valor de tipo <see cref="string"/> que representa el contenido de la propiedad.
    /// </value>
    public string NgFor { get; set; }
    /// <summary>
    /// Obtiene o establece la clase CSS que se aplicará a un elemento.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite asignar dinámicamente una o más clases CSS a un elemento en función de la lógica de la aplicación.
    /// </remarks>
    /// <value>
    /// Una cadena que representa la clase CSS que se aplicará.
    /// </value>
    public string NgClass { get; set; }
    /// <summary>
    /// Obtiene o establece el estilo CSS dinámico para un elemento.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite aplicar estilos CSS de manera dinámica a un elemento 
    /// en función de las condiciones definidas en la lógica de la aplicación.
    /// </remarks>
    /// <value>
    /// Un string que representa los estilos CSS aplicables al elemento.
    /// </value>
    public string NgStyle { get; set; }
    /// <summary>
    /// Obtiene o establece el Control de Acceso de la entidad.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para definir los permisos de acceso a los recursos asociados.
    /// </remarks>
    /// <value>
    /// Una cadena que representa el Control de Acceso.
    /// </value>
    public string Acl { get; set; }
    /// <summary>
    /// Obtiene o establece el identificador de la plantilla de ACL alternativa.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar el identificador de una plantilla de control de acceso alternativo,
    /// que puede ser utilizado en la configuración de permisos y accesos dentro de la aplicación.
    /// </remarks>
    /// <value>
    /// Un string que representa el identificador de la plantilla de ACL alternativa.
    /// </value>
    public string AclElseTemplateId { get; set; }
    /// <summary>
    /// Obtiene o establece el valor de la lista de control de acceso (ACL) vinculada.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para definir los permisos de acceso asociados a un recurso específico.
    /// </remarks>
    /// <value>
    /// Una cadena que representa la ACL vinculada.
    /// </value>
    public string BindAcl { get; set; }
    /// <summary>
    /// Obtiene o establece el comando que se ejecutará al hacer clic con el botón derecho del ratón.
    /// </summary>
    /// <value>
    /// Una cadena que representa el comando del menú contextual.
    /// </value>
    public string OnContextmenu { get; set; }
}