using System.ComponentModel.DataAnnotations;

namespace DevTool.Input2Execute.LoremIpSumInput
{
    public class LoremIpsumInput
    {
        public Dictionary<string, bool> toggle { get; set; }

        public int paragraphs { get; set; }

        [Range(1, 50)]
        public int sentencesPerParagraph { get; set; }

        [Range(1, 50)]
        public int wordsPerSentence { get; set; }
    }
}