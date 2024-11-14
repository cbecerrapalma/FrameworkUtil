namespace Util.Scheduling; 

/// <summary>
/// Representa un disparador de trabajos que utiliza la biblioteca Quartz.
/// Implementa la interfaz <see cref="IJobTrigger"/>.
/// </summary>
public class QuartzTrigger : IJobTrigger {
    /// <summary>
    /// Obtiene o establece el nombre.
    /// </summary>
    /// <remarks>
    /// Esta propiedad representa el nombre de un objeto y puede ser utilizado para identificarlo.
    /// </remarks>
    public string Name { get; set; }
    /// <summary>
    /// Obtiene o establece el grupo asociado.
    /// </summary>
    /// <value>
    /// Un string que representa el grupo. 
    /// </value>
    public string Group { get; set; }
    /// <summary>
    /// Obtiene o establece el número de repeticiones.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de tipo nullable, lo que significa que puede contener un valor entero o ser nula.
    /// Si es nula, indica que no se ha definido un número de repeticiones.
    /// </remarks>
    /// <value>
    /// Un entero que representa el número de repeticiones, o null si no se ha establecido.
    /// </value>
    public int? RepeatCount { get; set; }
    /// <summary>
    /// Obtiene o establece el intervalo de tiempo.
    /// </summary>
    /// <remarks>
    /// Este intervalo se representa como un objeto <see cref="TimeSpan"/> y puede ser nulo.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="TimeSpan"/> que representa el intervalo de tiempo, o <c>null</c> si no se ha establecido.
    /// </value>
    public TimeSpan? Interval { get; set; }
    /// <summary>
    /// Obtiene o establece el intervalo en horas.
    /// </summary>
    /// <remarks>
    /// Este valor es de tipo nullable, lo que significa que puede contener un valor entero que representa
    /// el número de horas o puede ser nulo si no se ha establecido.
    /// </remarks>
    /// <value>
    /// Un entero que representa el intervalo en horas, o null si no se ha definido.
    /// </value>
    public int? IntervalInHours { get; set; }
    /// <summary>
    /// Obtiene o establece el intervalo en minutos.
    /// </summary>
    /// <remarks>
    /// Este valor puede ser nulo. Si es nulo, significa que no se ha definido un intervalo.
    /// </remarks>
    /// <value>
    /// Un valor entero que representa el intervalo en minutos, o null si no se ha definido.
    /// </value>
    public int? IntervalInMinutes { get; set; }
    /// <summary>
    /// Obtiene o establece el intervalo en segundos.
    /// </summary>
    /// <remarks>
    /// Este valor puede ser nulo, lo que indica que no se ha definido un intervalo.
    /// </remarks>
    /// <value>
    /// Un entero que representa el intervalo en segundos, o <c>null</c> si no se ha definido.
    /// </value>
    public int? IntervalInSeconds { get; set; }
    /// <summary>
    /// Obtiene o establece la hora de inicio.
    /// </summary>
    /// <remarks>
    /// Este campo puede ser nulo. Si no se establece un valor, 
    /// se considera que la hora de inicio no está definida.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="DateTimeOffset"/> que representa la hora de inicio, 
    /// o <c>null</c> si no se ha definido.
    /// </value>
    public DateTimeOffset? StartTime { get; set; }
    /// <summary>
    /// Obtiene o establece la hora de finalización.
    /// </summary>
    /// <remarks>
    /// Este campo es de tipo <see cref="DateTimeOffset"/> y puede ser nulo.
    /// Si no se establece un valor, se considera que no hay una hora de finalización definida.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="DateTimeOffset"/> que representa la hora de finalización, o <c>null</c> si no se ha definido.
    /// </value>
    public DateTimeOffset? EndTime { get; set; }
    /// <summary>
    /// Obtiene o establece la expresión cron que define la programación de una tarea.
    /// </summary>
    /// <remarks>
    /// La expresión cron se utiliza para especificar la frecuencia con la que se debe ejecutar una tarea. 
    /// Asegúrese de que la expresión sea válida para evitar errores en la programación.
    /// </remarks>
    /// <value>
    /// Una cadena que representa la expresión cron.
    /// </value>
    public string Cron { get; set; }
    /// <summary>
    /// Obtiene o establece una acción que configura un <see cref="CronScheduleBuilder"/>.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite definir una acción personalizada que se ejecutará sobre un objeto de tipo <see cref="CronScheduleBuilder"/>.
    /// </remarks>
    /// <value>
    /// Una acción que toma un <see cref="CronScheduleBuilder"/> como parámetro y no devuelve ningún valor.
    /// </value>
    public Action<CronScheduleBuilder> CronScheduleAction { get; set; }

