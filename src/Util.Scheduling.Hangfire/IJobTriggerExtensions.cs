namespace Util.Scheduling; 

/// <summary>
/// Proporciona métodos de extensión para la interfaz <see cref="IJobTrigger"/>.
/// </summary>
public static class IJobTriggerExtensions {
    /// <summary>
    /// Establece un retraso en el desencadenador de trabajo especificado.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo al que se le aplicará el retraso.</param>
    /// <param name="value">El tiempo de retraso que se aplicará al desencadenador.</param>
    /// <returns>
    /// El desencadenador de trabajo modificado con el retraso aplicado.
    /// </returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de los desencadenadores de trabajo, permitiendo
    /// que se establezca un retraso antes de que se ejecute el trabajo asociado.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <seealso cref="IJobTrigger"/>
    /// <seealso cref="HangfireTrigger"/>
    public static IJobTrigger Delay( this IJobTrigger source, TimeSpan value ) {
        source.CheckNull( nameof( source ) );
        if ( source is HangfireTrigger trigger )
            trigger.Delay = value;
        return source;
    }

    /// <summary>
    /// Establece una expresión cron para el desencadenador de trabajos especificado.
    /// </summary>
    /// <param name="source">El desencadenador de trabajos al que se le asignará la expresión cron.</param>
    /// <param name="cron">La expresión cron que define la programación del trabajo.</param>
    /// <returns>El desencadenador de trabajos actualizado con la nueva expresión cron.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método es una extensión para la interfaz <see cref="IJobTrigger"/> y permite configurar
    /// la programación de un trabajo utilizando una expresión cron. Si el desencadenador es de tipo
    /// <see cref="HangfireTrigger"/>, se actualizará su propiedad <c>Cron</c> con el valor proporcionado.
    /// </remarks>
    public static IJobTrigger Cron( this IJobTrigger source, string cron ) {
        source.CheckNull( nameof( source ) );
        if ( source is HangfireTrigger trigger ) {
            trigger.Cron = cron;
        }
        return source;
    }

    /// <summary>
    /// Obtiene el retraso asociado a un disparador de trabajo.
    /// </summary>
    /// <param name="source">El disparador de trabajo del cual se desea obtener el retraso.</param>
    /// <returns>
    /// Un <see cref="TimeSpan?"/> que representa el retraso del disparador, 
    /// o null si el disparador no es del tipo <see cref="HangfireTrigger"/>.
    /// </returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de la interfaz <see cref="IJobTrigger"/> 
    /// para permitir la obtención del retraso específico de un disparador de tipo <see cref="HangfireTrigger"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <seealso cref="IJobTrigger"/>
    /// <seealso cref="HangfireTrigger"/>
    public static TimeSpan? GetDelay( this IJobTrigger source ) {
        source.CheckNull( nameof( source ) );
        if ( source is HangfireTrigger trigger )
            return trigger.Delay;
        return null;
    }

    /// <summary>
    /// Obtiene la expresión cron asociada a un disparador de trabajo.
    /// </summary>
    /// <param name="source">El disparador de trabajo del cual se desea obtener la expresión cron.</param>
    /// <returns>
    /// La expresión cron como una cadena si el disparador es de tipo <see cref="HangfireTrigger"/>; de lo contrario, devuelve <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de la interfaz <see cref="IJobTrigger"/> para proporcionar una forma sencilla de acceder a la expresión cron
    /// cuando el disparador es de tipo <see cref="HangfireTrigger"/>.
    /// </remarks>
    public static string GetCron( this IJobTrigger source ) {
        source.CheckNull( nameof( source ) );
        if ( source is HangfireTrigger trigger )
            return trigger.Cron;
        return null;
    }

    /// <summary>
    /// Configura un disparador para que se ejecute cada minuto.
    /// </summary>
    /// <param name="source">El disparador que se va a configurar.</param>
    /// <returns>El disparador configurado para ejecutarse cada minuto.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de <see cref="IJobTrigger"/> 
    /// para permitir la configuración de un disparador específico de Hangfire 
    /// que se ejecuta cada minuto utilizando una expresión cron adecuada.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    /// <seealso cref="HangfireTrigger"/>
    /// <seealso cref="Hangfire.Cron.Minutely"/>
    public static IJobTrigger Minutely( this IJobTrigger source ) {
        source.CheckNull( nameof( source ) );
        if ( source is HangfireTrigger trigger )
            trigger.Cron = Hangfire.Cron.Minutely();
        return source;
    }

