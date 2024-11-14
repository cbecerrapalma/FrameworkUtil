namespace Util.Helpers;

/// <summary>
/// Proporciona métodos para trabajar con reflexión en tipos y miembros de tipos.
/// </summary>
public static class Reflection
{

    #region GetDescription(Obtener descripción)

    /// <summary>
    /// Obtiene la descripción de un tipo genérico especificado.
    /// </summary>
    /// <typeparam name="T">El tipo del cual se desea obtener la descripción.</typeparam>
    /// <returns>
    /// Una cadena que representa la descripción del tipo especificado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el método <see cref="Common.GetType{T}"/> para obtener el tipo
    /// correspondiente a <typeparamref name="T"/> antes de llamar a otro método para
    /// obtener la descripción.
    /// </remarks>
    public static string GetDescription<T>()
    {
        return GetDescription(Common.GetType<T>());
    }

    /// <summary>
    /// Obtiene la descripción de un miembro de un tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo del que se desea obtener la descripción del miembro.</typeparam>
    /// <param name="memberName">El nombre del miembro del cual se desea obtener la descripción.</param>
    /// <returns>Una cadena que representa la descripción del miembro especificado.</returns>
    /// <remarks>
    /// Este método utiliza el tipo proporcionado por el parámetro genérico <typeparamref name="T"/> 
    /// para buscar la descripción del miembro indicado por <paramref name="memberName"/>.
    /// </remarks>
    /// <seealso cref="Common.GetType{T}"/>
    public static string GetDescription<T>(string memberName)
    {
        return GetDescription(Common.GetType<T>(), memberName);
    }

    /// <summary>
    /// Obtiene la descripción de un miembro de un tipo especificado.
    /// </summary>
    /// <param name="type">El tipo del cual se desea obtener la descripción del miembro.</param>
    /// <param name="memberName">El nombre del miembro cuya descripción se desea obtener.</param>
    /// <returns>
    /// Una cadena que representa la descripción del miembro especificado, 
    /// o una cadena vacía si el tipo es nulo o el nombre del miembro es nulo o está vacío.
    /// </returns>
    /// <remarks>
    /// Este método utiliza reflexión para obtener la información del miembro 
    /// y su descripción asociada. Si el miembro no se encuentra, se devolverá 
    /// una cadena vacía.
    /// </remarks>
    /// <seealso cref="System.Type"/>
    /// <seealso cref="System.Reflection.MemberInfo"/>
    public static string GetDescription(Type type, string memberName)
    {
        if (type == null)
            return string.Empty;
        if (string.IsNullOrWhiteSpace(memberName))
            return string.Empty;
        return GetDescription(type.GetTypeInfo().GetMember(memberName).FirstOrDefault());
    }

    /// <summary>
    /// Obtiene la descripción de un miembro, utilizando el atributo <see cref="DescriptionAttribute"/> si está presente.
    /// </summary>
    /// <param name="member">El miembro del cual se desea obtener la descripción.</param>
    /// <returns>
    /// Una cadena que representa la descripción del miembro. Si el miembro es nulo o no tiene un atributo de descripción, se devuelve el nombre del miembro.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el miembro proporcionado es nulo y, en caso afirmativo, devuelve una cadena vacía. 
    /// Si el atributo <see cref="DescriptionAttribute"/> está presente, se devuelve su descripción; de lo contrario, se devuelve el nombre del miembro.
    /// </remarks>
    public static string GetDescription(MemberInfo member)
    {
        if (member == null)
            return string.Empty;
        return member.GetCustomAttribute<DescriptionAttribute>() is { } attribute ? attribute.Description : member.Name;
    }

    #endregion

    #region GetDisplayName(Obtener nombre para mostrar)

    /// <summary>
    /// Obtiene el nombre para mostrar de un tipo genérico especificado.
    /// </summary>
    /// <typeparam name="T">El tipo del cual se desea obtener el nombre para mostrar.</typeparam>
    /// <returns>
    /// Un <see cref="string"/> que representa el nombre para mostrar del tipo especificado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la función <see cref="Common.GetType{T}"/> para obtener el tipo 
    /// correspondiente y luego llama a otro método sobrecargado para obtener el nombre para mostrar.
    /// </remarks>
    /// <seealso cref="Common.GetType{T}"/>
    public static string GetDisplayName<T>()
    {
        return GetDisplayName(Common.GetType<T>());
    }

