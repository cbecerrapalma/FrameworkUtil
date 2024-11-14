namespace Util.Validation.Validators; 

/// <summary>
/// Clase estática que proporciona métodos para validar patrones de datos.
/// </summary>
public static class ValidatePattern {
    public static string MobilePhonePattern = "^1[0-9]{10}$";
    public static string IdCardPattern = @"(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)";
}