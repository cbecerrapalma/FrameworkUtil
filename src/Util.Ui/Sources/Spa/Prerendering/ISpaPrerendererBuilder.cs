// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Util.Ui.Sources.Spa.Prerendering;

public interface ISpaPrerendererBuilder
{
    /// <summary>
    /// Define un método para construir la configuración del SPA (Single Page Application).
    /// </summary>
    /// <param name="spaBuilder">El objeto <see cref="ISpaBuilder"/> que se utiliza para configurar el SPA.</param>
    /// <remarks>
    /// Este método permite personalizar la configuración del SPA, incluyendo la definición de rutas,
    /// middleware y otros aspectos específicos de la aplicación de una sola página.
    /// </remarks>
    Task Build(ISpaBuilder spaBuilder);
}