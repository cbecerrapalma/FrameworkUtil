namespace Util.Scheduling; 

/// <summary>
/// Contiene métodos de extensión para la interfaz <see cref="ISchedulerManager"/>.
/// </summary>
public static class ISchedulerManagerExtensions {
    /// <summary>
    /// Encola un trabajo en segundo plano utilizando la expresión de acción proporcionada.
    /// </summary>
    /// <param name="source">La instancia de <see cref="ISchedulerManager"/> que invoca este método.</param>
    /// <param name="actionExpression">La expresión que representa la acción a ejecutar en segundo plano.</param>
    /// <returns>
    /// Un identificador único del trabajo encolado.
    /// </returns>
    /// <remarks>
    /// Este método permite a los usuarios encolar trabajos que se ejecutarán de manera asíncrona.
    /// Asegúrese de que la expresión de acción sea válida y que el método al que apunta sea accesible.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    /// <seealso cref="BackgroundJob.Enqueue(Expression{Action})"/>
    public static string Enqueue( this ISchedulerManager source, Expression<Action> actionExpression ) {
        return BackgroundJob.Enqueue( actionExpression );
    }

    /// <summary>
    /// Encola una tarea para su ejecución en segundo plano utilizando el administrador de programación.
    /// </summary>
    /// <param name="source">La instancia del administrador de programación que invoca el método.</param>
    /// <param name="actionExpression">Una expresión que representa la tarea que se va a encolar.</param>
    /// <returns>
    /// Un identificador único de la tarea encolada.
    /// </returns>
    /// <remarks>
    /// Este método permite a los usuarios encolar tareas que se ejecutarán de manera asíncrona.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    public static string Enqueue( this ISchedulerManager source, Expression<Func<Task>> actionExpression ) {
        return BackgroundJob.Enqueue( actionExpression );
    }

    /// <summary>
    /// Encola un trabajo en segundo plano utilizando una expresión de acción.
    /// </summary>
    /// <typeparam name="T">El tipo del contexto en el que se ejecutará la acción.</typeparam>
    /// <param name="source">La instancia de <see cref="ISchedulerManager"/> que invoca el método.</param>
    /// <param name="actionExpression">La expresión que representa la acción a ejecutar en segundo plano.</param>
    /// <returns>Un identificador único del trabajo encolado.</returns>
    /// <remarks>
    /// Este método permite encolar una acción para su ejecución asíncrona, facilitando la gestión de trabajos en segundo plano.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    /// <seealso cref="BackgroundJob"/>
    public static string Enqueue<T>( this ISchedulerManager source, Expression<Action<T>> actionExpression ) {
        return BackgroundJob.Enqueue( actionExpression );
    }

    /// <summary>
    /// Encola un trabajo en segundo plano utilizando una expresión que representa una acción asincrónica.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto que se pasará a la acción.</typeparam>
    /// <param name="source">La instancia de <see cref="ISchedulerManager"/> que invoca este método.</param>
    /// <param name="actionExpression">Una expresión que representa la acción asincrónica a ejecutar.</param>
    /// <returns>
    /// Una cadena que representa el identificador del trabajo encolado.
    /// </returns>
    /// <remarks>
    /// Este método permite encolar trabajos en segundo plano de manera sencilla utilizando expresiones lambda.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    /// <seealso cref="BackgroundJob.Enqueue{T}(Expression{Func{T, Task}})"/>
    public static string Enqueue<T>( this ISchedulerManager source, Expression<Func<T, Task>> actionExpression ) {
        return BackgroundJob.Enqueue( actionExpression );
    }

    /// <summary>
    /// Programa una tarea para que se ejecute después de un retraso especificado.
    /// </summary>
    /// <param name="source">La instancia de <see cref="ISchedulerManager"/> que invoca el método.</param>
    /// <param name="actionExpression">La expresión que representa la acción a programar.</param>
    /// <param name="delay">El tiempo de retraso antes de que se ejecute la acción.</param>
    /// <returns>
    /// Un identificador único de la tarea programada.
    /// </returns>
    /// <remarks>
    /// Este método permite programar una tarea que se ejecutará una vez después de un período de tiempo especificado.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    public static string Schedule( this ISchedulerManager source, Expression<Action> actionExpression, TimeSpan delay ) {
        return BackgroundJob.Schedule( actionExpression, delay );
    }

    /// <summary>
    /// Programa una tarea para que se ejecute después de un retraso específico.
    /// </summary>
    /// <param name="source">La instancia de <see cref="ISchedulerManager"/> que invoca el método.</param>
    /// <param name="actionExpression">Una expresión que representa la tarea a programar.</param>
    /// <param name="delay">El tiempo de retraso antes de que se ejecute la tarea.</param>
    /// <returns>
    /// Un identificador único de la tarea programada.
    /// </returns>
    /// <remarks>
    /// Este método permite programar una tarea que se ejecutará una sola vez después del retraso especificado.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    public static string Schedule( this ISchedulerManager source, Expression<Func<Task>> actionExpression, TimeSpan delay ) {
        return BackgroundJob.Schedule( actionExpression, delay );
    }

    /// <summary>
    /// Programa una tarea para que se ejecute en un momento específico.
    /// </summary>
    /// <param name="source">La instancia de <see cref="ISchedulerManager"/> que invoca el método.</param>
    /// <param name="actionExpression">La expresión que representa la acción a programar.</param>
    /// <param name="dateTime">La fecha y hora en la que se debe ejecutar la tarea.</param>
    /// <returns>Un identificador único para la tarea programada.</returns>
    /// <remarks>
    /// Este método permite programar una tarea que se ejecutará una sola vez en el momento especificado.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    public static string Schedule( this ISchedulerManager source, Expression<Action> actionExpression, DateTimeOffset dateTime ) {
        return BackgroundJob.Schedule( actionExpression, dateTime );
    }

    /// <summary>
    /// Programa una tarea para que se ejecute en un momento específico.
    /// </summary>
    /// <param name="source">La instancia de <see cref="ISchedulerManager"/> que invoca el método.</param>
    /// <param name="actionExpression">Una expresión que representa la tarea que se va a programar.</param>
    /// <param name="dateTime">La fecha y hora en la que se debe ejecutar la tarea.</param>
    /// <returns>
    /// Un identificador único de la tarea programada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el sistema de trabajos en segundo plano para programar la ejecución de la tarea.
    /// Asegúrese de que la expresión de acción sea válida y que el <paramref name="dateTime"/> sea en el futuro.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    public static string Schedule( this ISchedulerManager source, Expression<Func<Task>> actionExpression, DateTimeOffset dateTime ) {
        return BackgroundJob.Schedule( actionExpression, dateTime );
    }

    /// <summary>
    /// Programa una tarea para que se ejecute después de un retraso especificado.
    /// </summary>
    /// <typeparam name="T">El tipo de la clase que contiene el método a programar.</typeparam>
    /// <param name="source">La instancia de <see cref="ISchedulerManager"/> que invoca este método.</param>
    /// <param name="actionExpression">Una expresión que representa el método que se desea programar.</param>
    /// <param name="delay">El tiempo de retraso antes de que se ejecute la tarea.</param>
    /// <returns>
    /// Un identificador único de la tarea programada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el sistema de trabajos en segundo plano para programar la ejecución de una tarea.
    /// Asegúrese de que el método especificado en <paramref name="actionExpression"/> sea accesible y no tenga parámetros.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    public static string Schedule<T>( this ISchedulerManager source, Expression<Action<T>> actionExpression, TimeSpan delay ) {
        return BackgroundJob.Schedule( actionExpression, delay );
    }

    /// <summary>
    /// Programa una tarea en segundo plano para que se ejecute en un momento específico.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que contiene el método a ejecutar.</typeparam>
    /// <param name="source">La instancia de <see cref="ISchedulerManager"/> que invoca el método.</param>
    /// <param name="actionExpression">Una expresión que representa la acción a ejecutar.</param>
    /// <param name="dateTime">La fecha y hora en la que se debe ejecutar la tarea.</param>
    /// <returns>
    /// Un identificador único de la tarea programada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el sistema de trabajos en segundo plano para programar la ejecución
    /// de una acción en un momento específico, permitiendo la ejecución diferida de tareas.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    public static string Schedule<T>( this ISchedulerManager source, Expression<Action<T>> actionExpression, DateTimeOffset dateTime ) {
        return BackgroundJob.Schedule( actionExpression, dateTime );
    }

    /// <summary>
    /// Programa una tarea para que se ejecute después de un retraso especificado.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto que se utilizará en la expresión de acción.</typeparam>
    /// <param name="source">La instancia de <see cref="ISchedulerManager"/> que invoca el método.</param>
    /// <param name="actionExpression">Una expresión que representa la acción asíncrona a programar.</param>
    /// <param name="delay">El tiempo de retraso antes de que se ejecute la tarea.</param>
    /// <returns>
    /// Un identificador único de la tarea programada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el sistema de trabajos en segundo plano para programar la tarea.
    /// Asegúrese de que el <paramref name="actionExpression"/> sea una expresión válida que represente una tarea asíncrona.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    public static string Schedule<T>( this ISchedulerManager source, Expression<Func<T, Task>> actionExpression, TimeSpan delay ) {
        return BackgroundJob.Schedule( actionExpression, delay );
    }

    /// <summary>
    /// Programa una tarea para que se ejecute en un momento específico.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto que se utilizará en la expresión de acción.</typeparam>
    /// <param name="source">La instancia de <see cref="ISchedulerManager"/> que invoca el método.</param>
    /// <param name="actionExpression">Una expresión que representa la acción a programar, que debe devolver un <see cref="Task"/>.</param>
    /// <param name="dateTime">La fecha y hora en que se debe ejecutar la tarea programada.</param>
    /// <returns>
    /// Un identificador único de la tarea programada.
    /// </returns>
    /// <remarks>
    /// Este método permite programar una tarea que se ejecutará en el futuro, 
    /// utilizando el sistema de trabajos en segundo plano.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    /// <seealso cref="BackgroundJob.Schedule"/>
    public static string Schedule<T>( this ISchedulerManager source, Expression<Func<T, Task>> actionExpression, DateTimeOffset dateTime ) {
        return BackgroundJob.Schedule( actionExpression, dateTime );
    }

    /// <summary>
    /// Continúa un trabajo en segundo plano a partir de un trabajo padre especificado.
    /// </summary>
    /// <param name="source">La instancia de <see cref="ISchedulerManager"/> que invoca el método.</param>
    /// <param name="parentId">El identificador del trabajo padre que se desea continuar.</param>
    /// <param name="actionExpression">La expresión que representa la acción que se ejecutará al continuar el trabajo.</param>
    /// <returns>
    /// Un identificador del nuevo trabajo que se ha creado para continuar el trabajo padre.
    /// </returns>
    /// <remarks>
    /// Este método permite encadenar trabajos en segundo plano, asegurando que el nuevo trabajo se ejecute una vez que el trabajo padre haya finalizado.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    /// <seealso cref="BackgroundJob"/>
    public static string Continue( this ISchedulerManager source, string parentId, Expression<Action> actionExpression ) {
        return BackgroundJob.ContinueJobWith( parentId,actionExpression );
    }

    /// <summary>
    /// Continúa un trabajo en segundo plano a partir de un trabajo existente.
    /// </summary>
    /// <param name="source">El gestor de programación que invoca el método.</param>
    /// <param name="parentId">El identificador del trabajo padre que se desea continuar.</param>
    /// <param name="actionExpression">La expresión que representa la acción que se ejecutará como continuación.</param>
    /// <param name="options">Las opciones de continuación del trabajo.</param>
    /// <returns>
    /// Un identificador de cadena que representa el nuevo trabajo en segundo plano creado como continuación.
    /// </returns>
    /// <remarks>
    /// Este método permite encadenar trabajos en segundo plano, facilitando la ejecución de tareas dependientes.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    /// <seealso cref="JobContinuationOptions"/>
    public static string Continue( this ISchedulerManager source, string parentId, Expression<Action> actionExpression, JobContinuationOptions options ) {
        return BackgroundJob.ContinueJobWith( parentId, actionExpression, options );
    }

    /// <summary>
    /// Continúa un trabajo en segundo plano con el identificador especificado.
    /// </summary>
    /// <typeparam name="T">El tipo de la acción que se va a ejecutar.</typeparam>
    /// <param name="source">La instancia de <see cref="ISchedulerManager"/> que invoca este método.</param>
    /// <param name="parentId">El identificador del trabajo padre que se desea continuar.</param>
    /// <param name="actionExpression">La expresión que representa la acción que se ejecutará al continuar el trabajo.</param>
    /// <returns>
    /// Un <see cref="string"/> que representa el identificador del nuevo trabajo creado.
    /// </returns>
    /// <remarks>
    /// Este método permite encadenar trabajos en segundo plano, asegurando que el nuevo trabajo se ejecute solo después de que el trabajo padre haya finalizado.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    /// <seealso cref="BackgroundJob"/>
    public static string Continue<T>( this ISchedulerManager source, string parentId, Expression<Action<T>> actionExpression ) {
        return BackgroundJob.ContinueJobWith( parentId, actionExpression );
    }

    /// <summary>
    /// Continúa un trabajo en segundo plano utilizando el identificador del trabajo padre y una expresión de acción.
    /// </summary>
    /// <typeparam name="T">El tipo de la clase que contiene la acción a ejecutar.</typeparam>
    /// <param name="source">La instancia de <see cref="ISchedulerManager"/> que invoca este método.</param>
    /// <param name="parentId">El identificador del trabajo padre que se desea continuar.</param>
    /// <param name="actionExpression">La expresión que representa la acción a ejecutar como continuación.</param>
    /// <param name="options">Las opciones de continuación del trabajo.</param>
    /// <returns>El identificador del nuevo trabajo que se ha creado como continuación.</returns>
    /// <remarks>
    /// Este método permite encadenar trabajos en segundo plano, asegurando que el nuevo trabajo se ejecute después de que el trabajo padre haya finalizado.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    /// <seealso cref="JobContinuationOptions"/>
    public static string Continue<T>( this ISchedulerManager source, string parentId, Expression<Action<T>> actionExpression, JobContinuationOptions options ) {
        return BackgroundJob.ContinueJobWith( parentId, actionExpression, options );
    }

    /// <summary>
    /// Continúa un trabajo en segundo plano a partir de un trabajo existente.
    /// </summary>
    /// <param name="source">La instancia del gestor de programación que invoca el método.</param>
    /// <param name="parentId">El identificador del trabajo padre que se desea continuar.</param>
    /// <param name="actionExpression">La expresión que representa la acción que se ejecutará como continuación del trabajo.</param>
    /// <returns>
    /// El identificador del nuevo trabajo que se ha creado como continuación.
    /// </returns>
    /// <remarks>
    /// Este método permite encadenar trabajos en segundo plano, de manera que un trabajo se ejecute automáticamente después de que otro haya finalizado.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    public static string Continue( this ISchedulerManager source, string parentId, Expression<Func<Task>> actionExpression ) {
        return BackgroundJob.ContinueJobWith( parentId, actionExpression );
    }

    /// <summary>
    /// Continúa un trabajo en segundo plano a partir de un trabajo existente.
    /// </summary>
    /// <param name="source">La instancia de <see cref="ISchedulerManager"/> que invoca el método.</param>
    /// <param name="parentId">El identificador del trabajo padre que se desea continuar.</param>
    /// <param name="actionExpression">Una expresión que representa la acción que se ejecutará como continuación del trabajo.</param>
    /// <param name="options">Opciones que controlan el comportamiento de la continuación del trabajo.</param>
    /// <returns>
    /// Un identificador único para el nuevo trabajo que se ha creado como continuación.
    /// </returns>
    /// <remarks>
    /// Este método permite encadenar trabajos en segundo plano, asegurando que el nuevo trabajo se ejecute
    /// solo después de que el trabajo padre haya finalizado.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    /// <seealso cref="BackgroundJob"/>
    /// <seealso cref="JobContinuationOptions"/>
    public static string Continue( this ISchedulerManager source, string parentId, Expression<Func<Task>> actionExpression, JobContinuationOptions options ) {
        return BackgroundJob.ContinueJobWith( parentId, actionExpression, options );
    }

    /// <summary>
    /// Continúa un trabajo en segundo plano utilizando el identificador del trabajo padre.
    /// </summary>
    /// <typeparam name="T">El tipo de la expresión que representa la acción a ejecutar.</typeparam>
    /// <param name="source">La instancia de <see cref="ISchedulerManager"/> que invoca el método.</param>
    /// <param name="parentId">El identificador del trabajo padre que se desea continuar.</param>
    /// <param name="actionExpression">La expresión que representa la acción a ejecutar como continuación del trabajo.</param>
    /// <returns>
    /// Devuelve el identificador del nuevo trabajo que se ha creado como continuación.
    /// </returns>
    /// <remarks>
    /// Este método permite encadenar trabajos en segundo plano, donde el nuevo trabajo se ejecutará 
    /// una vez que el trabajo padre haya finalizado.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    /// <seealso cref="BackgroundJob"/>
    public static string Continue<T>( this ISchedulerManager source, string parentId, Expression<Func<T, Task>> actionExpression ) {
        return BackgroundJob.ContinueJobWith( parentId, actionExpression );
    }

    /// <summary>
    /// Continúa un trabajo en segundo plano utilizando el identificador del trabajo padre.
    /// </summary>
    /// <typeparam name="T">El tipo de dato que se utilizará en la expresión de acción.</typeparam>
    /// <param name="source">La instancia de <see cref="ISchedulerManager"/> que invoca este método.</param>
    /// <param name="parentId">El identificador del trabajo padre que se está continuando.</param>
    /// <param name="actionExpression">Una expresión que representa la acción a ejecutar como continuación del trabajo.</param>
    /// <param name="options">Opciones que controlan el comportamiento de la continuación del trabajo.</param>
    /// <returns>
    /// El identificador del nuevo trabajo que se ha creado como continuación.
    /// </returns>
    /// <remarks>
    /// Este método permite encadenar trabajos en segundo plano, asegurando que el nuevo trabajo se ejecute una vez que el trabajo padre haya finalizado.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    /// <seealso cref="JobContinuationOptions"/>
    public static string Continue<T>( this ISchedulerManager source, string parentId, Expression<Func<T, Task>> actionExpression, JobContinuationOptions options ) {
        return BackgroundJob.ContinueJobWith( parentId, actionExpression, options );
    }

    /// <summary>
    /// Programa una tarea recurrente en el administrador de programaciones.
    /// </summary>
    /// <param name="source">La instancia del administrador de programaciones donde se añadirá la tarea recurrente.</param>
    /// <param name="id">El identificador único de la tarea recurrente.</param>
    /// <param name="actionExpression">La expresión que representa la acción a ejecutar de forma recurrente.</param>
    /// <param name="cron">La expresión cron que define la frecuencia de ejecución de la tarea.</param>
    /// <param name="queue">El nombre de la cola donde se encolará la tarea. Por defecto es "default".</param>
    /// <param name="options">Opciones adicionales para la tarea recurrente. Si es null, se utilizarán las opciones predeterminadas.</param>
    /// <remarks>
    /// Este método permite programar tareas que se ejecutan de manera recurrente utilizando una expresión cron.
    /// Si no se proporcionan opciones, se utilizarán las opciones predeterminadas al agregar o actualizar la tarea.
    /// </remarks>
    /// <seealso cref="RecurringJob"/>
    public static void Repeat( this ISchedulerManager source, string id, Expression<Action> actionExpression, string cron, string queue = "default", RecurringJobOptions options = null ) {
        if ( options == null ) {
            RecurringJob.AddOrUpdate( id, queue, actionExpression, cron );
            return;
        }
        RecurringJob.AddOrUpdate( id, queue, actionExpression, cron, options );
    }

    /// <summary>
    /// Programa una tarea recurrente en el administrador de programaciones.
    /// </summary>
    /// <param name="source">El administrador de programaciones que se está utilizando.</param>
    /// <param name="id">El identificador único de la tarea recurrente.</param>
    /// <param name="actionExpression">La expresión que representa la acción a ejecutar de forma recurrente.</param>
    /// <param name="cron">La expresión cron que define la frecuencia de ejecución de la tarea.</param>
    /// <param name="queue">El nombre de la cola en la que se encolará la tarea (por defecto es "default").</param>
    /// <param name="options">Opciones adicionales para la tarea recurrente (puede ser <c>null</c>).</param>
    /// <remarks>
    /// Este método permite agregar o actualizar una tarea recurrente en el sistema de programación.
    /// Si no se proporcionan opciones, se utilizarán las configuraciones predeterminadas.
    /// </remarks>
    /// <seealso cref="RecurringJob"/>
    public static void Repeat( this ISchedulerManager source, string id, Expression<Func<Task>> actionExpression, string cron, string queue = "default", RecurringJobOptions options = null ) {
        if ( options == null ) {
            RecurringJob.AddOrUpdate( id, queue, actionExpression, cron );
            return;
        }
        RecurringJob.AddOrUpdate( id, queue, actionExpression, cron, options );
    }

    /// <summary>
    /// Programa una tarea recurrente en el administrador de programadores.
    /// </summary>
    /// <typeparam name="T">El tipo de la acción que se va a ejecutar.</typeparam>
    /// <param name="source">La instancia del administrador de programadores donde se registrará la tarea.</param>
    /// <param name="id">El identificador único de la tarea recurrente.</param>
    /// <param name="actionExpression">La expresión que representa la acción a ejecutar.</param>
    /// <param name="cron">La expresión cron que define la frecuencia de ejecución de la tarea.</param>
    /// <param name="queue">El nombre de la cola donde se encolará la tarea. Por defecto es "default".</param>
    /// <param name="options">Opciones adicionales para la tarea recurrente. Si es null, se utilizarán las opciones por defecto.</param>
    /// <remarks>
    /// Este método permite registrar una tarea que se ejecutará de forma recurrente según la expresión cron proporcionada.
    /// Si no se especifican opciones, se utilizarán las opciones predeterminadas al agregar o actualizar la tarea.
    /// </remarks>
    /// <seealso cref="RecurringJob"/>
    public static void Repeat<T>( this ISchedulerManager source, string id, Expression<Action<T>> actionExpression, string cron, string queue = "default", RecurringJobOptions options = null ) {
        if ( options == null ) {
            RecurringJob.AddOrUpdate( id, queue, actionExpression, cron );
            return;
        }
        RecurringJob.AddOrUpdate( id, queue, actionExpression, cron, options );
    }

    /// <summary>
    /// Programa una tarea recurrente utilizando una expresión de acción.
    /// </summary>
    /// <typeparam name="T">El tipo de datos que se utilizará en la expresión de acción.</typeparam>
    /// <param name="source">La instancia de <see cref="ISchedulerManager"/> que invoca este método.</param>
    /// <param name="id">El identificador único de la tarea recurrente.</param>
    /// <param name="actionExpression">La expresión que define la acción a ejecutar de forma recurrente.</param>
    /// <param name="cron">La expresión cron que determina la frecuencia de ejecución de la tarea.</param>
    /// <param name="queue">El nombre de la cola en la que se encolará la tarea. Por defecto es "default".</param>
    /// <param name="options">Opciones adicionales para la tarea recurrente. Si es <c>null</c>, se utilizarán las opciones predeterminadas.</param>
    /// <remarks>
    /// Este método permite programar una tarea que se ejecutará de acuerdo con la expresión cron especificada.
    /// Si no se proporcionan opciones, se utilizarán las opciones predeterminadas al agregar o actualizar la tarea.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    /// <seealso cref="RecurringJob"/>
    public static void Repeat<T>( this ISchedulerManager source, string id, Expression<Func<T, Task>> actionExpression, string cron, string queue = "default", RecurringJobOptions options = null ) {
        if ( options == null ) {
            RecurringJob.AddOrUpdate( id, queue, actionExpression, cron );
            return;
        }
        RecurringJob.AddOrUpdate( id, queue, actionExpression, cron, options );
    }

    /// <summary>
    /// Elimina un trabajo programado del gestor de trabajos en segundo plano.
    /// </summary>
    /// <param name="source">El gestor de programación desde el cual se eliminará el trabajo.</param>
    /// <param name="id">El identificador del trabajo que se desea eliminar.</param>
    /// <remarks>
    /// Este método intenta eliminar un trabajo utilizando su identificador.
    /// Si el trabajo no se encuentra, se intenta eliminar un trabajo recurrente con el mismo identificador.
    /// </remarks>
    /// <seealso cref="ISchedulerManager"/>
    /// <seealso cref="BackgroundJob.Delete(string)"/>
    /// <seealso cref="RecurringJob.RemoveIfExists(string)"/>
    public static void Remove( this ISchedulerManager source, string id ) {
        var result = BackgroundJob.Delete( id );
        if ( result )
            return;
        RecurringJob.RemoveIfExists( id );
    }
}