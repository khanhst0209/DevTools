using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DevTool.Categories;
using DevTool.Input2Execute.Content_Expansion;
using DevTool.Roles;
using DevTool.UISchema;
using Plugins.DevTool;

namespace Content_Expansion;

public class Content_Expansion : IDevToolPlugin
{
    public int Id { get; set; }
    public string Name => "Content Expansion";

    public Category Category => Category.AI;


    public string Description { get; set; } = "Expanding user text input using Gemini LLM";

    public Roles AccessiableRole { get; set; } = Roles.Anonymous;
    public bool IsActive { get; set; } = true;
    public bool IsPremium { get; set; } = true;
   public string Icon { get; set; } = @"
<svg xmlns=""http://www.w3.org/2000/svg"" fill=""currentColor"" viewBox=""0 -960 960 960"" style=""width: 100%; height: auto;"">
  <path d=""M323-160q-11 0-20.5-5.5T288-181l-78-139h58l40 80h92v-40h-68l-40-80H188l-57-100q-2-5-3.5-10t-1.5-10q0-4 5-20l57-100h104l40-80h68v-40h-92l-40 80h-58l78-139q5-10 14.5-15.5T323-800h97q17 0 28.5 11.5T460-760v160h-60l-40 40h100v120h-88l-40-80h-92l-40 40h108l40 80h112v200q0 17-11.5 28.5T420-160h-97Zm217 0q-17 0-28.5-11.5T500-200v-200h112l40-80h108l-40-40h-92l-40 80h-88v-120h100l-40-40h-60v-160q0-17 11.5-28.5T540-800h97q11 0 20.5 5.5T672-779l78 139h-58l-40-80h-92v40h68l40 80h104l57 100q2 5 3.5 10t1.5 10q0 4-5 20l-57 100H668l-40 80h-68v40h92l40-80h58l-78 139q-5 10-14.5 15.5T637-160h-97Z"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""></path>
</svg>";






    public Schema schema => new Schema
    {
        id = Id,
        uiSchemas = new List<UISchema>{
            new UISchema {
                inputs = new List<SchemaInput>{
                    new SchemaInput{
                        id = "inputText",
                        label = "Your text",
                        type = ComponentType.textarea.ToString(),
                        defaultValue = "Xin chào mọi người, Tụi mình là nhóm 22, môn thiết kế phần mềm",
                        rows = 8
                    }
                },
                outputs = new List<SchemaOutput>{
                    new SchemaOutput{
                        id = "textoutput",
                        label = "Your Expanded text",
                        type = ComponentType.text.ToString(),
                        rows = 8
                    }
                }
            },
        }
    };

    private readonly string GeminiApiKey = "AIzaSyAhSqv-qMAco2y7Yy4w1_JXAGvFxRT-lWI"; // 🔁 Thay bằng API Key thật của bạn

    private string ExpandText(string inputText)
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
                    new { text = $"Hãy mở rộng văn bản sau một cách tự nhiên và chi tiết hơn, chỉ cần khoảng 3–4 câu mở rộng: {inputText}" }
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

        var result = ExpandText(myInput.inputText);

        return new Dictionary<string, object>
        {
            { "textoutput", result }
        };
    }

    public string GetSheme1()
    {
        throw new NotImplementedException();
    }
}
