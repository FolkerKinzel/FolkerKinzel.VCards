using System.Globalization;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.PropertyParts;
using FolkerKinzel.Strings.Polyfills;
using FolkerKinzel.VCards.Intls;

namespace FolkerKinzel.VCards;

/// <summary>Encapsulates information about the geographical position.</summary>
public sealed class GeoCoordinate : IEquatable<GeoCoordinate?>
{
    private const double _6 = 0.000001;

    /// <summary>
    /// Distance in meters for 1� at the Equator.
    /// </summary>
    private const double ONE_DEGREE_DISTANCE = 111300;

    /// <summary>
    /// Minimum recognized distance (11,13 cm).
    /// </summary>
    private const double MIN_DISTANCE = ONE_DEGREE_DISTANCE * _6;
    private const string GEO_URI_PROTOCOL = "geo:";

    /// <summary> Initializes a new <see cref="GeoCoordinate" /> objekt. </summary>
    /// <param name="latitude">Latitude (value between -90 and 90).</param>
    /// <param name="longitude">Longitude (value between -180 and 180).</param>
    /// <param name="uncertainty">The amount of uncertainty in the location as a 
    /// value in meters, or <c>null</c> to leave this unspecified.</param>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="latitude" />
    /// or <paramref name="longitude" /> does not have a valid value.</exception>
    /// <seealso cref="GeoProperty"/>
    /// <seealso cref="VCard.GeoCoordinates"/>
    /// <seealso cref="ParameterSection.GeoPosition"/>
    public GeoCoordinate(double latitude, double longitude, double? uncertainty = null)
    {
        if (double.IsNaN(latitude) || latitude < -100.0 || latitude > 100.0)
        {
            throw new ArgumentOutOfRangeException(nameof(latitude));
        }

        if (double.IsNaN(longitude) || longitude < -200.0 || longitude > 200.0)
        {
            throw new ArgumentOutOfRangeException(nameof(longitude));
        }

        if (uncertainty.HasValue)
        {
            double val = uncertainty.Value;

            if (double.IsNaN(val) || val < 0.0 || double.IsInfinity(val))
            {
                throw new ArgumentOutOfRangeException(nameof(uncertainty));
            }

            Uncertainty = (float)Math.Round(val, 1, MidpointRounding.ToEven);
        }

        NormalizeLatitude(ref latitude, ref longitude);
        longitude = NormalizeLongitude(longitude);

        Latitude = Math.Round(latitude, 6, MidpointRounding.ToEven);
        Longitude = Math.Round(longitude, 6, MidpointRounding.ToEven);

        static double NormalizeLongitude(double longitude)
        {
            // RFC 5870, 3.4.4
            // -180 and 180 are equal
            if (longitude <= -180.0)
            {
                longitude += 360.0;
            }
            else if (longitude > 180.0)
            {
                longitude -= 360.0;
            }

            return longitude;
        }

        static void NormalizeLatitude(ref double latitude, ref double longitude)
        {
            // flying over the pole
            if (latitude > 90.0)
            {
                latitude = 180.0 - latitude;
                longitude = WrapLongitude(longitude);
            }
            else if (latitude < -90.0)
            {
                latitude = -180.0 - latitude;
                longitude = WrapLongitude(longitude);
            }

            // Longitude should be '0' at the poles
            // See RFC 5870, 3.4.2
            if (90.0 - Math.Abs(latitude) < _6)
            {
                longitude = 0;
            }

            static double WrapLongitude(double longitude)
                => longitude > 0.0 ? longitude - 180.0 : longitude + 180.0;
        }
    }

    /// <summary>Latitude.</summary>
    public double Latitude { get; }

    /// <summary>Longitude.</summary>
    public double Longitude { get; }

