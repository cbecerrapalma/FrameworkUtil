using Util.Helpers;
using Util.Properties;

namespace Util;

/// <summary>
/// Proporciona métodos de extensión para la estructura <see cref="DateTime"/>.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Convierte un objeto <see cref="DateTime"/> en una representación de cadena de fecha y hora.
    /// </summary>
    /// <param name="dateTime">El objeto <see cref="DateTime"/> que se desea convertir.</param>
    /// <param name="removeSecond">Indica si se deben omitir los segundos en la representación de cadena. Por defecto es <c>false</c>.</param>
    /// <returns>
    /// Una cadena que representa la fecha y hora en el formato especificado.
    /// Si <paramref name="removeSecond"/> es <c>true</c>, el formato será "yyyy-MM-dd HH:mm"; 
    /// de lo contrario, será "yyyy-MM-dd HH:mm:ss".
    /// </returns>
    /// <remarks>
    /// Este método utiliza la función <see cref="GetLocalDateTime(DateTime)"/> para obtener la fecha y hora local 
    /// antes de realizar la conversión a cadena.
    /// </remarks>
    public static string ToDateTimeString(this DateTime dateTime, bool removeSecond = false)
    {
        dateTime = GetLocalDateTime(dateTime);
        if (removeSecond)
            return dateTime.ToString("yyyy-MM-dd HH:mm");
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

    /// <summary>
    /// Convierte un objeto <see cref="DateTime"/> a la hora local.
    /// </summary>
    /// <param name="dateTime">El objeto <see cref="DateTime"/> que se desea convertir a la hora local.</param>
    /// <returns>
    /// Un objeto <see cref="DateTime"/> que representa la hora local correspondiente al <paramref name="dateTime"/> proporcionado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la configuración de zona horaria del sistema para realizar la conversión.
    /// Asegúrese de que el <paramref name="dateTime"/> esté en una zona horaria válida antes de llamar a este método.
    /// </remarks>
    private static DateTime GetLocalDateTime(DateTime dateTime)
    {
        return Time.ToLocalTime(dateTime);
    }

    /// <summary>
    /// Convierte un objeto <see cref="DateTime?"/> en una representación de cadena de fecha y hora.
    /// </summary>
    /// <param name="dateTime">El objeto <see cref="DateTime?"/> que se va a convertir.</param>
    /// <param name="removeSecond">Indica si se debe eliminar la parte de los segundos de la cadena resultante. El valor predeterminado es <c>false</c>.</param>
    /// <returns>Una cadena que representa la fecha y hora, o una cadena vacía si <paramref name="dateTime"/> es <c>null</c>.</returns>
    /// <seealso cref="ToDateTimeString(DateTime, bool)"/>
    public static string ToDateTimeString(this DateTime? dateTime, bool removeSecond = false)
    {
        if (dateTime == null)
            return string.Empty;
        return ToDateTimeString(dateTime.Value, removeSecond);
    }

    /// <summary>
    /// Convierte un objeto <see cref="DateTime"/> en una representación de cadena de fecha en formato "yyyy-MM-dd".
    /// </summary>
    /// <param name="dateTime">El objeto <see cref="DateTime"/> que se desea convertir.</param>
    /// <returns>Una cadena que representa la fecha en el formato especificado.</returns>
    /// <remarks>
    /// Este método primero ajusta el <paramref name="dateTime"/> a la hora local antes de realizar la conversión a cadena.
    /// </remarks>
    /// <seealso cref="GetLocalDateTime(DateTime)"/>
    public static string ToDateString(this DateTime dateTime)
    {
        dateTime = GetLocalDateTime(dateTime);
        return dateTime.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// Convierte un objeto <see cref="DateTime?"/> en una representación de cadena de fecha.
    /// </summary>
    /// <param name="dateTime">El objeto <see cref="DateTime?"/> que se va a convertir. Si es nulo, se devolverá una cadena vacía.</param>
    /// <returns>
    /// Una cadena que representa la fecha en formato de cadena. Si <paramref name="dateTime"/> es nulo, se devuelve una cadena vacía.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión para objetos de tipo <see cref="DateTime?"/> y permite manejar fechas que pueden ser nulas.
    /// </remarks>
    public static string ToDateString(this DateTime? dateTime)
    {
        if (dateTime == null)
            return string.Empty;
        return ToDateString(dateTime.Value);
    }

    /// <summary>
    /// Convierte un objeto <see cref="DateTime"/> en una representación de cadena de tiempo en formato "HH:mm:ss".
    /// </summary>
    /// <param name="dateTime">El objeto <see cref="DateTime"/> que se desea convertir.</param>
    /// <returns>
    /// Una cadena que representa la hora en formato "HH:mm:ss".
    /// </returns>
    /// <remarks>
    /// Este método ajusta primero el <paramref name="dateTime"/> a la hora local antes de realizar la conversión a cadena.
    /// </remarks>
    /// <seealso cref="GetLocalDateTime(DateTime)"/>
    public static string ToTimeString(this DateTime dateTime)
    {
        dateTime = GetLocalDateTime(dateTime);
        return dateTime.ToString("HH:mm:ss");
    }

    /// <summary>
    /// Convierte un objeto <see cref="DateTime?"/> en una representación de cadena de tiempo.
    /// </summary>
    /// <param name="dateTime">El objeto <see cref="DateTime?"/> que se desea convertir. Puede ser nulo.</param>
    /// <returns>
    /// Una cadena que representa la parte de tiempo del objeto <see cref="DateTime?"/>. 
    /// Devuelve una cadena vacía si el objeto es nulo.
    /// </returns>
    public static string ToTimeString(this DateTime? dateTime)
    {
        if (dateTime == null)
            return string.Empty;
        return ToTimeString(dateTime.Value);
    }

    /// <summary>
    /// Convierte un objeto <see cref="DateTime"/> a una representación de cadena en milisegundos.
    /// </summary>
    /// <param name="dateTime">El objeto <see cref="DateTime"/> que se desea convertir.</param>
    /// <returns>
    /// Una cadena que representa la fecha y hora en el formato "yyyy-MM-dd HH:mm:ss.fff".
    /// </returns>
    /// <remarks>
    /// Este método ajusta primero la fecha y hora a la hora local antes de formatearla.
    /// </remarks>
    public static string ToMillisecondString(this DateTime dateTime)
    {
        dateTime = GetLocalDateTime(dateTime);
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
    }

    /// <summary>
    /// Convierte un objeto <see cref="DateTime?"/> en una representación de cadena en milisegundos.
    /// </summary>
    /// <param name="dateTime">El objeto <see cref="DateTime?"/> que se desea convertir. Si es null, se devolverá una cadena vacía.</param>
    /// <returns>
    /// Una cadena que representa el valor en milisegundos del objeto <see cref="DateTime?"/>. 
    /// Si el parámetro es null, se devuelve una cadena vacía.
    /// </returns>
    public static string ToMillisecondString(this DateTime? dateTime)
    {
        if (dateTime == null)
            return string.Empty;
        return ToMillisecondString(dateTime.Value);
    }

    /// <summary>
    /// Convierte un objeto <see cref="DateTime"/> en una cadena que representa la fecha en formato chino.
    /// </summary>
    /// <param name="dateTime">La fecha y hora que se desea convertir.</param>
    /// <returns>
    /// Una cadena que representa la fecha en formato chino, en el formato "Año年Mes月Día日".
    /// </returns>
    /// <remarks>
    /// Este método utiliza la función <see cref="GetLocalDateTime(DateTime)"/> para obtener la fecha y hora local antes de realizar la conversión.
    /// </remarks>
    public static string ToChineseDateString(this DateTime dateTime)
    {
        dateTime = GetLocalDateTime(dateTime);
        return $"año {dateTime.Year} mes {dateTime.Month} día {dateTime.Day}";
    }

    /// <summary>
    /// Convierte un objeto <see cref="DateTime?"/> en una representación de fecha en chino.
    /// </summary>
    /// <param name="dateTime">La fecha que se desea convertir. Puede ser nula.</param>
    /// <returns>
    /// Una cadena que representa la fecha en formato chino. 
    /// Devuelve una cadena vacía si <paramref name="dateTime"/> es nulo.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión para objetos de tipo <see cref="DateTime?"/>.
    /// Si el valor proporcionado es nulo, se devuelve una cadena vacía.
    /// De lo contrario, se llama a otro método para realizar la conversión.
    /// </remarks>
    public static string ToChineseDateString(this DateTime? dateTime)
    {
        if (dateTime == null)
            return string.Empty;
        return ToChineseDateString(dateTime.SafeValue());
    }

    /// <summary>
    /// Convierte un objeto <see cref="DateTime"/> en una cadena que representa la fecha y hora en formato chino.
    /// </summary>
    /// <param name="dateTime">La fecha y hora que se desea convertir.</param>
    /// <param name="removeSecond">Indica si se debe omitir los segundos en la cadena resultante. El valor predeterminado es <c>false</c>.</param>
    /// <returns>
    /// Una cadena que representa la fecha y hora en formato chino, por ejemplo, "2023年10月5日 14时30分45秒".
    /// Si <paramref name="removeSecond"/> es <c>true</c>, los segundos no se incluirán en la cadena.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la función <see cref="GetLocalDateTime(DateTime)"/> para obtener la fecha y hora local antes de formatearla.
    /// </remarks>
    public static string ToChineseDateTimeString(this DateTime dateTime, bool removeSecond = false)
    {
        dateTime = GetLocalDateTime(dateTime);
        StringBuilder result = new StringBuilder();
        result.AppendFormat("año {0} mes {1} día {2} ", dateTime.Year, dateTime.Month, dateTime.Day);
        result.AppendFormat(" {0} horas {1} minutos", dateTime.Hour, dateTime.Minute);
        if (removeSecond == false)
            result.AppendFormat("{0} segundos", dateTime.Second);
        return result.ToString();
    }

    /// <summary>
    /// Convierte un objeto <see cref="DateTime?"/> en una representación de fecha y hora en chino.
    /// </summary>
    /// <param name="dateTime">La fecha y hora a convertir. Puede ser nulo.</param>
    /// <param name="removeSecond">Indica si se deben omitir los segundos en la representación resultante. El valor predeterminado es <c>false</c>.</param>
    /// <returns>
    /// Una cadena que representa la fecha y hora en formato chino. Si <paramref name="dateTime"/> es nulo, se devuelve una cadena vacía.
    /// </returns>
    /// <seealso cref="ToChineseDateTimeString(DateTime, bool)"/>
    public static string ToChineseDateTimeString(this DateTime? dateTime, bool removeSecond = false)
    {
        if (dateTime == null)
            return string.Empty;
        return ToChineseDateTimeString(dateTime.Value, removeSecond);
    }

    /// <summary>
    /// Genera una representación en forma de cadena de un objeto <see cref="TimeSpan"/>.
    /// </summary>
    /// <param name="timeSpan">El objeto <see cref="TimeSpan"/> que se va a describir.</param>
    /// <returns>
    /// Una cadena que representa la duración del <see cref="TimeSpan"/> en días, horas, minutos, segundos y milisegundos.
    /// Si todos los componentes son cero, se devuelve el total de milisegundos.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión de la estructura <see cref="TimeSpan"/> y permite obtener una descripción legible
    /// de la duración representada por el objeto <see cref="TimeSpan"/>. Se incluyen solo los componentes que son mayores que cero.
    /// </remarks>
    /// <seealso cref="TimeSpan"/>
    public static string Description(this TimeSpan timeSpan)
    {
        StringBuilder result = new StringBuilder();
        if (timeSpan.Days > 0)
        {
            result.Append(timeSpan.Days);
            result.Append(R.Days);
        }
        if (timeSpan.Hours > 0)
        {
            result.Append(timeSpan.Hours);
            result.Append(R.Hours);
        }
        if (timeSpan.Minutes > 0)
        {
            result.Append(timeSpan.Minutes);
            result.Append(R.Minutes);
        }
        if (timeSpan.Seconds > 0)
        {
            result.Append(timeSpan.Seconds);
            result.Append(R.Seconds);
        }
        if (timeSpan.Milliseconds > 0)
        {
            result.Append(timeSpan.Milliseconds);
            result.Append(R.Milliseconds);
        }
        if (result.Length > 0)
            return result.ToString();
        return $"{timeSpan.TotalMilliseconds}{R.Milliseconds}";
    }
}