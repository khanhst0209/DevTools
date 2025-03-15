using System.ComponentModel.DataAnnotations.Schema;

namespace DevTools.data
{
    [Table("concacnho")]
    public class concac
    {
        public int id {get; set;}
        public string name{get; set;}
    }
}