    /// <summary>
    /// Convierte la instancia actual en un objeto de tipo <see cref="ITrigger"/>.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="ITrigger"/> que representa el desencadenador configurado.
    /// </returns>
    /// <remarks>
    /// Este método inicializa la configuración del desencadenador y establece
    /// su identidad, programación simple, tiempo de inicio, tiempo de finalización
    /// y expresión cron, si es necesario.
    /// </remarks>
    public ITrigger ToTrigger() {
        Init();
        var build = TriggerBuilder.Create()
            .WithIdentity( Name, Group )
            .WithSimpleSchedule( simpleScheduleBuilder => {
                SetRepeatCount( simpleScheduleBuilder );
                SetInterval( simpleScheduleBuilder );
            } );
        SetStartTime( build );
        SetEndTime( build );
        SetCron( build );
        return build.Build();
    }

    /// <summary>
    /// Inicializa los intervalos de tiempo si no se han establecido previamente.
    /// </summary>
    /// <remarks>
    /// Este método verifica si todos los intervalos de tiempo (Interval, IntervalInHours, IntervalInMinutes, IntervalInSeconds) 
    /// y la expresión cron están vacíos. Si es así, establece el intervalo en minutos a 1.
    /// </remarks>
    private void Init() {
        if ( Interval == null && IntervalInHours == null && IntervalInMinutes == null && IntervalInSeconds == null && Cron.IsEmpty() )
            IntervalInMinutes = 1;
    }

    /// <summary>
    /// Establece el conteo de repeticiones en el constructor de programación simple.
    /// </summary>
    /// <param name="builder">El constructor de programación simple que se va a configurar.</param>
    /// <remarks>
    /// Si el conteo de repeticiones es nulo, se configurará para repetir indefinidamente.
    /// De lo contrario, se establecerá el conteo de repeticiones especificado.
    /// </remarks>
    private void SetRepeatCount( SimpleScheduleBuilder builder ) {
        if ( RepeatCount == null ) {
            builder.RepeatForever();
            return;
        }
        builder.WithRepeatCount( RepeatCount.Value );
    }

    /// <summary>
    /// Configura el intervalo de un constructor de programación simple.
    /// </summary>
    /// <param name="builder">El constructor de programación simple que se va a configurar.</param>
    /// <remarks>
    /// Este método establece el intervalo en el constructor según los valores de intervalo
    /// especificados en las propiedades de la clase. Se pueden establecer intervalos en 
    /// diferentes unidades de tiempo: segundos, minutos, horas o un intervalo general.
    /// </remarks>
    private void SetInterval( SimpleScheduleBuilder builder ) {
        if ( Interval != null )
            builder.WithInterval( Interval.Value );
        if ( IntervalInHours != null )
            builder.WithIntervalInHours( IntervalInHours.Value );
        if ( IntervalInMinutes != null )
            builder.WithIntervalInMinutes( IntervalInMinutes.Value );
        if ( IntervalInSeconds != null )
            builder.WithIntervalInSeconds( IntervalInSeconds.Value );
    }

    /// <summary>
    /// Establece el tiempo de inicio para el desencadenador.
    /// </summary>
    /// <param name="builder">El objeto <see cref="TriggerBuilder"/> que se utilizará para configurar el desencadenador.</param>
    /// <remarks>
    /// Si <c>StartTime</c> es nulo, el desencadenador comenzará inmediatamente.
    /// De lo contrario, se establecerá en el valor de <c>StartTime</c>.
    /// </remarks>
    private void SetStartTime( TriggerBuilder builder ) {
        if ( StartTime == null ) {
            builder.StartNow();
            return;
        }
        builder.StartAt( StartTime.Value );
    }

    /// <summary>
    /// Establece la hora de finalización en el constructor del disparador.
    /// </summary>
    /// <param name="builder">El constructor del disparador que se está configurando.</param>
    /// <remarks>
    /// Si la propiedad <c>EndTime</c> es nula, el método no realiza ninguna acción.
    /// De lo contrario, se establece la hora de finalización en el constructor del disparador.
    /// </remarks>
    private void SetEndTime( TriggerBuilder builder ) {
        if ( EndTime == null )
            return;
        builder.EndAt( EndTime.Value );
    }

    /// <summary>
    /// Establece la programación cron en el constructor del disparador.
    /// </summary>
    /// <param name="builder">El constructor del disparador que se va a configurar con la programación cron.</param>
    /// <remarks>
    /// Este método verifica si la propiedad <c>Cron</c> es nula. Si es así, no realiza ninguna acción.
    /// Si <c>CronScheduleAction</c> es nula, se establece la programación cron utilizando solo la cadena <c>Cron</c>.
    /// De lo contrario, se establece la programación cron junto con la acción de programación especificada.
    /// </remarks>
    private void SetCron( TriggerBuilder builder ) {
        if ( Cron == null )
            return;
        if ( CronScheduleAction == null ) {
            builder.WithCronSchedule( Cron );
            return;
        }
        builder.WithCronSchedule( Cron, CronScheduleAction );
    }
}