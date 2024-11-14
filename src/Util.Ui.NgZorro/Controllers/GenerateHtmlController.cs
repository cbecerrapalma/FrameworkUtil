using Util.Ui.Razor;

namespace Util.Ui.NgZorro.Controllers;

/// <summary>
/// Razor生成Html控制器
/// </summary>
[ApiController]
[Route("api/html")]
public class GenerateHtmlController : ControllerBase
{
    /// <summary>
    /// 生成所有Razor页面的Html
    /// </summary>
    [HttpGet]
    public async Task<string> GenerateAsync()
    {
        var message = new StringBuilder();
        var result = await HtmlGenerator.GenerateAsync();
        message.AppendLine("======================= Bienvenido a la aplicación Util - Comienza a generar todos los HTML de las páginas Razor. =======================");
        message.AppendLine();
        message.AppendLine();
        foreach (var path in result)
            message.AppendLine(path);
        message.AppendLine();
        message.AppendLine();
        message.Append("======================================== El archivo HTML se ha generado con éxito. ========================================");
        return message.ToString();
    }
}