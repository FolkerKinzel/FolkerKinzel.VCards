﻿using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

public readonly struct AccessBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal AccessBuilder(VCardBuilder builder) => _builder = builder;

    /// <summary>
    /// Sets the <see cref="VCard.Access"/> property to an <see cref="AccessProperty"/> instance that is newly 
    /// initialized using the specified arguments.
    /// </summary>
    /// <param name="value">A member of the <see cref="Access" /> enum.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group. The function is called with the <see cref="VCardBuilder.VCard"/>
    /// instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="AccessBuilder"/> to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been initialized using the default constructor.</exception>
    public VCardBuilder Set(Access value,
                            Func<VCard, string?>? group = null)
    {
        Builder.VCard.Set(Prop.Access, new AccessProperty(value, group?.Invoke(_builder.VCard)));
        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.Access"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="AccessBuilder"/> to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had been initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.Access, null);
        return _builder;
    }

    ///// <inheritdoc/>
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public override bool Equals([NotNullWhen(true)] object? obj) => base.Equals(obj);

    ///// <inheritdoc/>
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public override int GetHashCode() => base.GetHashCode();

    ///// <inheritdoc/>
    //[EditorBrowsable(EditorBrowsableState.Never)]
    //public override string ToString() => base.ToString()!;
}
