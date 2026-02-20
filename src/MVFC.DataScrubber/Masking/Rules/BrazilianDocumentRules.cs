namespace MVFC.DataScrubber.Masking.Rules;

using MVFC.DataScrubber.Core.Abstractions;

/// <summary>
/// Masking rule for Brazilian CPF (Cadastro de Pessoas Físicas).
/// </summary>
internal sealed class CpfMaskingRule : BaseMaskingRule
{
    public override string RuleId => "cpf";
    public override string DisplayName => "Brazilian CPF";

    public override object? Mask(object? value, MaskingContext context)
    {
        if (IsNullOrEmpty(value))
            return null;

        var str = value!.ToString()!;
        
        // Remove non-numeric characters
        var numeric = new string(str.Where(char.IsDigit).ToArray());

        if (numeric.Length < 11)
            return value; // Invalid CPF, return as-is

        // Generate random valid-looking CPF
        var randomCpf = GenerateRandomNumeric(11);
        
        // Format as CPF if original was formatted
        if (str.Contains(".") || str.Contains("-"))
            return FormatCpf(randomCpf);

        return randomCpf;
    }

    private static string FormatCpf(string cpf) => cpf.Length == 11 
        ? $"{cpf[..3]}.{cpf[3..6]}.{cpf[6..9]}-{cpf[9..]}" 
        : cpf;
}

/// <summary>
/// Masking rule for Brazilian CNPJ (Cadastro Nacional da Pessoa Jurídica).
/// </summary>
internal sealed class CnpjMaskingRule : BaseMaskingRule
{
    public override string RuleId => "cnpj";
    public override string DisplayName => "Brazilian CNPJ";

    public override object? Mask(object? value, MaskingContext context)
    {
        if (IsNullOrEmpty(value))
            return null;

        var str = value!.ToString()!;
        
        // Remove non-numeric characters
        var numeric = new string(str.Where(char.IsDigit).ToArray());

        if (numeric.Length < 14)
            return value; // Invalid CNPJ, return as-is

        // Generate random valid-looking CNPJ
        var randomCnpj = GenerateRandomNumeric(14);
        
        // Format as CNPJ if original was formatted
        if (str.Contains(".") || str.Contains("/") || str.Contains("-"))
            return FormatCnpj(randomCnpj);

        return randomCnpj;
    }

    private static string FormatCnpj(string cnpj) => cnpj.Length == 14
        ? $"{cnpj[..2]}.{cnpj[2..5]}.{cnpj[5..8]}/{cnpj[8..12]}-{cnpj[12..]}"
        : cnpj;
}
