﻿using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Syncs;
using FolkerKinzel.VCards.Tests;

namespace FolkerKinzel.VCards.Models.Tests;

[TestClass]
public class VCardClientPropertyTests
{
    [TestMethod]
    public void VCardClientTest1()
    {
        var prop = new AppIDProperty(new AppID(7, "http://folkerkinzel.de/"));

        var vcard = new VCard
        {
            AppIDs = prop
        };

        string s = vcard.ToVcfString(VCdVersion.V4_0);

        IList<VCard> list = VCard.ParseVcf(s);

        Assert.IsNotNull(list);
        Assert.AreEqual(1, list.Count);

        vcard = list[0];

        Assert.IsNotNull(vcard.AppIDs);

        AppIDProperty? prop2 = vcard.AppIDs!.First();

        Assert.IsNotNull(prop);
        Assert.AreEqual(prop2!.Value!.LocalID, prop!.Value?.LocalID);
        Assert.AreEqual(prop2!.Value!.GlobalID, prop!.Value?.GlobalID);
        Assert.IsNull(prop.Group);
        Assert.IsFalse(prop.IsEmpty);
    }

    [TestMethod]
    public void TryParseTest2()
    {
        var row = VcfRow.Parse("CLIENTPIDMAP:", new VcfDeserializationInfo());
        Assert.IsFalse(AppIDProperty.TryParse(row!, out _));

    }

    [TestMethod]
    public void IEnumerableTest1()
    {
        var prop = new AppIDProperty(new AppID(4, "https://contoso.com"));
        Assert.AreEqual(1, prop.AsWeakEnumerable().Count());
    }
}
