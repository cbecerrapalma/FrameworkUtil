// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Util.Ui.Sources.Spa.Prerendering;

public interface ISpaPrerendererBuilder
{
    /// <summary>
    /// Define un m�todo para construir la configuraci�n del SPA (Single Page Application).
    /// </summary>
    /// <param name="spaBuilder">El objeto <see cref="ISpaBuilder"/> que se utiliza para configurar el SPA.</param>
    /// <remarks>
    /// Este m�todo permite personalizar la configuraci�n del SPA, incluyendo la definici�n de rutas,
    /// middleware y otros aspectos espec�ficos de la aplicaci�n de una sola p�gina.
    /// </remarks>
    Task Build(ISpaBuilder spaBuilder);
}