    /// <summary>
    /// The amount of uncertainty in the location as a value in meters, or <c>null</c>
    /// if this is not specified.
    /// </summary>
    /// <remarks>If the value is <c>null</c> or zero, this implementation has a minimal 
    /// uncertainty of about 12 cm.</remarks>
    public float? Uncertainty { get; }

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) => Equals(obj as GeoCoordinate);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals([NotNullWhen(true)] GeoCoordinate? other)
        => other != null
           && other.Latitude == Latitude
           && other.Longitude == Longitude
           && other.Uncertainty == Uncertainty;

    /// <summary>
    /// Indicates whether the current object desribes a geographical position that is 
    /// equal to that of <paramref name="other"/>.
    /// </summary>
    /// <param name="other">A <see cref="GeoCoordinate"/> object to compare with the 
    /// current instance.</param>
    /// 
    /// <returns><c>true</c> if the geographical position of <paramref name="other"/>
    /// is equal to that of the current instance, otherwise <c>false</c>.</returns>
    public bool IsSamePosition(GeoCoordinate other)
    {
        _ArgumentNullException.ThrowIfNull(other, nameof(other));

        double minDist = (Uncertainty ?? 0) + (other.Uncertainty ?? 0);
        return ComputeDistanceToCompareEquality(other) < (minDist < 0.2 ? MIN_DISTANCE : minDist);
    }

    public static bool AreSamePosition(GeoCoordinate coordinate1, GeoCoordinate coordinate2)
        => coordinate1?.IsSamePosition(coordinate2) ?? throw new ArgumentNullException(nameof(coordinate1));

    /// <summary>
    /// Computes the distance between this and <paramref name="other"/> in <c>m</c> simply with 
    /// Pythagoras (that's precisely enough for very short distances).
    /// </summary>
    /// <param name="other">The <see cref="GeoCoordinate"/> to compare with.</param>
    /// <returns>The distance in <c>m</c> between this and <paramref name="other"/>.</returns>
    private double ComputeDistanceToCompareEquality(GeoCoordinate other)
    {
        double diffLat = ONE_DEGREE_DISTANCE * (Latitude - other.Latitude);

        // radians of the average latitude
        double latRad = (Latitude + other.Latitude) * (Math.PI / 360);

        double diffAngleLong = Math.Abs(Longitude - other.Longitude);

        // take the shortest direction around the globe:
        if (diffAngleLong > 180.0)
        {
            diffAngleLong = 360.0 - diffAngleLong;
        }

        double diffLong = ONE_DEGREE_DISTANCE * Math.Cos(latRad) * diffAngleLong;

        return Math.Sqrt(diffLat * diffLat + diffLong * diffLong);
    }

    /// <summary>
    /// Overloads the equality operator for <see cref="GeoCoordinate"/> instances.
    /// </summary>
    /// <param name="left">The left <see cref="GeoCoordinate"/> object or <c>null</c>.</param>
    /// <param name="right">The right <see cref="GeoCoordinate"/> object or <c>null</c>.</param>
    /// <returns><c>true</c> if the values of <paramref name="left"/> and <paramref name="right"/>
    /// are equal, otherwise <c>false</c>.</returns>
    public static bool operator ==(GeoCoordinate? left, GeoCoordinate? right)
        => ReferenceEquals(left, right) || (left?.Equals(right) ?? false);

    /// <summary>
    /// Overloads the not-equal-to operator for <see cref="GeoCoordinate"/> instances.
    /// </summary>
    /// <param name="left">The left <see cref="GeoCoordinate"/> object or <c>null</c>.</param>
    /// <param name="right">The right <see cref="GeoCoordinate"/> object or <c>null</c>.</param>
    /// <returns><c>true</c> if the values of <paramref name="left"/> and <paramref name="right"/>
    /// are not equal, otherwise <c>false</c>.</returns>
    public static bool operator !=(GeoCoordinate? left, GeoCoordinate? right)
        => !(left == right);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => HashCode.Combine(Latitude, Longitude, Uncertainty);

    /// <inheritdoc/>
    public override string ToString()
    {
        string latitude = Latitude.ToString("F6");
        string longitude = Longitude.ToString("F6");

        if (Uncertainty.HasValue)
        {
            string m = Uncertainty.Value >= 0.1 ? "m" : "";

            return $"""
                Latitude:    {latitude}
                Longitude:   {longitude}
                Uncertainty: {Uncertainty.Value:F1} {m}
                """;
        }

        return $"""
                Latitude:  {latitude,11}
                Longitude: {longitude,11}
                """;
    }

    internal static bool TryParse(ReadOnlySpan<char> value, [NotNullWhen(true)] out GeoCoordinate? coordinate)
    {
        coordinate = null;

        value = value.TrimStart();

        if (value.IsEmpty)
        {
            return false;
        }

        if (IsGeoUri(value))
        {
            return TryParseGeoUri(value, out coordinate);
        }

        int splitIndex = value.IndexOf(';');

        if (splitIndex == -1) 
        {
            return false;
        }

        NumberStyles numStyle = NumberStyles.AllowDecimalPoint
                                  | NumberStyles.AllowLeadingSign
                                  | NumberStyles.AllowLeadingWhite
                                  | NumberStyles.AllowTrailingWhite;

        CultureInfo culture = CultureInfo.InvariantCulture;

        if (!_Double.TryParse(value.Slice(0, splitIndex), numStyle, culture, out double latitude)
            || !_Double.TryParse(value.Slice(splitIndex + 1), numStyle, culture, out double longitude))
        {
            return false;
        }

        try
        {
            coordinate = new GeoCoordinate(latitude, longitude);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsGeoUri(ReadOnlySpan<char> value) => value.StartsWith(GEO_URI_PROTOCOL, StringComparison.OrdinalIgnoreCase);


    [SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
    private static bool TryParseGeoUri(ReadOnlySpan<char> value, [NotNullWhen(true)] out GeoCoordinate? coordinate)
    {
        coordinate = default;

        value = value.Slice(GEO_URI_PROTOCOL.Length);

        int splitIndex = value.IndexOf(',');

        if (splitIndex == -1) { return false; }

        NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;

        if (!_Double.TryParse(value.Slice(0, splitIndex),
                              styles,
                              CultureInfo.InvariantCulture,
                              out double latitude))
        {
            return false;
        }

        value = value.Slice(splitIndex + 1);

        splitIndex = value.IndexOfAny(",;");

        if (!_Double.TryParse(splitIndex == -1 ? value : value.Slice(0, splitIndex),
                              styles,
                              CultureInfo.InvariantCulture,
                              out double longitude))
        {
            return false;
        }
        double? uncertainty = null;

        if (splitIndex != -1)
        {

            value = value.Slice(splitIndex);

            int uParameterStart = value.IndexOf(GeoCoordinateConverter.U_PARAMETER, StringComparison.OrdinalIgnoreCase);

            if (uParameterStart != -1)
            {
                value = value.Slice(uParameterStart + GeoCoordinateConverter.U_PARAMETER.Length);

                int uParameterEnd = value.IndexOf(';');

                if (uParameterEnd != -1)
                {
                    value = value.Slice(0, uParameterEnd);
                }

                if (_Double.TryParse(value,
                                  styles,
                                  CultureInfo.InvariantCulture,
                                  out double uValue))
                {
                    uncertainty = uValue;
                }
            }
        }

        try
        {
            coordinate = new GeoCoordinate(latitude, longitude, uncertainty);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
