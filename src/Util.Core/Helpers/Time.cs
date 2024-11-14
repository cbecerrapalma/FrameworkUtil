using Util.Dates;

namespace Util.Helpers; 

/// <summary>
/// Proporciona métodos estáticos para trabajar con el tiempo.
/// </summary>
public static class Time {
    private static readonly AsyncLocal<DateTime?> _dateTime = new();
    private static readonly AsyncLocal<bool?> _isUseUtc = new();
    /// <summary>
    /// Obtiene un valor que indica si se debe utilizar la hora UTC.
    /// </summary>
    /// <remarks>
    /// Este valor se determina a partir de la propiedad <c>_isUseUtc</</c> si está disponible,
    /// de lo contrario, se utiliza la opción predeterminada de <c>TimeOptions.IsUseUtc</c>.
    /// </remarks>
    /// <returns>
    /// <c>true</c> si se debe utilizar la hora UTC; de lo contrario, <c>false</c>.
    /// </returns>
    private static bool IsUseUtc => _isUseUtc.Value != null ? _isUseUtc.Value.SafeValue() : TimeOptions.IsUseUtc;

    /// <summary>
    /// Establece el valor de la fecha y hora.
    /// </summary>
    /// <param name="dateTime">El valor de fecha y hora a establecer. Puede ser nulo.</param>
    /// <remarks>
    /// Este método asigna el valor proporcionado al campo estático _dateTime. 
    /// Si el valor es nulo, se asignará un valor nulo a _dateTime.
    /// </remarks>
    public static void SetTime( DateTime? dateTime ) {
        _dateTime.Value = dateTime;
    }

    /// <summary>
    /// Establece la hora a partir de una cadena de texto que representa una fecha y hora.
    /// </summary>
    /// <param name="dateTime">Una cadena que representa la fecha y hora que se desea establecer.</param>
    /// <remarks>
    /// Este método convierte la cadena proporcionada en un objeto DateTime.
    /// Si la conversión falla, se lanzará una excepción.
    /// </remarks>
    /// <seealso cref="Convert.ToDateTimeOrNull(string)"/>
    public static void SetTime( string dateTime ) {
        SetTime( Convert.ToDateTimeOrNull( dateTime ) );
    }

    /// <summary>
    /// Establece si se debe utilizar la hora UTC.
    /// </summary>
    /// <param name="isUseUtc">Un valor booleano que indica si se debe utilizar la hora UTC. Si es null, se establece en true por defecto.</param>
    public static void UseUtc( bool? isUseUtc = true ) {
        _isUseUtc.Value = isUseUtc;
    }

    /// <summary>
    /// Restablece los valores de las variables estáticas <c>_dateTime</c> y <c>_isUseUtc</c> a <c>null</c>.
    /// </summary>
    /// <remarks>
    /// Este método se utiliza para reiniciar el estado de las variables estáticas, 
    /// permitiendo que se vuelvan a establecer en futuras operaciones.
    /// </remarks>
    public static void Reset() {
        _dateTime.Value = null;
        _isUseUtc.Value = null;
    }

    /// <summary>
    /// Obtiene la fecha y hora actual.
    /// </summary>
    /// <remarks>
    /// Este miembro está diseñado para devolver la fecha y hora actual, ya sea en formato UTC o local,
    /// dependiendo del valor de la propiedad <see cref="IsUseUtc"/>. Si se ha establecido un valor
    /// para <c>_dateTime</c>, se devolverá ese valor en su lugar.
    /// </remarks>
    /// <returns>
    /// Un objeto <see cref="DateTime"/> que representa la fecha y hora actual.
    /// </returns>
    public static DateTime Now {
        get {
            if ( _dateTime.Value != null )
                return _dateTime.Value.Value;
            return IsUseUtc ? DateTime.UtcNow : DateTime.Now;
        }
    }


