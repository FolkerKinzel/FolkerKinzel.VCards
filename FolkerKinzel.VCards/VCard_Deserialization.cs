﻿using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FolkerKinzel.VCards
{
    public sealed partial class VCard : IEnumerable<KeyValuePair<VCdProp, object>>
    {
        /// <summary>
        /// Lädt eine VCF-Datei.
        /// </summary>
        /// 
        /// <param name="fileName">Absoluter oder relativer Pfad zu einer VCF-Datei.</param>
        /// <param name="textEncoding">Die zum Einlesen der Datei zu verwendende Textenkodierung oder <c>null</c>, um die Datei mit der 
        /// standardgerechten Enkodierung <see cref="Encoding.UTF8"/> einzulesen.</param>
        /// 
        /// <returns>Eine Auflistung geparster <see cref="VCard"/>-Objekte, die den Inhalt der VCF-Datei repräsentieren.</returns>
        /// 
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="fileName"/> ist <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName"/> ist kein gültiger Dateipfad.</exception>
        /// <exception cref="IOException">Die Datei konnte nicht geladen werden.</exception>
        public static List<VCard> Load(string fileName, Encoding? textEncoding = null)
        {
            try
            {
                using var reader = new StreamReader(fileName, textEncoding ?? Encoding.UTF8, true);
                return DoParse(reader);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(nameof(fileName));
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message, nameof(fileName), e);
            }
            catch (UnauthorizedAccessException e)
            {
                throw new IOException(e.Message, e);
            }
            catch (NotSupportedException e)
            {
                throw new ArgumentException(e.Message, nameof(fileName), e);
            }
            catch (System.Security.SecurityException e)
            {
                throw new IOException(e.Message, e);
            }
            catch (PathTooLongException e)
            {
                throw new ArgumentException(e.Message, nameof(fileName), e);
            }
            catch (Exception e)
            {
                throw new IOException(e.Message, e);
            }
        }


        /// <summary>
        /// Parst einen <see cref="string"/>, der vCard-Daten enthält.
        /// </summary>
        /// <param name="content">Ein <see cref="string"/>, der VCF-Daten enthält.</param>
        /// <returns>Eine Collection geparster <see cref="VCard"/>-Objekte, die den Inhalt von <paramref name="content"/> darstellen.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="content"/> ist <c>null</c>.</exception>
        public static List<VCard> Parse(string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            using var reader = new StringReader(content);
            return DoParse(reader);
        }



        /// <summary>
        /// Deserialisiert eine Auflistung von <see cref="VCard"/>-Objekten mit einem <see cref="TextReader"/>.
        /// </summary>
        /// <param name="reader">Ein <see cref="TextReader"/>.</param>
        /// <returns>Eine Auflistung deserialisierter <see cref="VCard"/>-Objekte.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> ist <c>null</c>.</exception>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static List<VCard> Deserialize(TextReader reader)
            => DoParse(reader ?? throw new ArgumentNullException(nameof(reader)));



        private static List<VCard> DoParse(TextReader reader, VCdVersion versionHint = VCdVersion.V2_1)
        {
            Debug.Assert(reader != null);
            DebugWriter.WriteMethodHeader(nameof(VCard) + nameof(DoParse) + "(TextReader)");

            var info = new VCardDeserializationInfo();
            var vCardList = new List<VCard>();
            var vcfReader = new VcfReader(reader, info);
            var queue = new Queue<VcfRow>(DESERIALIZER_QUEUE_INITIAL_CAPACITY);

            do
            {
                foreach (VcfRow vcfRow in vcfReader)
                {
                    queue.Enqueue(vcfRow);
                }

                if (queue.Count != 0)
                {
                    var vCard = new VCard(queue, info, versionHint);
                    vCardList.Add(vCard);

                    Debug.WriteLine("");
                    Debug.WriteLine("", "Parsed " + nameof(VCard));
                    Debug.WriteLine("");
                    Debug.WriteLine(vCard);

                    queue.Clear();

                    if (info.Builder.Capacity > VCardDeserializationInfo.MAX_STRINGBUILDER_CAPACITY)
                    {
                        info.Builder.Clear().Capacity = VCardDeserializationInfo.INITIAL_STRINGBUILDER_CAPACITY;
                    }
                }
            } while (!vcfReader.EOF);

            VCard.Dereference(vCardList!);
            return vCardList;
        }



        private static VCard? ParseNestedVcard(string? content, VCardDeserializationInfo info, VCdVersion versionHint)
        {
            // Version 2.1 ist unmaskiert:
            content = versionHint == VCdVersion.V2_1 ? content : info.Builder.Clear().Append(content).UnMask(versionHint).ToString();

            using var reader = new StringReader(content);

            List<VCard>? list = DoParse(reader, versionHint);

            return list.FirstOrDefault();
        }



    }
}
