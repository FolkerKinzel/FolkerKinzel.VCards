using System;

namespace FolkerKinzel.VCards.Models.Enums
{
    /// <summary>
    /// Benannte Konstanten, um in vCards die Art einer Telefonnummer zu beschreiben. Die Konstanten k�nnen kombiniert werden.
    /// </summary>
    /// <remarks>
    /// <note type="tip">Verwenden Sie bei der Arbeit mit der Enum die Erweiterungsmethoden aus der
    /// <see cref="Models.Helpers.TelTypesExtension"/>-Klasse.</note>
    /// </remarks>
    [Flags]
    public enum TelTypes
    {
        // ACHTUNG: Wenn die Enum erweitert wird, m�ssen TelTypesConverter 
        // und TelTypesCollector angepasst werden!


        /// <summary>
        /// <c>VOICE</c>: Telefonnummer f�r Sprachkommunikation (Default). <c>(2,3,4)</c>
        /// </summary>
        Voice = 1,

        /// <summary>
        /// <c>FAX</c>: Fax-Nummer <c>(2,3,4)</c>
        /// </summary>
        Fax = 1 << 1,

        /// <summary>
        /// <c>MSG</c>: Voice Message (z.B. MMS). <c>(2,3)</c>
        /// </summary>
        Msg = 1 << 2,

        /// <summary>
        /// <c>CELL</c>: Handy <c>(2,3,4)</c>
        /// </summary>
        Cell = 1 << 3,

        /// <summary>
        /// <c>PAGER</c>: Pager-Nummer <c>(2,3,4)</c>
        /// </summary>
        Pager = 1 << 4,

        /// <summary>
        /// <c>BBS</c>: Anrufbeantworter (Bulletin board system) <c>(2,3)</c>
        /// </summary>
        BBS = 1 << 5,

        /// <summary>
        /// <c>MODEM</c>: Modem <c>(2,3)</c>
        /// </summary>
        Modem = 1 << 6,

        /// <summary>
        /// <c>CAR</c>: Autotelefon <c>(2,3)</c>
        /// </summary>
        Car = 1 << 7,

        /// <summary>
        /// <c>ISDN</c>: ISDN-Nummer <c>(2,3)</c>
        /// </summary>
        ISDN = 1 << 8,

        /// <summary>
        /// <c>VIDEO</c>: Telefonnummer f�r Videokonferenzen <c>(2,3,4)</c>
        /// </summary>
        Video = 1 << 9,

        /// <summary>
        /// <c>PCS</c>: Mobilfunkdienst (Personal communication services) <c>(3)</c>
        /// </summary>
        PCS = 1 << 10, // nur vCard 3.0

        /// <summary>
        /// <c>TEXTPHONE</c>: Telefonnummer f�r Menschen mit H�r- oder 
        /// Sprachst�rungen. <c>(4)</c>
        /// </summary>
        TextPhone = 1 << 11,

        /// <summary>
        /// <c>TEXT</c>: Die Telefonnummer unterst�tzt Textnachrichten (SMS). <c>(4)</c>
        /// </summary>
        Text = 1 << 12
    }
}