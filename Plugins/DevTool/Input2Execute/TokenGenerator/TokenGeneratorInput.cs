using System.ComponentModel.DataAnnotations;

namespace DevTool.Input2Execute.TokenGenerator
{
    public class TokenGeneratorInput
    {
        [Required]
        public Dictionary<string, bool> toggle {get; set;}

        [Required]
        [Range(1, 512)]
        public int slider { get; set; }
    }
}