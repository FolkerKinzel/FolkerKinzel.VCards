using System.Collections;
using System.Runtime.InteropServices;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Intls.Extensions;

namespace FolkerKinzel.VCards.Intls.Deserializers;

internal static class ValueSplitter
{
    public static IEnumerable<string> Split(ReadOnlyMemory<char> mem,
                                            char splitChar,
                                            StringSplitOptions options,
                                            bool unMask,
                                            VCdVersion version)
    {
        if (mem.IsEmpty)
        {
            if(!options.HasFlag(StringSplitOptions.RemoveEmptyEntries))
            {
                yield return string.Empty;
            }

            yield break;
        }

        while (true)
        {
            int splitIndex = GetNextSplitIndex(mem.Span, splitChar);

            ReadOnlyMemory<char> nextChunk = mem.Slice(0, splitIndex);

            if (!options.HasFlag(StringSplitOptions.RemoveEmptyEntries) || !nextChunk.Span.IsWhiteSpace())
            {
                yield return unMask
                             ? nextChunk.Span.UnMask(version)
                             : nextChunk.ToString();
            }

            if (splitIndex == mem.Length)
            {
                yield break;
            }

            mem = mem.Slice(splitIndex + 1);
        }
    }

    public static IEnumerable<ReadOnlyMemory<char>> Split(ReadOnlyMemory<char> mem,
                                                          char splitChar)
    {
        if(mem.IsEmpty)
        {
            yield break ;
        }

        while (true)
        {
            int splitIndex = GetNextSplitIndex(mem.Span, splitChar);
            ReadOnlyMemory<char> nextChunk = mem.Slice(0, splitIndex);

            yield return nextChunk;

            if (splitIndex == mem.Length)
            {
                yield break;
            }

            mem = mem.Slice(splitIndex + 1);
        }
    }

    private static int GetNextSplitIndex(ReadOnlySpan<char> span, char splitChar)
    {
        bool masked = false;

        for (int i = 0; i < span.Length; i++)
        {
            if (masked)
            {
                masked = false;
                continue;
            }

            char c = span[i];

            if (c == splitChar)
            {
                return i;
            }

            if (c == '\\')
            {
                masked = true;
            }

        }//for

        return span.Length;
    }
}


