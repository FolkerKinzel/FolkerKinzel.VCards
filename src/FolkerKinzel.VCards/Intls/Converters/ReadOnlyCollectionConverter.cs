using System.Collections.ObjectModel;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class ReadOnlyCollectionConverter
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ReadOnlyCollection<string> ToReadOnlyCollection(string? s)
        => string.IsNullOrWhiteSpace(s) ? ReadOnlyCollectionString.Empty : new ReadOnlyCollection<string>(new string[] { s });

    internal static ReadOnlyCollection<string> ToReadOnlyCollection(IEnumerable<string?>? coll)
    {
        return coll is null || !coll.Any(x => !string.IsNullOrWhiteSpace(x))
            ? ReadOnlyCollectionString.Empty
            : coll.All(x => !string.IsNullOrWhiteSpace(x))
                  ? coll is ReadOnlyCollection<string> roc
                          ? roc
                          : coll is IList<string> list
                                 ? new ReadOnlyCollection<string>(list)
                                 : new ReadOnlyCollection<string>(coll.ToArray()!)
                  : new ReadOnlyCollection<string>(coll.Where(x => !string.IsNullOrWhiteSpace(x))
                                                       .ToArray()!);
    }
}
