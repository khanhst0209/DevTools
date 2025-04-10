using System.ComponentModel.DataAnnotations;

namespace DevTool.Input2Execute.Text_Statistics
{
    public class TextStatisticsInput
    {
        [Required]
        public string textInput { get; set; }
    }

}