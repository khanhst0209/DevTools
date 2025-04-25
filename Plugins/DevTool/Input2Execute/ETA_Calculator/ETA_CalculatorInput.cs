using System.ComponentModel.DataAnnotations;

namespace DevTool.Input2Execute.ETACalculator
{
    public class ETACalculatorInput
    {
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Total amount must be at least 1")]
        public double totalAmount { get; set; }

        [Required]
        public string startTime { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Unit count must be at least 1")]
        public double unitCount { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Time span must be at least 1")]
        public double unitTimeValue { get; set; }

        [Required]
        public string unitTimeType { get; set; }
    }
}