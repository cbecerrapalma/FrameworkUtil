using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RazorEngineCore; 

/// <summary>
/// Proporciona métodos de extensión para objetos.
/// </summary>
/// <remarks>
/// Esta clase contiene métodos que permiten extender la funcionalidad de los objetos en C#,
/// facilitando operaciones comunes y mejorando la legibilidad del código.
/// </remarks>
public static class ObjectExtenders
{
    /// <summary>
    /// Convierte un objeto en un <see cref="ExpandoObject"/>.
    /// </summary>
    /// <param name="obj">El objeto que se desea convertir a <see cref="ExpandoObject"/>.</param>
    /// <returns>Un <see cref="ExpandoObject"/> que representa el objeto original, donde las propiedades son claves y sus valores son los valores de las propiedades del objeto.</returns>
    /// <remarks>
    /// Este método utiliza reflexión para obtener las propiedades del objeto y sus valores,
    /// y los agrega a un <see cref="ExpandoObject"/> que permite un acceso dinámico a sus propiedades.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="obj"/> es null.</exception>
    /// <seealso cref="ExpandoObject"/>
    public static ExpandoObject ToExpando(this object obj)
    {
        ExpandoObject expando = new ExpandoObject();
        IDictionary<string, object> dictionary = expando;

        foreach (var property in obj.GetType().GetProperties())
        {
            dictionary.Add(property.Name, property.GetValue(obj));
        }

        return expando;
    }

    /// <summary>
    /// Determina si el objeto especificado es un tipo anónimo.
    /// </summary>
    /// <param name="obj">El objeto que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el objeto es un tipo anónimo; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el tipo del objeto tiene el atributo <see cref="CompilerGeneratedAttribute"/> y si cumple con las características típicas de un tipo anónimo,
    /// como ser un tipo genérico, contener "AnonymousType" en su nombre, comenzar con "<>" o "VB$", y tener atributos de tipo no público.
    /// </remarks>
    public static bool IsAnonymous(this object obj)
    {
        Type type = obj.GetType();

        return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
               && type.IsGenericType && type.Name.Contains("AnonymousType")
               && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
               && type.Attributes.HasFlag(TypeAttributes.NotPublic);
    }
}