    /// <summary>
    /// Obtiene el nombre para mostrar de un miembro dado, utilizando atributos de visualización.
    /// </summary>
    /// <param name="member">El miembro del cual se desea obtener el nombre para mostrar. Puede ser nulo.</param>
    /// <returns>
    /// Un string que representa el nombre para mostrar del miembro. 
    /// Devuelve una cadena vacía si el miembro es nulo o si no se encuentran atributos de visualización.
    /// </returns>
    /// <remarks>
    /// Este método busca primero un atributo de tipo <see cref="DisplayAttribute"/> y, si no se encuentra, 
    /// busca un atributo de tipo <see cref="DisplayNameAttribute"/>. Si se encuentra alguno de estos atributos, 
    /// se devuelve el nombre correspondiente. Si no se encuentra ninguno, se devuelve una cadena vacía.
    /// </remarks>
    public static string GetDisplayName(MemberInfo member)
    {
        if (member == null)
            return string.Empty;
        if (member.GetCustomAttribute<DisplayAttribute>() is { } displayAttribute)
            return displayAttribute.Name;
        if (member.GetCustomAttribute<DisplayNameAttribute>() is { } displayNameAttribute)
            return displayNameAttribute.DisplayName;
        return string.Empty;
    }

    #endregion

    #region GetDisplayNameOrDescription(Obtener nombre de visualización o descripción.)

    /// <summary>
    /// Obtiene el nombre para mostrar o la descripción de un tipo genérico especificado.
    /// </summary>
    /// <typeparam name="T">El tipo del cual se desea obtener el nombre para mostrar o la descripción.</typeparam>
    /// <returns>
    /// Una cadena que representa el nombre para mostrar o la descripción del tipo especificado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el método <see cref="Common.GetType{T}"/> para obtener el tipo correspondiente
    /// y luego llama a otro método para obtener el nombre o la descripción.
    /// </remarks>
    public static string GetDisplayNameOrDescription<T>()
    {
        return GetDisplayNameOrDescription(Common.GetType<T>());
    }

    /// <summary>
    /// Obtiene el nombre para mostrar o la descripción de un miembro.
    /// </summary>
    /// <param name="member">El miembro del cual se desea obtener el nombre para mostrar o la descripción.</param>
    /// <returns>
    /// Un string que representa el nombre para mostrar del miembro si está disponible; de lo contrario, retorna la descripción del miembro.
    /// </returns>
    /// <remarks>
    /// Este método primero intenta obtener el nombre para mostrar utilizando el método <see cref="GetDisplayName(MemberInfo)"/>.
    /// Si el nombre para mostrar es nulo o está vacío, se intenta obtener la descripción utilizando el método <see cref="GetDescription(MemberInfo)"/>.
    /// </remarks>
    public static string GetDisplayNameOrDescription(MemberInfo member)
    {
        var result = GetDisplayName(member);
        return string.IsNullOrWhiteSpace(result) ? GetDescription(member) : result;
    }

    #endregion

    #region CreateInstance(Creación dinámica de instancias)

    /// <summary>
    /// Crea una instancia de un tipo especificado utilizando el constructor que coincide con los parámetros proporcionados.
    /// </summary>
    /// <typeparam name="T">El tipo al que se convertirá la instancia creada.</typeparam>
    /// <param name="type">El tipo del objeto que se va a crear.</param>
    /// <param name="parameters">Los parámetros que se pasarán al constructor del tipo especificado.</param>
    /// <returns>Una instancia del tipo especificado convertida al tipo T.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="type"/> es nulo.</exception>
    /// <exception cref="Activator.CreateInstanceException">Se lanza si no se puede crear una instancia del tipo especificado.</exception>
    /// <seealso cref="System.Activator"/>
    /// <seealso cref="System.Convert"/>
    public static T CreateInstance<T>(Type type, params object[] parameters)
    {
        return Convert.To<T>(Activator.CreateInstance(type, parameters));
    }

