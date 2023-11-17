﻿using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Extensions;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;

namespace FolkerKinzel.VCards;

public sealed partial class VCardBuilder
{
    public VCardBuilder SetDirectoryName(string? value)
    {
        _vCard.DirectoryName = new TextProperty(value);
        return this;
    }

    public VCardBuilder ClearDirectoryName()
    {
        _vCard.DirectoryName = null;
        return this;
    }
}
