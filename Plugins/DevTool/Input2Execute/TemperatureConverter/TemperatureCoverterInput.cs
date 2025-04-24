using System.ComponentModel.DataAnnotations;

namespace DevTool.Input2Execute.TemperatureConverterInput
{
    public class TemperatureCoverterInput
    {
        [Required]
        public double value { get; set; }

        [Required]
        public string fromUnit { get; set; }

        [Required]
        public string toUnit { get; set; }
    }

    public enum TemperatureUnit
    {
        Celsius,
        Fahrenheit,
        Kelvin,
        Rankine,
        Delisle,
        Newton,
        Reaumur,
        Romer
    }
}