using System.ComponentModel.DataAnnotations;

namespace DevTool.Input2Execute.DistanceCoverterInput
{
    public class DistanceConverterInput
    {
        [Required]
        public double value { get; set; }

        [Required]
        public string fromUnit { get; set; }

        [Required]
        public string toUnit { get; set; }
    }

    public enum DistanceUnit
    {
        Millimeter,
        Centimeter,
        Meter,
        Kilometer,
        Inch,
        Foot,
        Yard,
        Mile,
        Nauticalmile
    }
}