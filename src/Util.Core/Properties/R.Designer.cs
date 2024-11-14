namespace Util.Properties {
    using System;
    
    
    /// <summary>
    /// Indica que la clase o el recurso ha sido generado automáticamente por el compilador de recursos.
    /// </summary>
    /// <remarks>
    /// Este atributo se utiliza para marcar el código que ha sido creado por herramientas de generación de código,
    /// como el generador de recursos fuertemente tipados. No se debe modificar manualmente el código marcado con este atributo.
    /// </remarks>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class R {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="R"/>.
        /// </summary>
        /// <remarks>
        /// Este constructor es interno y no se puede acceder desde fuera de su ensamblado.
        /// </remarks>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal R() {
        }
        
        /// <summary>
        /// Obtiene una instancia del <see cref="ResourceManager"/> que se utiliza para acceder a los recursos de la aplicación.
        /// </summary>
        /// <remarks>
        /// Este método implementa un patrón de inicialización perezosa para garantizar que el <see cref="ResourceManager"/> 
        /// se cree solo cuando se necesite, mejorando así el rendimiento de la aplicación.
        /// </remarks>
        /// <returns>
        /// Una instancia de <see cref="ResourceManager"/> que permite acceder a los recursos localizados.
        /// </returns>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Util.Properties.R", typeof(R).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        /// Obtiene o establece la cultura de los recursos utilizados por la aplicación.
        /// </summary>
        /// <value>
        /// Un objeto <see cref="System.Globalization.CultureInfo"/> que representa la cultura actual.
        /// </value>
        /// <remarks>
        /// Esta propiedad permite cambiar la cultura de los recursos en tiempo de ejecución,
        /// lo que puede afectar cómo se presentan los datos, como fechas, números y cadenas,
        /// de acuerdo con las convenciones culturales específicas.
        /// </remarks>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        /// Obtiene el valor de la cadena "CanOnlyOneCondition" desde el administrador de recursos.
        /// </summary>
        /// <value>
        /// Una cadena que representa el valor de "CanOnlyOneCondition".
        /// </value>
        /// <remarks>
        /// Este recurso se utiliza para proporcionar una representación localizable de la condición "CanOnlyOneCondition".
        /// Asegúrese de que el recurso esté definido en el archivo de recursos correspondiente.
        /// </remarks>
        public static string CanOnlyOneCondition {
            get {
                return ResourceManager.GetString("CanOnlyOneCondition", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene el mensaje de excepción de concurrencia desde el recurso.
        /// </summary>
        /// <value>
        /// Un string que representa el mensaje de excepción de concurrencia.
        /// </value>
        /// <remarks>
        /// Este mensaje se obtiene utilizando el administrador de recursos, lo que permite la localización 
        /// y la personalización del mensaje según el idioma y la cultura configurados.
        /// </remarks>
        public static string ConcurrencyExceptionMessage {
            get {
                return ResourceManager.GetString("ConcurrencyExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene el valor de la cadena "Days" desde el administrador de recursos.
        /// </summary>
        /// <value>
        /// Una cadena que representa el valor asociado a "Days" en los recursos.
        /// </value>
        /// <remarks>
        /// Este miembro está diseñado para facilitar la localización de la cadena "Days".
        /// Asegúrese de que la cadena esté definida en los recursos correspondientes para cada cultura.
        /// </remarks>
        public static string Days {
            get {
                return ResourceManager.GetString("Days", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene el mensaje de éxito para la operación de eliminación.
        /// </summary>
        /// <value>
        /// Un string que representa el mensaje de éxito de eliminación.
        /// </value>
        /// <remarks>
        /// Este mensaje se recupera de un recurso localizado utilizando el 
        /// <see cref="ResourceManager"/>. Asegúrese de que la clave "DeleteSuccess" 
        /// esté definida en los recursos correspondientes.
        /// </remarks>
        public static string DeleteSuccess {
            get {
                return ResourceManager.GetString("DeleteSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene el valor de la propiedad "ExtraProperties" desde el recurso correspondiente.
        /// </summary>
        /// <value>
        /// Una cadena que representa el valor de "ExtraProperties".
        /// </value>
        /// <remarks>
        /// Esta propiedad utiliza un administrador de recursos para recuperar el valor asociado
        /// a la clave "ExtraProperties" en el archivo de recursos.
        /// </remarks>
        public static string ExtraProperties {
            get {
                return ResourceManager.GetString("ExtraProperties", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene el valor de la cadena de recurso "GlobalDuplicateRequest".
        /// </summary>
        /// <value>
        /// Una cadena que representa el valor de la cadena de recurso "GlobalDuplicateRequest".
        /// </value>
        /// <remarks>
        /// Este valor se obtiene utilizando el <see cref="ResourceManager"/> y el contexto de cultura especificado.
        /// </remarks>
        public static string GlobalDuplicateRequest {
            get {
                return ResourceManager.GetString("GlobalDuplicateRequest", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene una cadena que representa la palabra "Horas" en el idioma especificado por el recurso.
        /// </summary>
        /// <value>
        /// Una cadena que contiene la representación localizada de "Horas".
        /// </value>
        /// <remarks>
        /// Este miembro está diseñado para acceder a recursos de cadena que se encuentran en un archivo de recursos.
        /// Asegúrese de que el archivo de recursos contenga la clave "Hours" para evitar excepciones.
        /// </remarks>
        public static string Hours {
            get {
                return ResourceManager.GetString("Hours", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene el valor de la cadena de recursos asociada con la clave "Id".
        /// </summary>
        /// <value>
        /// Una cadena que representa el valor de la clave "Id" en los recursos.
        /// </value>
        /// <remarks>
        /// Este miembro está diseñado para facilitar la localización de la aplicación,
        /// permitiendo obtener el valor correspondiente a la clave "Id" en función de la cultura actual.
        /// </remarks>
        public static string Id {
            get {
                return ResourceManager.GetString("Id", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene el valor de la cadena correspondiente a la clave "IdIsEmpty" desde el gestor de recursos.
        /// </summary>
        /// <value>
        /// Una cadena que representa el mensaje asociado a la clave "IdIsEmpty".
        /// </value>
        /// <remarks>
        /// Este miembro está diseñado para facilitar la localización de mensajes de error o advertencia
        /// relacionados con la validez de un identificador. Se utiliza comúnmente en validaciones
        /// donde se requiere verificar si un identificador está vacío.
        /// </remarks>
        public static string IdIsEmpty {
            get {
                return ResourceManager.GetString("IdIsEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene el mensaje de error para un número de identificación inválido.
        /// </summary>
        /// <value>
        /// Una cadena que representa el mensaje de error asociado con un número de identificación no válido.
        /// </value>
        /// <remarks>
        /// Este miembro utiliza un administrador de recursos para recuperar la cadena correspondiente
        /// al identificador "InvalidIdCard" en el idioma y cultura especificados.
        /// </remarks>
        public static string InvalidIdCard {
            get {
                return ResourceManager.GetString("InvalidIdCard", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene el valor de la cadena "IsDeleted" desde el administrador de recursos.
        /// </summary>
        /// <value>
        /// Una cadena que representa el estado de eliminación.
        /// </value>
        /// <remarks>
        /// Este miembro está diseñado para recuperar el valor de la cadena de recursos 
        /// correspondiente a "IsDeleted", que puede ser utilizada en la interfaz de usuario 
        /// o en la lógica de negocio para determinar si un elemento ha sido eliminado.
        /// </remarks>
        public static string IsDeleted {
            get {
                return ResourceManager.GetString("IsDeleted", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene el número de línea desde los recursos.
        /// </summary>
        /// <value>
        /// Una cadena que representa el número de línea.
        /// </value>
        /// <remarks>
        /// Este miembro está diseñado para recuperar el valor de "LineNumber" de los recursos localizados.
        /// Asegúrese de que el recurso esté disponible en el archivo de recursos correspondiente.
        /// </remarks>
        public static string LineNumber {
            get {
                return ResourceManager.GetString("LineNumber", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene una cadena que representa "Milliseconds" desde el recurso asociado.
        /// </summary>
        /// <value>
        /// Una cadena que contiene el valor de "Milliseconds".
        /// </value>
        /// <remarks>
        /// Este miembro está diseñado para acceder a los recursos de la aplicación, 
        /// permitiendo la localización y la internacionalización de la cadena.
        /// </remarks>
        public static string Milliseconds {
            get {
                return ResourceManager.GetString("Milliseconds", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene la representación en cadena de la cadena de recursos "Minutes".
        /// </summary>
        /// <remarks>
        /// Este miembro está diseñado para acceder a la cadena de recursos correspondiente
        /// al nombre "Minutes" utilizando el administrador de recursos.
        /// </remarks>
        /// <returns>
        /// Una cadena que representa el valor de la cadena de recursos "Minutes".
        /// </returns>
        public static string Minutes {
            get {
                return ResourceManager.GetString("Minutes", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene el valor de la cadena "No" desde el administrador de recursos.
        /// </summary>
        /// <value>
        /// Una cadena que representa el valor asociado a la clave "No" en los recursos.
        /// </value>
        /// <remarks>
        /// Este miembro está diseñado para facilitar la localización de la aplicación, 
        /// permitiendo obtener el texto correspondiente en función de la cultura actual.
        /// </remarks>
        public static string No {
            get {
                return ResourceManager.GetString("No", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene una cadena que representa "Seconds" desde el recurso localizado.
        /// </summary>
        /// <value>
        /// Una cadena que contiene el valor del recurso "Seconds".
        /// </value>
        /// <remarks>
        /// Este miembro está diseñado para acceder a recursos de localización y devolver el valor correspondiente
        /// basado en la cultura actual del recurso.
        /// </remarks>
        public static string Seconds {
            get {
                return ResourceManager.GetString("Seconds", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene el mensaje de éxito desde el recurso.
        /// </summary>
        /// <value>
        /// Una cadena que representa el mensaje de éxito.
        /// </value>
        /// <remarks>
        /// Este miembro está diseñado para acceder a un recurso localizado que contiene el mensaje de éxito.
        /// Asegúrese de que el recurso esté disponible en el archivo de recursos correspondiente.
        /// </remarks>
        public static string Success {
            get {
                return ResourceManager.GetString("Success", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene el mensaje de error del sistema desde los recursos.
        /// </summary>
        /// <value>
        /// Un string que representa el mensaje de error del sistema.
        /// </value>
        /// <remarks>
        /// Este mensaje se obtiene utilizando el <see cref="ResourceManager"/> 
        /// y se busca la clave "SystemError" en la cultura de recursos especificada.
        /// </remarks>
        public static string SystemError {
            get {
                return ResourceManager.GetString("SystemError", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene el identificador del inquilino (TenantId) desde el recurso.
        /// </summary>
        /// <value>
        /// Un <see cref="string"/> que representa el identificador del inquilino.
        /// </value>
        /// <remarks>
        /// Este identificador se utiliza para identificar de manera única un inquilino en un entorno multi-inquilino.
        /// </remarks>
        public static string TenantId {
            get {
                return ResourceManager.GetString("TenantId", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene el mensaje de error asociado a un tipo que no es un enumerado.
        /// </summary>
        /// <value>
        /// Una cadena que representa el mensaje de error "TypeNotEnum".
        /// </value>
        /// <remarks>
        /// Este mensaje se recupera de un recurso localizado utilizando el 
        /// <see cref="ResourceManager"/>. Asegúrese de que el recurso esté 
        /// disponible en la cultura especificada.
        /// </remarks>
        public static string TypeNotEnum {
            get {
                return ResourceManager.GetString("TypeNotEnum", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene el mensaje de error para acceso no autorizado.
        /// </summary>
        /// <value>
        /// Un string que representa el mensaje de error para acceso no autorizado.
        /// </value>
        /// <remarks>
        /// Este mensaje se obtiene a través de un administrador de recursos, lo que permite la localización 
        /// y la adaptación del mensaje a diferentes culturas.
        /// </remarks>
        public static string UnauthorizedMessage {
            get {
                return ResourceManager.GetString("UnauthorizedMessage", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene la cadena de recurso correspondiente a "Upload".
        /// </summary>
        /// <value>
        /// La cadena de recurso que representa la acción de subir.
        /// </value>
        /// <remarks>
        /// Este miembro está diseñado para acceder a recursos localizados en función de la cultura actual.
        /// </remarks>
        /// <seealso cref="ResourceManager"/>
        public static string Upload {
            get {
                return ResourceManager.GetString("Upload", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene el mensaje de solicitud de usuario duplicado desde el recurso local.
        /// </summary>
        /// <value>
        /// Una cadena que representa el mensaje de solicitud de usuario duplicado.
        /// </value>
        /// <remarks>
        /// Este mensaje se utiliza para informar al usuario que ya existe una solicitud con los mismos datos.
        /// </remarks>
        public static string UserDuplicateRequest {
            get {
                return ResourceManager.GetString("UserDuplicateRequest", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene la versión de la aplicación desde los recursos.
        /// </summary>
        /// <value>
        /// Una cadena que representa la versión de la aplicación.
        /// </value>
        /// <remarks>
        /// Este miembro está diseñado para acceder a la cadena de versión almacenada en los recursos
        /// de la aplicación, utilizando el administrador de recursos.
        /// </remarks>
        public static string Version {
            get {
                return ResourceManager.GetString("Version", resourceCulture);
            }
        }
        
        /// <summary>
        /// Obtiene el valor de la cadena "Yes" desde el administrador de recursos.
        /// </summary>
        /// <value>
        /// Una cadena que representa el valor correspondiente a "Yes" en el recurso.
        /// </value>
        /// <remarks>
        /// Este miembro está diseñado para facilitar la localización de la aplicación,
        /// permitiendo que el texto se adapte según el idioma y la cultura del usuario.
        /// </remarks>
        public static string Yes {
            get {
                return ResourceManager.GetString("Yes", resourceCulture);
            }
        }
    }
}