    /// <summary>
    /// Normaliza un objeto <see cref="DateTime"/> nullable.
    /// </summary>
    /// <param name="date">La fecha a normalizar. Puede ser null.</param>
    /// <returns>
    /// Un objeto <see cref="DateTime"/> normalizado si <paramref name="date"/> no es null; de lo contrario, retorna null.
    /// </returns>
    /// <remarks>
    /// Este método verifica si la fecha proporcionada es null. Si es así, devuelve null. 
    /// Si no es null, llama a otra sobrecarga del método <see cref="Normalize(DateTime)"/> 
    /// para realizar la normalización.
    /// </remarks>
    public static DateTime? Normalize( DateTime? date ) {
        if( date == null )
            return null;
        return Normalize( date.Value );
    }

    /// <summary>
    /// Normaliza la fecha proporcionada a la hora correspondiente, ya sea en tiempo universal o local.
    /// </summary>
    /// <param name="date">La fecha que se desea normalizar.</param>
    /// <returns>
    /// La fecha normalizada en formato <see cref="DateTime"/>. 
    /// Si <see cref="IsUseUtc"/> es verdadero, se devuelve la fecha en tiempo universal; de lo contrario, se devuelve en tiempo local.
    /// </returns>
    /// <remarks>
    /// Este método es útil para asegurar que las fechas se manejen de manera consistente en función de la configuración de tiempo utilizada.
    /// </remarks>
    /// <seealso cref="IsUseUtc"/>
    /// <seealso cref="ToUniversalTime(DateTime)"/>
    /// <seealso cref="ToLocalTime(DateTime)"/>
    public static DateTime Normalize( DateTime date ) {
        if ( IsUseUtc )
            return ToUniversalTime( date );
        return ToLocalTime( date );
    }

