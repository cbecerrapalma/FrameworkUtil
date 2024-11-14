using Util.Localization.Json;
using Util.Localization.Store;

namespace Util.Localization;

/// <summary>
/// Proporciona métodos de extensión para configurar la aplicación.
/// </summary>
public static class AppBuilderExtensions {

    #region AddJsonLocalization

    /// <summary>
    /// Agrega la localización basada en JSON al constructor de aplicaciones.
    /// </summary>
    /// <param name="builder">El constructor de aplicaciones al que se le añadirá la localización.</param>
    /// <returns>El constructor de aplicaciones modificado con la localización añadida.</returns>
    public static IAppBuilder AddJsonLocalization( this IAppBuilder builder ) {
        return builder.AddJsonLocalization( "Resources" );
    }

    /// <summary>
    /// Agrega la localización basada en JSON al middleware de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación.</param>
    /// <param name="resourcesPath">La ruta donde se encuentran los archivos de recursos JSON.</param>
    /// <returns>El constructor de la aplicación con la localización JSON añadida.</returns>
    /// <remarks>
    /// Este método permite configurar la localización de la aplicación utilizando archivos JSON
    /// que contienen las traducciones necesarias para diferentes idiomas.
    /// </remarks>
    /// <seealso cref="IAppBuilder"/>
    public static IAppBuilder AddJsonLocalization( this IAppBuilder builder, string resourcesPath ) {
        return builder.AddJsonLocalization( t => t.ResourcesPath = resourcesPath );
    }

    /// <summary>
    /// Agrega la localización basada en JSON al pipeline de la aplicación.
    /// </summary>
    /// <param name="builder">El <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de localización JSON.</param>
    /// <returns>El <see cref="IAppBuilder"/> modificado para incluir la localización JSON.</returns>
    /// <remarks>
    /// Este método configura los servicios necesarios para la localización de cadenas utilizando archivos JSON.
    /// Se asegura de que las opciones de localización se configuren correctamente y se añadan los servicios requeridos.
    /// </remarks>
    /// <seealso cref="JsonLocalizationOptions"/>
    /// <seealso cref="LocalizationOptions"/>
    /// <seealso cref="IStringLocalizerFactory"/>
    /// <seealso cref="IStringLocalizer"/>
    public static IAppBuilder AddJsonLocalization( this IAppBuilder builder, Action<JsonLocalizationOptions> setupAction ) {
        builder.CheckNull( nameof( builder ) );
        var options = new JsonLocalizationOptions();
        setupAction?.Invoke( options );
        builder.Host.ConfigureServices( ( context, services ) => {
            if ( setupAction != null ) {
                services.Configure( setupAction );
                void Action( LocalizationOptions localizationOptions ) {
                    localizationOptions.Expiration = options.Expiration;
                    localizationOptions.IsLocalizeWarning = options.IsLocalizeWarning;
                    localizationOptions.Cultures = options.Cultures;
                }
                services.Configure( (Action<LocalizationOptions>)Action );
            }
            services.AddMemoryCache();
            services.RemoveAll( typeof( IStringLocalizerFactory ) );
            services.RemoveAll( typeof( IStringLocalizer<> ) );
            services.RemoveAll( typeof( IStringLocalizer ) );
            services.TryAddSingleton<IPathResolver, PathResolver>();
            services.TryAddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.TryAddTransient( typeof( IStringLocalizer<> ), typeof( StringLocalizer<> ) );
            services.TryAddTransient( typeof( IStringLocalizer ), typeof( StringLocalizer ) );
        } );
        if( options.Cultures == null || options.Cultures.Count == 0 )
            return builder;
        builder.Host.ConfigureServices( ( _, services ) => {
            ConfigRequestLocalization( services, options );
        } );
        return builder;
    }

