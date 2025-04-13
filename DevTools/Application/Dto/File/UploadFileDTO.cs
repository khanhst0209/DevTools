namespace DevTools.Application.Dto.File
{
    public class UploadFileDTO
    {
        public IFormFile DllFile { get; set; }
        public List<IFormFile>? Libaries { get; set; } = null;
    }
}