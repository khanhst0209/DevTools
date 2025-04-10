using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using DevTool.Categories;
using DevTool.Input2Execute.LoremIpSumInput;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace Lorem_Ipsum_Generator;

public class Lorem_Ipsum_Generator : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Lorem ipsum generator";

    public Category Category => Category.Text;


    public string Description { get; set; } = "Lorem ipsum is a placeholder text commonly used to demonstrate the visual form of a document or a typeface without relying on meaningful content";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = false;
    public string Icon { get; set; } = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24""><g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""><path d=""M4 6h16""></path><path d=""M4 12h16""></path><path d=""M4 18h12""></path></g></svg>";

    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
        new UISchema {
            inputs = new List<SchemaInput>{
                new SchemaInput{
                    id = "paragraphs",
                    label = "Paragraphs",
                    type = ComponentType.slider.ToString(),
                    min = 1,
                    max = 100,
                    step = 1,
                    defaultValue = 1
                },
                new SchemaInput{
                    id = "sentencesPerParagraph",
                    label = "Sentences per Paragraph",
                    type = ComponentType.slider.ToString(),
                    min = 1,
                    max = 50,
                    step = 1,
                    defaultValue = 10
                },
                new SchemaInput{
                    id = "wordsPerSentence",
                    label = "Words per Sentence",
                    type = ComponentType.slider.ToString(),
                    min = 1,
                    max = 50,
                    step = 1,
                    defaultValue = 10
                },
                new SchemaInput{
                    id = "toggle",
                    type = ComponentType.toggle.ToString(),
                    options = new List<ComponentOption>{
                        new ComponentOption{
                            value = "startWithLorem",
                            label = "Start with 'Lorem ipsum...' ?"
                        },
                        new ComponentOption{
                            value = "asHtml",
                            label = "As HTML?"
                        }
                    }
                }
            },
            outputs = new List<SchemaOutput>{
                new SchemaOutput{
                    id = "textoutput",
                    label = "Generated Lorem Ipsum",
                    type = ComponentType.text.ToString(),
                    rows = 4
                }
            }
        }
    }
    };


    public object Execute(object input)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
        var json = JsonSerializer.Serialize(dict);
        var myInput = JsonSerializer.Deserialize<LoremIpsumInput>(json);
        Console.WriteLine("Mapped Input: " + JsonSerializer.Serialize(myInput));

        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);
        Console.WriteLine("Validated Input");

        var result = GenerateLoremIpsum(myInput);
        return new { textoutput = result };
    }

    private static readonly string[] LoremWords = new[]
{
    "lorem", "ipsum", "dolor", "sit", "amet", "consectetur",
    "adipiscing", "elit", "sed", "do", "eiusmod", "tempor",
    "incididunt", "ut", "labore", "et", "dolore", "magna", "aliqua"
};

    private static readonly Random random = new();

    private string GenerateSentence(int wordCount)
    {
        var words = Enumerable.Range(0, wordCount)
            .Select(_ => LoremWords[random.Next(LoremWords.Length)])
            .ToArray();

        words[0] = char.ToUpper(words[0][0]) + words[0][1..]; // Capitalize first word
        return string.Join(" ", words) + ".";
    }

    private string GenerateParagraph(int sentenceCount, int wordCount, bool startWithLorem)
    {
        var sentences = new List<string>();

        if (startWithLorem)
        {
            sentences.Add("Lorem ipsum dolor sit amet.");
            sentenceCount--;
        }

        for (int i = 0; i < sentenceCount; i++)
        {
            sentences.Add(GenerateSentence(wordCount));
        }

        return string.Join(" ", sentences);
    }

    private string GenerateLoremIpsum(LoremIpsumInput input)
    {
        bool startWithLorem = input.toggle.TryGetValue("startWithLorem", out bool val1) && val1;
        bool asHtml = input.toggle.TryGetValue("asHtml", out bool val2) && val2;

        var paragraphs = new List<string>();
        for (int i = 0; i < input.paragraphs; i++)
        {
            bool start = i == 0 && startWithLorem;
            paragraphs.Add(GenerateParagraph(input.sentencesPerParagraph, input.wordsPerSentence, start));
        }

        return asHtml
            ? string.Join("\n", paragraphs.Select(p => $"<p>{p}</p>"))
            : string.Join("\n\n", paragraphs);
    }


    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
