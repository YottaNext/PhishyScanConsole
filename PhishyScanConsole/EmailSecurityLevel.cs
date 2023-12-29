// See https://aka.ms/new-console-template for more information

/// <summary>
/// Email security level.
/// </summary>
public enum EmailSecurityLevel
{
    /// <summary>
    /// Passed indicates that the email passed all checks for phishing.
    /// </summary>
    Passed,
    /// <summary>
    /// Warn indicates that the email passed all checks for known phishing, but some of the submitted data was suspicious.
    /// </summary>
    Warn,
    /// <summary>
    /// Failed indicates that the email failed at least one check for known phishing.
    /// </summary>
    Failed
}