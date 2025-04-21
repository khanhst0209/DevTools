using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DevTool.Categories;
using DevTool.Input2Execute.Content_Expansion;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace Text_Generation;

public class Text_Generation : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Text Generation";

    public Category Category => Category.AI;

    public string Description { get; set; } = "Generate text based on user input using Gemini LLM";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = true;
    public string Icon { get; set; } = @"
<svg xmlns=""http://www.w3.org/2000/svg"" fill=""currentColor"" viewBox=""0 -960 960 960"" style=""width: 100%; height: auto;"">
  <path d=""M160-360q-50 0-85-35t-35-85q0-50 35-85t85-35v-80q0-33 23.5-56.5T240-760h120q0-50 35-85t85-35q50 0 85 35t35 85h120q33 0 56.5 23.5T800-680v80q50 0 85 35t35 85q0 50-35 85t-85 35v160q0 33-23.5 56.5T720-120H240q-33 0-56.5-23.5T160-200v-160Zm200-80q25 0 42.5-17.5T420-500q0-25-17.5-42.5T360-560q-25 0-42.5 17.5T300-500q0 25 17.5 42.5T360-440Zm240 0q25 0 42.5-17.5T660-500q0-25-17.5-42.5T600-560q-25 0-42.5 17.5T540-500q0 25 17.5 42.5T600-440ZM320-280h320v-80H320v80Zm-80 80h480v-480H240v480Zm240-240Z"" />
</svg>";





    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema> {
            new UISchema {
                inputs = new List<SchemaInput> {
                    new SchemaInput {
                        id = "inputText",
                        label = "Your Text",
                        type = ComponentType.textarea.ToString(),
                        defaultValue = "Write a short story about a boy and a dog.",
                        rows = 8
                    }
                },
                outputs = new List<SchemaOutput> {
                    new SchemaOutput {
                        id = "textoutput",
                        label = "Your Generated Text",
                        type = ComponentType.text.ToString(),
                        rows = 8
                    }
                }
            },
        }
    };

    private readonly string GeminiApiKey = "AIzaSyAhSqv-qMAco2y7Yy4w1_JXAGvFxRT-lWI"; // 🔁 Thay bằng API Key thật của bạn

    private string GenerateText(string inputText)
    {
        using var client = new HttpClient();

        var requestUri = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={GeminiApiKey}";

        var requestBody = new
        {
            contents = new[]
            {
            new
            {
                parts = new[]
                {
                    new { text = $"Hãy tạo ra văn bản dựa vào văn bản của người dùng, tạo ra một đoạn khoảng 6-7 câu, bạn chỉ cần trả ra kết quả mà không cần nói gì thêm, đây là văn bản của người dùng: {inputText}" }
                }
            }
        }
        };

        var jsonBody = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var response = client.PostAsync(requestUri, content).Result;

        if (!response.IsSuccessStatusCode)
        {
            var error = response.Content.ReadAsStringAsync().Result;
            return $"[Lỗi gọi Gemini API]: {error}";
        }

        var responseString = response.Content.ReadAsStringAsync().Result;

        try
        {
            using var doc = JsonDocument.Parse(responseString);
            var root = doc.RootElement;

            // Truy xuất phần text trả về
            var text = root.GetProperty("candidates")[0]
                           .GetProperty("content")
                           .GetProperty("parts")[0]
                           .GetProperty("text")
                           .GetString();

            return text ?? "[Lỗi] Không nhận được văn bản từ Gemini.";
        }
        catch (Exception ex)
        {
            return "[Lỗi] Không thể phân tích phản hồi từ Gemini: " + ex.Message;
        }
    }

    public object Execute(object input)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(input.ToString());
        var json = JsonSerializer.Serialize(dict);
        var myInput = JsonSerializer.Deserialize<Content_ExpansionInput>(json);

        Console.WriteLine("Mapped Input: " + JsonSerializer.Serialize(myInput));
        Validator.ValidateObject(myInput, new ValidationContext(myInput), validateAllProperties: true);
        Console.WriteLine("Validated Input");

        var result = GenerateText(myInput.inputText);

        return new Dictionary<string, object> {
            { "textoutput", result }
        };
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
