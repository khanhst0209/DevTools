namespace DevTool.UISchema
{
    public class SchemaOutput
    {
        public string id { get; set; }
        public string label { get; set; }
        public string? description { get; set; } = null;

        private string _type;
        public string type
        {
            get => _type;
            set
            {
                if (value != ComponentType.text.ToString() && value != ComponentType.textarea.ToString()
                && value != ComponentType.xml.ToString() && value != ComponentType.json.ToString())
                {
                    throw new ArgumentException("Only 'text' and 'textarea' are allowed for this component.");
                }
                _type = value;
            }
        }

        public int? rows { get; set; } = null;
        public string? placeholder { get; set; } = null;
        public ComponentResize? resize { get; set; } = null;
    }
}