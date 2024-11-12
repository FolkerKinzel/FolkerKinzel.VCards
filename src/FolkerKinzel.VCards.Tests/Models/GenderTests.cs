﻿using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class GenderTests
{
    [DataTestMethod]
    [DataRow(null, null, true)]
    [DataRow(null, "Other", false)]
    [DataRow(Sex.Unknown, null, false)]
    public void IsEmptyTest1(Sex? gender, string? identity, bool expected)
    {
        var info = new Gender(gender, identity);
        Assert.AreEqual(expected, info.IsEmpty);
    }
}