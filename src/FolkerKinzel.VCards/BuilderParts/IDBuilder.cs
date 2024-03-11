﻿using System.ComponentModel;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

/// <summary>
/// Provides methods for editing the <see cref="VCard.ID"/> property.
/// </summary>
/// <remarks>
/// <note type="important">
/// Only use this structure in conjunction with <see cref="VCardBuilder"/>!
/// </note>
/// </remarks>
[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals",
    Justification = "Overriding does not change the default behavior.")]
public readonly struct IDBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal IDBuilder(VCardBuilder builder) => _builder = builder;

    /// <summary>
    /// Allows to edit the content of the <see cref="VCard.ID"/> property with a specified delegate.
    /// </summary>
    /// <param name="action">An <see cref="Action{T}"/> delegate that's invoked with the content of the 
    /// <see cref="VCard.ID"/> property as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="IDBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit(Action<IDProperty?> action)
    {
        var prop = Builder.VCard.ID;
        _ArgumentNullException.ThrowIfNull(action, nameof(action));
        action.Invoke(prop);
        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.ID"/> property to a <see cref="IDProperty"/> instance that is newly 
    /// initialized from a new <see cref="Guid"/>.
    /// </summary>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty"
    /// /> objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c>
    /// to indicate that the <see cref="VCardProperty" /> does not belong to any group. The function is 
    /// called with the <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="IDBuilder"/> to 
    /// be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Set(Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        var vc = Builder.VCard;
        var property = new IDProperty(group?.Invoke(vc));
        parameters?.Invoke(property.Parameters);

        vc.Set(Prop.ID, property);
        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.ID"/> property to a <see cref="IDProperty"/> instance that is newly 
    /// initialized using a specified <see cref="Guid"/>.
    /// </summary>
    /// <param name="value">A <see cref="Guid" /> value.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" />
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that 
    /// the <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="IDBuilder"/> to 
    /// be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Set(Guid value,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        var vc = Builder.VCard;
        var property = new IDProperty(value, group?.Invoke(vc));
        parameters?.Invoke(property.Parameters);

        vc.Set(Prop.ID, property);
        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.ID"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="IDBuilder"/> to 
    /// be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.ID, null);
        return _builder;
    }

    // Overriding Equals, GetHashCode and ToString to hide these methods in IntelliSense:

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals([NotNullWhen(true)] object? obj) => base.Equals(obj);

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString()!;

}
