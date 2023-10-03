﻿using FolkerKinzel.VCards.Intls.Deserializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FolkerKinzel.VCards.Models.PropertyParts;

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


    [TestMethod]
    public void ValueTest1()
    {
        var prop = DataProperty.FromText("abc");
        DataPropertyValue? val1 = prop.Value;
        DataPropertyValue? val2 = prop.Value;
        Assert.IsNotNull(val1);
        Assert.AreEqual(val1, val2);
        Assert.AreSame(val1, val2);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void FromUriTest1()
    {
        Assert.IsTrue(Uri.TryCreate("../relative", UriKind.Relative, out Uri? uri));
        _ = DataProperty.FromUri(uri);
    }

    [TestMethod]
    public void FromBytesTest1()
    {
        var prop = DataProperty.FromBytes(null, "blabla");
        Assert.AreEqual("application/octet-stream", prop.Parameters.MediaType);
    }
}