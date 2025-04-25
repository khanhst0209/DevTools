using System.ComponentModel.DataAnnotations;

namespace DevTool.Input2Execute.Time_Converter
{
    public class TimeConverterInput
    {
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Time value must be non-negative")]
        public double inputTime { get; set; }

        [Required]
        public string fromUnit { get; set; }

        [Required]
        public string toUnit { get; set; }
    }
}