    /// <summary>
    /// Convierte un objeto <see cref="DateTime"/> a tiempo universal coordinado (UTC).
    /// </summary>
    /// <param name="date">La fecha y hora que se desea convertir a UTC.</param>
    /// <returns>
    /// Un objeto <see cref="DateTime"/> que representa la fecha y hora en UTC.
    /// Si la fecha proporcionada es <see cref="DateTime.MinValue"/>, se devuelve <see cref="DateTime.MinValue"/>.
    /// </returns>
    /// <remarks>
    /// Este método maneja diferentes tipos de <see cref="DateTimeKind"/>:
    /// <list type="bullet">
    /// <item>
    /// <description><see cref="DateTimeKind.Local"/>: Se convierte a UTC utilizando <see cref="DateTime.ToUniversalTime"/>.</description>
    /// </item>
    /// <item>
    /// <description><see cref="DateTimeKind.Unspecified"/>: Se trata como si fuera local y luego se convierte a UTC.</description>
    /// </item>
    /// <item>
    /// <description><see cref="DateTimeKind.Utc"/>: Se devuelve la fecha sin cambios.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public static DateTime ToUniversalTime( DateTime date ) {
        if ( date == DateTime.MinValue )
            return DateTime.MinValue;
        switch ( date.Kind ) {
            case DateTimeKind.Local:
                return date.ToUniversalTime();
            case DateTimeKind.Unspecified:
                return DateTime.SpecifyKind( date, DateTimeKind.Local ).ToUniversalTime();
            default:
                return date;
        }
    }

    /// <summary>
    /// Convierte un objeto <see cref="DateTime"/> a la hora local.
    /// </summary>
    /// <param name="date">La fecha y hora que se desea convertir a la hora local.</param>
    /// <returns>
    /// Un objeto <see cref="DateTime"/> que representa la fecha y hora en la zona horaria local.
    /// Si el valor de entrada es <see cref="DateTime.MinValue"/>, se devolverá <see cref="DateTime.MinValue"/>.
    /// </returns>
    /// <remarks>
    /// Este método maneja diferentes tipos de <see cref="DateTimeKind"/>:
    /// <list type="bullet">
    /// <item>
    /// <description><see cref="DateTimeKind.Utc"/>: Se convierte a la hora local utilizando <see cref="DateTime.ToLocalTime"/>.</description>
    /// </item>
    /// <item>
    /// <description><see cref="DateTimeKind.Unspecified"/>: Se especifica que la fecha es de tipo local.</description>
    /// </item>
    /// <item>
    /// <description>Para otros tipos, se devuelve el valor original sin cambios.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public static DateTime ToLocalTime( DateTime date ) {
        if ( date == DateTime.MinValue )
            return DateTime.MinValue;
        switch ( date.Kind ) {
            case DateTimeKind.Utc:
                return date.ToLocalTime();
            case DateTimeKind.Unspecified:
                return DateTime.SpecifyKind( date, DateTimeKind.Local );
            default:
                return date;
        }
    }

    /// <summary>
    /// Convierte una fecha y hora en formato UTC a la hora local.
    /// </summary>
    /// <param name="date">La fecha y hora en formato UTC que se desea convertir.</param>
    /// <returns>La fecha y hora convertida a la hora local. Si la fecha es <see cref="DateTime.MinValue"/>, se devuelve <see cref="DateTime.MinValue"/>.</returns>
    /// <remarks>
    /// Este método verifica el tipo de la fecha proporcionada. Si la fecha es de tipo UTC, se convierte a la hora local.
    /// Si la fecha ya es de tipo local, se devuelve tal cual. Si la fecha no tiene un tipo especificado y la opción 
    /// <c>IsUseUtc</c> está habilitada, se trata como UTC antes de la conversión. Si ninguna de estas condiciones se cumple, 
    /// se devuelve la fecha original.
    /// </remarks>
    public static DateTime UtcToLocalTime( DateTime date ) {
        if ( date == DateTime.MinValue )
            return DateTime.MinValue;
        if( date.Kind == DateTimeKind.Utc )
            return date.ToLocalTime();
        if ( date.Kind == DateTimeKind.Local )
            return date;
        if ( IsUseUtc )
            return DateTime.SpecifyKind( date, DateTimeKind.Utc ).ToLocalTime();
        return date;
    }

    /// <summary>
    /// Obtiene la marca de tiempo Unix actual.
    /// </summary>
    /// <returns>
    /// Un valor de tipo <see cref="long"/> que representa la cantidad de segundos transcurridos desde el 1 de enero de 1970 a las 00:00:00 UTC hasta la fecha y hora actuales.
    /// </returns>
    public static long GetUnixTimestamp() {
        return GetUnixTimestamp( DateTime.Now );
    }

    /// <summary>
    /// Obtiene el timestamp Unix correspondiente a una fecha y hora especificadas.
    /// </summary>
    /// <param name="time">La fecha y hora para la cual se desea obtener el timestamp Unix.</param>
    /// <returns>El timestamp Unix como un valor de tipo <see cref="long"/>.</returns>
    /// <remarks>
    /// El timestamp Unix es el número de segundos que han transcurrido desde el 1 de enero de 1970 a las 00:00:00 UTC.
    /// Esta función considera la zona horaria local y ajusta la fecha y hora en consecuencia.
    /// </remarks>
    public static long GetUnixTimestamp( DateTime time ) {
        var start = TimeZoneInfo.ConvertTime( new DateTime( 1970, 1, 1 ), TimeZoneInfo.Local );
        long ticks = ( time - start.Add( new TimeSpan( 8, 0, 0 ) ) ).Ticks;
        return Util.Helpers.Convert.ToLong( ticks / TimeSpan.TicksPerSecond );
    }

    /// <summary>
    /// Convierte un timestamp de Unix a un objeto DateTime en la zona horaria local.
    /// </summary>
    /// <param name="timestamp">El timestamp de Unix que se desea convertir. Debe ser un valor en segundos desde el 1 de enero de 1970.</param>
    /// <returns>Un objeto <see cref="DateTime"/> que representa la fecha y hora correspondiente al timestamp proporcionado en la zona horaria local, ajustado por 8 horas.</returns>
    /// <remarks>
    /// El método asume que el timestamp está en segundos y lo convierte a nanosegundos para crear un <see cref="TimeSpan"/>.
    /// Luego, suma este <see cref="TimeSpan"/> a la fecha de inicio del 1 de enero de 1970, ajustando además por 8 horas.
    /// </remarks>
    public static DateTime GetTimeFromUnixTimestamp( long timestamp ) {
        var start = TimeZoneInfo.ConvertTime( new DateTime( 1970, 1, 1 ), TimeZoneInfo.Local );
        TimeSpan span = new TimeSpan( long.Parse( timestamp + "0000000" ) );
        return start.Add( span ).Add( new TimeSpan( 8, 0, 0 ) );
    }
}