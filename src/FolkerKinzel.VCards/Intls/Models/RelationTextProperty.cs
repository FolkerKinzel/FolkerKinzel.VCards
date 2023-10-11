﻿using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Models;

/// <summary>
/// Spezialisierung der <see cref="RelationProperty"/>-Klasse, um den Namen einer Person, zu der eine Beziehung besteht, anzugeben.
/// </summary>
internal sealed class RelationTextProperty : RelationProperty
{
    private readonly TextProperty _textProp;

    internal RelationTextProperty(TextProperty textProp)
       : base(textProp.Parameters,
              textProp.Group) => _textProp = textProp;


    internal RelationTextProperty(VcfRow vcfRow, VCdVersion version)
        : base(vcfRow.Parameters,
               vcfRow.Group) => _textProp = new TextProperty(vcfRow, version);


    /// <summary>
    /// Die von der <see cref="RelationTextProperty"/> zur Verfügung gestellten Daten.
    /// </summary>
    public new string? Value => _textProp.Value;

    
    /// <inheritdoc/>
    [MemberNotNullWhen(false, nameof(Value))]
    public override bool IsEmpty => _textProp.IsEmpty;


    internal override void PrepareForVcfSerialization(VcfSerializer serializer)
    {
        Debug.Assert(serializer != null);
        _textProp.PrepareForVcfSerialization(serializer);
        Parameters.DataType = VCdDataType.Text;
    }


    internal override void AppendValue(VcfSerializer serializer)
     => _textProp.AppendValue(serializer);


    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override object Clone() => new RelationTextProperty((TextProperty)_textProp.Clone());

}