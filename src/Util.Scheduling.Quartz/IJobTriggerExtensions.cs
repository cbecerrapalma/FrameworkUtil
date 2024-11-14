namespace Util.Scheduling; 

/// <summary>
/// Proporciona métodos de extensión para la interfaz <see cref="IJobTrigger"/>.
/// </summary>
/// <remarks>
/// Esta clase contiene métodos que permiten manipular y trabajar con instancias de <see cref="IJobTrigger"/> de manera más sencilla.
/// </remarks>
public static class IJobTriggerExtensions {
    /// <summary>
    /// Establece el nombre de un disparador de trabajo (job trigger).
    /// </summary>
    /// <param name="source">El disparador de trabajo al que se le asignará el nombre.</param>
    /// <param name="name">El nombre que se asignará al disparador de trabajo.</param>
    /// <returns>El disparador de trabajo con el nuevo nombre asignado.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de la interfaz <see cref="IJobTrigger"/> 
    /// permitiendo establecer un nombre de manera fluida.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    public static IJobTrigger Name( this IJobTrigger source, string name ) {
        if ( source is QuartzTrigger trigger )
            trigger.Name = name;
        return source;
    }

    /// <summary>
    /// Establece el grupo de un desencadenador de trabajo de Quartz.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo que se va a modificar.</param>
    /// <param name="name">El nombre del grupo que se asignará al desencadenador.</param>
    /// <returns>El desencadenador de trabajo modificado.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de la interfaz <see cref="IJobTrigger"/> 
    /// permitiendo cambiar el grupo al que pertenece el desencadenador.
    /// Si el desencadenador no es un <see cref="QuartzTrigger"/>, no se realizará ningún cambio.
    /// </remarks>
    public static IJobTrigger Group( this IJobTrigger source, string name ) {
        if ( source is QuartzTrigger trigger )
            trigger.Group = name;
        return source;
    }

    /// <summary>
    /// Establece el número de repeticiones para un desencadenador de trabajo de Quartz.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo al que se le aplicará el número de repeticiones.</param>
    /// <param name="value">El número de repeticiones que se deben establecer. Puede ser nulo.</param>
    /// <returns>
    /// El desencadenador de trabajo modificado con el número de repeticiones establecido.
    /// </returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de la interfaz <see cref="IJobTrigger"/> 
    /// permitiendo configurar el número de repeticiones en un desencadenador específico de Quartz.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    /// <seealso cref="QuartzTrigger"/>
    public static IJobTrigger RepeatCount( this IJobTrigger source, int? value ) {
        if ( source is QuartzTrigger trigger )
            trigger.RepeatCount = value;
        return source;
    }

    /// <summary>
    /// Establece un intervalo para el desencadenador de trabajo especificado.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo al que se le asignará el intervalo.</param>
    /// <param name="value">El intervalo de tiempo que se aplicará al desencadenador.</param>
    /// <returns>El desencadenador de trabajo modificado.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de IJobTrigger, permitiendo configurar un intervalo
    /// de tiempo específico para un desencadenador de tipo QuartzTrigger.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    /// <seealso cref="QuartzTrigger"/>
    public static IJobTrigger Interval( this IJobTrigger source, TimeSpan value ) {
        if ( source is QuartzTrigger trigger )
            trigger.Interval = value;
        return source;
    }

    /// <summary>
    /// Establece el intervalo en horas para un desencadenador de trabajo.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo al que se le aplicará el intervalo.</param>
    /// <param name="value">El valor del intervalo en horas.</param>
    /// <returns>El desencadenador de trabajo modificado.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de la interfaz <see cref="IJobTrigger"/> 
    /// permitiendo establecer un intervalo en horas si el desencadenador es de tipo <see cref="QuartzTrigger"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    public static IJobTrigger IntervalInHours( this IJobTrigger source, int value ) {
        if ( source is QuartzTrigger trigger )
            trigger.IntervalInHours = value;
        return source;
    }

    /// <summary>
    /// Establece el intervalo en minutos para un disparador de trabajos de Quartz.
    /// </summary>
    /// <param name="source">El disparador de trabajos al que se le aplicará el intervalo.</param>
    /// <param name="value">El valor del intervalo en minutos.</param>
    /// <returns>El disparador de trabajos modificado.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de la interfaz <see cref="IJobTrigger"/> 
    /// permitiendo ajustar el intervalo de tiempo en el que se ejecutará el trabajo.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    /// <seealso cref="QuartzTrigger"/>
    public static IJobTrigger IntervalInMinutes( this IJobTrigger source, int value ) {
        if ( source is QuartzTrigger trigger )
            trigger.IntervalInMinutes = value;
        return source;
    }

    /// <summary>
    /// Establece el intervalo en segundos para un disparador de trabajos de Quartz.
    /// </summary>
    /// <param name="source">El disparador de trabajos al que se le establecerá el intervalo.</param>
    /// <param name="value">El valor del intervalo en segundos.</param>
    /// <returns>El disparador de trabajos modificado.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de la interfaz <see cref="IJobTrigger"/> 
    /// permitiendo la configuración del intervalo de ejecución de un trabajo.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    /// <seealso cref="QuartzTrigger"/>
    public static IJobTrigger IntervalInSeconds( this IJobTrigger source, int value ) {
        if ( source is QuartzTrigger trigger )
            trigger.IntervalInSeconds = value;
        return source;
    }

    /// <summary>
    /// Establece el tiempo de inicio para un desencadenador de trabajo.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo al que se le establecerá el tiempo de inicio.</param>
    /// <param name="value">El tiempo de inicio que se asignará al desencadenador.</param>
    /// <returns>El desencadenador de trabajo modificado.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de la interfaz <see cref="IJobTrigger"/> 
    /// permitiendo establecer un tiempo de inicio específico si el desencadenador es del tipo <see cref="QuartzTrigger"/>.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    /// <seealso cref="QuartzTrigger"/>
    public static IJobTrigger StartTime( this IJobTrigger source, DateTimeOffset value ) {
        if ( source is QuartzTrigger trigger )
            trigger.StartTime = value;
        return source;
    }

    /// <summary>
    /// Establece el tiempo de finalización para el desencadenador de trabajo especificado.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo al que se le asignará el tiempo de finalización.</param>
    /// <param name="value">El tiempo de finalización que se establecerá.</param>
    /// <returns>El desencadenador de trabajo modificado.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de un desencadenador de trabajo que implementa la interfaz <see cref="IJobTrigger"/>.
    /// Si el desencadenador es de tipo <see cref="QuartzTrigger"/>, se actualizará su propiedad <see cref="QuartzTrigger.EndTime"/>.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    /// <seealso cref="QuartzTrigger"/>
    public static IJobTrigger EndTime( this IJobTrigger source, DateTimeOffset value ) {
        if ( source is QuartzTrigger trigger )
            trigger.EndTime = value;
        return source;
    }

    /// <summary>
    /// Establece una expresión cron en el desencadenador de trabajo especificado.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo al que se le aplicará la expresión cron.</param>
    /// <param name="cron">La expresión cron que define la programación del trabajo.</param>
    /// <returns>El desencadenador de trabajo modificado con la nueva expresión cron.</returns>
    /// <remarks>
    /// Este método solo modifica el desencadenador si el objeto proporcionado es de tipo <see cref="QuartzTrigger"/>.
    /// De lo contrario, se devuelve el desencadenador original sin cambios.
    /// </remarks>
    /// <seealso cref="QuartzTrigger"/>
    public static IJobTrigger Cron( this IJobTrigger source, string cron ) {
        if ( source is QuartzTrigger trigger ) {
            trigger.Cron = cron;
        }
        return source;
    }

    /// <summary>
    /// Establece una expresión cron para el desencadenador de trabajos y permite configurar el calendario asociado.
    /// </summary>
    /// <param name="source">El desencadenador de trabajos sobre el cual se aplicará la configuración.</param>
    /// <param name="cron">La expresión cron que define la programación del trabajo.</param>
    /// <param name="action">Una acción que permite configurar el constructor del calendario cron.</param>
    /// <returns>El desencadenador de trabajos modificado con la nueva configuración cron.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de un desencadenador de trabajos existente, permitiendo la 
    /// configuración de una programación basada en una expresión cron. Si el desencadenador proporcionado 
    /// no es del tipo esperado, no se aplicará ninguna modificación.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    /// <seealso cref="QuartzTrigger"/>
    /// <seealso cref="CronScheduleBuilder"/>
    public static IJobTrigger Cron( this IJobTrigger source, string cron, Action<CronScheduleBuilder> action ) {
        if ( source is QuartzTrigger trigger ) {
            trigger.Cron = cron;
            trigger.CronScheduleAction = action;
        }
        return source;
    }

    /// <summary>
    /// Crea un desencadenador que se ejecuta cada minuto.
    /// </summary>
    /// <param name="source">El desencadenador original al que se le aplicará el nuevo desencadenador minucial.</param>
    /// <returns>
    /// Un nuevo desencadenador que se ejecuta cada minuto basado en el desencadenador original.
    /// </returns>
    public static IJobTrigger Minutely( this IJobTrigger source ) {
        return source.Minutely( 0 );
    }

    /// <summary>
    /// Configura un disparador para que se ejecute cada minuto en el segundo especificado.
    /// </summary>
    /// <param name="source">El disparador que se va a configurar.</param>
    /// <param name="second">El segundo del minuto en el que se debe activar el disparador.</param>
    /// <returns>El disparador configurado.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de <see cref="IJobTrigger"/> para permitir la configuración de un disparador que se activa en un segundo específico de cada minuto.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    /// <seealso cref="QuartzTrigger"/>
    /// <seealso cref="CronHelper.Minutely(int)"/>
    public static IJobTrigger Minutely( this IJobTrigger source, int second ) {
        source.CheckNull( nameof( source ) );
        if ( source is QuartzTrigger trigger )
            trigger.Cron = CronHelper.Minutely( second );
        return source;
    }

    /// <summary>
    /// Crea un disparador que se activa cada hora.
    /// </summary>
    /// <param name="source">El disparador de trabajo existente al que se le añadirá la configuración horaria.</param>
    /// <returns>
    /// Un nuevo disparador de trabajo que se activa cada hora, basado en el disparador de trabajo proporcionado.
    /// </returns>
    public static IJobTrigger Hourly( this IJobTrigger source ) {
        return source.Hourly( 0 );
    }

    /// <summary>
    /// Crea un desencadenador que se activa cada hora en el minuto especificado.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo actual al que se le añadirá la configuración horaria.</param>
    /// <param name="minute">El minuto en el que se activará el desencadenador cada hora.</param>
    /// <returns>
    /// Un nuevo desencadenador de trabajo configurado para activarse cada hora en el minuto especificado.
    /// </returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite establecer el minuto de activación sin especificar los segundos, 
    /// utilizando un valor de 0 para los segundos por defecto.
    /// </remarks>
    public static IJobTrigger Hourly( this IJobTrigger source, int minute ) {
        return source.Hourly( minute, 0 );
    }

    /// <summary>
    /// Configura un desencadenador de trabajo para que se ejecute de forma horaria.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo que se va a configurar.</param>
    /// <param name="minute">El minuto en el que se debe ejecutar el desencadenador (0-59).</param>
    /// <param name="second">El segundo en el que se debe ejecutar el desencadenador (0-59).</param>
    /// <returns>El desencadenador de trabajo configurado.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de un desencadenador de trabajo existente,
    /// permitiendo establecer una programación específica para que se ejecute cada hora
    /// en el minuto y segundo especificados.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    /// <seealso cref="QuartzTrigger"/>
    /// <seealso cref="CronHelper"/>
    public static IJobTrigger Hourly( this IJobTrigger source, int minute, int second ) {
        source.CheckNull( nameof( source ) );
        if ( source is QuartzTrigger trigger )
            trigger.Cron = CronHelper.Hourly( minute, second );
        return source;
    }

    /// <summary>
    /// Crea un desencadenador que se activa diariamente a la hora especificada.
    /// </summary>
    /// <param name="source">El desencadenador original que se está extendiendo.</param>
    /// <returns>Un nuevo desencadenador que se activa diariamente a la hora especificada.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que utiliza la hora predeterminada de 0 (medianoche) para el desencadenador diario.
    /// </remarks>
    public static IJobTrigger Daily( this IJobTrigger source ) {
        return source.Daily( 0 );
    }

    /// <summary>
    /// Crea un desencadenador que se activa diariamente a una hora específica.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo existente al que se le añadirá la configuración diaria.</param>
    /// <param name="hour">La hora del día (en formato de 24 horas) a la que se activará el desencadenador.</param>
    /// <returns>Un nuevo desencadenador de trabajo configurado para activarse diariamente a la hora especificada.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que establece los minutos en cero.
    /// </remarks>
    public static IJobTrigger Daily( this IJobTrigger source, int hour ) {
        return source.Daily( hour, 0 );
    }

    /// <summary>
    /// Configura un desencadenador de trabajo para que se ejecute diariamente a una hora y minuto específicos.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo existente al que se le aplicará la configuración diaria.</param>
    /// <param name="hour">La hora a la que se debe activar el desencadenador, en formato de 24 horas.</param>
    /// <param name="minute">El minuto en el que se debe activar el desencadenador.</param>
    /// <returns>Un nuevo desencadenador de trabajo configurado para ejecutarse diariamente a la hora y minuto especificados.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que establece los segundos en cero.
    /// </remarks>
    public static IJobTrigger Daily( this IJobTrigger source, int hour, int minute ) {
        return source.Daily( hour, minute, 0 );
    }

    /// <summary>
    /// Configura un disparador para que se ejecute diariamente a una hora, minuto y segundo específicos.
    /// </summary>
    /// <param name="source">El disparador que se va a configurar.</param>
    /// <param name="hour">La hora a la que se debe ejecutar el disparador (0-23).</param>
    /// <param name="minute">El minuto a la que se debe ejecutar el disparador (0-59).</param>
    /// <param name="second">El segundo a la que se debe ejecutar el disparador (0-59).</param>
    /// <returns>El disparador configurado para la ejecución diaria.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de un disparador existente, permitiendo que se ejecute
    /// diariamente a la hora, minuto y segundo especificados. Asegúrese de que el disparador
    /// proporcionado sea de tipo <see cref="QuartzTrigger"/> para que la configuración sea efectiva.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    /// <seealso cref="QuartzTrigger"/>
    /// <seealso cref="CronHelper"/>
    public static IJobTrigger Daily( this IJobTrigger source, int hour, int minute, int second ) {
        source.CheckNull( nameof( source ) );
        if ( source is QuartzTrigger trigger )
            trigger.Cron = CronHelper.Daily( hour, minute, second );
        return source;
    }

    /// <summary>
    /// Crea un desencadenador de trabajo que se activa semanalmente el día especificado.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo existente sobre el cual se basa el nuevo desencadenador semanal.</param>
    /// <returns>
    /// Un nuevo desencadenador de trabajo que se activa semanalmente el día lunes.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión que permite configurar un desencadenador semanal de manera más sencilla,
    /// utilizando el día lunes como día predeterminado.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    public static IJobTrigger Weekly( this IJobTrigger source ) {
        return source.Weekly( DayOfWeek.Monday );
    }

    /// <summary>
    /// Crea un desencadenador que se activa semanalmente en un día específico de la semana.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo original al que se le aplicará la configuración semanal.</param>
    /// <param name="dayOfWeek">El día de la semana en el que se activará el desencadenador.</param>
    /// <returns>Un nuevo desencadenador que se activa semanalmente en el día especificado.</returns>
    /// <remarks>
    /// Este método es una extensión para la interfaz <see cref="IJobTrigger"/> que permite configurar un desencadenador 
    /// para que se ejecute en un día específico de la semana sin especificar la hora.
    /// </remarks>
    public static IJobTrigger Weekly( this IJobTrigger source, DayOfWeek dayOfWeek ) {
        return source.Weekly( dayOfWeek, 0 );
    }

    /// <summary>
    /// Crea un desencadenador de trabajo que se activa semanalmente en un día específico de la semana a una hora determinada.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo base sobre el cual se construye el nuevo desencadenador semanal.</param>
    /// <param name="dayOfWeek">El día de la semana en el que se debe activar el desencadenador.</param>
    /// <param name="hour">La hora del día en la que se debe activar el desencadenador, en formato de 24 horas.</param>
    /// <returns>
    /// Un nuevo desencadenador de trabajo que se activa semanalmente en el día y hora especificados.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión del interfaz <see cref="IJobTrigger"/> y permite configurar un desencadenador que se repite semanalmente.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    public static IJobTrigger Weekly( this IJobTrigger source, DayOfWeek dayOfWeek, int hour ) {
        return source.Weekly( dayOfWeek, hour, 0 );
    }

    /// <summary>
    /// Crea un desencadenador que se activa semanalmente en un día y hora específicos.
    /// </summary>
    /// <param name="source">El desencadenador original al que se le añadirá la programación semanal.</param>
    /// <param name="dayOfWeek">El día de la semana en el que se activará el desencadenador.</param>
    /// <param name="hour">La hora del día en que se activará el desencadenador (en formato de 24 horas).</param>
    /// <param name="minute">Los minutos en que se activará el desencadenador.</param>
    /// <returns>
    /// Un nuevo desencadenador que se activa semanalmente en el día y hora especificados.
    /// </returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite establecer un desencadenador semanal sin especificar los segundos.
    /// </remarks>
    /// <seealso cref="IJobTrigger"/>
    public static IJobTrigger Weekly( this IJobTrigger source, DayOfWeek dayOfWeek, int hour, int minute ) {
        return source.Weekly( dayOfWeek, hour, minute, 0 );
    }

    /// <summary>
    /// Configura un desencadenador para que se ejecute semanalmente en un día y hora específicos.
    /// </summary>
    /// <param name="source">El desencadenador que se va a configurar.</param>
    /// <param name="dayOfWeek">El día de la semana en que se debe ejecutar el desencadenador.</param>
    /// <param name="hour">La hora del día en que se debe ejecutar el desencadenador (en formato de 24 horas).</param>
    /// <param name="minute">El minuto en que se debe ejecutar el desencadenador.</param>
    /// <param name="second">El segundo en que se debe ejecutar el desencadenador.</param>
    /// <returns>El desencadenador configurado para la ejecución semanal.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de la interfaz <see cref="IJobTrigger"/> 
    /// para permitir la configuración de un desencadenador que se ejecute semanalmente 
    /// en un día y hora específicos. Asegúrese de que el desencadenador sea del tipo 
    /// <see cref="QuartzTrigger"/> para que la configuración sea aplicada correctamente.
    /// </remarks>
    public static IJobTrigger Weekly( this IJobTrigger source, DayOfWeek dayOfWeek, int hour, int minute, int second ) {
        source.CheckNull( nameof( source ) );
        if ( source is QuartzTrigger trigger )
            trigger.Cron = CronHelper.Weekly( dayOfWeek, hour, minute, second );
        return source;
    }

    /// <summary>
    /// Crea un desencadenador que se activa mensualmente.
    /// </summary>
    /// <param name="source">El desencadenador original que se está extendiendo.</param>
    /// <returns>Un nuevo desencadenador que se activa una vez al mes.</returns>
    public static IJobTrigger Monthly( this IJobTrigger source ) {
        return source.Monthly( 1 );
    }

    /// <summary>
    /// Crea un desencadenador que se activa mensualmente en un día específico.
    /// </summary>
    /// <param name="source">El desencadenador original al que se le aplicará la configuración mensual.</param>
    /// <param name="day">El día del mes en el que se activará el desencadenador. Debe estar en el rango de 1 a 31.</param>
    /// <returns>
    /// Un nuevo desencadenador que se activa mensualmente en el día especificado.
    /// </returns>
    /// <remarks>
    /// Este método es una sobrecarga que establece el día del mes sin especificar la hora.
    /// </remarks>
    /// <seealso cref="Monthly(IJobTrigger, int, int)"/>
    public static IJobTrigger Monthly( this IJobTrigger source, int day ) {
        return source.Monthly( day, 0 );
    }

    /// <summary>
    /// Crea un desencadenador de trabajo que se activa mensualmente en un día y hora específicos.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo existente al que se le añadirá la configuración mensual.</param>
    /// <param name="day">El día del mes en que se activará el desencadenador.</param>
    /// <param name="hour">La hora del día en que se activará el desencadenador (en formato de 24 horas).</param>
    /// <returns>Un nuevo desencadenador de trabajo configurado para activarse mensualmente en el día y hora especificados.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que establece los minutos en cero.
    /// </remarks>
    public static IJobTrigger Monthly( this IJobTrigger source, int day, int hour ) {
        return source.Monthly( day, hour, 0 );
    }

    /// <summary>
    /// Crea un desencadenador que se activa mensualmente en un día y hora específicos.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo existente al que se le añadirá la configuración mensual.</param>
    /// <param name="day">El día del mes en que se activará el desencadenador. Debe estar entre 1 y 31.</param>
    /// <param name="hour">La hora del día en que se activará el desencadenador. Debe estar entre 0 y 23.</param>
    /// <param name="minute">El minuto de la hora en que se activará el desencadenador. Debe estar entre 0 y 59.</param>
    /// <returns>Un nuevo desencadenador de trabajo configurado para activarse mensualmente en la fecha y hora especificadas.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite especificar solo el día, la hora y el minuto,
    /// asumiendo un valor de 0 para los segundos.
    /// </remarks>
    public static IJobTrigger Monthly( this IJobTrigger source, int day, int hour, int minute ) {
        return source.Monthly( day, hour, minute, 0 );
    }

    /// <summary>
    /// Configura un desencadenador de trabajo para que se ejecute mensualmente en un día, hora, minuto y segundo específicos.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo que se va a configurar.</param>
    /// <param name="day">El día del mes en que se debe ejecutar el desencadenador (1-31).</param>
    /// <param name="hour">La hora del día en que se debe ejecutar el desencadenador (0-23).</param>
    /// <param name="minute">El minuto de la hora en que se debe ejecutar el desencadenador (0-59).</param>
    /// <param name="second">El segundo del minuto en que se debe ejecutar el desencadenador (0-59).</param>
    /// <returns>El desencadenador de trabajo configurado.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método utiliza un helper para generar una expresión cron que representa la programación mensual.
    /// </remarks>
    /// <seealso cref="QuartzTrigger"/>
    /// <seealso cref="CronHelper"/>
    public static IJobTrigger Monthly( this IJobTrigger source, int day, int hour, int minute, int second ) {
        source.CheckNull( nameof( source ) );
        if ( source is QuartzTrigger trigger )
            trigger.Cron = CronHelper.Monthly( day, hour, minute, second );
        return source;
    }

    /// <summary>
    /// Crea un desencadenador que se activa anualmente.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo original al que se le aplicará la configuración anual.</param>
    /// <returns>
    /// Un nuevo desencadenador que se activa una vez al año.
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
    /// Crea un desencadenador de trabajo que se activa anualmente en un mes específico.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo existente al que se le añadirá la configuración anual.</param>
    /// <param name="month">El mes en el que se activará el desencadenador, representado como un número del 1 al 12.</param>
    /// <returns>Un nuevo desencadenador de trabajo que se activa anualmente en el mes especificado.</returns>
    /// <remarks>
    /// Este método utiliza un día predeterminado de 1 para la activación anual.
    /// </remarks>
    /// <seealso cref="Yearly(IJobTrigger, int, int)"/>
    public static IJobTrigger Yearly( this IJobTrigger source, int month ) {
        return source.Yearly( month, 1 );
    }

    /// <summary>
    /// Crea un desencadenador que se activa anualmente en una fecha específica.
    /// </summary>
    /// <param name="source">El desencadenador original al que se le añadirá la configuración anual.</param>
    /// <param name="month">El mes en el que se activará el desencadenador (1-12).</param>
    /// <param name="day">El día del mes en el que se activará el desencadenador (1-31).</param>
    /// <returns>Un nuevo desencadenador que se activa anualmente en la fecha especificada.</returns>
    public static IJobTrigger Yearly( this IJobTrigger source, int month, int day ) {
        return source.Yearly( month, day,0 );
    }

    /// <summary>
    /// Crea un desencadenador de trabajo que se activa anualmente en una fecha y hora específicas.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo existente al que se le añadirá la configuración anual.</param>
    /// <param name="month">El mes en el que se activará el desencadenador (1-12).</param>
    /// <param name="day">El día del mes en el que se activará el desencadenador (1-31).</param>
    /// <param name="hour">La hora del día en la que se activará el desencadenador (0-23).</param>
    /// <returns>Un nuevo desencadenador de trabajo configurado para activarse anualmente en la fecha y hora especificadas.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite especificar solo el mes, el día y la hora, 
    /// utilizando un valor predeterminado de 0 para los minutos.
    /// </remarks>
    public static IJobTrigger Yearly( this IJobTrigger source, int month, int day, int hour ) {
        return source.Yearly( month, day, hour, 0 );
    }

    /// <summary>
    /// Crea un desencadenador de trabajo que se activa anualmente en una fecha y hora específicas.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo original al que se le añade la funcionalidad anual.</param>
    /// <param name="month">El mes en el que se activará el desencadenador (1 para enero, 12 para diciembre).</param>
    /// <param name="day">El día del mes en el que se activará el desencadenador.</param>
    /// <param name="hour">La hora del día en la que se activará el desencadenador (en formato de 24 horas).</param>
    /// <param name="minute">El minuto de la hora en el que se activará el desencadenador.</param>
    /// <returns>Un nuevo desencadenador de trabajo que se activa anualmente en la fecha y hora especificadas.</returns>
    public static IJobTrigger Yearly( this IJobTrigger source, int month, int day, int hour, int minute ) {
        return source.Yearly( month, day, hour, minute, 0 );
    }

    /// <summary>
    /// Configura un desencadenador de trabajo para que se ejecute anualmente en una fecha y hora específicas.
    /// </summary>
    /// <param name="source">El desencadenador de trabajo que se va a configurar.</param>
    /// <param name="month">El mes en el que se debe ejecutar el trabajo (1-12).</param>
    /// <param name="day">El día del mes en el que se debe ejecutar el trabajo (1-31).</param>
    /// <param name="hour">La hora del día en la que se debe ejecutar el trabajo (0-23).</param>
    /// <param name="minute">El minuto de la hora en el que se debe ejecutar el trabajo (0-59).</param>
    /// <param name="second">El segundo del minuto en el que se debe ejecutar el trabajo (0-59).</param>
    /// <returns>El desencadenador de trabajo configurado.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de un desencadenador de trabajo existente,
    /// permitiendo establecer una programación anual basada en los parámetros proporcionados.
    /// Asegúrese de que los valores de mes, día, hora, minuto y segundo sean válidos.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="source"/> es nulo.</exception>
    /// <seealso cref="IJobTrigger"/>
    /// <seealso cref="CronHelper.Yearly(int, int, int, int, int)"/>
    public static IJobTrigger Yearly( this IJobTrigger source, int month, int day, int hour, int minute, int second ) {
        source.CheckNull( nameof( source ) );
        if ( source is QuartzTrigger trigger )
            trigger.Cron = CronHelper.Yearly( month, day, hour, minute, second );
        return source;
    }
}