    #endregion

    #region FindImplementTypes(Buscar lista de tipos de implementación.)

    /// <summary>
    /// Busca todos los tipos que implementan una interfaz o heredan de una clase base especificada.
    /// </summary>
    /// <typeparam name="TFind">El tipo de la interfaz o clase base que se desea buscar.</typeparam>
    /// <param name="assemblies">Un arreglo de ensamblados en los cuales se realizará la búsqueda.</param>
    /// <returns>Una lista de tipos que implementan la interfaz o heredan de la clase base especificada.</returns>
    /// <remarks>
    /// Este método es útil para encontrar todos los tipos que cumplen con un contrato específico 
    /// en un conjunto de ensamblados, facilitando la reflexión y la inyección de dependencias.
    /// </remarks>
    /// <seealso cref="FindImplementTypes(Type, Assembly[])"/>
    public static List<Type> FindImplementTypes<TFind>(params Assembly[] assemblies)
    {
        return FindImplementTypes(typeof(TFind), assemblies);
    }

    /// <summary>
    /// Busca los tipos que implementan o heredan de un tipo específico en los ensamblados proporcionados.
    /// </summary>
    /// <param name="findType">El tipo del cual se desean encontrar las implementaciones o herencias.</param>
    /// <param name="assemblies">Un arreglo de ensamblados en los que se realizará la búsqueda.</param>
    /// <returns>
    /// Una lista de tipos que implementan o heredan del tipo especificado, sin duplicados.
    /// </returns>
    /// <remarks>
    /// Este método recorre cada ensamblado proporcionado y utiliza el método <see cref="GetTypes(Type, Assembly)"/> 
    /// para obtener los tipos que cumplen con el criterio de búsqueda.
    /// </remarks>
    public static List<Type> FindImplementTypes(Type findType, params Assembly[] assemblies)
    {
        var result = new List<Type>();
        foreach (var assembly in assemblies)
            result.AddRange(GetTypes(findType, assembly));
        return result.Distinct().ToList();
    }

    /// <summary>
    /// Obtiene una lista de tipos de un ensamblado específico que coinciden con un tipo dado.
    /// </summary>
    /// <param name="findType">El tipo que se desea encontrar en el ensamblado.</param>
    /// <param name="assembly">El ensamblado del cual se obtendrán los tipos.</param>
    /// <returns>
    /// Una lista de tipos que coinciden con el tipo especificado en el ensamblado.
    /// Si el ensamblado es nulo o no se pueden cargar los tipos, se devuelve una lista vacía.
    /// </returns>
    /// <remarks>
    /// Este método intenta obtener todos los tipos del ensamblado proporcionado.
    /// Si ocurre una excepción de carga de tipo, se captura y se devuelve una lista vacía.
    /// </remarks>
    private static List<Type> GetTypes(Type findType, Assembly assembly)
    {
        var result = new List<Type>();
        if (assembly == null)
            return result;
        Type[] types;
        try
        {
            types = assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException)
        {
            return result;
        }
        foreach (var type in types)
            AddType(result, findType, type);
        return result;
    }

    /// <summary>
    /// Agrega un tipo a la lista de resultados si cumple con ciertas condiciones.
    /// </summary>
    /// <param name="result">La lista donde se agregarán los tipos que coincidan.</param>
    /// <param name="findType">El tipo que se utilizará para verificar la asignabilidad.</param>
    /// <param name="type">El tipo que se evaluará para determinar si se debe agregar a la lista.</param>
    /// <remarks>
    /// Este método verifica si el tipo proporcionado es una interfaz o abstracto, 
    /// y si se puede asignar desde el tipo buscado o si coincide con un tipo genérico.
    /// Si se cumplen estas condiciones, el tipo se agrega a la lista de resultados.
    /// </remarks>
    private static void AddType(List<Type> result, Type findType, Type type)
    {
        if (type.IsInterface || type.IsAbstract)
            return;
        if (findType.IsAssignableFrom(type) == false && MatchGeneric(findType, type) == false)
            return;
        result.Add(type);
    }

