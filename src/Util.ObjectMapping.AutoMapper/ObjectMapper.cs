using Util.Helpers;

namespace Util.ObjectMapping;

/// <summary>
/// Clase que implementa la interfaz <see cref="IObjectMapper"/>.
/// Proporciona funcionalidades para mapear objetos entre diferentes tipos.
/// </summary>
public class ObjectMapper : IObjectMapper
{
    private const int MaxGetResultCount = 16;
    private static readonly object Sync = new();
    private readonly MapperConfigurationExpression _configExpression;
    private IConfigurationProvider _config;
    private IMapper _mapper;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ObjectMapper"/>.
    /// </summary>
    /// <param name="expression">La expresión de configuración del mapeador que se utilizará para crear el mapeador.</param>
    /// <exception cref="ArgumentNullException">Se lanza cuando <paramref name="expression"/> es <c>null</c>.</exception>
    public ObjectMapper(MapperConfigurationExpression expression)
    {
        _configExpression = expression ?? throw new ArgumentNullException(nameof(expression));
        _config = new MapperConfiguration(expression);
        _mapper = _config.CreateMapper();
    }

    /// <summary>
    /// Mapea un objeto de tipo <typeparamref name="TSource"/> a un objeto de tipo <typeparamref name="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">El tipo del objeto fuente que se va a mapear.</typeparam>
    /// <typeparam name="TDestination">El tipo del objeto destino al que se va a mapear.</typeparam>
    /// <param name="source">El objeto fuente que se va a mapear.</param>
    /// <returns>Un objeto de tipo <typeparamref name="TDestination"/> que representa el resultado del mapeo.</returns>
    /// <remarks>
    /// Este método utiliza un mapeo por defecto si no se proporciona un segundo parámetro.
    /// </remarks>
    /// <seealso cref="Map{TSource, TDestination}(TSource, TDestination)"/>
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return Map<TSource, TDestination>(source, default);
    }

    /// <summary>
    /// Mapea un objeto de tipo <typeparamref name="TSource"/> a un objeto de tipo <typeparamref name="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">El tipo del objeto fuente que se va a mapear.</typeparam>
    /// <typeparam name="TDestination">El tipo del objeto de destino al que se va a mapear.</typeparam>
    /// <param name="source">El objeto fuente que se va a mapear. Si es <c>null</c>, se devuelve el valor predeterminado de <typeparamref name="TDestination"/>.</param>
    /// <param name="destination">El objeto de destino donde se mapearán los valores del objeto fuente.</param>
    /// <returns>Un objeto de tipo <typeparamref name="TDestination"/> que contiene los valores mapeados del objeto fuente, o el valor predeterminado si el objeto fuente es <c>null</c>.</returns>
    /// <remarks>
    /// Este método utiliza reflexión para determinar los tipos de los objetos fuente y destino,
    /// y realiza el mapeo de las propiedades correspondientes. 
    /// Asegúrese de que las propiedades de ambos tipos sean compatibles para un mapeo exitoso.
    /// </remarks>
    /// <seealso cref="GetType(object)"/>
    /// <seealso cref="GetResult(Type, Type, TSource, TDestination, int)"/>
    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        if (source == null)
            return default;
        var sourceType = GetType(source);
        var destinationType = GetType(destination);
        return GetResult(sourceType, destinationType, source, destination, 0);
    }

    /// <summary>
    /// Obtiene el tipo de un objeto dado. Si el objeto es nulo, se devuelve el tipo genérico especificado.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que se está evaluando.</typeparam>
    /// <param name="obj">El objeto del cual se desea obtener el tipo.</param>
    /// <returns>El tipo del objeto proporcionado o el tipo genérico si el objeto es nulo.</returns>
    /// <remarks>
    /// Este método utiliza la reflexión para determinar el tipo del objeto. Si el objeto es nulo,
    /// se utiliza el tipo genérico <typeparamref name="T"/> para obtener el tipo.
    /// </remarks>
    /// <seealso cref="System.Type"/>
    private Type GetType<T>(T obj)
    {
        if (obj == null)
            return GetType(typeof(T));
        return GetType(obj.GetType());
    }

    /// <summary>
    /// Obtiene el tipo de elemento de un tipo dado.
    /// </summary>
    /// <param name="type">El tipo del cual se desea obtener el tipo de elemento.</param>
    /// <returns>El tipo de elemento asociado al tipo proporcionado, o <c>null</c> si el tipo no es un arreglo o un tipo genérico.</returns>
    private Type GetType(Type type)
    {
        return Reflection.GetElementType(type);
    }

    /// <summary>
    /// Obtiene el resultado de la conversión de un objeto fuente a un objeto de destino.
    /// </summary>
    /// <typeparam name="TDestination">El tipo del objeto de destino.</typeparam>
    /// <param name="sourceType">El tipo del objeto fuente.</param>
    /// <param name="destinationType">El tipo del objeto de destino.</param>
    /// <param name="source">El objeto fuente que se va a convertir.</param>
    /// <param name="destination">El objeto de destino donde se almacenará el resultado de la conversión.</param>
    /// <param name="i">Un contador que limita el número de intentos de conversión.</param>
    /// <returns>El objeto de destino convertido, o el valor predeterminado si se alcanza el límite de intentos.</returns>
    /// <remarks>
    /// Este método intenta realizar la conversión del objeto fuente al objeto de destino. Si la conversión falla debido a una falta de configuración de mapeo, 
    /// se intentará nuevamente con el tipo de origen y destino corregido. El proceso se repetirá hasta que se alcance el número máximo de intentos definidos por <c>MaxGetResultCount</c>.
    /// </remarks>
    /// <exception cref="AutoMapperMappingException">
    /// Se lanza si ocurre un error durante el mapeo de AutoMapper que no puede ser manejado internamente.
    /// </exception>
    private TDestination GetResult<TDestination>(Type sourceType, Type destinationType, object source, TDestination destination, int i)
    {
        try
        {
            if (i >= MaxGetResultCount)
                return default;
            i += 1;
            if (Exists(sourceType, destinationType))
                return GetResult(source, destination);
            lock (Sync)
            {
                if (Exists(sourceType, destinationType))
                    return GetResult(source, destination);
                ConfigMap(sourceType, destinationType);
            }
            return GetResult(source, destination);
        }
        catch (AutoMapperMappingException ex)
        {
            if (ex.InnerException != null && ex.InnerException.Message.StartsWith("Falta la configuración del mapa de tipos."))
                return GetResult(GetType(ex.MemberMap.SourceType), GetType(ex.MemberMap.DestinationType), source, destination, i);
            throw;
        }
    }

    /// <summary>
    /// Verifica si existe un mapeo de tipo entre el tipo de origen y el tipo de destino.
    /// </summary>
    /// <param name="sourceType">El tipo de origen que se va a verificar.</param>
    /// <param name="destinationType">El tipo de destino que se va a verificar.</param>
    /// <returns>Devuelve true si existe un mapeo para los tipos especificados; de lo contrario, devuelve false.</returns>
    private bool Exists(Type sourceType, Type destinationType)
    {
        return _config.Internal().FindTypeMapFor(sourceType, destinationType) != null;
    }

    /// <summary>
    /// Mapea un objeto de tipo <typeparamref name="TSource"/> a un objeto de tipo <typeparamref name="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">El tipo del objeto fuente que se va a mapear.</typeparam>
    /// <typeparam name="TDestination">El tipo del objeto destino al que se va a mapear.</typeparam>
    /// <param name="source">El objeto fuente que se va a mapear.</param>
    /// <param name="destination">El objeto destino donde se almacenará el resultado del mapeo.</param>
    /// <returns>El objeto destino mapeado a partir del objeto fuente.</returns>
    /// <remarks>
    /// Este método utiliza un mapeador para realizar la conversión entre los tipos especificados.
    /// Asegúrese de que los tipos de origen y destino sean compatibles para evitar excepciones durante el mapeo.
    /// </remarks>
    /// <seealso cref="AutoMapper.IMapper"/>
    private TDestination GetResult<TSource, TDestination>(TSource source, TDestination destination)
    {
        return _mapper.Map(source, destination);
    }

    /// <summary>
    /// Configura un mapeo entre dos tipos utilizando AutoMapper.
    /// </summary>
    /// <param name="sourceType">El tipo de origen que se va a mapear.</param>
    /// <param name="destinationType">El tipo de destino al que se va a mapear.</param>
    /// <remarks>
    /// Este método crea una expresión de mapeo entre el tipo de origen y el tipo de destino,
    /// y luego inicializa una configuración de mapeo que se puede utilizar para realizar
    /// la conversión de objetos entre estos tipos.
    /// </remarks>
    private void ConfigMap(Type sourceType, Type destinationType)
    {
        _configExpression.CreateMap(sourceType, destinationType);
        _config = new MapperConfiguration(_configExpression);
        _mapper = _config.CreateMapper();
    }
}