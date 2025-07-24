using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esCAMTParser.Classes
{
    public class DecimalParser
    {
        public static decimal? ParseInvariant(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;

            return decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out var value)
                ? value
                : null;
        }

        public static decimal ParseInvariantOrThrow(string? input, string errorMessage)
        {
            if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                return value;

            throw new FormatException(errorMessage);
        }
    }
}