    /// <summary>
    /// Determina si un tipo dado implementa un tipo genérico específico.
    /// </summary>
    /// <param name="findType">El tipo genérico que se desea encontrar.</param>
    /// <param name="type">El tipo que se va a verificar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el tipo especificado implementa el tipo genérico; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el tipo proporcionado es una definición de tipo genérico.
    /// Luego, busca en las interfaces implementadas por el tipo dado y comprueba si alguna de ellas es asignable desde la definición del tipo genérico buscado.
    /// </remarks>
    private static bool MatchGeneric(Type findType, Type type)
    {
        if (findType.IsGenericTypeDefinition == false)
            return false;
        var definition = findType.GetGenericTypeDefinition();
        foreach (var implementedInterface in type.FindInterfaces((filter, criteria) => true, null))
        {
            if (implementedInterface.IsGenericType == false)
                continue;
            return definition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());
        }
        return false;
    }

    #endregion

    #region GetDirectInterfaceTypes(Obtener lista de tipos de interfaz directa.)

    /// <summary>
    /// Obtiene una lista de tipos de interfaz directa para un tipo específico.
    /// </summary>
    /// <typeparam name="T">El tipo del cual se desean obtener las interfaces directas.</typeparam>
    /// <param name="baseInterfaceTypes">Una lista de tipos de interfaz base que se utilizarán como filtro.</param>
    /// <returns>
    /// Una lista de tipos que representan las interfaces directas implementadas por el tipo especificado.
    /// </returns>
    /// <remarks>
    /// Este método es útil para determinar las interfaces que un tipo específico implementa directamente,
    /// en lugar de aquellas que se heredan a través de una jerarquía de clases.
    /// </remarks>
    /// <seealso cref="GetDirectInterfaceTypes(Type, Type[])"/>
    public static List<Type> GetDirectInterfaceTypes<T>(params Type[] baseInterfaceTypes)
    {
        return GetDirectInterfaceTypes(typeof(T), baseInterfaceTypes);
    }

    /// <summary>
    /// Obtiene una lista de los tipos de interfaz directos implementados por un tipo dado.
    /// </summary>
    /// <param name="type">El tipo del cual se desean obtener las interfaces directas.</param>
    /// <param name="baseInterfaceTypes">Un conjunto opcional de tipos de interfaz base para filtrar los resultados.</param>
    /// <returns>Una lista de tipos de interfaz que son implementaciones directas del tipo especificado.</returns>
    /// <remarks>
    /// Este método excluye las interfaces que son heredadas indirectamente.
    /// Si no se proporcionan tipos de interfaz base, se devolverán todas las interfaces directas.
    /// </remarks>
    /// <seealso cref="GetInterfaceTypes(Type[], Type[])"/>
    public static List<Type> GetDirectInterfaceTypes(Type type, params Type[] baseInterfaceTypes)
    {
        var interfaceTypes = type.GetInterfaces();
        var directInterfaceTypes = interfaceTypes.Except(interfaceTypes.SelectMany(t => t.GetInterfaces())).ToList();
        if (baseInterfaceTypes == null || baseInterfaceTypes.Length == 0)
            return directInterfaceTypes;
        return GetInterfaceTypes(directInterfaceTypes, baseInterfaceTypes);
    }

    /// <summary>
    /// Obtiene una lista de tipos de interfaz que implementan al menos una de las interfaces base especificadas.
    /// </summary>
    /// <param name="interfaceTypes">Una colección de tipos de interfaz a evaluar.</param>
    /// <param name="baseInterfaceTypes">Un arreglo de tipos de interfaz base que se utilizarán como filtro.</param>
    /// <returns>Una lista de tipos de interfaz que cumplen con el criterio de implementación de las interfaces base.</returns>
    /// <remarks>
    /// Este método verifica cada tipo de interfaz en <paramref name="interfaceTypes"/> para determinar si implementa
    /// alguna de las interfaces en <paramref name="baseInterfaceTypes"/>. Si el tipo de interfaz es genérico y no es
    /// una definición de tipo genérico, se agrega su definición de tipo genérico a la lista de resultados.
    /// </remarks>
    private static List<Type> GetInterfaceTypes(IEnumerable<Type> interfaceTypes, Type[] baseInterfaceTypes)
    {
        var result = new List<Type>();
        foreach (var interfaceType in interfaceTypes)
        {
            if (interfaceType.GetInterfaces().Any(baseInterfaceTypes.Contains) == false)
                continue;
            if (interfaceType.IsGenericType && !interfaceType.IsGenericTypeDefinition && interfaceType.FullName == null)
            {
                result.Add(interfaceType.GetGenericTypeDefinition());
                continue;
            }
            result.Add(interfaceType);
        }
        return result;
    }

    #endregion

    #region GetInterfaceTypes(Obtener lista de tipos de interfaz.)

    /// <summary>
    /// Obtiene una lista de tipos de interfaz que implementa el tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo del cual se desean obtener las interfaces.</typeparam>
    /// <param name="baseInterfaceTypes">Un arreglo de tipos de interfaz base que se utilizarán como filtro.</param>
    /// <returns>Una lista de tipos que representan las interfaces implementadas por el tipo especificado.</returns>
    /// <seealso cref="GetInterfaceTypes(Type, Type[])"/>
    public static List<Type> GetInterfaceTypes<T>(params Type[] baseInterfaceTypes)
    {
        return GetInterfaceTypes(typeof(T), baseInterfaceTypes);
    }

    /// <summary>
    /// Obtiene una lista de tipos de interfaz que implementa un tipo específico.
    /// </summary>
    /// <param name="type">El tipo del cual se desean obtener las interfaces.</param>
    /// <param name="baseInterfaceTypes">Opcional. Un conjunto de tipos de interfaz base que se utilizarán como filtro.</param>
    /// <returns>Una lista de tipos de interfaz que implementa el tipo especificado.</returns>
    /// <remarks>
    /// Si no se proporcionan tipos de interfaz base, se devolverán todas las interfaces implementadas por el tipo.
    /// Si se proporcionan tipos de interfaz base, solo se devolverán aquellas interfaces que también sean de los tipos base especificados.
    /// </remarks>
    /// <seealso cref="Type.GetInterfaces"/>
    public static List<Type> GetInterfaceTypes(Type type, params Type[] baseInterfaceTypes)
    {
        var interfaceTypes = type.GetInterfaces();
        if (baseInterfaceTypes == null || baseInterfaceTypes.Length == 0)
            return interfaceTypes.ToList();
        return GetInterfaceTypes(interfaceTypes, baseInterfaceTypes);
    }

    #endregion

    #region IsCollection(¿Es un conjunto?)

    /// <summary>
    /// Determina si el tipo especificado es una colección.
    /// </summary>
    /// <param name="type">El tipo que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el tipo es un arreglo o una colección genérica; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    public static bool IsCollection(Type type)
    {
        if (type.IsArray)
            return true;
        return IsGenericCollection(type);
    }

    #endregion

    #region IsGenericCollection(是否泛型集合)

    /// <summary>
    /// Determina si el tipo especificado es una colección genérica.
    /// </summary>
    /// <param name="type">El tipo que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el tipo es una colección genérica; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el tipo proporcionado es un tipo genérico y si su definición 
    /// de tipo genérico coincide con alguna de las colecciones genéricas comunes como 
    /// <see cref="IEnumerable{T}"/>, <see cref="IReadOnlyCollection{T}"/>, 
    /// <see cref="IReadOnlyList{T}"/>, <see cref="ICollection{T}"/>, 
    /// <see cref="IList{T}"/> o <see cref="List{T}"/>.
    /// </remarks>
    public static bool IsGenericCollection(Type type)
    {
        if (!type.IsGenericType)
            return false;
        var typeDefinition = type.GetGenericTypeDefinition();
        return typeDefinition == typeof(IEnumerable<>)
               || typeDefinition == typeof(IReadOnlyCollection<>)
               || typeDefinition == typeof(IReadOnlyList<>)
               || typeDefinition == typeof(ICollection<>)
               || typeDefinition == typeof(IList<>)
               || typeDefinition == typeof(List<>);
    }

    #endregion

    #region IsBool(¿Es de tipo booleano?)

    /// <summary>
    /// Determina si el miembro especificado es de tipo booleano.
    /// </summary>
    /// <param name="member">El miembro que se va a verificar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el miembro es de tipo booleano; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el miembro es de tipo <see cref="MemberTypes.TypeInfo"/> 
    /// y compara su cadena de representación con "System.Boolean". 
    /// Si el miembro es de tipo <see cref="MemberTypes.Property"/>, 
    /// se llama a otro método para verificar si la propiedad es de tipo booleano.
    /// </remarks>
    public static bool IsBool(MemberInfo member)
    {
        if (member == null)
            return false;
        switch (member.MemberType)
        {
            case MemberTypes.TypeInfo:
                return member.ToString() == "System.Boolean";
            case MemberTypes.Property:
                return IsBool((PropertyInfo)member);
        }
        return false;
    }

    /// <summary>
    /// Determina si la propiedad especificada es de tipo booleano o nullable booleano.
    /// </summary>
    /// <param name="property">La información de la propiedad que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la propiedad es de tipo <see cref="bool"/> o <see cref="bool?"/>; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    private static bool IsBool(PropertyInfo property)
    {
        return property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?);
    }

    #endregion

    #region IsEnum(¿Es un tipo enumerado?)

    /// <summary>
    /// Determina si el miembro especificado es un enumerado.
    /// </summary>
    /// <param name="member">El miembro que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el miembro es un enumerado; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica el tipo de miembro y determina si es un tipo de información que representa un enumerado.
    /// Si el miembro es de tipo <see cref="MemberTypes.TypeInfo"/>, se verifica si es un enumerado.
    /// Si el miembro es de tipo <see cref="MemberTypes.Property"/>, se llama recursivamente a este método para evaluar la propiedad.
    /// </remarks>
    /// <seealso cref="MemberInfo"/>
    /// <seealso cref="TypeInfo"/>
    /// <seealso cref="PropertyInfo"/>
    public static bool IsEnum(MemberInfo member)
    {
        if (member == null)
            return false;
        switch (member.MemberType)
        {
            case MemberTypes.TypeInfo:
                return ((TypeInfo)member).IsEnum;
            case MemberTypes.Property:
                return IsEnum((PropertyInfo)member);
        }
        return false;
    }

    /// <summary>
    /// Determina si la propiedad especificada es de tipo enumeración.
    /// </summary>
    /// <param name="property">La información de la propiedad que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la propiedad es de tipo enumeración; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Esta función verifica si el tipo de la propiedad es una enumeración o si es un tipo anulable que tiene una enumeración como tipo subyacente.
    /// </remarks>
    private static bool IsEnum(PropertyInfo property)
    {
        if (property.PropertyType.GetTypeInfo().IsEnum)
            return true;
        var value = Nullable.GetUnderlyingType(property.PropertyType);
        if (value == null)
            return false;
        return value.GetTypeInfo().IsEnum;
    }

    #endregion

    #region IsDate(¿Es de tipo fecha?)

    /// <summary>
    /// Determina si el miembro especificado es de tipo fecha (DateTime).
    /// </summary>
    /// <param name="member">El miembro que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el miembro es de tipo fecha; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica el tipo del miembro y determina si es un tipo de información de fecha.
    /// Si el miembro es de tipo <see cref="MemberTypes.TypeInfo"/>, se compara su cadena de representación
    /// con "System.DateTime". Si es de tipo <see cref="MemberTypes.Property"/>, se llama a otro método
    /// que se encarga de verificar si la propiedad es de tipo fecha.
    /// </remarks>
    /// <seealso cref="MemberInfo"/>
    /// <seealso cref="PropertyInfo"/>
    public static bool IsDate(MemberInfo member)
    {
        if (member == null)
            return false;
        switch (member.MemberType)
        {
            case MemberTypes.TypeInfo:
                return member.ToString() == "System.DateTime";
            case MemberTypes.Property:
                return IsDate((PropertyInfo)member);
        }
        return false;
    }

    /// <summary>
    /// Determina si la propiedad especificada es de tipo DateTime o DateTime?.
    /// </summary>
    /// <param name="property">La información de la propiedad que se va a evaluar.</param>
    /// <returns>
    /// Devuelve true si la propiedad es de tipo DateTime o DateTime?, de lo contrario, devuelve false.
    /// </returns>
    private static bool IsDate(PropertyInfo property)
    {
        if (property.PropertyType == typeof(DateTime))
            return true;
        if (property.PropertyType == typeof(DateTime?))
            return true;
        return false;
    }

    #endregion

    #region IsInt(¿Es un número entero?)

    /// <summary>
    /// Determina si el miembro especificado es de tipo entero.
    /// </summary>
    /// <param name="member">El miembro que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el miembro es de tipo entero (System.Int32, System.Int16 o System.Int64);
    /// de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica el tipo de miembro y, en caso de que sea una propiedad,
    /// llama a otro método para determinar si la propiedad es de tipo entero.
    /// </remarks>
    public static bool IsInt(MemberInfo member)
    {
        if (member == null)
            return false;
        switch (member.MemberType)
        {
            case MemberTypes.TypeInfo:
                return member.ToString() == "System.Int32" || member.ToString() == "System.Int16" || member.ToString() == "System.Int64";
            case MemberTypes.Property:
                return IsInt((PropertyInfo)member);
        }
        return false;
    }

    /// <summary>
    /// Determina si el tipo de propiedad especificado es un tipo entero.
    /// </summary>
    /// <param name="property">La información de la propiedad a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el tipo de propiedad es un entero (int, short, long) 
    /// o sus versiones anulables (int?, short?, long?); de lo contrario, devuelve <c>false</c>.
    /// </returns>
    private static bool IsInt(PropertyInfo property)
    {
        if (property.PropertyType == typeof(int))
            return true;
        if (property.PropertyType == typeof(int?))
            return true;
        if (property.PropertyType == typeof(short))
            return true;
        if (property.PropertyType == typeof(short?))
            return true;
        if (property.PropertyType == typeof(long))
            return true;
        if (property.PropertyType == typeof(long?))
            return true;
        return false;
    }

    #endregion

    #region IsNumber(¿Es de tipo numérico?)

    /// <summary>
    /// Determina si el miembro especificado es un número.
    /// </summary>
    /// <param name="member">El miembro que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el miembro es un número; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el miembro es de tipo entero o si es un tipo de información que representa un número, 
    /// como <c>Double</c>, <c>Decimal</c> o <c>Single</c>. También puede evaluar propiedades que son números.
    /// </remarks>
    /// <seealso cref="IsInt(MemberInfo)"/>
    /// <seealso cref="IsNumber(PropertyInfo)"/>
    public static bool IsNumber(MemberInfo member)
    {
        if (member == null)
            return false;
        if (IsInt(member))
            return true;
        switch (member.MemberType)
        {
            case MemberTypes.TypeInfo:
                return member.ToString() == "System.Double" || member.ToString() == "System.Decimal" || member.ToString() == "System.Single";
            case MemberTypes.Property:
                return IsNumber((PropertyInfo)member);
        }
        return false;
    }

    /// <summary>
    /// Determina si la propiedad especificada es de un tipo numérico.
    /// </summary>
    /// <param name="property">La propiedad que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la propiedad es de tipo <c>double</c>, <c>double?</c>, <c>decimal</c>, <c>decimal?</c>, <c>float</c> o <c>float?</c>; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    private static bool IsNumber(PropertyInfo property)
    {
        if (property.PropertyType == typeof(double))
            return true;
        if (property.PropertyType == typeof(double?))
            return true;
        if (property.PropertyType == typeof(decimal))
            return true;
        if (property.PropertyType == typeof(decimal?))
            return true;
        if (property.PropertyType == typeof(float))
            return true;
        if (property.PropertyType == typeof(float?))
            return true;
        return false;
    }

    #endregion

    #region GetElementType(Obtener tipo de elemento)

    /// <summary>
    /// Obtiene el tipo de elemento de un tipo dado.
    /// </summary>
    /// <param name="type">El tipo del cual se desea obtener el tipo de elemento.</param>
    /// <returns>El tipo de elemento si el tipo es una colección; de lo contrario, devuelve el tipo original.</returns>
    /// <exception cref="ArgumentException">Se lanza si el tipo genérico no tiene argumentos.</exception>
    /// <remarks>
    /// Este método verifica si el tipo proporcionado es una colección. Si es un arreglo, 
    /// utiliza el método <see cref="Type.GetElementType"/> para obtener el tipo de elemento. 
    /// Si el tipo es un tipo genérico, se obtienen los argumentos genéricos y se devuelve el primero.
    /// </remarks>
    public static Type GetElementType(Type type)
    {
        if (IsCollection(type) == false)
            return type;
        if (type.IsArray)
            return type.GetElementType();
        var genericArgumentsTypes = type.GetTypeInfo().GetGenericArguments();
        if (genericArgumentsTypes == null || genericArgumentsTypes.Length == 0)
            throw new ArgumentException(nameof(genericArgumentsTypes));
        return genericArgumentsTypes[0];
    }

    #endregion

    #region GetTopBaseType(Obtener la clase base superior.)

    /// <summary>
    /// Obtiene el tipo base más alto de un tipo genérico especificado.
    /// </summary>
    /// <typeparam name="T">El tipo genérico del cual se desea obtener el tipo base.</typeparam>
    /// <returns>El tipo base más alto del tipo especificado.</returns>
    /// <remarks>
    /// Este método utiliza la reflexión para determinar la jerarquía de tipos y
    /// devuelve el tipo base más alto en la cadena de herencia.
    /// </remarks>
    /// <seealso cref="GetTopBaseType(Type)"/>
    public static Type GetTopBaseType<T>()
    {
        return GetTopBaseType(typeof(T));
    }

    /// <summary>
    /// Obtiene el tipo base más alto de un tipo dado.
    /// </summary>
    /// <param name="type">El tipo del cual se desea obtener el tipo base más alto.</param>
    /// <returns>El tipo base más alto si existe; de lo contrario, null.</returns>
    /// <remarks>
    /// Este método recorre la jerarquía de herencia del tipo proporcionado.
    /// Si el tipo es una interfaz, se devuelve el mismo tipo.
    /// Si el tipo base es <see cref="object"/>, se devuelve el tipo actual.
    /// De lo contrario, se llama recursivamente al método para obtener el tipo base.
    /// </remarks>
    /// <seealso cref="Type"/>
    public static Type GetTopBaseType(Type type)
    {
        if (type == null)
            return null;
        if (type.IsInterface)
            return type;
        if (type.BaseType == typeof(object))
            return type;
        return GetTopBaseType(type.BaseType);
    }

    #endregion

    #region GetPropertyValue(Obtener el valor de la propiedad.)

    /// <summary>
    /// Obtiene el valor de una propiedad de un objeto dado.
    /// </summary>
    /// <param name="instance">La instancia del objeto del cual se desea obtener el valor de la propiedad.</param>
    /// <param name="propertyName">El nombre de la propiedad cuyo valor se desea obtener.</param>
    /// <returns>
    /// El valor de la propiedad especificada, o null si la instancia es null o si la propiedad no existe.
    /// </returns>
    /// <remarks>
    /// Este método utiliza reflexión para acceder a la propiedad del objeto. 
    /// Si la propiedad no se encuentra, se devuelve null.
    /// </remarks>
    public static object GetPropertyValue(object instance, string propertyName)
    {
        if (instance == null)
            return null;
        var property = instance.GetType().GetProperty(propertyName);
        return property == null ? null : property.GetValue(instance);
    }

    #endregion
}