using System.Data;


namespace DevTool.UISchema
{
    public class SchemaInput
    {
        public string id { get; set; }
        public string? lable { get; set; } = null;
        public string? description { get; set; } = null;
        public bool? required { get; set; } = null;

        public string? Name { get; set; } = null;

        public string? placeholder { get; set; } = null;

        public object defaultValue { get; set; } = null;

        public float? min { get; set; } = null;
        public float? max { get; set; } = null;

        public float? step { get; set; } = null;

        public float? rows { get; set; } = null;

        public List<ComponentOption>? options { get; set; } = null;
        public string type { get; set; }
        
    }
}