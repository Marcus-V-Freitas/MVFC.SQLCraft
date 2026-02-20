namespace MVFC.DataScrubber.Masking.Rules;

using MVFC.DataScrubber.Core.Abstractions;
using System.Text.RegularExpressions;

/// <summary>
/// Masking rule for personal names.
/// </summary>
internal sealed class NameMaskingRule : BaseMaskingRule
{
    private static readonly string[] GenericNames = 
    {
        "Maria Silva", "João Santos", "Ana Oliveira", "Carlos Ferreira", "Paula Costa",
        "Pedro Sousa", "Lucia Martins", "Lucas Gomes", "Angela Ribeiro", "Felipe Alves"
    };

    public override string RuleId => "name";
    public override string DisplayName => "Personal Name";

    public override object? Mask(object? value, MaskingContext context)
    {
        if (IsNullOrEmpty(value))
            return null;

        var random = new Random();
        return GenericNames[random.Next(GenericNames.Length)];
    }
}

/// <summary>
/// Masking rule for email addresses (full masking).
/// </summary>
internal sealed class EmailMaskingRule : BaseMaskingRule
{
    public override string RuleId => "email";
    public override string DisplayName => "Email Address";

    public override object? Mask(object? value, MaskingContext context)
    {
        if (IsNullOrEmpty(value))
            return null;

        return $"user{Guid.NewGuid():N}@example.com";
    }
}

/// <summary>
/// Masking rule for email addresses (preserving domain).
/// </summary>
internal sealed class EmailPreservingDomainMaskingRule : BaseMaskingRule
{
    public override string RuleId => "email_preserve_domain";
    public override string DisplayName => "Email Address (Preserve Domain)";

    public override object? Mask(object? value, MaskingContext context)
    {
        if (IsNullOrEmpty(value))
            return null;

        var str = value!.ToString()!;
        var atIndex = str.LastIndexOf('@');

        if (atIndex == -1)
            return value; // Invalid email, return as-is

        var domain = str[(atIndex + 1)..];
        var localPart = $"user{Guid.NewGuid():N}";

        return $"{localPart}@{domain}";
    }
}

/// <summary>
/// Masking rule for Brazilian phone numbers.
/// </summary>
internal sealed class PhoneBrMaskingRule : BaseMaskingRule
{
    public override string RuleId => "phone_br";
    public override string DisplayName => "Brazilian Phone";

    public override object? Mask(object? value, MaskingContext context)
    {
        if (IsNullOrEmpty(value))
            return null;

        var str = value!.ToString()!;
        var numeric = new string(str.Where(char.IsDigit).ToArray());

        if (numeric.Length < 10)
            return value; // Invalid phone, return as-is

        // Preserve country and area code if present, mask the rest
        var formatted = Regex.Replace(numeric, @"^(\d{0,2})(\d{0,2})", "$1$2");
        var areaCode = formatted[..Math.Min(2, formatted.Length)];
        var maskedNumber = GenerateRandomNumeric(numeric.Length - areaCode.Length);

        // Format as phone if original was formatted
        if (str.Contains("(") || str.Contains(")") || str.Contains("-"))
            return FormatPhone(areaCode + maskedNumber);

        return areaCode + maskedNumber;
    }

    private static string FormatPhone(string phone) => phone.Length switch
    {
        10 => $"({phone[..2]}) {phone[2..6]}-{phone[6..]}",
        11 => $"({phone[..2]}) {phone[2..7]}-{phone[7..]}",
        _ => phone
    };
}

/// <summary>
/// Masking rule for addresses.
/// </summary>
internal sealed class AddressMaskingRule : BaseMaskingRule
{
    private static readonly string[] GenericAddresses =
    {
        "Rua das Flores, 123, São Paulo, SP, 01311-100",
        "Avenida Paulista, 456, Rio de Janeiro, RJ, 20040-020",
        "Rua Augusta, 789, Belo Horizonte, MG, 30130-100",
        "Avenida Brasil, 1000, Brasília, DF, 70000-000"
    };

    public override string RuleId => "address";
    public override string DisplayName => "Address";

    public override object? Mask(object? value, MaskingContext context)
    {
        if (IsNullOrEmpty(value))
            return null;

        var random = new Random();
        return GenericAddresses[random.Next(GenericAddresses.Length)];
    }
}
