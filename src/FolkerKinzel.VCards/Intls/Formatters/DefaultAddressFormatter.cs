﻿using FolkerKinzel.VCards.Formatters;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Intls.Formatters;

internal sealed class DefaultAddressFormatter : IAddressFormatter
{
    public string? ToLabel(AddressProperty addressProperty)
    {
        _ArgumentNullException.ThrowIfNull(addressProperty, nameof(addressProperty));
        return AddressLabelFormatter.ToLabel(addressProperty);
    }
}