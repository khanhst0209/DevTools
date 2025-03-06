namespace MyWebAPI.Helper
{
    public class ItemQuerry
    {
        public double? Price {get; set;}
        public string? Name { get; set; }
        public int? CategoryId {get; set;}

        public string? SortBy {get; set;}
        public bool IsDescending {get; set;}

        public int Pagenumbers {get; set;}
        public int Page_Size {get; set;}
    }
}