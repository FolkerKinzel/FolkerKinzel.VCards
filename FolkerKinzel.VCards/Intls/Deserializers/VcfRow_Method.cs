﻿using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings.QuotedPrintable;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Diagnostics;
using System.Text;

namespace FolkerKinzel.VCards.Intls.Deserializers
{
    internal sealed partial class VcfRow
    {
        /// <summary>
        /// Dekodiert Quoted-Printable kodierten Text, der sich in <see cref="Value"/> befindet, wenn 
        /// <see cref="VCards.Models.PropertyParts.ParameterSection.Encoding"/>
        /// gleich <see cref="VCdEncoding.QuotedPrintable"/> ist.
        /// </summary>
        internal void DecodeQuotedPrintable()
        {
            if (this.Parameters.Encoding == VCdEncoding.QuotedPrintable)
            {
                this.Value = QuotedPrintableConverter.Decode(this.Value, // null-Prüfung nicht erforderlich
                    TextEncodingConverter.GetEncoding(this.Parameters.Charset)); // null-Prüfung nicht erforderlich
            }
        }


        /// <summary>
        /// Unmaskiert maskierten Text, der sich in <see cref="Value"/> befindet, nach den Maßgaben des
        /// verwendeten vCard-Standards.
        /// </summary>
        /// <param name="version">Die Versionsnummer des vCard-Standards.</param>
        internal void UnMask(VCdVersion version)
        {
            this.Value = Info.Builder.Clear().Append(this.Value).UnMask(version).ToString();

            if (this.Value.Length == 0)
            {
                this.Value = null;
            }
        }


        /// <summary>
        /// Unmaskiert maskierten Text, der sich in <see cref="Value"/> befindet, nach den Maßgaben des
        /// verwendeten vCard-Standards und entfernt führenden und nachgestellten Leerraum sowie einfache und doppelte Gänsefüßchen,
        /// die sich am Beginn oder Ende von <see cref="Value"/> befinden.
        /// </summary>
        /// <param name="version">Die Versionsnummer des vCard-Standards.</param>
        internal void UnMaskAndTrim(VCdVersion version)
        {
            this.Value = Info.Builder.Clear().Append(this.Value).UnMask(version).Trim().RemoveQuotes().ToString();

            if (this.Value.Length == 0)
            {
                this.Value = null;
            }
        }


    }
}