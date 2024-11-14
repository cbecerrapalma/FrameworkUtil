using System.Text;
using Util.Ui.Angular.Configs;
using Util.Ui.Builders;
using Util.Ui.Configs;

namespace Util.Ui.Angular.Extensions;

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="TagBuilder"/>.
/// </summary>
public static class TagBuilderExtensions {
    /// <summary>
    /// Extiende la funcionalidad de un <see cref="TagBuilder"/> para incluir directivas de Angular.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo de constructor que se está utilizando, que debe heredar de <see cref="TagBuilder"/>.</typeparam>
    /// <param name="builder">El objeto <typeparamref name="TBuilder"/> que se está configurando.</param>
    /// <param name="config">La configuración que se aplicará a las directivas de Angular.</param>
    /// <returns>
    /// El objeto <typeparamref name="TBuilder"/> modificado con las directivas de Angular aplicadas.
    /// </returns>
    /// <remarks>
    /// Este método permite encadenar múltiples configuraciones de Angular en un solo objeto <see cref="TagBuilder"/>.
    /// Las directivas que se aplican incluyen NgIf, NgSwitch, NgSwitchCase, NgSwitchDefault, NgFor, NgClass, NgStyle, Acl y BindAcl.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder Angular<TBuilder>( this TBuilder builder, Config config ) where TBuilder : TagBuilder {
        builder
            .NgIf( config )
            .NgSwitch( config ).NgSwitchCase( config ).NgSwitchDefault( config )
            .NgFor( config )
            .NgClass( config )
            .NgStyle( config )
            .Acl( config )
            .BindAcl( config );
        return builder;
    }

    /// <summary>
    /// Establece una condición de visualización en el elemento HTML utilizando la directiva *ngIf.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo del constructor de etiquetas que se está utilizando.</typeparam>
    /// <param name="builder">El constructor de etiquetas al que se le aplicará la directiva *ngIf.</param>
    /// <param name="value">La expresión que determina si el elemento debe ser visible o no.</param>
    /// <returns>El constructor de etiquetas modificado con la directiva *ngIf aplicada.</returns>
    /// <remarks>
    /// Esta extensión permite agregar la directiva *ngIf a un elemento HTML, 
    /// lo que permite que el elemento se muestre o se oculte en función del valor proporcionado.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder NgIf<TBuilder>( this TBuilder builder, string value ) where TBuilder : TagBuilder {
        builder.AttributeIfNotEmpty( "*ngIf", value );
        return builder;
    }

    /// <summary>
    /// Establece la directiva NgIf en el generador de etiquetas.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo del generador de etiquetas que se está utilizando.</typeparam>
    /// <param name="builder">El generador de etiquetas en el que se aplicará la directiva NgIf.</param>
    /// <param name="config">La configuración que contiene el valor para la directiva NgIf.</param>
    /// <returns>El generador de etiquetas modificado con la directiva NgIf aplicada.</returns>
    /// <remarks>
    /// Este método permite a los desarrolladores aplicar la directiva NgIf de Angular a un generador de etiquetas específico,
    /// utilizando un valor obtenido de la configuración proporcionada.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder NgIf<TBuilder>( this TBuilder builder, Config config ) where TBuilder : TagBuilder {
        builder.NgIf( config.GetValue( AngularConst.NgIf ) );
        return builder;
    }

    /// <summary>
    /// Establece el atributo ngSwitch en el objeto TagBuilder especificado.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo de constructor que hereda de TagBuilder.</typeparam>
    /// <param name="builder">El objeto TagBuilder en el que se establecerá el atributo.</param>
    /// <param name="value">El valor que se asignará al atributo ngSwitch.</param>
    /// <returns>El objeto TagBuilder modificado con el atributo ngSwitch establecido.</returns>
    /// <remarks>
    /// Este método es una extensión para la clase TagBuilder que permite agregar el atributo ngSwitch
    /// solo si el valor proporcionado no está vacío.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder NgSwitch<TBuilder>( this TBuilder builder, string value ) where TBuilder : TagBuilder {
        builder.AttributeIfNotEmpty( "[ngSwitch]", value );
        return builder;
    }

    /// <summary>
    /// Extiende la funcionalidad de <see cref="TagBuilder"/> para configurar el atributo NgSwitch.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo de builder que hereda de <see cref="TagBuilder"/>.</typeparam>
    /// <param name="builder">El objeto builder que se está configurando.</param>
    /// <param name="config">La configuración que contiene el valor para el atributo NgSwitch.</param>
    /// <returns>El mismo objeto builder con el atributo NgSwitch configurado.</returns>
    /// <remarks>
    /// Este método permite establecer el valor del atributo NgSwitch en el builder basado en la configuración proporcionada.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder NgSwitch<TBuilder>( this TBuilder builder, Config config ) where TBuilder : TagBuilder {
        builder.NgSwitch( config.GetValue( AngularConst.NgSwitch ) );
        return builder;
    }

    /// <summary>
    /// Establece el atributo *ngSwitchCase en el objeto TagBuilder especificado.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo de objeto que hereda de TagBuilder.</typeparam>
    /// <param name="builder">El objeto TagBuilder en el que se establecerá el atributo.</param>
    /// <param name="value">El valor del atributo *ngSwitchCase.</param>
    /// <returns>El mismo objeto TagBuilder con el atributo establecido.</returns>
    /// <remarks>
    /// Este método se utiliza para agregar un caso específico a una estructura de control ngSwitch en Angular.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder NgSwitchCase<TBuilder>( this TBuilder builder, string value ) where TBuilder : TagBuilder {
        builder.AttributeIfNotEmpty( "*ngSwitchCase", value );
        return builder;
    }

    /// <summary>
    /// Establece el caso de un NgSwitch en el constructor especificado.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo del constructor que hereda de TagBuilder.</typeparam>
    /// <param name="builder">El constructor en el que se aplicará el caso de NgSwitch.</param>
    /// <param name="config">La configuración que contiene el valor del caso de NgSwitch.</param>
    /// <returns>El constructor modificado con el caso de NgSwitch aplicado.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de TagBuilder permitiendo configurar
    /// el caso de NgSwitch de manera fluida.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder NgSwitchCase<TBuilder>( this TBuilder builder, Config config ) where TBuilder : TagBuilder {
        builder.NgSwitchCase( config.GetValue( AngularConst.NgSwitchCase ) );
        return builder;
    }

    /// <summary>
    /// Establece el atributo <c>*ngSwitchDefault</c> en el <see cref="TagBuilder"/> especificado si el valor es verdadero.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo del constructor de etiquetas que se está utilizando.</typeparam>
    /// <param name="builder">El constructor de etiquetas en el que se establecerá el atributo.</param>
    /// <param name="value">Un valor booleano que determina si se debe agregar el atributo.</param>
    /// <returns>El mismo constructor de etiquetas con el atributo agregado si <paramref name="value"/> es verdadero.</returns>
    /// <remarks>
    /// Este método es útil para configurar el comportamiento de las directivas de Angular en la construcción de componentes.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder NgSwitchDefault<TBuilder>( this TBuilder builder, bool? value ) where TBuilder : TagBuilder {
        if ( value == true )
            builder.Attribute( "*ngSwitchDefault" );
        return builder;
    }

    /// <summary>
    /// Establece el valor predeterminado para la directiva NgSwitch en el generador de etiquetas.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo del generador de etiquetas que se está utilizando.</typeparam>
    /// <param name="builder">El generador de etiquetas en el que se aplicará la configuración.</param>
    /// <param name="config">La configuración que contiene el valor para NgSwitchDefault.</param>
    /// <returns>El generador de etiquetas modificado con la configuración NgSwitchDefault aplicada.</returns>
    /// <remarks>
    /// Este método permite configurar el comportamiento predeterminado de NgSwitch en función de un valor 
    /// obtenido de la configuración proporcionada.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder NgSwitchDefault<TBuilder>( this TBuilder builder, Config config ) where TBuilder : TagBuilder {
        return builder.NgSwitchDefault( config.GetValue<bool?>( AngularConst.NgSwitchDefault ) );
    }

    /// <summary>
    /// Extiende la funcionalidad de la clase <see cref="TagBuilder"/> para agregar una directiva NgFor.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo de constructor que hereda de <see cref="TagBuilder"/>.</typeparam>
    /// <param name="builder">El objeto <see cref="TBuilder"/> que se está construyendo.</param>
    /// <param name="config">La configuración que contiene los valores necesarios para la directiva NgFor.</param>
    /// <returns>El objeto <see cref="TBuilder"/> modificado con la directiva NgFor aplicada.</returns>
    /// <remarks>
    /// Este método permite aplicar la directiva NgFor a un elemento HTML mediante la configuración proporcionada.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder NgFor<TBuilder>( this TBuilder builder, Config config ) where TBuilder : TagBuilder {
        builder.NgFor( config.GetValue( AngularConst.NgFor ) );
        return builder;
    }

    /// <summary>
    /// Extiende la funcionalidad de la clase <see cref="TagBuilder"/> para agregar un atributo 
    /// *ngFor a un elemento HTML.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo del constructor de etiquetas que se está utilizando.</typeparam>
    /// <param name="builder">El constructor de etiquetas al que se le añadirá el atributo.</param>
    /// <param name="value">El valor que se asignará al atributo *ngFor.</param>
    /// <returns>El mismo constructor de etiquetas con el atributo *ngFor agregado.</returns>
    /// <remarks>
    /// Este método permite agregar la directiva *ngFor de Angular a un elemento HTML, 
    /// facilitando la creación de listas dinámicas en la interfaz de usuario.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder NgFor<TBuilder>( this TBuilder builder, string value ) where TBuilder : TagBuilder {
        builder.AttributeIfNotEmpty( "*ngFor", value );
        return builder;
    }

    /// <summary>
    /// Extiende la funcionalidad de la clase <see cref="TagBuilder"/> para agregar un atributo 
    /// <c>ngClass</c> a un elemento HTML, utilizando la configuración proporcionada.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo de builder que hereda de <see cref="TagBuilder"/>.</typeparam>
    /// <param name="builder">El objeto <typeparamref name="TBuilder"/> que se está construyendo.</param>
    /// <param name="config">La configuración que contiene el valor para el atributo <c>ngClass</c>.</param>
    /// <returns>
    /// El mismo objeto <typeparamref name="TBuilder"/> con el atributo <c>ngClass</c> agregado, 
    /// permitiendo la encadenación de métodos.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el valor del atributo <c>ngClass</c> no está vacío antes de 
    /// agregarlo al builder. Esto ayuda a evitar la inclusión de atributos innecesarios en el 
    /// HTML generado.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    /// <seealso cref="Config"/>
    /// <seealso cref="AngularConst"/>
    public static TBuilder NgClass<TBuilder>( this TBuilder builder, Config config ) where TBuilder : TagBuilder {
        builder.AttributeIfNotEmpty( "[ngClass]", config.GetValue( AngularConst.NgClass ) );
        return builder;
    }

    /// <summary>
    /// Establece el atributo ngStyle en el builder si el valor correspondiente no está vacío.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo del builder que hereda de TagBuilder.</typeparam>
    /// <param name="builder">El builder al que se le aplicará el atributo ngStyle.</param>
    /// <param name="config">La configuración que contiene el valor para el atributo ngStyle.</param>
    /// <returns>
    /// El builder modificado con el atributo ngStyle establecido.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión para la clase TagBuilder y permite agregar dinámicamente
    /// el atributo ngStyle a un elemento HTML basado en la configuración proporcionada.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder NgStyle<TBuilder>( this TBuilder builder, Config config ) where TBuilder : TagBuilder {
        builder.AttributeIfNotEmpty( "[ngStyle]", config.GetValue( AngularConst.NgStyle ) );
        return builder;
    }

    /// <summary>
    /// Extiende la funcionalidad de un <see cref="TagBuilder"/> para agregar atributos de control de acceso (ACL) 
    /// basados en la configuración proporcionada.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo de constructor que extiende <see cref="TagBuilder"/>.</typeparam>
    /// <param name="builder">El objeto <typeparamref name="TBuilder"/> sobre el cual se aplicará la extensión.</param>
    /// <param name="config">La configuración que contiene los valores necesarios para determinar el ACL.</param>
    /// <returns>El mismo objeto <typeparamref name="TBuilder"/> con los atributos ACL aplicados.</returns>
    /// <remarks>
    /// Si el valor de ACL está vacío, se devuelve el constructor sin modificaciones. 
    /// Si el ID de la plantilla alternativa está vacío, se agrega un atributo "*aclIf" con el valor del ACL. 
    /// Si ambos valores están presentes, se agrega un atributo "*aclIf" que incluye el ACL y el ID de la plantilla alternativa.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder Acl<TBuilder>( this TBuilder builder, Config config ) where TBuilder : TagBuilder {
        var acl = config.GetValue( UiConst.Acl );
        var templateId = config.GetValue( UiConst.AclElseTemplateId );
        if ( acl.IsEmpty() )
            return builder;
        if ( templateId.IsEmpty() ) {
            builder.Attribute( "*aclIf", GetAcl( acl ) );
            return builder;
        }
        builder.Attribute( "*aclIf", $"{GetAcl( acl )}; else {templateId}" );
        return builder;
    }

    /// <summary>
    /// Obtiene una lista de control de acceso (ACL) a partir de una cadena de entrada.
    /// </summary>
    /// <param name="acl">La cadena que representa la ACL que se va a procesar.</param>
    /// <returns>
    /// Una cadena que representa la ACL procesada, que puede ser el resultado de una división por "||", 
    /// "&&" o una ACL segura si no se encuentran esos operadores.
    /// </returns>
    /// <remarks>
    /// Este método verifica si la cadena de entrada contiene los operadores lógicos "||" o "&&". 
    /// Dependiendo del operador encontrado, se llama a un método diferente para procesar la ACL.
    /// Si no se encuentra ninguno de los operadores, se llama a <see cref="GetSafeAcl"/>.
    /// </remarks>
    /// <seealso cref="SplitOr"/>
    /// <seealso cref="SplitAnd"/>
    /// <seealso cref="GetSafeAcl"/>
    private static string GetAcl( string acl ) {
        if ( acl.Contains( "||" ) )
            return SplitOr( acl );
        if ( acl.Contains( "&&" ) )
            return SplitAnd( acl );
        return GetSafeAcl( acl );
    }

    /// <summary>
    /// Divide una cadena de acceso (ACL) en elementos separados por "||".
    /// </summary>
    /// <param name="acl">La cadena de acceso que se va a dividir.</param>
    /// <returns>Una representación en forma de lista de los elementos de la ACL, en formato JSON.</returns>
    /// <remarks>
    /// Este método utiliza el método <see cref="GetSafeAcl(string)"/> para procesar cada elemento de la ACL.
    /// Los elementos vacíos se omiten en el resultado final.
    /// </remarks>
    private static string SplitOr( string acl ) {
        var list = acl.Split( "||" );
        var result = new StringBuilder();
        result.Append( "[" );
        foreach ( var item in list ) {
            if ( item.IsEmpty() )
                continue;
            result.Append( $"{GetSafeAcl( item )}," );
        }
        result.RemoveEnd( "," );
        result.Append( "]" );
        return result.ToString();
    }

    /// <summary>
    /// Divide una cadena de control de acceso (ACL) utilizando el delimitador "&&".
    /// </summary>
    /// <param name="acl">La cadena de control de acceso que se desea dividir.</param>
    /// <returns>
    /// Una cadena formateada que representa los roles extraídos de la ACL, 
    /// estructurada en un formato específico con un modo 'allOf'.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un <see cref="StringBuilder"/> para construir la cadena de resultado,
    /// eliminando cualquier entrada vacía y asegurándose de que el formato final sea correcto.
    /// </remarks>
    private static string SplitAnd( string acl ) {
        var list = acl.Split( "&&" );
        var result = new StringBuilder();
        result.Append( "{role:[" );
        foreach ( var item in list ) {
            if ( item.IsEmpty() )
                continue;
            result.Append( $"{GetSafeAcl( item )}," );
        }
        result.RemoveEnd( "," );
        result.Append( "],mode:'allOf'}" );
        return result.ToString();
    }

    /// <summary>
    /// Obtiene una representación segura de una lista de control de acceso (ACL).
    /// </summary>
    /// <param name="acl">La cadena que representa la ACL que se desea procesar.</param>
    /// <returns>
    /// Una cadena que representa la ACL de forma segura. Si la ACL comienza con un corchete
    /// o contiene comillas simples, se devuelve tal cual, de lo contrario, se envuelve en comillas simples.
    /// </returns>
    private static string GetSafeAcl( string acl ) {
        if ( acl.StartsWith( "[" ) || acl.Contains( "'" ) )
            return acl.SafeString();
        return $"'{acl.SafeString()}'";
    }

    /// <summary>
    /// Asocia una configuración de ACL (Control de Acceso) al constructor de etiquetas especificado.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo del constructor de etiquetas que se está utilizando.</typeparam>
    /// <param name="builder">El constructor de etiquetas al que se le aplicará la configuración de ACL.</param>
    /// <param name="config">La configuración que contiene el valor de ACL a asociar.</param>
    /// <returns>El constructor de etiquetas modificado con la configuración de ACL aplicada.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de los constructores de etiquetas permitiendo
    /// agregar un atributo de ACL si el valor correspondiente no está vacío.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder BindAcl<TBuilder>( this TBuilder builder, Config config ) where TBuilder : TagBuilder {
        builder.AttributeIfNotEmpty( "[acl]", config.GetValue( AngularConst.BindAcl ) );
        return builder;
    }

    /// <summary>
    /// Establece el atributo ngModel en un TagBuilder.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo del constructor que hereda de TagBuilder.</typeparam>
    /// <param name="builder">El constructor al que se le aplicará el atributo ngModel.</param>
    /// <param name="value">El valor que se asignará al atributo ngModel.</param>
    /// <returns>El constructor modificado con el atributo ngModel establecido.</returns>
    /// <remarks>
    /// Este método permite agregar el atributo ngModel a un TagBuilder solo si el valor proporcionado no está vacío.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder NgModel<TBuilder>( this TBuilder builder, string value ) where TBuilder : TagBuilder {
        builder.AttributeIfNotEmpty( "[(ngModel)]", value );
        return builder;
    }

    /// <summary>
    /// Asocia un modelo de Angular a un elemento HTML representado por el <see cref="TagBuilder"/>.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo de constructor de etiquetas que se está utilizando.</typeparam>
    /// <param name="builder">El constructor de etiquetas al que se le va a asociar el modelo.</param>
    /// <param name="value">El valor del modelo que se va a asociar al elemento.</param>
    /// <returns>El mismo constructor de etiquetas con el modelo asociado.</returns>
    /// <remarks>
    /// Este método agrega un atributo "[ngModel]" al elemento HTML si el valor proporcionado no está vacío.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder BindNgModel<TBuilder>( this TBuilder builder, string value ) where TBuilder : TagBuilder {
        builder.AttributeIfNotEmpty( "[ngModel]", value );
        return builder;
    }

    /// <summary>
    /// Extiende la funcionalidad de un <see cref="TagBuilder"/> para configurar el modelo de Angular.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo del constructor que extiende <see cref="TagBuilder"/>.</typeparam>
    /// <param name="builder">El objeto <see cref="TBuilder"/> que se está configurando.</param>
    /// <param name="config">La configuración que contiene los valores necesarios para establecer el modelo de Angular.</param>
    /// <returns>
    /// El mismo objeto <see cref="TBuilder"/> después de aplicar las configuraciones de modelo de Angular.
    /// </returns>
    /// <remarks>
    /// Este método permite establecer el modelo de Angular y la vinculación del modelo utilizando los valores
    /// proporcionados en la configuración.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder NgModel<TBuilder>( this TBuilder builder, Config config ) where TBuilder : TagBuilder {
        builder.NgModel( config.GetValue( AngularConst.NgModel ) );
        builder.BindNgModel( config.GetValue( AngularConst.BindNgModel ) );
        return builder;
    }

    /// <summary>
    /// Establece un evento de clic en el objeto builder utilizando la configuración proporcionada.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo del constructor de etiquetas que se está utilizando.</typeparam>
    /// <param name="builder">El objeto builder sobre el cual se aplicará el evento de clic.</param>
    /// <param name="config">La configuración que contiene el valor del evento de clic.</param>
    /// <returns>El objeto builder modificado con el evento de clic establecido.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de la clase <see cref="TagBuilder"/> 
    /// permitiendo que se configure un evento de clic de manera fluida.
    /// </remarks>
    public static TBuilder OnClick<TBuilder>( this TBuilder builder, Config config ) where TBuilder : TagBuilder {
        return builder.OnClick( config.GetValue( UiConst.OnClick ) );
    }

    /// <summary>
    /// Establece un atributo de evento de clic en el generador de etiquetas.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo del generador de etiquetas que se está utilizando.</typeparam>
    /// <param name="builder">El generador de etiquetas actual.</param>
    /// <param name="onclick">La cadena que representa la acción a realizar cuando se produce el evento de clic.</param>
    /// <returns>
    /// El generador de etiquetas modificado con el atributo de clic establecido.
    /// </returns>
    /// <remarks>
    /// Este método agrega un atributo de clic al generador de etiquetas solo si el valor proporcionado no está vacío.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder OnClick<TBuilder>( this TBuilder builder, string onclick ) where TBuilder : TagBuilder {
        builder.AttributeIfNotEmpty( "(click)", onclick );
        return builder;
    }

    /// <summary>
    /// Establece el atributo "id" en el constructor de etiquetas si el valor no está vacío.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo del constructor de etiquetas que se está utilizando.</typeparam>
    /// <param name="builder">El constructor de etiquetas en el que se establecerá el atributo.</param>
    /// <param name="config">La configuración desde la cual se obtiene el valor del atributo "id".</param>
    /// <returns>El constructor de etiquetas modificado.</returns>
    /// <remarks>
    /// Este método es una extensión que permite agregar un atributo "id" a un objeto de tipo <typeparamref name="TBuilder"/> 
    /// solo si el valor correspondiente en la configuración no está vacío.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder RawId<TBuilder>( this TBuilder builder, Config config ) where TBuilder : TagBuilder {
        builder.AttributeIfNotEmpty( "id", config.GetValue( AngularConst.RawId ) );
        return builder;
    }

    /// <summary>
    /// Establece el identificador (ID) para el objeto <typeparamref name="TBuilder"/> utilizando la configuración proporcionada.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo de constructor de etiquetas que se está utilizando.</typeparam>
    /// <param name="builder">El objeto <typeparamref name="TBuilder"/> en el que se establecerá el ID.</param>
    /// <param name="config">La configuración que se utilizará para obtener el valor del ID.</param>
    /// <param name="value">Un valor opcional que se usará como ID. Si es nulo, se utilizará el valor obtenido de la configuración.</param>
    /// <returns>El objeto <typeparamref name="TBuilder"/> con el ID establecido.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de la clase <typeparamref name="TBuilder"/> permitiendo establecer un ID de manera fluida.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder Id<TBuilder>( this TBuilder builder, Config config, string value = null ) where TBuilder : TagBuilder {
        return builder.Id( config.GetValue( UiConst.Id ), value );
    }

    /// <summary>
    /// Establece un atributo de identificación en el objeto <see cref="TagBuilder"/> especificado.
    /// </summary>
    /// <typeparam name="TBuilder">El tipo del constructor de etiquetas que se está utilizando.</typeparam>
    /// <param name="builder">El constructor de etiquetas en el que se establecerá el atributo de identificación.</param>
    /// <param name="name">El nombre del atributo de identificación que se va a establecer.</param>
    /// <param name="value">El valor del atributo de identificación. Si no se proporciona, se establece como nulo.</param>
    /// <returns>El mismo objeto <typeparamref name="TBuilder"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método solo establece el atributo si el nombre proporcionado no está vacío o es nulo.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    public static TBuilder Id<TBuilder>( this TBuilder builder, string name, string value = null ) where TBuilder : TagBuilder {
        if ( string.IsNullOrWhiteSpace( name ) == false )
            builder.Attribute( $"#{name}", value );
        return builder;
    }
}