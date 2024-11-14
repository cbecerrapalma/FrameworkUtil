namespace Util.Helpers; 

/// <summary>
/// Proporciona métodos para crear y controlar hilos en la aplicación.
/// </summary>
/// <remarks>
/// Esta clase está diseñada para facilitar la creación y gestión de hilos 
/// en aplicaciones multihilo. Permite a los desarrolladores ejecutar 
/// tareas en paralelo, mejorando así el rendimiento de la aplicación.
/// </remarks>
public static class Thread {
	/// <summary>
	/// Ejecuta todas las acciones proporcionadas en paralelo y espera a que todas finalicen.
	/// </summary>
	/// <param name="actions">Una lista de acciones que se ejecutarán de manera concurrente.</param>
	/// <remarks>
	/// Este método utiliza tareas para ejecutar las acciones en paralelo. Si el parámetro <paramref name="actions"/> es nulo, el método no realizará ninguna acción.
	/// </remarks>
	/// <exception cref="AggregateException">Se lanza si alguna de las tareas genera una excepción.</exception>
	public static void WaitAll( params Action[] actions ) {
		if ( actions == null )
			return;
		var tasks = new List<Task>();
		foreach ( var action in actions )
			tasks.Add( Task.Factory.StartNew( action, TaskCreationOptions.None ) );
		Task.WaitAll( tasks.ToArray() );
	}

	/// <summary>
	/// Ejecuta un conjunto de acciones de manera paralela.
	/// </summary>
	/// <param name="actions">Un arreglo de acciones que se ejecutarán en paralelo.</param>
	/// <remarks>
	/// Este método utiliza el método <see cref="System.Threading.Tasks.Parallel.Invoke"/> 
	/// para ejecutar las acciones proporcionadas. Cada acción se ejecutará en un hilo separado,
	/// lo que puede mejorar el rendimiento en operaciones que pueden ejecutarse de manera concurrente.
	/// </remarks>
	/// <example>
	/// <code>
	/// ParallelInvoke(
	///     () => { /* Acción 1 */ },
	///     () => { /* Acción 2 */ },
	///     () => { /* Acción 3 */ }
	/// );
	/// </code>
	/// </example>
	public static void ParallelInvoke( params Action[] actions ) {
		Parallel.Invoke( actions );
	}

	/// <summary>
	/// Ejecuta una acción en paralelo un número especificado de veces.
	/// </summary>
	/// <param name="action">La acción que se ejecutará en paralelo.</param>
	/// <param name="count">El número de veces que se ejecutará la acción. Por defecto es 1.</param>
	/// <param name="options">Opciones de paralelismo que controlan la ejecución. Puede ser nulo.</param>
	/// <remarks>
	/// Si <paramref name="options"/> es nulo, se utilizará la configuración predeterminada para la ejecución en paralelo.
	/// </remarks>
	/// <example>
	/// <code>
	/// ParallelFor(() => Console.WriteLine("Hola, mundo!"), 5);
	/// </code>
	/// Este ejemplo ejecuta la acción de imprimir "Hola, mundo!" cinco veces en paralelo.
	/// </example>
	public static void ParallelFor( Action action, int count = 1,ParallelOptions options = null ) {
		if ( options == null ) {
			Parallel.For( 0, count, i => action() );
			return;
		}
		Parallel.For( 0, count, options, i => action() );
	}

	/// <summary>
	/// Ejecuta una acción de forma paralela un número especificado de veces, utilizando tareas asíncronas.
	/// </summary>
	/// <param name="action">La acción asíncrona que se ejecutará en paralelo.</param>
	/// <param name="count">El número de veces que se debe ejecutar la acción. El valor predeterminado es 1.</param>
	/// <param name="options">Opciones de paralelismo que controlan la ejecución de la acción. Si es null, se utilizarán las opciones predeterminadas.</param>
	/// <returns>Una tarea que representa la operación asíncrona.</returns>
	/// <remarks>
	/// Este método utiliza <see cref="Parallel.ForEachAsync"/> para ejecutar la acción de forma paralela.
	/// Si no se proporcionan opciones, se utilizarán las opciones predeterminadas para el paralelismo.
	/// </remarks>
	/// <seealso cref="ParallelOptions"/>
	public static async Task ParallelForAsync( Func<ValueTask> action, int count = 1, ParallelOptions options = null ) {
		if ( options == null ) {
			await Parallel.ForEachAsync( Enumerable.Range( 1, count ), async ( i, token ) => await action() );
			return;
		}
		await Parallel.ForEachAsync( Enumerable.Range( 1,count ), options, async (i,token)=> await action() );
	}

	/// <summary>
	/// Ejecuta una acción de forma paralela para un rango de números enteros, utilizando tareas asincrónicas.
	/// </summary>
	/// <param name="action">La función que se ejecutará para cada número en el rango. Debe aceptar un entero y devolver un <see cref="ValueTask"/>.</param>
	/// <param name="count">El número de iteraciones a realizar. Por defecto es 1.</param>
	/// <param name="options">Opciones de paralelismo que controlan la ejecución. Si es <c>null</c>, se utilizarán las opciones predeterminadas.</param>
	/// <returns>Una tarea que representa la operación asincrónica.</returns>
	/// <remarks>
	/// Este método permite la ejecución de acciones de manera concurrente, lo que puede mejorar el rendimiento 
	/// en operaciones que pueden ser ejecutadas en paralelo. Se recomienda que la función proporcionada sea 
	/// idempotente y no tenga efectos secundarios, ya que puede ser llamada en paralelo.
	/// </remarks>
	/// <seealso cref="ValueTask"/>
	/// <seealso cref="ParallelOptions"/>
	public static async Task ParallelForAsync( Func<int,ValueTask> action, int count = 1, ParallelOptions options = null ) {
		if ( options == null ) {
			await Parallel.ForEachAsync( Enumerable.Range( 1, count ), async ( i, token ) => await action(i) );
			return;
		}
		await Parallel.ForEachAsync( Enumerable.Range( 1, count ), options, async ( i, token ) => await action(i) );
	}
}