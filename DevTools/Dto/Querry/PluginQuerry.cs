namespace DevTools.Dto.Querry
{
    public class PluginQuerry
    {
        public string? Name { get; set; }
        public int? CategoryId { get; set; }

        public bool? Premium { get; set; }

        public string? SortBy { get; set; }
        public bool IsDescending { get; set; }
    }
}