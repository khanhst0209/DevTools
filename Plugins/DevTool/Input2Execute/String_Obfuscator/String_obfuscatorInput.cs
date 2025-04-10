using System.ComponentModel.DataAnnotations;

namespace DevTool.Input2Execute.String_Obfuscator
{
    public class StringObfuscatorInput
    {
        public string textToObfuscate { get; set; }

        [Range(0, 100)]
        public int keepFirst { get; set; }

        [Range(0, 100)]
        public int keepLast { get; set; }

        public Dictionary<string, bool> keepSpaces { get; set; }
    }
}