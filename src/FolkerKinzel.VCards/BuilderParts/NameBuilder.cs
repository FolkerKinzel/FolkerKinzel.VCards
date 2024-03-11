﻿using System.ComponentModel;
using System.Xml.Linq;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.VCards.Resources;

namespace FolkerKinzel.VCards.BuilderParts;

/// <summary>
/// Provides methods for editing the <see cref="VCard.NameViews"/> property.
/// </summary>
/// <remarks>
/// <note type="important">
/// Only use this structure in conjunction with <see cref="VCardBuilder"/>!
/// </note>
/// </remarks>
[SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals",
    Justification = "Overriding does not change the default behavior.")]
public readonly struct NameBuilder
{
    private readonly VCardBuilder? _builder;

    [MemberNotNull(nameof(_builder))]
    private VCardBuilder Builder => _builder ?? throw new InvalidOperationException(Res.DefaultCtor);

    internal NameBuilder(VCardBuilder builder) => _builder = builder;

    /// <summary>
    /// Allows to edit the items of the <see cref="VCard.NameViews"/> property with a specified delegate.
    /// </summary>
    /// <param name="action">An <see cref="Action{T}"/> delegate that's invoked with the items of the 
    /// <see cref="VCard.NameViews"/> property that are not <c>null</c>.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Edit(Action<IEnumerable<NameProperty>> action)
    {
        var props = Builder.VCard.NameViews?.WhereNotNull() ?? [];
        _ArgumentNullException.ThrowIfNull(action, nameof(action));
        action.Invoke(props);
        return _builder;
    }

    /// <summary>
    /// Adds a <see cref="NameProperty"/> instance, which is newly 
    /// initialized using the specified arguments, to the <see cref="VCard.NameViews"/> property.
    /// </summary>
    /// <param name="familyNames">Family Names (also known as surnames)</param>
    /// <param name="givenNames">Given Names (first names)</param>
    /// <param name="additionalNames">Additional Names (middle names)</param>
    /// <param name="prefixes">Honorific Prefixes</param>
    /// <param name="suffixes">Honorific Suffixes</param>
    /// <param name="displayName">An <see cref="Action{T1, T2}"/> delegate that's invoked with the 
    /// <see cref="TextBuilder"/> the <see cref="VCardBuilder.DisplayNames"/> property returns and 
    /// the newly created <see cref="NameProperty"/> instance as arguments.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" />
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that the 
    /// <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(IEnumerable<string?>? familyNames = null,
                            IEnumerable<string?>? givenNames = null,
                            IEnumerable<string?>? additionalNames = null,
                            IEnumerable<string?>? prefixes = null,
                            IEnumerable<string?>? suffixes = null,
                            Action<TextBuilder, NameProperty>? displayName = null,
                            Action<ParameterSection>? parameters = null,
                            Func<VCard, string?>? group = null)
    {
        var vc = Builder.VCard;
        var prop = new NameProperty(familyNames,
                                    givenNames,
                                    additionalNames,
                                    prefixes,
                                    suffixes, group?.Invoke(vc));
        vc.Set(Prop.NameViews,
               VCardBuilder.Add(prop,
               vc.Get<IEnumerable<NameProperty?>?>(Prop.NameViews),
               parameters,
               false));

        displayName?.Invoke(Builder.DisplayNames, prop);

        return _builder;
    }

    /// <summary>
    /// Adds a <see cref="NameProperty"/> instance, which is newly 
    /// initialized using the specified arguments, to the <see cref="VCard.NameViews"/> property.
    /// </summary>
    /// <param name="familyName">Family Name (also known as surname)</param>
    /// <param name="givenName">Given Name (first name)</param>
    /// <param name="additionalName">Additional Name (middle name)</param>
    /// <param name="prefix">Honorific Prefix</param>
    /// <param name="suffix">Honorific Suffix</param>
    /// <param name="displayName">An <see cref="Action{T1, T2}"/> delegate that's invoked with the 
    /// <see cref="TextBuilder"/> the <see cref="VCardBuilder.DisplayNames"/> property returns and the newly
    /// created <see cref="NameProperty"/> instance as arguments.</param>
    /// <param name="parameters">An <see cref="Action{T}"/> delegate that's invoked with the 
    /// <see cref="ParameterSection"/> of the newly created <see cref="VCardProperty"/> as argument.</param>
    /// <param name="group">A function that returns the identifier of the group of <see cref="VCardProperty" />
    /// objects, which the <see cref="VCardProperty" /> should belong to, or <c>null</c> to indicate that 
    /// the <see cref="VCardProperty" /> does not belong to any group. The function is called with the 
    /// <see cref="VCardBuilder.VCard"/> instance as argument.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameBuilder"/> to 
    /// be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Add(string? familyName,
                            string? givenName = null,
                            string? additionalName = null,
                            string? prefix = null,
                            string? suffix = null,
                            Action<TextBuilder, NameProperty>? displayName = null,
                            Func<VCard, string?>? group = null,
                            Action<ParameterSection>? parameters = null)
    {
        var vc = Builder.VCard;
        var prop = new NameProperty(familyName,
                                    givenName,
                                    additionalName,
                                    prefix,
                                    suffix,
                                    group?.Invoke(vc));
        vc.Set(Prop.NameViews,
               VCardBuilder.Add(prop,
                                vc.Get<IEnumerable<NameProperty?>?>(Prop.NameViews),
                                parameters,
                                false));

        displayName?.Invoke(Builder.DisplayNames, prop);

        return _builder;
    }

    /// <summary>
    /// Sets the <see cref="VCard.NameViews"/> property to <c>null</c>.
    /// </summary>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameBuilder"/> 
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Clear()
    {
        Builder.VCard.Set(Prop.NameViews, null);
        return _builder;
    }

    /// <summary>
    /// Removes <see cref="NameProperty"/> objects that match a specified predicate from the 
    /// <see cref="VCard.NameViews"/> property.
    /// </summary>
    /// <param name="predicate">A function that returns <c>true</c> for <see cref="NameProperty"/> 
    /// objects that shall be removed.</param>
    /// <returns>The <see cref="VCardBuilder"/> instance that initialized this <see cref="NameBuilder"/>
    /// to be able to chain calls.</returns>
    /// <exception cref="InvalidOperationException">The method has been called on an instance that had 
    /// been initialized using the default constructor.</exception>
    public VCardBuilder Remove(Func<NameProperty, bool> predicate)
    {
        Builder.VCard.Set(Prop.NameViews, 
                          _builder.VCard.Get<IEnumerable<NameProperty?>?>(Prop.NameViews)
                                        .Remove(predicate)
                          );
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

