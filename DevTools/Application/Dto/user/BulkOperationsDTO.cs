namespace DevTools.Dto.user
{
    public class BulkDeleteUsersDTO
    {
        public List<string> UserIds { get; set; } = new List<string>();
    }

    public class BulkChangeRoleDTO
    {
        public List<string> UserIds { get; set; } = new List<string>();
        public string NewRole { get; set; }
    }
}