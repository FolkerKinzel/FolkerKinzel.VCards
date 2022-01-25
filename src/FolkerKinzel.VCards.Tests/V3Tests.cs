﻿using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Tests;

[TestClass]
public class V3Tests
{
    [TestMethod]
    public void ParseTest()
    {
        IList<VCard>? vcard = VCard.LoadVcf(TestFiles.V3vcf);

        Assert.IsNotNull(vcard);
        Assert.AreEqual(2, vcard.Count);
    }


    [TestMethod]
    public void ParseTest2()
    {
        IList<VCard>? vcard = VCard.LoadVcf(@"C:\Users\fkinz\OneDrive\Kontakte\Thunderbird\21-01-13.vcf");

        Assert.IsNotNull(vcard);
        Assert.AreNotEqual(0, vcard.Count);
    }

    [TestMethod]
    public void ParseTest3()
    {
        IList<VCard>? vcard = VCard.LoadVcf(TestFiles.PhotoV3vcf);

        Assert.IsNotNull(vcard);
        Assert.AreNotEqual(0, vcard.Count);
    }


    [TestMethod]
    public void WriteEmptyVCardTest()
    {
        var vcard = new VCard();

        string s = vcard.ToVcfString(VCdVersion.V3_0);

        IList<VCard>? cards = VCard.ParseVcf(s);

        Assert.AreEqual(cards.Count, 1);

        vcard = cards[0];

        Assert.AreEqual(VCdVersion.V3_0, vcard.Version);

        Assert.IsNotNull(vcard.DisplayNames);
        Assert.AreEqual(vcard.DisplayNames!.Count(), 1);
        Assert.IsNotNull(vcard.DisplayNames!.First());
        Assert.IsNotNull(vcard.NameViews);
        Assert.AreEqual(vcard.NameViews!.Count(), 1);
        Assert.IsNotNull(vcard.NameViews!.First());
    }


    [TestMethod]
    public void LineWrappingTest()
    {
        var vcard = new VCard();

        string UNITEXT = "Dies ist ein wirklich sehr sehr sehr langer Text mit ü, Ö, und ä " + "" +
            "damit das Line-Wrappping getestet werden kann. " + Environment.NewLine +
            "Um noch eine Zeile einzufügen, folgt hier noch ein Satz. ";

        const string ASCIITEXT = "This is really a very very long ASCII-Text. This is needed to test the " +
            "LineWrapping. That's why I have to write so much even though I have nothing to say." +
            "For a better test, I write the same again: " +
            "This is really a very very long ASCII-Text. This is needed to test the " +
            "LineWrapping. That's why I have to write so much even though I have nothing to say.";

        byte[] bytes = CreateBytes();

        vcard.Notes = new TextProperty[]
        {
                new TextProperty(UNITEXT)
        };

        vcard.Keys = new DataProperty[] { new DataProperty(DataUrl.FromText(ASCIITEXT)) };

        vcard.Photos = new DataProperty[] { new DataProperty(DataUrl.FromBytes(bytes, "image/jpeg")) };

        string s = vcard.ToVcfString(VCdVersion.V3_0);


        Assert.IsNotNull(s);

        Assert.IsTrue(s.Split(new string[] { VCard.NewLine }, StringSplitOptions.None)
            .All(x => x != null && x.Length <= VCard.MAX_BYTES_PER_LINE));

        _ = VCard.ParseVcf(s);

        Assert.AreEqual(((DataUrl?)vcard.Keys?.First()?.Value)?.GetEmbeddedText(), ASCIITEXT);
        Assert.AreEqual(vcard.Photos?.First()?.Parameters.MediaType, "image/jpeg");
        Assert.IsTrue(((DataUrl?)vcard.Photos?.First()?.Value)?.GetEmbeddedBytes()?.SequenceEqual(bytes) ?? false);


        static byte[] CreateBytes()
        {
            const int DATA_LENGTH = 200;

            var arr = new byte[DATA_LENGTH];

            for (byte i = 0; i < arr.Length; i++)
            {
                arr[i] = i;
            }

            return arr;
        }

    }

    [TestMethod]
    public void SaveTest()
    {
        var vcard = new VCard();

        string UNITEXT = "Dies ist ein wirklich sehr sehr sehr langer Text mit ü, Ö, und ä " + "" +
            "damit das Line-Wrappping getestet werden kann. " + Environment.NewLine +
            "Um noch eine Zeile einzufügen, folgt hier noch ein Satz. ";

        vcard.NameViews = new NameProperty[]
        {
                new NameProperty("Test", "Paul", null, null, null)
        };

        vcard.DisplayNames = new TextProperty[]
        {
                new TextProperty("Paul Test")
        };

        vcard.Notes = new TextProperty[]
        {
                new TextProperty(UNITEXT)
        };

        vcard.SaveVcf(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Paul Test.vcf"));
    }


    [TestMethod]
    public void SerializeVCard()
    {
        string s = Utility.CreateVCard().ToVcfString(VCdVersion.V3_0, options: VcfOptions.All);

        Assert.IsNotNull(s);

        IList<VCard> list = VCard.ParseVcf(s);

        Assert.IsNotNull(list);

        Assert.AreEqual(1, list.Count);

        VCard vcard = list[0];

        Assert.AreEqual(VCdVersion.V3_0, vcard.Version);
        Assert.IsNotNull(vcard.DirectoryName);
        Assert.IsNotNull(vcard.Mailer);
    }


    [TestMethod]
    public void TimeDataTypeTest()
    {
        const string vcardString = @"BEGIN:VCARD
VERSION:3.0
BDAY;Value=Time:05:30:00
END:VCARD";

        VCard vcard = VCard.ParseVcf(vcardString)[0];

        Assert.IsNotNull(vcard.BirthDayViews);

        DateTimeProperty? bday = vcard.BirthDayViews!.First();

        if (bday is DateTimeOffsetProperty prop)
        {
            Assert.AreEqual(prop?.Value?.TimeOfDay, new TimeSpan(5, 30, 0));
        }
        else
        {
            Assert.Fail();
        }
    }

}
