using System.ComponentModel.DataAnnotations;

namespace DevTool.UISchema
{
    public class Schema
    {
        public int id { get; set; }

        public List<UISchema> uiSchemas { get; set; }


    }
}