﻿using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Interface, das von <see cref="VCardProperty{T}"/>-Objekten implementiert wird, um den Zugriff auf die in Ihnen gespeicherten
    /// Daten zu erleichtern.
    /// </summary>
    public interface IVCardData
    {
        /// <summary>
        /// Repräsentiert den Inhalt einer vCard-Property.
        /// </summary>
        public object? Value { get; }

        /// <summary>
        /// Gruppenbezeichner eine vCard-Property oder <c>null</c>, wenn die vCard-Property keinen Gruppenbezeichner hat.
        /// </summary>
        public string? Group { get; set; }

        /// <summary>
        /// Entspricht dem Parameter-Teil einer vCard-Property (nie <c>null</c>).
        /// </summary>
        public ParameterSection Parameters { get; }

        /// <summary>
        /// <c>true</c>, wenn das Objekt keine Daten enthält.
        /// </summary>
        public bool IsEmpty { get; }
    }
}
