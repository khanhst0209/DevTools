namespace MyWebAPI.Exceptions
{
    public class CategoryNotFoundException : Exception
    {
        private int Id { get; set; }
        public CategoryNotFoundException(int Id) : base($"Category with Id : {Id} can not be found") { }
    }
}