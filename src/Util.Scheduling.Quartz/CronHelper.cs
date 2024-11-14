namespace Util.Scheduling; 

/// <summary>
/// Proporciona métodos de utilidad para trabajar con expresiones cron.
/// </summary>
public static class CronHelper {
    /// <summary>
    /// Genera una expresión cron que representa una tarea que se ejecuta cada minuto.
    /// </summary>
    /// <param name="second">El segundo en el que se debe ejecutar la tarea. Debe ser un valor entre 0 y 59.</param>
    /// <returns>
    /// Una cadena que representa la expresión cron en formato "segundo * * * * ? *".
    /// </returns>
    /// <remarks>
    /// Esta función es útil para programar tareas que necesitan ejecutarse en un intervalo de un minuto,
    /// especificando el segundo exacto en el que debe comenzar la ejecución.
    /// </remarks>
    public static string Minutely( int second ) {
        return $"{second} * * * * ? *";
    }

    /// <summary>
    /// Genera una expresión cron que representa un horario específico basado en los minutos y segundos proporcionados.
    /// </summary>
    /// <param name="minute">El valor de los minutos en el rango de 0 a 59.</param>
    /// <param name="second">El valor de los segundos en el rango de 0 a 59.</param>
    /// <returns>
    /// Una cadena que representa la expresión cron generada en el formato "segundos minutos * * * ? *".
    /// </returns>
    public static string Hourly( int minute, int second ) {
        return $"{second} {minute} * * * ? *";
    }

    /// <summary>
    /// Genera una expresión cron que representa un horario diario basado en la hora, minuto y segundo proporcionados.
    /// </summary>
    /// <param name="hour">La hora en formato de 24 horas (0-23) que se utilizará en la expresión cron.</param>
    /// <param name="minute">El minuto (0-59) que se utilizará en la expresión cron.</param>
    /// <param name="second">El segundo (0-59) que se utilizará en la expresión cron.</param>
    /// <returns>
    /// Una cadena que representa la expresión cron en el formato "segundo minuto hora * * ? *".
    /// </returns>
    public static string Daily( int hour, int minute, int second ) {
        return $"{second} {minute} {hour} * * ? *";
    }

    /// <summary>
    /// Genera una expresión cron semanal basada en el día de la semana y la hora especificada.
    /// </summary>
    /// <param name="dayOfWeek">El día de la semana para el cual se generará la expresión cron.</param>
    /// <param name="hour">La hora en formato de 24 horas (0-23) para la ejecución programada.</param>
    /// <param name="minute">El minuto (0-59) para la ejecución programada.</param>
    /// <param name="second">El segundo (0-59) para la ejecución programada.</param>
    /// <returns>
    /// Una cadena que representa la expresión cron semanal en formato específico.
    /// </returns>
    /// <remarks>
    /// La expresión cron generada tiene el formato "segundo minuto hora ? * díaDeLaSemana *".
    /// El carácter '?' se utiliza para indicar que no se especifica un valor para el día del mes.
    /// </remarks>
    /// <seealso cref="DayOfWeek"/>
    public static string Weekly( DayOfWeek dayOfWeek, int hour, int minute, int second ) {
        return $"{second} {minute} {hour} ? * {GetDayOfWeek(dayOfWeek)} *";
    }

    /// <summary>
    /// Obtiene la representación en formato abreviado del día de la semana.
    /// </summary>
    /// <param name="dayOfWeek">El día de la semana para el cual se desea obtener la representación abreviada.</param>
    /// <returns>
    /// Una cadena que representa el día de la semana en formato abreviado. 
    /// Retorna <c>null</c> si el día proporcionado no es válido.
    /// </returns>
    private static string GetDayOfWeek( DayOfWeek dayOfWeek ) {
        switch ( dayOfWeek ) {
            case DayOfWeek.Monday:
                return "MON";
            case DayOfWeek.Tuesday:
                return "TUE";
            case DayOfWeek.Wednesday:
                return "WED";
            case DayOfWeek.Thursday:
                return "THUR";
            case DayOfWeek.Friday:
                return "FRI";
            case DayOfWeek.Saturday:
                return "SAT";
            case DayOfWeek.Sunday:
                return "SUN";
        }
        return null;
    }

    /// <summary>
    /// Genera una expresión cron mensual en formato de cadena.
    /// </summary>
    /// <param name="day">El día del mes en el que se ejecutará la tarea.</param>
    /// <param name="hour">La hora del día en formato de 24 horas.</param>
    /// <param name="minute">El minuto de la hora en el que se ejecutará la tarea.</param>
    /// <param name="second">El segundo del minuto en el que se ejecutará la tarea.</param>
    /// <returns>
    /// Una cadena que representa la expresión cron mensual en el formato "segundo minuto hora día * ? *".
    /// </returns>
    public static string Monthly( int day, int hour, int minute, int second ) {
        return $"{second} {minute} {hour} {day} * ? *";
    }

    /// <summary>
    /// Genera una expresión cron que representa un evento programado anualmente en una fecha y hora específicas.
    /// </summary>
    /// <param name="month">El mes en el que se debe ejecutar el evento (1-12).</param>
    /// <param name="day">El día del mes en el que se debe ejecutar el evento (1-31).</param>
    /// <param name="hour">La hora del día en la que se debe ejecutar el evento (0-23).</param>
    /// <param name="minute">El minuto de la hora en la que se debe ejecutar el evento (0-59).</param>
    /// <param name="second">El segundo del minuto en el que se debe ejecutar el evento (0-59).</param>
    /// <returns>
    /// Una cadena que representa la expresión cron para el evento programado anualmente.
    /// </returns>
    public static string Yearly( int month, int day, int hour, int minute, int second ) {
        return $"{second} {minute} {hour} {day} {month} ? *";
    }
}