﻿using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Properties;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class RawDataTests
{
    [TestMethod]
    public void SwitchTest1()
    {
        var rel = new RawData(Array.Empty<byte>());
        rel.Switch(s => rel = null, null!, null!);
        Assert.IsNull(rel);
    }

    [TestMethod]
    public void ValueTest1()
    {
        var rel = new RawData("Hi");
        Assert.IsNotNull(rel.Object);
        Assert.IsNotNull(rel.String);
        Assert.IsNull(rel.Bytes);
        Assert.IsNull(rel.Uri);
    }


    [TestMethod]
    public void ValueTest2()
    {
        var rel = new RawData(new Uri("http://folker.de/"));
        Assert.IsNotNull(rel.Object);
        Assert.IsNull(rel.String);
        Assert.IsNull(rel.Bytes);
        Assert.IsNotNull(rel.Uri);
    }


    [TestMethod]
    public void ValueTest3()
    {
        var rel = new RawData(Array.Empty<byte>());
        Assert.IsNotNull(rel.Object);
        Assert.IsNull(rel.String);
        Assert.IsNotNull(rel.Bytes);
        Assert.IsNull(rel.Uri);
    }

    [TestMethod]
    public void ToStringTest1()
    {
        var prop = DataProperty.FromBytes([1, 2, 3]);
        string s = prop.ToString();
        StringAssert.Contains(s, "3 Bytes");
    }

    [TestMethod]
    public void ToStringTest2()
    {
        var prop = DataProperty.FromUri(new Uri("http://contoso.com"));
        string s = prop.ToString();
        StringAssert.Contains(s, "contoso");
    }

    [TestMethod]
    public void ToStringTest3()
    {
        const string passWord = "Simsalabim";
        var prop = DataProperty.FromText(passWord);
        string s = prop.ToString();
        StringAssert.Contains(s, passWord);
    }
}