    /// <summary>
    /// Configura las opciones de localización para las solicitudes.
    /// </summary>
    /// <param name="services">La colección de servicios donde se registrarán las opciones de localización.</param>
    /// <param name="options">Las opciones de localización que contienen las culturas soportadas.</param>
    /// <remarks>
    /// Este método establece la cultura predeterminada y las culturas soportadas para la localización de la aplicación.
    /// También agrega un proveedor de cultura personalizado que determina la cultura a partir de los encabezados de la solicitud.
    /// Si no se encuentra una cultura en los encabezados, se establece "zh-CN" como cultura predeterminada.
    /// </remarks>
    private static void ConfigRequestLocalization( IServiceCollection services, LocalizationOptions options ) {
        services.Configure<RequestLocalizationOptions>( localizationOptions => {
            var supportedCultures = options.Cultures.Select( culture => new CultureInfo( culture ) ).ToList();
            localizationOptions.DefaultRequestCulture = new RequestCulture( culture: supportedCultures[0], uiCulture: supportedCultures[0] );
            localizationOptions.SupportedCultures = supportedCultures;
            localizationOptions.SupportedUICultures = supportedCultures;
            localizationOptions.AddInitialRequestCultureProvider( new CustomRequestCultureProvider( async context => {
                var culture = context.Request.Headers.ContentLanguage.FirstOrDefault();
                if ( culture.IsEmpty() )
                    culture = "zh-CN";
                return await Task.FromResult( new ProviderCultureResult( culture ) );
            } ) );
        } );
    }

    #endregion

    #region AddStoreLocalization

    /// <summary>
    /// Agrega la localización de la tienda al constructor de la aplicación.
    /// </summary>
    /// <typeparam name="TStore">El tipo de la tienda que implementa <see cref="ILocalizedStore"/>.</typeparam>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> al que se le agrega la localización.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> modificado con la localización de la tienda.</returns>
    /// <remarks>
    /// Este método configura la expiración de la localización a 28800 segundos (8 horas) por defecto.
    /// </remarks>
    /// <seealso cref="ILocalizedStore"/>
    public static IAppBuilder AddStoreLocalization<TStore>( this IAppBuilder builder ) where TStore : ILocalizedStore {
        return builder.AddStoreLocalization<TStore>( options => options.Expiration = 28800 );
    }

    /// <summary>
    /// Agrega la localización de la tienda al pipeline de la aplicación.
    /// </summary>
    /// <typeparam name="TStore">El tipo de la tienda que implementa <see cref="ILocalizedStore"/>.</typeparam>
    /// <param name="builder">El <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de localización.</param>
    /// <returns>El <see cref="IAppBuilder"/> modificado para incluir la localización de la tienda.</returns>
    /// <remarks>
    /// Este método configura los servicios necesarios para la localización, incluyendo la adición de
    /// un caché en memoria y la configuración de localizadores de cadenas.
    /// </remarks>
    /// <seealso cref="ILocalizedStore"/>
    /// <seealso cref="LocalizationOptions"/>
    /// <seealso cref="IStringLocalizerFactory"/>
    /// <seealso cref="ILocalizedManager"/>
    public static IAppBuilder AddStoreLocalization<TStore>( this IAppBuilder builder, Action<LocalizationOptions> setupAction ) where TStore : ILocalizedStore {
        builder.CheckNull( nameof( builder ) );
        var options = new LocalizationOptions();
        setupAction?.Invoke( options );
        builder.Host.ConfigureServices( ( context, services ) => {
            if ( setupAction != null )
                services.Configure( setupAction );
            services.AddMemoryCache();
            services.RemoveAll( typeof( IStringLocalizerFactory ) );
            services.RemoveAll( typeof( IStringLocalizer<> ) );
            services.RemoveAll( typeof( IStringLocalizer ) );
            services.TryAddSingleton<IStringLocalizerFactory, StoreStringLocalizerFactory>();
            services.TryAddTransient( typeof( IStringLocalizer<> ), typeof( StringLocalizer<> ) );
            services.TryAddTransient( typeof( IStringLocalizer ), typeof( StringLocalizer ) );
            services.TryAddTransient( typeof( ILocalizedStore ), typeof( TStore ) );
            services.TryAddTransient<ILocalizedManager, LocalizedManager>();
        } );
        builder.Host.ConfigureServices( ( _, services ) => {
            if ( options.Cultures == null || options.Cultures.Count == 0 )
                return;
            ConfigRequestLocalization( services, options );
        } );
        return builder;
    }

    #endregion
}