    /// <summary>
    /// Crea un desencadenador que se activa cada hora.
    /// </summary>
    /// <param name="source">El desencadenador original que se está extendiendo.</param>
    /// <returns>Un nuevo desencadenador que se activa cada hora.</returns>
    public static IJobTrigger Hourly( this IJobTrigger source ) {
        return source.Hourly( 0 );
    }

    /// <summary>
    /// Configura un desencadenador de trabajo para que se ejecute de manera horaria en un minuto específico.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo que se va a configurar.</param>
    /// <param name="minute">El minuto en el que se debe ejecutar el trabajo cada hora.</param>
    /// <returns>El desencadenador de trabajo configurado.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de <see cref="IJobTrigger"/> permitiendo establecer un cronograma
    /// para que un trabajo se ejecute cada hora en el minuto especificado.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    /// <seealso cref="HangfireTrigger"/>
    /// <seealso cref="Hangfire.Cron"/>
    public static IJobTrigger Hourly( this IJobTrigger source, int minute ) {
        source.CheckNull( nameof( source ) );
        if ( source is HangfireTrigger trigger )
            trigger.Cron = Hangfire.Cron.Hourly( minute );
        return source;
    }

    /// <summary>
    /// Crea un desencadenador que se activa diariamente a la hora especificada.
    /// </summary>
    /// <param name="source">El desencadenador existente que se utilizará como base para crear el nuevo desencadenador diario.</param>
    /// <returns>
    /// Un nuevo desencadenador que se activa diariamente a la hora especificada.
    /// </returns>
    /// <remarks>
    /// Este método proporciona una forma conveniente de establecer un desencadenador diario sin necesidad de especificar la hora.
    /// Por defecto, se activa a la medianoche.
    /// </remarks>
    /// <seealso cref="Daily(IJobTrigger, int)"/>
    public static IJobTrigger Daily( this IJobTrigger source ) {
        return source.Daily( 0 );
    }

    /// <summary>
    /// Crea un desencadenador que se activa diariamente a una hora específica.
    /// </summary>
    /// <param name="source">El desencadenador original al que se le aplicará la configuración diaria.</param>
    /// <param name="hour">La hora del día (en formato de 24 horas) en la que se activará el desencadenador.</param>
    /// <returns>Un nuevo desencadenador configurado para activarse diariamente a la hora especificada.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que establece los minutos en 0 por defecto.
    /// </remarks>
    public static IJobTrigger Daily( this IJobTrigger source, int hour ) {
        return source.Daily( hour, 0 );
    }

    /// <summary>
    /// Configura un desencadenador para que se ejecute diariamente a una hora y minuto específicos.
    /// </summary>
    /// <param name="source">El desencadenador que se va a configurar.</param>
    /// <param name="hour">La hora a la que se debe ejecutar el desencadenador (en formato de 24 horas).</param>
    /// <param name="minute">El minuto a la que se debe ejecutar el desencadenador.</param>
    /// <returns>El desencadenador configurado para la ejecución diaria.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de <see cref="IJobTrigger"/> para permitir la configuración de un desencadenador 
    /// que se ejecute diariamente a la hora y minuto especificados.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    /// <seealso cref="HangfireTrigger"/>
    public static IJobTrigger Daily( this IJobTrigger source, int hour, int minute ) {
        source.CheckNull( nameof( source ) );
        if ( source is HangfireTrigger trigger )
            trigger.Cron = Hangfire.Cron.Daily( hour, minute );
        return source;
    }

