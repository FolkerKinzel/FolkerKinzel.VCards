﻿using FolkerKinzel.VCards.Intls.Deserializers;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class DataPropertyTests
{
    [TestMethod]
    public void DataPropertyTest3()
    {
        VcfRow row = VcfRow.Parse("PHOTO:", new VcfDeserializationInfo())!;
        var prop = DataProperty.Create(row, VCdVersion.V3_0);

        Assert.IsNull(prop.Value);
    }
}