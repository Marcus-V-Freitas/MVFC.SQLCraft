namespace MVFC.DataScrubber.Masking.Rules;

using MVFC.DataScrubber.Core.Abstractions;

/// <summary>
/// Factory class for creating predefined masking rules.
/// </summary>
public static class Mask
{
    /// <summary>
    /// Creates a masking rule for Brazilian CPF (Cadastro de Pessoas Físicas).
    /// Preserves format but anonymizes the content.
    /// </summary>
    public static IMaskingRule Cpf() => new CpfMaskingRule();

    /// <summary>
    /// Creates a masking rule for Brazilian CNPJ (Cadastro Nacional da Pessoa Jurídica).
    /// Preserves format but anonymizes the content.
    /// </summary>
    public static IMaskingRule Cnpj() => new CnpjMaskingRule();

    /// <summary>
    /// Creates a masking rule for personal names.
    /// Replaces with generic names while preserving structure.
    /// </summary>
    public static IMaskingRule Name() => new NameMaskingRule();

    /// <summary>
    /// Creates a masking rule for email addresses.
    /// Randomizes the local part while preserving the domain.
    /// </summary>
    public static IMaskingRule Email() => new EmailMaskingRule();

    /// <summary>
    /// Creates a masking rule for email addresses that also masks the domain.
    /// </summary>
    public static IMaskingRule EmailPreservingDomain() => new EmailPreservingDomainMaskingRule();

    /// <summary>
    /// Creates a masking rule for Brazilian phone numbers.
    /// Preserves country and area code but masks the subscriber number.
    /// </summary>
    public static IMaskingRule PhoneBr() => new PhoneBrMaskingRule();

    /// <summary>
    /// Creates a masking rule for credit card numbers.
    /// Shows only the last 4 digits.
    /// </summary>
    public static IMaskingRule CreditCardPartial() => new CreditCardPartialMaskingRule();

    /// <summary>
    /// Creates a masking rule for completely masking credit card numbers.
    /// </summary>
    public static IMaskingRule CreditCardFull() => new CreditCardFullMaskingRule();

    /// <summary>
    /// Creates a masking rule for Brazilian addresses.
    /// Replaces with generic address.
    /// </summary>
    public static IMaskingRule Address() => new AddressMaskingRule();

    /// <summary>
    /// Creates a masking rule for URLs.
    /// Replaces with a generic URL.
    /// </summary>
    public static IMaskingRule Url() => new UrlMaskingRule();

    /// <summary>
    /// Creates a masking rule that replaces text with a fixed placeholder.
    /// </summary>
    /// <param name="placeholder">The placeholder text to use (default: "[MASKED]").</param>
    public static IMaskingRule Placeholder(string placeholder = "[MASKED]") => new PlaceholderMaskingRule(placeholder);

    /// <summary>
    /// Creates a masking rule that replaces text with hashes, preserving length.
    /// </summary>
    public static IMaskingRule Hash() => new HashMaskingRule();

    /// <summary>
    /// Creates a masking rule for numeric IDs while preserving uniqueness.
    /// </summary>
    public static IMaskingRule NumericId() => new NumericIdMaskingRule();

    /// <summary>
    /// Creates a masking rule for GUIDs/UUIDs.
    /// </summary>
    public static IMaskingRule Guid() => new GuidMaskingRule();

    /// <summary>
    /// Creates a masking rule that nullifies the value.
    /// </summary>
    public static IMaskingRule Null() => new NullMaskingRule();

    /// <summary>
    /// Creates a masking rule that leaves the value unchanged.
    /// Useful for fields that should be preserved.
    /// </summary>
    public static IMaskingRule Identity() => new IdentityMaskingRule();
}
