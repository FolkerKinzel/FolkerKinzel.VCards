namespace FolkerKinzel.VCards.Models.Enums;

/// <summary>
/// Benannte Konstanten, um die Art des Objekts anzugeben, das die vCard repr�sentiert.
/// </summary>
public enum VCdKind
{
    /// <summary>
    /// <c>INDIVIDUAL</c>: Einzelne Person oder Entit�t. <c>4</c>
    /// </summary>
    Individual,

    /// <summary>
    /// <c>GROUP</c>: Gruppe von Personen oder Entit�ten. <c>4</c>
    /// </summary>
    Group,

    /// <summary>
    /// <c>ORGANIZATION</c>: Eine Organisation. <c>4</c>
    /// </summary>
    Organization,

    /// <summary>
    /// <c>LOCATION</c>: Einen geographischen Ort. <c>4</c>
    /// </summary>
    Location,

    /// <summary>
    /// <c>APPLICATION</c>: Ein Software-Programm (Server, Online-Service etc.). <c>(4 - RFC 6473)</c>
    /// </summary>
    Application
}
