namespace FolkerKinzel.VCards.Models.Enums
{
    /// <summary>
    /// Spezifiziert die Art des Objekts, das die vCard repr�sentiert.
    /// </summary>
    public enum VCdKind
    {
        /// <summary>
        /// Einzelne Person oder Entit�t.
        /// </summary>
        Individual,

        /// <summary>
        /// Gruppe von Personen oder Entit�ten.
        /// </summary>
        Group,

        /// <summary>
        /// Eine Organisation.
        /// </summary>
        Organization,

        /// <summary>
        /// Einen geographischen Ort.
        /// </summary>
        Location,


        /// <summary>
        /// Ein Software-Programm (Server, Online-Service etc.). (siehe RFC 6473)
        /// </summary>
        Application

    }
}