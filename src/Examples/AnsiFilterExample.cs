﻿// Compile for .NET 6.0 or above and FolkerKinzel.VCards 5.0.0 or above
using System.Diagnostics;
using System.Text;
using FolkerKinzel.VCards;

namespace Examples;

public static class AnsiFilterExample
{
    /// <summary>
    /// The example loads several vCard 2.1 files which have different encodings and 
    /// shows their content in the text editor. The encoding is selected automatically.
    /// </summary>
    /// <remarks>
    /// Download the example files at
    /// https://github.com/FolkerKinzel/VCards/tree/31a5d5a88ad2ae5a6b27b4fb212ab53a9f8c8f92/src/Examples/MultiAnsiFilterTests
    /// </remarks>
    /// <param name="directoryPath">Path to the directory containing the example files.</param>
    public static void LoadVcfFilesWhichHaveDifferentAnsiEncodings(string directoryPath)
    {
        // To load VCF files that could be ANSI encoded automatically with the right encoding,
        // use the AnsiFilter class with the ANSI codepage which is most likely. In our example
        // we choose windows-1255 (Hebrew).
        // AnsiFilter switches - depending of the content of the VCF file - automatically between
        // UTF-8 and this code page.
        var ansiFilter = new AnsiFilter(1255);

        //// In most cases the AnsiFilter class is the best choice. In our example we have the
        //// special case of vCard 2.1 files with different ANSI encodings. So we wrap the 
        //// AnsiFilter object with a MultiAnsiFilter to take the CHARSET parameters of the
        //// vCard 2.1 files into account. Keep in mind that CHARSET parameters exist only in
        //// vCard 2.1. 
        //var multiAnsiFilter = new MultiAnsiFilter(ansiFilter);

        var outFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".txt");
        using (StreamWriter writer = File.AppendText(outFileName))
        {
            foreach (string vcfFileName in Directory
                .EnumerateFiles(directoryPath)
                .Where(x => StringComparer.OrdinalIgnoreCase.Equals(Path.GetExtension(x), ".vcf")))
            {
                IList<VCard> vCards = ansiFilter.LoadVcf(vcfFileName, out string encodingWebName);
                WriteToTextFile(vcfFileName, vCards, encodingWebName, writer);
            }
        }
        ShowInTextEditorAndDelete(outFileName);
    }
           
    private static void WriteToTextFile(string vcfFileName, IList<VCard> vCards, string encodingWebName, TextWriter writer)
    {
        const string indent = "    ";
        writer.Write(Path.GetFileName(vcfFileName));
        writer.WriteLine(':');
        writer.Write(indent);
        writer.WriteLine(vCards.FirstOrDefault()?.DisplayNames?.FirstOrDefault()?.Value);
        writer.Write(indent);
        writer.Write("Encoding: ");
        writer.WriteLine(encodingWebName);
        writer.WriteLine();
    }

    private static void ShowInTextEditorAndDelete(string outFileName)
    {
        Process.Start(new ProcessStartInfo { FileName = outFileName, UseShellExecute = true })?
               .WaitForExit();
        File.Delete(outFileName);
    }
}

/*
Output:

 German.vcf:
    Sören Täve Nüßlebaum
    Encoding: windows-1252

Greek.vcf:
    Βαγγέλης
    Encoding: windows-1253

Hebrew.vcf:
    אפרים קישון
    Encoding: windows-1255

Ukrainian.vcf:
    Віталій Володимирович Кличко
    Encoding: windows-1251

utf-8.vcf:
    孔夫子
    Encoding: utf-8

Please note that Hebrew.vcf and utf-8.vcf have been read properly without
any CHARSET parameter in the VCF files: UTF-8 is the default charset and 
windows-1255 (Hebrew) had been set as the default fallback value in the 
AnsiFilter constructor.
 */
