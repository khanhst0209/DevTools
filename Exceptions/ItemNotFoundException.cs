namespace MyWebAPI.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public int ItemId {get;}
        public ItemNotFoundException(int id) : base($"Item with Id : {id } can not be found.")
        {
            ItemId = id;
        }
    }
}