using System.Globalization;
using FolkerKinzel.VCards.Enums;

namespace FolkerKinzel.VCards.Intls.Converters;

internal static class GeoCoordinateConverter
{
    private const string FLOAT_FORMAT = "0.0#####";

    internal static void AppendTo(StringBuilder builder, GeoCoordinate? coordinate, VCdVersion version)
    {
        Debug.Assert(builder != null);

        if (coordinate is null)
        {
            return;
        }

        CultureInfo culture = CultureInfo.InvariantCulture;

        string latitude = coordinate.Latitude.ToString(FLOAT_FORMAT, culture);
        string longitude = coordinate.Longitude.ToString(FLOAT_FORMAT, culture);

        switch (version)
        {
            case VCdVersion.V2_1:
            case VCdVersion.V3_0:
                _ = builder.Append(latitude).Append(';').Append(longitude);
                break;
            default:
                _ = builder.Append("geo:").Append(latitude).Append(',').Append(longitude);
                if(coordinate.Uncertainty.HasValue) 
                {
                    _ = builder.Append(";u=").Append(coordinate.Uncertainty.Value.ToString("0.",culture));
                }
                break;
        }//switch
    }
}