    /// <summary>
    /// Crea un desencadenador de trabajo que se activa semanalmente en un día específico.
    /// </summary>
    /// <param name="source">La instancia de <see cref="IJobTrigger"/> sobre la cual se invoca el método.</param>
    /// <returns>
    /// Un nuevo <see cref="IJobTrigger"/> que se activa semanalmente el lunes.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión que permite configurar un desencadenador semanal
    /// de manera más sencilla, utilizando el lunes como día predeterminado.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    public static IJobTrigger Weekly( this IJobTrigger source ) {
        return source.Weekly( DayOfWeek.Monday );
    }

    /// <summary>
    /// Crea un desencadenador de trabajo que se activa semanalmente en un día específico de la semana.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo original sobre el cual se basa el nuevo desencadenador semanal.</param>
    /// <param name="dayOfWeek">El día de la semana en el que se activará el desencadenador.</param>
    /// <returns>
    /// Un nuevo desencadenador de trabajo que se activa semanalmente en el día especificado.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión que permite simplificar la creación de desencadenadores semanales.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    public static IJobTrigger Weekly( this IJobTrigger source, DayOfWeek dayOfWeek ) {
        return source.Weekly( dayOfWeek, 0 );
    }

    /// <summary>
    /// Crea un desencadenador de trabajo que se activa semanalmente en un día y hora específicos.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo existente al que se le añadirá la configuración semanal.</param>
    /// <param name="dayOfWeek">El día de la semana en el que se debe activar el desencadenador.</param>
    /// <param name="hour">La hora del día en la que se debe activar el desencadenador (en formato de 24 horas).</param>
    /// <returns>
    /// Un nuevo desencadenador de trabajo configurado para activarse semanalmente en el día y hora especificados.
    /// </returns>
    /// <remarks>
    /// Este método es una sobrecarga que establece los minutos en 0 por defecto.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    public static IJobTrigger Weekly( this IJobTrigger source, DayOfWeek dayOfWeek, int hour ) {
        return source.Weekly( dayOfWeek, hour, 0 );
    }

    /// <summary>
    /// Configura un desencadenador de trabajo para que se ejecute semanalmente en un día y hora específicos.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo que se va a configurar.</param>
    /// <param name="dayOfWeek">El día de la semana en que se debe ejecutar el trabajo.</param>
    /// <param name="hour">La hora del día en que se debe ejecutar el trabajo (en formato de 24 horas).</param>
    /// <param name="minute">El minuto en que se debe ejecutar el trabajo.</param>
    /// <returns>El desencadenador de trabajo configurado.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de un desencadenador de trabajo existente, permitiendo
    /// que se ejecute de manera programada cada semana en el día y la hora especificados.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="source"/> es nulo.</exception>
    /// <seealso cref="IJobTrigger"/>
    public static IJobTrigger Weekly( this IJobTrigger source, DayOfWeek dayOfWeek, int hour, int minute ) {
        source.CheckNull( nameof( source ) );
        if ( source is HangfireTrigger trigger )
            trigger.Cron = Hangfire.Cron.Weekly( dayOfWeek, hour, minute );
        return source;
    }

    /// <summary>
    /// Crea un desencadenador que se activa mensualmente.
    /// </summary>
    /// <param name="source">El desencadenador existente al que se le añadirá la funcionalidad mensual.</param>
    /// <returns>
    /// Un nuevo desencadenador que se activa una vez al mes.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión que permite configurar un desencadenador para que se ejecute mensualmente,
    /// utilizando un valor predeterminado de 1 para el número de meses.
    /// </remarks>
    /// <seealso cref="Monthly(IJobTrigger)"/>
    public static IJobTrigger Monthly( this IJobTrigger source ) {
        return source.Monthly( 1 );
    }

    /// <summary>
    /// Crea un desencadenador que se activa mensualmente en un día específico del mes.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo original al que se le añadirá la configuración mensual.</param>
    /// <param name="day">El día del mes en que se activará el desencadenador. Debe ser un valor entre 1 y 31.</param>
    /// <returns>
    /// Un nuevo desencadenador que se activa mensualmente en el día especificado.
    /// </returns>
    /// <remarks>
    /// Este método es una sobrecarga que establece el día del mes para el desencadenador.
    /// Si el día especificado no es válido para un mes determinado (por ejemplo, el 30 de febrero),
    /// el comportamiento del desencadenador puede variar según la implementación.
    /// </remarks>
    public static IJobTrigger Monthly( this IJobTrigger source, int day ) {
        return source.Monthly( day, 0 );
    }

    /// <summary>
    /// Crea un desencadenador que se activará mensualmente en un día y hora específicos.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo original al que se le añadirá la configuración mensual.</param>
    /// <param name="day">El día del mes en que se activará el desencadenador. Debe ser un valor entre 1 y 31.</param>
    /// <param name="hour">La hora del día en que se activará el desencadenador. Debe ser un valor entre 0 y 23.</param>
    /// <returns>
    /// Un nuevo desencadenador de trabajo que se activará mensualmente en el día y hora especificados.
    /// </returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite especificar solo el día y la hora, 
    /// utilizando 0 como valor predeterminado para los minutos.
    /// </remarks>
    /// <seealso cref="Monthly(IJobTrigger, int, int, int)"/>
    public static IJobTrigger Monthly( this IJobTrigger source, int day, int hour ) {
        return source.Monthly( day, hour, 0 );
    }

    /// <summary>
    /// Configura un desencadenador para que se ejecute mensualmente en un día, hora y minuto específicos.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo que se va a configurar.</param>
    /// <param name="day">El día del mes en que se debe ejecutar el desencadenador (1-31).</param>
    /// <param name="hour">La hora en que se debe ejecutar el desencadenador (0-23).</param>
    /// <param name="minute">El minuto en que se debe ejecutar el desencadenador (0-59).</param>
    /// <returns>El desencadenador de trabajo configurado.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método modifica el desencadenador para que utilice una expresión cron que representa la ejecución mensual.
    /// Si el desencadenador proporcionado no es del tipo <see cref="HangfireTrigger"/>, no se aplicará ninguna configuración.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    /// <seealso cref="HangfireTrigger"/>
    /// <seealso cref="Hangfire.Cron"/>
    public static IJobTrigger Monthly( this IJobTrigger source, int day, int hour, int minute ) {
        source.CheckNull( nameof( source ) );
        if ( source is HangfireTrigger trigger )
            trigger.Cron = Hangfire.Cron.Monthly( day, hour, minute );
        return source;
    }

    /// <summary>
    /// Crea un desencadenador de trabajo que se activa anualmente.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo existente al que se le añadirá la configuración anual.</param>
    /// <returns>
    /// Un nuevo desencadenador de trabajo que se activa una vez al año.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión que permite simplificar la creación de un desencadenador anual
    /// utilizando un intervalo de un año.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    public static IJobTrigger Yearly( this IJobTrigger source ) {
        return source.Yearly( 1 );
    }

    /// <summary>
    /// Crea un desencadenador que se activa anualmente en el mes especificado.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo existente al que se le añadirá la configuración anual.</param>
    /// <param name="month">El mes en el que se activará el desencadenador, representado como un número del 1 al 12.</param>
    /// <returns>Un nuevo desencadenador de trabajo que se activa anualmente en el mes especificado.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que utiliza el día 1 del mes especificado como fecha de activación.
    /// </remarks>
    public static IJobTrigger Yearly( this IJobTrigger source, int month ) {
        return source.Yearly( month,1 );
    }

    /// <summary>
    /// Crea un desencadenador de trabajo que se activa anualmente en una fecha específica.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo existente al que se le añadirá la configuración anual.</param>
    /// <param name="month">El mes en el que se activará el desencadenador (1-12).</param>
    /// <param name="day">El día del mes en el que se activará el desencadenador (1-31).</param>
    /// <returns>Un nuevo desencadenador de trabajo configurado para activarse anualmente en la fecha especificada.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite especificar el mes y el día para el desencadenador anual.
    /// El año se establece en 0, lo que indica que se activará cada año en la misma fecha.
    /// </remarks>
    public static IJobTrigger Yearly( this IJobTrigger source, int month, int day ) {
        return source.Yearly( month, day,0 );
    }

    /// <summary>
    /// Crea un desencadenador que se activa anualmente en una fecha y hora específicas.
    /// </summary>
    /// <param name="source">El desencadenador original al que se le aplicará la configuración anual.</param>
    /// <param name="month">El mes en el que se activará el desencadenador (1-12).</param>
    /// <param name="day">El día del mes en el que se activará el desencadenador (1-31).</param>
    /// <param name="hour">La hora del día en la que se activará el desencadenador (0-23).</param>
    /// <returns>Un nuevo desencadenador que se activa anualmente en la fecha y hora especificadas.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite especificar solo el mes, día y hora,
    /// utilizando un valor predeterminado de 0 para los minutos.
    /// </remarks>
    public static IJobTrigger Yearly( this IJobTrigger source, int month, int day, int hour ) {
        return source.Yearly( month, day, hour, 0 );
    }

    /// <summary>
    /// Configura un desencadenador (trigger) para que se ejecute anualmente en una fecha y hora específicas.
    /// </summary>
    /// <param name="source">El desencadenador que se va a configurar.</param>
    /// <param name="month">El mes en el que se debe ejecutar el desencadenador (1-12).</param>
    /// <param name="day">El día del mes en el que se debe ejecutar el desencadenador (1-31).</param>
    /// <param name="hour">La hora del día en la que se debe ejecutar el desencadenador (0-23).</param>
    /// <param name="minute">El minuto de la hora en el que se debe ejecutar el desencadenador (0-59).</param>
    /// <returns>El desencadenador configurado para la ejecución anual.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de la interfaz <see cref="IJobTrigger"/> 
    /// permitiendo la configuración de un desencadenador que se ejecuta anualmente en una fecha y hora específicas.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    public static IJobTrigger Yearly( this IJobTrigger source, int month, int day, int hour, int minute ) {
        source.CheckNull( nameof( source ) );
        if ( source is HangfireTrigger trigger )
            trigger.Cron = Hangfire.Cron.Yearly( month, day, hour, minute );
        return source;
    }
}