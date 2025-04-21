namespace Text_Summarization
{
    using System.ComponentModel.DataAnnotations;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using DevTool.Categories;
    using DevTool.Input2Execute.Content_Expansion;
    using DevTool.Roles;
    using DevTool.UISchema;
    using Plugins.DevTool;

    public class Text_Summarization : IDevToolPlugin
    {
        public int Id { get; set; }
        public string Name => "Text Summarization";

        public Category Category => Category.AI;

        public string Description { get; set; } = "Summarize text based on user input using Gemini LLM";

        public Roles AccessiableRole { get; set; } = Roles.Anonymous;
        public bool IsActive { get; set; } = true;
        public bool IsPremium { get; set; } = true;
        public string Icon { get; set; } = @"
<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" viewBox=""0 0 24 24"">
    <g fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"">
        <path d=""M7 4a2 2 0 0 0-2 2v3a2 3 0 0 1-2 3a2 3 0 0 1 2 3v3a2 2 0 0 0 2 2""></path>
        <path d=""M17 4a2 2 0 0 1 2 2v3a2 3 0 0 0 2 3a2 3 0 0 0-2 3v3a2 2 0 0 1-2 2""></path>
    </g>
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
                            defaultValue = "Trong một thế giới đang thay đổi nhanh chóng, công nghệ đã trở thành yếu tố không thể thiếu trong cuộc sống hàng ngày của chúng ta. Từ việc sử dụng điện thoại thông minh, máy tính bảng cho đến các thiết bị gia đình thông minh, công nghệ đã thay đổi cách chúng ta tương tác với môi trường xung quanh. Cùng với sự phát triển mạnh mẽ của Internet, việc kết nối với bạn bè, gia đình và đồng nghiệp trở nên dễ dàng hơn bao giờ hết. Tuy nhiên, sự phát triển này cũng mang lại những thách thức lớn, đặc biệt là vấn đề bảo mật và quyền riêng tư. Các công ty công nghệ đang đối mặt với áp lực không ngừng trong việc bảo vệ dữ liệu người dùng và đảm bảo rằng thông tin cá nhân không bị lộ ra ngoài.Một trong những yếu tố quan trọng trong việc bảo vệ thông tin là mã hóa. Mã hóa dữ liệu giúp đảm bảo rằng chỉ những người có quyền truy cập mới có thể xem được thông tin cá nhân của bạn. Điều này rất quan trọng trong bối cảnh các cuộc tấn công mạng ngày càng trở nên tinh vi và khó lường. Các hacker không chỉ tấn công vào các hệ thống của các công ty lớn, mà còn nhắm đến người dùng cá nhân, đánh cắp thông tin và sử dụng vào các mục đích xấu. Vì vậy, mỗi cá nhân cần phải có ý thức bảo vệ dữ liệu của mình bằng cách sử dụng các biện pháp bảo mật mạnh mẽ, chẳng hạn như mật khẩu phức tạp và phần mềm chống virus.",
                            rows = 8
                        }
                    },
                    outputs = new List<SchemaOutput> {
                        new SchemaOutput {
                            id = "textoutput",
                            label = "Your Summarized Text",
                            type = ComponentType.text.ToString(),
                            rows = 8
                        }
                    }
                },
            }
        };

        private readonly string GeminiApiKey = "AIzaSyAhSqv-qMAco2y7Yy4w1_JXAGvFxRT-lWI"; // 🔁 Thay bằng API Key thật của bạn

        private string SummarizeText(string inputText)
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
                            new { text = $"Tóm tắt văn bản dựa vào văn bản của người dùng, bạn chỉ cần trả ra kết quả mà không cần nói gì thêm, đây là văn bản của người dùng : {inputText}" }
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

                // Truy xuất phần text tóm tắt trả về
                var text = root.GetProperty("candidates")[0]
                               .GetProperty("content")
                               .GetProperty("parts")[0]
                               .GetProperty("text")
                               .GetString();

                return text ?? "[Lỗi] Không nhận được tóm tắt từ Gemini.";
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

            var result = SummarizeText(myInput.inputText);

            return new Dictionary<string, object> {
                { "textoutput", result }
            };
        }

        public string GetSheme1()
        {
            throw new NotImplementedException();
        }
    }
}
