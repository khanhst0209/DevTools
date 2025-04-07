namespace DevTool.UISchema
{
    public class UISchema
    {
        public string name { get; set; }
        public List<SchemaInput> inputs { get; set; }

        public List<SchemaOutput> outputs { get; set; }
    }
}